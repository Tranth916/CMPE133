using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections;

namespace GraduationHelper.Utils
{
	public class XmlHelper
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
			catch(Exception)
			{
				retValue = int.MinValue;
			}
			return retValue;
		}
		
		public static Dictionary<string, string> LoadXMLConfiguration(string path)
		{
			if (path == null || path.Length == 0)
				return null;

			Dictionary<string, string> retDiction = new Dictionary<string, string>();

			XDocument configFile = XDocument.Load(path);

			if (configFile == null)
				return null;

			XElement parent = configFile.Root;

			retDiction.Add(parent.Name.ToString(), parent.Value ?? "");

			var childrens = parent.Elements();
			string xName = "", xVal = "";

			foreach(XElement child in childrens)
			{
				xName = child.Name.ToString();
				xVal = child.Value;
				
				if(!retDiction.ContainsKey(xName))
				{
					retDiction.Add(xName, xVal);
				}
			}
			
			return retDiction;
		}

	}
}
