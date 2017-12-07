using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GradHelperWPF.Utils
{
    public class XmlUtil
    {
        public static string ReadXPathStrValue( XDocument doc, string xPath )
        {
            var result = "";
            try
            {
                result = doc.XPathSelectElement( xPath ).Value;
            }
            catch ( Exception )
            {
                result = "INVALID XPATH";
            }
            return result;
        }

        public static int ReadXPathIntValue( XDocument doc, string xPath )
        {
            var retValue = -1;
            var strVal = "";

            try
            {
                strVal = doc.XPathSelectElement( xPath ).Value;
                int.TryParse( strVal, out retValue );
            }
            catch ( Exception )
            {
                retValue = int.MinValue;
            }
            return retValue;
        }

        public static Dictionary<string, string> LoadXMLConfiguration( string path )
        {
            if ( string.IsNullOrEmpty( path ) || !File.Exists( path ) )
                return null;

            var retDiction = new Dictionary<string, string>();

            var configFile = XDocument.Load(path);

            if ( configFile == null )
                return null;

            var parent = configFile.Root;

            retDiction.Add( parent.Name.ToString( ), parent.Value ?? "" );

            var childrens = parent.Elements();
            string xName = "", xVal = "";

            foreach ( var child in childrens )
            {
                xName = child.Name.ToString( );
                xVal = child.Value;

                if ( !retDiction.ContainsKey( xName ) )
                    retDiction.Add( xName, xVal );
            }

            return retDiction;
        }

        public static string SaveXMLConfiguration( string path, Dictionary<string, string> data )
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

                root.Add( new XElement( entry.Key, entry.Value ?? "" ) );
            }

            using ( var fs = new FileStream( path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite ) )
            {
                doc.Save( fs );
            }

            return result;
        }

        public static bool AppendToXmlFile( Stream fs, Dictionary<string, string> data )
        {
            return false;
        }
    }
}