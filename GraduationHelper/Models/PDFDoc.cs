using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GraduationHelper.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfiumViewer;
using GraduationHelper.Utils;

namespace GraduationHelper.Models
{
	public class PDFDoc : iTextSharp.text.pdf.PdfReader, IDoc
	{
		#region IDoc Members
		public string FileName { get; set; }
		public string FileLocation { get; set; }
		public string FileVersion { get; set; }
		#endregion

		#region Private Members
		private const PdfViewerZoomMode FitWidth = PdfViewerZoomMode.FitWidth;
		private Rectangle _rect;
		private string _copiedFileName;
		#endregion

		#region Public Members
		public bool IsMyAcademicRequirement { set; get; }
		public bool IsTranscript { set; get; }
		public PdfReader Reader
		{
			private set;
			get;
		}
		public PdfViewer View
		{
			private set;
			get;
		}
		public Document DocTextSharp
		{
			private set;
			get;
		}
		public PdfiumViewer.PdfDocument PdfiumDoc
		{
			private set;
			get;
		}
		public Dictionary<string, Course> CourseDictionary
			{
				private set;
				get;
			}
		#endregion
		
		/// <summary>
		/// Extends iTextSharp...PdfReader and uses PdfiumViewer
		/// </summary>
		/// <param name="filePath"></param>
		public PDFDoc(string filePath) : base(filePath)
		{
			Reader = this;
			FileName = Path.GetFileName(filePath);
			FileLocation = filePath;
			Init();
		}

		private void Init()
		{
			if (!File.Exists(FileLocation) && !FileLocation.ToLower().EndsWith(".pdf"))
				FileLocation += "\\.pdf";

			try
			{
				//Set the PdfiumDoc so it can be rendered in WinForm, treat this as model obj.
				PdfiumDoc = PdfiumViewer.PdfDocument.Load(FileLocation);

				View = new PdfViewer()
				{
					Document = PdfiumDoc,
					Name = FileName,
					ZoomMode = FitWidth,
					ShowToolbar = true,
				};

				ParsePdf();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception throw while PdfDoc Init: " + ex);
			}
		}
		
		public void ParsePdf()
		{
			StringBuilder sb = new StringBuilder();

			for(int i = 1; i < PdfiumDoc.PageCount; i++)
			{
				try
				{
					sb.Append(PdfiumDoc.GetPdfText(i).Replace(PdfHelper.SplitPatternCarriage, ""));
				}
				catch(Exception e)
				{
					Debug.WriteLine("Exception throw while reading pdf files." + e.Message);
				}
			}

			try
			{
				//Reader.Dispose();
				//PdfiumDoc.Dispose();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}

			Dictionary<string, Course> transcripts = PdfHelper.ParsePDF(ImportedPDF.MyProgressTranscript, sb) as Dictionary<string,Course>;

			if (transcripts != null)
			{
				Debug.WriteLine("Transcript Dictionary !null");
				this.CourseDictionary = transcripts;
			}

			sb.Clear();
		}
		
		public Dictionary<string,string[]> GetInfoForView()
		{
			Dictionary<string, string[]> data = new Dictionary<string, string[]>();
			
			data.Add("TableType", new string[] { "CourseTranscript" });

			if (CourseDictionary == null)
				return data;
			
			Course c;
			string name;
			string[] dat;

			foreach(var entry in CourseDictionary)
			{
				c = entry.Value;
				name = c.FullName;
				dat = c.DataArray;
				
				if (!data.ContainsKey(name))
					data.Add(name, dat);
			}
			
			return data;
		}
		
		public bool WriteDataToPdf(string path, PdfReader template = null)
		{
			//int fontSize = 15;
			//int topOfPage = 792;
			//int rightOfPage = 612;
			//int centerX = rightOfPage / 2;

			//if (_rect == null)
			//	_rect = new Rectangle(Reader.GetPageSize(1));
			
			//Create a output doc.
			Document output = new Document(_rect);

			FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
			
			PdfWriter writer = PdfWriter.GetInstance(output, fs);

			output.Open();

			PdfContentByte contentWriter = writer.DirectContent;

			if(template != null)
			{ 
				PdfImportedPage page = writer.GetImportedPage(template, 1);
				contentWriter.AddTemplate(page, 0, 0);
			}
			
			BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
			
			//contentWriter.SetFontAndSize(baseFont, fontSize);

			//int startingY = topOfPage - fontSize;

			//var textDict = CourseDictionary.Keys.ToArray();

			//foreach(string s in textDict)
			//{
			//	contentWriter.BeginText();
			//	contentWriter.ShowTextAligned(0, s, 10f, (float)startingY, 0);
			//	startingY -= fontSize;
			//	contentWriter.EndText();
			//}
		
			//output.Close();
			//fs.Close();
			//writer.Close();

			//Process.Start("explorer.exe", path);
			return true;
		}	
	}
}
