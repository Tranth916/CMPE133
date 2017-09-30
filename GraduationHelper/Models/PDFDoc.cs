using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper.Interfaces;

namespace GraduationHelper.Models
{
	public class PDFDoc : IDoc
	{
		public PDFDoc()
		{

		}


		#region IDoc Members
		public string FileName { get; set; }
		public string FileLocation { get; set; }
		public string FileVersion { get; set; }
		#endregion
	}
}
