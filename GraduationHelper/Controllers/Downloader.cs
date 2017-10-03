using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using GraduationHelper.Interfaces;
using GraduationHelper.Utils;


namespace GraduationHelper.Controllers
{
	public class Downloader : IFileHandler
	{
		private static string _nodeRootXpath = $"//Student";

		private string _checkListFormXPath = $"{_nodeRootXpath}/Forms/CheckListForm";
		private string _majorFormXPath = $"{_nodeRootXpath}/Forms/MajorForm";
		private string _gradAppXPath = $"{_nodeRootXpath}/Forms/GradAppForm";
		private string _equivalencyFormXPath = $"{_nodeRootXpath}/Forms/EquivalencyForm";
		private string _substitutionForm = $"{_nodeRootXpath}/Forms/SubsitutionForm";

		private Controller _controller;
		private Dictionary<string, string> _fileUrls;

		public Downloader(Controller controller)
		{
			_controller = controller;
		}

		private void Init()
		{
			if(_fileUrls == null && _controller != null && _controller.ConfigFile != null)
			{
				_fileUrls = new Dictionary<string, string>();
				
				_fileUrls.Add("CheckListForm", XmlHelper.ReadXPathStrValue( _controller.ConfigFile, _checkListFormXPath ?? ""));
				_fileUrls.Add("MajorForm", XmlHelper.ReadXPathStrValue(_controller.ConfigFile, _majorFormXPath) ?? "");
				_fileUrls.Add("GradAppForm", XmlHelper.ReadXPathStrValue(_controller.ConfigFile, _gradAppXPath) ?? "");
				_fileUrls.Add("EquivalencyForm", XmlHelper.ReadXPathStrValue(_controller.ConfigFile, _equivalencyFormXPath) ?? "");
				_fileUrls.Add("SubstitutionForm", XmlHelper.ReadXPathStrValue(_controller.ConfigFile,_substitutionForm) ?? "");
			}
		}

		public void GetFile(string[] files = null)
		{
			if (files == null)
				return;

			List<string> toDownload = new List<string>();

			string key = "";

			foreach (string str in files)
			{
				key = str.ToLower().Trim();

				if (key.Contains("checklist"))
				{
					toDownload.Add(_fileUrls["CheckListForm"]);
				}
				else if (key.Contains("major"))
				{
					toDownload.Add(_fileUrls["MajorForm"]);	
				}
				else if (key.Contains("gradapp"))
				{
					toDownload.Add(_fileUrls["GradAppForm"]);
				}
				else if (key.Contains("equivalency"))
				{
					toDownload.Add(_fileUrls["EquivalencyForm"]);
				}
			}

			if (toDownload.Count > 0)
				StartDownloaderThreads(toDownload);
		}

		private void StartDownloaderThreads(List<string> downloads)
		{
			Thread[] threads = new Thread[downloads.Count];

			int index = 0;
			foreach(var file in downloads)
			{
				threads[index++] = new Thread(() => 
					 {
						 WebClient wb = new WebClient();
						 wb.DownloadFile(file, Path.GetFileName(file));
					 });
			}
		}
	}
}
