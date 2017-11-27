using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;

namespace GradHelperWPF.Utils
{
    public class XmlUtil
    {
        public static string ReadXPathStrValue(XDocument doc, string xPath)
        {
            string result = "";
            try
            {
                result = doc.XPathSelectElement(xPath).Value;
            }
            catch (Exception)
            {
                result = "INVALID XPATH";
            }
            return result;
        }

        public static int ReadXPathIntValue(XDocument doc, string xPath)
        {
            int retValue = -1;
            string strVal = "";

            try
            {
                strVal = doc.XPathSelectElement(xPath).Value;
                int.TryParse(strVal, out retValue);
            }
            catch (Exception)
            {
                retValue = int.MinValue;
            }
            return retValue;
        }

        public static Dictionary<string, string> LoadXMLConfiguration(string path)
        {
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return null;

            Dictionary<string, string> retDiction = new Dictionary<string, string>();

            XDocument configFile = XDocument.Load(path);

            if (configFile == null)
                return null;

            XElement parent = configFile.Root;

            retDiction.Add(parent.Name.ToString(), parent.Value ?? "");

            var childrens = parent.Elements();
            string xName = "", xVal = "";

            foreach (XElement child in childrens)
            {
                xName = child.Name.ToString();
                xVal = child.Value;

                if (!retDiction.ContainsKey(xName))
                {
                    retDiction.Add(xName, xVal);
                }
            }

            return retDiction;
        }

        public static string SaveXMLConfiguration(string path, Dictionary<string,string> data)
        {
            // delete the existing file if it exists.
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.StackTrace);                    
                }
            }

            string result = path;

            XDocument doc = new XDocument(  new XDeclaration("1.0","utf-8","yes") );

            //Make the root
            doc.Add(new XElement("GradAppDataRoot"));

            var root = doc.Root;

            foreach ( var entry in data)
            {
                if (string.IsNullOrEmpty(entry.Key) || string.IsNullOrEmpty(entry.Value))
                    continue;

                root.Add(new XElement(entry.Key, entry.Value ?? ""));
            }

            using( FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.ReadWrite) )
            {
                doc.Save(fs);    
            }

            return result;
        }

    }
}
