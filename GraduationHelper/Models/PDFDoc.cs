using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper.Interfaces;
using System.IO;
using iTextSharp.text.io;
using iTextSharp.text.pdf;
using System.Diagnostics;
using iTextSharp.text;

namespace GraduationHelper.Models
{
	public class PDFDoc : iTextSharp.text.pdf.PdfReader,IDoc
	{
		public PDFDoc(string filePath) : base(filePath)
		{
			FileName = Path.GetFileName(filePath);
			FileLocation = filePath;
		}
		
		public void WriteAndSaveFirstName()
		{
			Rectangle size = this.GetPageSizeWithRotation(1);

			Document docc = new Document(size);
			
			FileStream fs = new FileStream
				(
					$"C:\\GraduationHelper\\GraduationHelper\\GraduationHelper\\bin\\Debug\\test", 
					FileMode.Create,
					FileAccess.Write, 
					FileShare.None
				);

			PdfWriter writer = PdfWriter.GetInstance(docc, fs);

			docc.Open();
			PdfContentByte cb = writer.DirectContent;

			// select the font properties
			BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
			cb.SetColorFill(BaseColor.DARK_GRAY);
			cb.SetFontAndSize(bf, 8);

			// write the text in the pdf content
			cb.BeginText();
			string text = "Some random blablablabla...";
			// put the alignment and coordinates here
			cb.ShowTextAligned(1, text, 520, 640, 0);
			cb.EndText();
			cb.BeginText();
			text = "Other random blabla...";
			// put the alignment and coordinates here
			cb.ShowTextAligned(2, text, 100, 200, 0);
			cb.EndText();

			// create the new page and add it to the pdf
			PdfImportedPage page = writer.GetImportedPage(this, 1);
			
			cb.AddTemplate(page, 0, 0);
			
			docc.Close();
			fs.Close();
			writer.Close();
			this.Close();
		}
		
		#region IDoc Members
		public string FileName { get; set; }
		public string FileLocation { get; set; }
		public string FileVersion { get; set; }
		#endregion
	}
}
