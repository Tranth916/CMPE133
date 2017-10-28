using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Parser
{

	public class MajorFormExtractor
	{
		private string _filePath;
		private PdfReader _reader;
		private Thread[] _threads = new Thread[2];

		public MajorFormExtractor(string filePath)
		{
			_filePath = filePath;
			LoadReader(_filePath);
		}

		public void CreateThreadsExtractors()
		{



		}

		private void LoadReader(string filePath)
		{
			try
			{
				if (File.Exists(filePath) && filePath.EndsWith("pdf", StringComparison.CurrentCultureIgnoreCase))
				{
					_reader = new PdfReader(filePath);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		public Dictionary<string, float[]> ExtractStringsByRectangle()
		{
			return ExtractStringsByRectangle("");
		}

		public Dictionary<string, float[]> ExtractStringsByRectangle(string filePath, int dY = 10, bool dualThread = false, int splitHalf = 1)
		{
			if (_reader == null && filePath != "")
				LoadReader(filePath);

			if (_reader == null || _reader.NumberOfPages == 0)
				return null;

			int deltaY = dY;
			int numOfPages = _reader.NumberOfPages;

			var pageRectSize = _reader.GetPageSize(1);
			float pageHeight = pageRectSize.Height;
			float pageWidth = pageRectSize.Width;

			int top = (int)pageHeight;
			int bot = (int)(top - deltaY);

			var data = new Dictionary<string, float[]>();

			var rect = !dualThread ? new Rectangle(0, 0, pageWidth, pageHeight) : new Rectangle(0, 0, pageWidth / 2, pageHeight);

			// Declare variables before going into the loop.
			RegionTextRenderFilter renderFilter;
			LocationTextExtractionStrategy locStrat;
			FilteredTextRenderListener textRect;
			string result;
			int yPtr;

			// The PdfReader pages[] starts at 1.
			for (int currentPage = 1; currentPage <= numOfPages; currentPage++)
			{
				yPtr = (int)pageHeight;

				while (yPtr >= deltaY && (yPtr - deltaY) >= 0)
				{
					renderFilter = new RegionTextRenderFilter(rect);
					locStrat = new LocationTextExtractionStrategy();
					textRect = new FilteredTextRenderListener(locStrat, renderFilter);
					result = PdfTextExtractor.GetTextFromPage(_reader, currentPage, textRect);

					if (result != null && result.Length > 0)
					{
						while (result.Contains("  "))
						{
							result = result.Replace("  ", "");
						}

						if (!result.Contains("\n"))
						{
							if (!data.ContainsKey(result))
								data.Add(result, new float[] { rect.Left, rect.Bottom });
						}
						else
						{
							var list = result.Split('\n').ToList();

							foreach (var s in list)
							{
								if (!data.ContainsKey(s))
								{
									// data.Add(s, new float[] { rect.Top, rect.Bottom });
									float xPos = rect.Left;
									float yPos = rect.Bottom;

									data.Add(s, new float[] { xPos, yPos });
								}
							}
						}
					}

					yPtr -= deltaY;
					rect.Top = yPtr;
					rect.Bottom = rect.Top - deltaY;
				}
			}

			_reader.Close();
			return data;
		}
		public void CloseReader()
		{
			if (_reader != null)
				_reader.Close();
		}
	}
}
