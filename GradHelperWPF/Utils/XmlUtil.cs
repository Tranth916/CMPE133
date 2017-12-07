using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GradHelperWPF.Utils
{
    public class XmlUtil
    {
        public static bool AppendToXmlFile( Stream fs, Dictionary<string, string> data )
        {
            return false;
        }

        public static Dictionary<string, string> LoadXmlConfiguration( string path )
        {
            if ( string.IsNullOrEmpty( path ) || !File.Exists( path ) )
                return null;

            var retDiction = new Dictionary<string, string>();

            var configFile = XDocument.Load(path);

            var parent = configFile.Root;

            if ( parent != null )
            {
                retDiction.Add( parent.Name.ToString( ), parent.Value ?? "" );

                var childrens = parent.Elements();

                foreach ( var child in childrens )
                {
                    var xName = child.Name.ToString();
                    var xVal = child.Value;

                    if ( !retDiction.ContainsKey( xName ) )
                        retDiction.Add( xName, xVal );
                }
            }

            return retDiction;
        }

        public static int ReadXPathIntValue( XDocument doc, string xPath )
        {
            var retValue = -1;

            try
            {
                var strVal = doc.XPathSelectElement( xPath )?.Value;
                int.TryParse( strVal, out retValue );
            }
            catch ( Exception )
            {
                retValue = int.MinValue;
            }
            return retValue;
        }

        public static string ReadXPathStrValue( XDocument doc, string xPath )
        {
            var result = "";
            try
            {
                result = doc.XPathSelectElement( xPath )?.Value;
            }
            catch ( Exception )
            {
                result = "INVALID XPATH";
            }
            return result;
        }
        public static string SaveXmlConfiguration( string path, Dictionary<string, string> data )
        {
            // delete the existing file if it exists.
            if ( File.Exists( path ) )
                try
                {
                    File.Delete( path );
                }
                catch ( Exception ex )
                {
                    throw new Exception( ex.StackTrace );
                }

            var result = path;

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            //Make the root
            doc.Add( new XElement( "GradAppDataRoot" ) );

            var root = doc.Root;

            foreach ( var entry in data )
            {
                if ( string.IsNullOrEmpty( entry.Key ) || string.IsNullOrEmpty( entry.Value ) )
                    continue;

                root?.Add(new XElement(entry.Key, entry.Value ?? ""));
            }

            using ( var fs = new FileStream( path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite ) )
            {
                doc.Save( fs );
            }

            return result;
        }
    }
}