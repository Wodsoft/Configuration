// <auto-generated />
namespace Microsoft.Extensions.Configuration.Xml
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.Extensions.Configuration.Xml.Resources", typeof(Resources).Assembly);

        /// <summary>
        /// Encrypted XML is not supported on this platform.
        /// </summary>
        internal static string Error_EncryptedXmlNotSupported
        {
            get { return GetString("Error_EncryptedXmlNotSupported"); }
        }

        /// <summary>
        /// Encrypted XML is not supported on this platform.
        /// </summary>
        internal static string FormatError_EncryptedXmlNotSupported()
        {
            return GetString("Error_EncryptedXmlNotSupported");
        }

        /// <summary>
        /// File path must be a non-empty string.
        /// </summary>
        internal static string Error_InvalidFilePath
        {
            get { return GetString("Error_InvalidFilePath"); }
        }

        /// <summary>
        /// File path must be a non-empty string.
        /// </summary>
        internal static string FormatError_InvalidFilePath()
        {
            return GetString("Error_InvalidFilePath");
        }

        /// <summary>
        /// A duplicate key '{0}' was found.{1}
        /// </summary>
        internal static string Error_KeyIsDuplicated
        {
            get { return GetString("Error_KeyIsDuplicated"); }
        }

        /// <summary>
        /// A duplicate key '{0}' was found.{1}
        /// </summary>
        internal static string FormatError_KeyIsDuplicated(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Error_KeyIsDuplicated"), p0, p1);
        }

        /// <summary>
        /// XML namespaces are not supported.{0}
        /// </summary>
        internal static string Error_NamespaceIsNotSupported
        {
            get { return GetString("Error_NamespaceIsNotSupported"); }
        }

        /// <summary>
        /// XML namespaces are not supported.{0}
        /// </summary>
        internal static string FormatError_NamespaceIsNotSupported(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Error_NamespaceIsNotSupported"), p0);
        }

        /// <summary>
        /// Unsupported node type '{0}' was found.{1}
        /// </summary>
        internal static string Error_UnsupportedNodeType
        {
            get { return GetString("Error_UnsupportedNodeType"); }
        }

        /// <summary>
        /// Unsupported node type '{0}' was found.{1}
        /// </summary>
        internal static string FormatError_UnsupportedNodeType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Error_UnsupportedNodeType"), p0, p1);
        }

        /// <summary>
        ///  Line {0}, position {1}.
        /// </summary>
        internal static string Msg_LineInfo
        {
            get { return GetString("Msg_LineInfo"); }
        }

        /// <summary>
        ///  Line {0}, position {1}.
        /// </summary>
        internal static string FormatMsg_LineInfo(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Msg_LineInfo"), p0, p1);
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
