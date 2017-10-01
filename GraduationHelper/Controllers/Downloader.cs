using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GraduationHelper.Interfaces;

namespace GraduationHelper.Controllers
{
	public class Downloader : IFileHandler
	{
		string _dest = $"C:\\Downloader\\test.pdf";
		string _checkList = $"https://cmpe.sjsu.edu/files/public/media/resources/studentforms/graduationpacket-checklist-se1_0.pdf";

		private Controller _controller;

		public Downloader(Controller controller)
		{
			_controller = controller;
		}

		public void GetFile()
		{
			WebClient wb = new WebClient();
			wb.DownloadFile(_checkList, _dest);


		}
	}
}
