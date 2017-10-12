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
		const string dateMMDDYEARREGEX = "((?:[0]?[1-9]|[1][012])[-:\\/.](?:(?:[0-2]?\\d{1})|(?:[3][01]{1}))[-:\\/.](?:(?:[1]{1}\\d{1}\\d{1}\\d{1})|(?:[2]{1}\\d{3})))(?![\\d])";
		const string floatUnitRegex = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";

		public void ParsePdf()
		{
			if (PdfiumDoc.PageCount > 0)
				_rect = GetPageSizeWithRotation(1);

			string textFromPage;
			StringBuilder sb = new StringBuilder();
			StringBuilder cleanedString = new StringBuilder();

			char[] splitNewLine = new char[] { '\n' };
			
			//testing- remove this
			string filePath = $"C:\\Users\\Thao Tran\\Desktop\\FUCKTHISFILTER.TXT";
			StreamWriter sw = new StreamWriter(filePath);
			
			//First Pass
			for (int i = 1; i < PdfiumDoc.PageCount; i++)
			{
				try
				{
					textFromPage = PdfiumDoc.GetPdfText(i).Replace("\r", "");

					var textList = textFromPage.Split(splitNewLine, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 0; j < textList.Length; j++)
					{
						string s = textList[j]
										.Replace("My Academic Requirements", "")
										.Replace("Course Descr iption Units When Grade Requi rement", "")
										.Replace("Units When Grade Status", "")
										.Replace("The following courses may", "")
										.Replace("be used to satisfy this requi rement:", "")
										.Replace("Designation Status", "")
										.Replace("Course Descr iption", "")
										.Replace("Search Plan Enroll My Academics go to", "")
										.Replace("  ", "")
										.Replace("   ", "")
										.Trim();

						s = s
							.Replace("The following courses were used to satisfy this requi rement:", "")
							.Replace("Units When Grade Notes Requi rement", "")
							.Trim();

						if (s.StartsWith("|"))
							sb.Append(Environment.NewLine);

						if (PassFilter(s))
							sb.Append($"{s.Trim()} ");

						if (s.StartsWith("|"))
							sb.Append(Environment.NewLine);
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception throw while parsing pdf: " + ex.StackTrace);
				}
			}

			//Second pass rebuild all lines.
			string[] secondPass = sb.ToString().Split(splitNewLine);

			sb.Clear();
			string toAdd = "";
			int startSaveIndex;

			//Perserve everything after "Designation Status";
			for (int i = 0; i < secondPass.Length; i++)
			{
				toAdd = secondPass[i];

				startSaveIndex = secondPass[i].IndexOf("Designation Status");

				if (startSaveIndex > 0)
					toAdd = secondPass[i].Substring(startSaveIndex);

				sb.Append(toAdd);
			}

			string current, next;
			string dateRemoval;
			string[] thirdPass = sb.ToString().Split('\r');

			sb.Clear();
			for (int i = 0; i < thirdPass.Length; i++)
			{
				if ((i + 1) >= thirdPass.Length)
					break;

				current = thirdPass[i].Replace("Designation Status", "").Trim();
				next = thirdPass[i + 1].Trim();

				if (!current.Contains("1 of 1") && next.Contains("1 of 1"))
				{
					current = current.Replace("  ", " ");
					dateRemoval = current.Split(' ').FirstOrDefault(f => Regex.IsMatch(f, dateMMDDYEARREGEX));					

					if (dateRemoval != null)
						current = current.Replace(dateRemoval, "").Trim();

					if(current.Contains("Complete 1 course.") && current.Contains("(") && current.Contains(")"))
					{
						current = current.Substring(current.LastIndexOf(")") + 1).Trim();
					}
					
					sb.AppendLine(current);
				}
			}
			
			sw.Write(sb.ToString());
			sw.Close();
		}

		public bool PassFilter(string s)
		{
			if (s.Contains("https:"))
				return false;









			return true;
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
			catch (Exception ex)
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
			for (int i = 0; i < geTokens.Count; i++)
			{
				key = geTokens[i];

				result = cleaned.FirstOrDefault(str => str.Contains(key));

				if (result != null && !GeneralEdDictionary.ContainsKey(key))
				{
					splitForGE = result.Split('|');

					if (splitForGE != null && splitForGE.Length == 2)
					{
						key = splitForGE[0].Trim();
						result = splitForGE[1].Trim();
					}

					if (!GeneralEdDictionary.ContainsKey(key))
						GeneralEdDictionary.Add(key, result);
				}
			}

		}

		public bool WriteText()
		{
			//Create a output doc.
			Document output = new Document(_rect);

			//Open FileStream & Writer
			FileStream fs = new FileStream(FileLocation.Replace(".pdf", "") + "_copy.pdf", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

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
			contentWriter.AddTemplate(page, 0, 0);

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
