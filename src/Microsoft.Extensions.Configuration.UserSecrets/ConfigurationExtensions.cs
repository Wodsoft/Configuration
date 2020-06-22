// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Configuration extensions for adding user secrets configuration source.
    /// </summary>
    public static class ConfigurationExtensions
    {
        private const string Secrets_File_Name = "secrets.json";

        /// <summary>
        /// Adds the user secrets configuration source. Searches the assembly that contains type <typeparamref name="T"/>
        /// for an instance of <see cref="UserSecretsIdAttribute"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <typeparam name="T">The type from the assembly to search for an instance of <see cref="UserSecretsIdAttribute"/>.</typeparam>
        /// <returns></returns>
        public static IConfigurationBuilder AddUserSecrets<T>(this IConfigurationBuilder configuration)
            where T : class
            => configuration.AddUserSecrets(typeof(T).Assembly);

        /// <summary>
        /// <para>
        /// This method is obsolete and will be removed in a future version.
        /// The recommended alternative is <see cref="AddUserSecrets(IConfigurationBuilder, string)"/> or <see cref="AddUserSecrets{T}(IConfigurationBuilder)"/> .
        /// </para>
        /// <para>
        /// Adds the user secrets configuration source. Searches the assembly from <see cref="Assembly.GetEntryAssembly"/>
        /// for an instance of <see cref="UserSecretsIdAttribute"/>.
        /// </para>
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is .AddUserSecrets(string userSecretsId) or .AddUserSecrets<TStartup>().")]
        public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.LoadFile(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Substring(0, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Length - 7));
            if (entryAssembly == null)
            {
                // can occur inside an app domain
                throw new InvalidOperationException(Resources.Error_EntryAssemblyNull);
            }

            var attribute = entryAssembly.GetCustomAttributes(typeof(UserSecretsIdAttribute), true).Cast<UserSecretsIdAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                return AddUserSecrets(configuration, attribute.UserSecretsId);
            }

            // try fallback to project.json for legacy support
            try
            {
                var fileProvider = configuration.GetFileProvider();
#pragma warning disable CS0618
                return AddSecretsFile(configuration, PathHelper.GetSecretsPath(fileProvider));
#pragma warning restore CS0618
            }
            catch
            { }

            // Show the error about missing UserSecretIdAttribute instead an error about missing
            // project.json as PJ is going away.
            throw MissingAttributeException(entryAssembly);
        }

        /// <summary>
        /// Adds the user secrets configuration source.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="assembly">The assembly with the <see cref="UserSecretsIdAttribute" /></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder configuration, Assembly assembly)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var attribute = assembly.GetCustomAttributes(typeof(UserSecretsIdAttribute), true).Cast<UserSecretsIdAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                throw MissingAttributeException(assembly);
            }

            return AddUserSecrets(configuration, attribute.UserSecretsId);
        }

        /// <summary>
        /// Adds the user secrets configuration source with specified secrets id.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userSecretsId"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder configuration, string userSecretsId)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (userSecretsId == null)
            {
                throw new ArgumentNullException(nameof(userSecretsId));
            }

            return AddSecretsFile(configuration, PathHelper.GetSecretsPathFromSecretsId(userSecretsId));
        }

        private static IConfigurationBuilder AddSecretsFile(IConfigurationBuilder configuration, string secretPath)
        {
            var directoryPath = Path.GetDirectoryName(secretPath);
            var fileProvider = Directory.Exists(directoryPath)
                ? new PhysicalFileProvider(directoryPath)
                : null;
            return configuration.AddJsonFile(fileProvider, PathHelper.Secrets_File_Name, optional: true, reloadOnChange: false);
        }

        private static Exception MissingAttributeException(Assembly assembly)
            => new InvalidOperationException(Resources.FormatError_Missing_UserSecretsIdAttribute(assembly.FullName));
    }
}