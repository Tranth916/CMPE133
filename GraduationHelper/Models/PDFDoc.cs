using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper.Interfaces;
using System.IO;
using iTextSharp.text.pdf;

namespace GraduationHelper.Models
{
	public class PDFDoc : iTextSharp.text.pdf.PdfReader, IDoc
	{
		public PDFDoc(string filePath) : base(filePath)
		{
			FileName = Path.GetFileName(filePath);
			FileLocation = filePath;
		}
		
		public void WriteAndSaveFirstName()
		{
		//	base.AcroFields.
		}






		
		#region IDoc Members
		public string FileName { get; set; }
		public string FileLocation { get; set; }
		public string FileVersion { get; set; }
		#endregion
	}
}
