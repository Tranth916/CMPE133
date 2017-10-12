using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper.Interfaces;
using System.IO;
using iTextSharp.text.io;
using iTextSharp.text.api;
using iTextSharp.text.factories;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Diagnostics;
using iTextSharp.text;
using PdfiumViewer;
using System.Linq;
using System.Text.RegularExpressions;

namespace GraduationHelper.Models
{
	public class PDFDoc : iTextSharp.text.pdf.PdfReader, IDoc
	{
		#region IDoc Members
		public string FileName { get; set; }
		public string FileLocation { get; set; }
		public string FileVersion { get; set; }
		#endregion

		private const PdfViewerZoomMode FitWidth = PdfViewerZoomMode.FitWidth;

		private Rectangle _rect;
		private string _copiedFileName;

		private float StartingX { get { return Reader.GetPageSize(1).Left; } }
		private float StartingY { get { return Reader.GetPageSize(1).Bottom; } }
		private float EndingX { get { return Reader.GetPageSize(1).Right; } }
		private float EndingY { get { return Reader.GetPageSize(1).Top; } }

		#region Public Members
		public Dictionary<string, string> GeneralEdDictionary;
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
		#endregion

		/// <summary>
		/// Extends iTextSharp...PdfReader and uses PdfiumViewer
		/// </summary>
		/// <param name="filePath"></param>
		public PDFDoc(string filePath) : base(filePath)
		{
			Reader = this;
			FileName = System.IO.Path.GetFileName(filePath);
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

		public void RefreshReload()
		{




		}
		
		public void ParsePdf()
		{
			if(PdfiumDoc.PageCount > 0)
				_rect = GetPageSizeWithRotation(1);
			
			string textFromPage;
			List<string> filtered = new List<string>();
			
			for (int i = 1; i < PdfiumDoc.PageCount; i++)
			{
				try
				{
					textFromPage = PdfiumDoc.GetPdfText(i);
					if (textFromPage == null || textFromPage.Length == 0)
						continue;

					textFromPage = textFromPage.Replace("Requi rement", "Requirements");
					textFromPage = textFromPage.Replace("requi rement", "requirements");
					textFromPage = textFromPage.Replace("Descr iption", "Description");
					textFromPage = textFromPage.Replace("\r", "");

					char[] sep = new char[] { '\n' };
					List<string> lines = textFromPage.Split(sep, StringSplitOptions.RemoveEmptyEntries).ToList();

					List<string> vertBars = new List<string>();
					
					for(int j=0; j < lines.Count; j++)
					{
						try
						{
							if (lines[j].Contains("|") && j-1 > 0 && j + 1 < lines.Count)
								vertBars.Add(lines[j - 1]);
						}
						catch (Exception) { }
					}

					foreach (var ff in vertBars)
						Debug.WriteLine(ff);


					
					
					if (lines.Count == 0)
						continue;

					ParseAcademicReqs(ref lines, ref filtered);
				}
				catch(Exception ex)
				{
					Debug.WriteLine("Exception throw while parsing pdf: " + ex.StackTrace);
				}		
			}

			if(GeneralEdDictionary != null)
			{
				foreach(var entry in GeneralEdDictionary)
				{
					Debug.WriteLine($"key: {entry.Key}  val: {entry.Value}");
				}
			}
		}
		
		public void ParseAcademicReqs(ref List<string> uncleaned, ref List<string> cleaned)
		{
			string toAdd = "";
			string[] firstSplit;
			string year;
			float units = 0;
			string s;

			try
			{
				for (int i = 0; i < uncleaned.Count; i++)
				{
					s = uncleaned[i];

					if (s.StartsWith("|") && s.Contains("of"))
						continue;

					if (s.StartsWith("http"))
						continue;

					if ((s.Contains("Spring") || s.Contains("Fall") || s.Contains("Summer")) && s.Contains("&"))
						continue;

					if (s.Contains("Complete 1 course."))
						continue;

					if (s.Replace(" ", "").Contains("Thefollowingcoursesmaybeusedtosatisfythisrequirement:"))
						continue;

					if (s.Contains("My Academic Requirements"))
						continue;

					if (s.Contains("continuous enrollment in Fall 2011 or later, you must earn a minimum aggregate"))
						continue;

					if (!(s.Contains("Fall") || s.Contains("Summer") || s.Contains("Spring")))
						continue;

					firstSplit = s.Split(' ');

					var hasAllCapLetters = firstSplit.FirstOrDefault(cap => cap.ToUpper() == cap);

					if (hasAllCapLetters == null)
						continue;

					if (firstSplit.Length <= 2)
						continue;
					
					// Need a year string in there.
					year = firstSplit.FirstOrDefault(p => Regex.IsMatch(p, @"\d{4}"));
					if (year == null)
						continue;

					// Almost there.
					if (float.TryParse(firstSplit[0], out units))
						continue;
				
					try
					{
						// Append the next string if it contains the GE.
						if (i + 1 < uncleaned.Count && uncleaned[i + 1].Contains("GE"))
							toAdd += $"|{uncleaned[i + 1]}";
					}
					catch (Exception)
					{

					}
					cleaned.Add(toAdd);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
			
			if (GeneralEdDictionary == null)
				GeneralEdDictionary = new Dictionary<string, string>();
			
			List<string> geTokens = new List<string>()
			{
				"GE A1:", "GE A2:", "GE A3:", "GE B1:","GE B2:","GE B3:",
				"GE C1:", "GE C2:", "GE C3:", "GE D1:","GE D2:","GE D3:",
				"GE E1:"
			};
			
			string key, result;
			string[] splitForGE;
			for(int i = 0; i < geTokens.Count; i++)
			{
				key = geTokens[i];

				result = cleaned.FirstOrDefault(str => str.Contains(key));

				if(result != null && !GeneralEdDictionary.ContainsKey(key))
				{
					splitForGE = result.Split('|');

					if (splitForGE != null && splitForGE.Length == 2)
					{
						key = splitForGE[0].Trim();
						result = splitForGE[1].Trim();
					}

					if(!GeneralEdDictionary.ContainsKey(key))
						GeneralEdDictionary.Add(key, result);
				}
			}
			
		}
		
		public bool WriteText()
		{
			//Create a output doc.
			Document output = new Document(_rect);

			//Open FileStream & Writer
			FileStream fs = new FileStream(FileLocation.Replace(".pdf","") + "_copy.pdf", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

			PdfWriter writer = PdfWriter.GetInstance(output, fs);

			output.Open();

			PdfContentByte contentWriter = writer.DirectContent;
			
			BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

			contentWriter.SetFontAndSize(baseFont, 50);

			contentWriter.BeginText();
			string txt = "I WROTE TO THIS PDF!";
			contentWriter.ShowTextAligned(0, txt, 10, 500, 0);
			contentWriter.EndText();

			PdfImportedPage page = writer.GetImportedPage(this, 1);
			contentWriter.AddTemplate(page,0,0);

			output.Close();
			fs.Close();
			writer.Close();
			
			FileLocation = FileLocation.Replace(".pdf", "") + "_copy.pdf";
			_copiedFileName = FileLocation;
			Init();

			return true;
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

	}
}
