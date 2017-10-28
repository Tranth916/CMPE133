using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Parser
{
	public class PDFFileWriter
	{
		public static bool WriteDataToPdf(string path, PdfReader template = null, Dictionary<string, float[]> dataWithPos = null)
		{
			if (!File.Exists(path))
				return false;

			string originalPath = path;
			string orgRootPath = Path.GetDirectoryName(path);
			string orgFileName = Path.GetFileNameWithoutExtension(path);
			string orgExtension = Path.GetExtension(path);

			// make a copy of the file.
			string copyOfFile = Path.Combine(orgRootPath, orgFileName + "_copy" + orgExtension);

			try
			{
				if (File.Exists(copyOfFile))
					File.Delete(copyOfFile);
			}
			catch(Exception)
			{
				
			}
			
			File.Copy(path, copyOfFile);

			path = copyOfFile;

			// Need the size of the pages.
			PdfReader reader = File.Exists(path) ? new PdfReader(path) : template;

			if (reader == null)
				return false;

			var _rect = new Rectangle(reader.GetPageSize(1));
			reader.Close();

			int rightOfPage = 612;
			int centerX = rightOfPage / 2;

			//Create a output doc.
			Document output = new Document(_rect);
			FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
			PdfWriter writer = PdfWriter.GetInstance(output, fs);

			output.Open();

			PdfContentByte contentWriter = writer.DirectContent;

			if (template != null)
			{
				PdfImportedPage page = writer.GetImportedPage(template, 1);
				//contentWriter.AddTemplate(page, 0, 0);
			}

			var docFonts = BaseFont.GetDocumentFonts(reader);

			var fontStrs = docFonts.Select(ss => ss.FirstOrDefault()).Cast<String>();

			int smallestFont = int.MaxValue;

			PRIndirectReference prIndrectForFont = null;

			var quer = from df in docFonts
					   let s = df.FirstOrDefault().ToString()
					   where !s.Contains("Bold") && !s.Contains("Italic")
					   select df.LastOrDefault() as PRIndirectReference;

			if (quer != null && quer.Count() > 0)
				prIndrectForFont = quer.FirstOrDefault();

			var smallestFontQuery = from sf in quer
									select sf.Type;

			smallestFont = smallestFontQuery.Min();

			BaseFont baseFont;
			baseFont = prIndrectForFont != null ? BaseFont.CreateFont(prIndrectForFont) :
										BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

			contentWriter.SetFontAndSize(baseFont, 8);

			Dictionary<float, float> writtenPos = new Dictionary<float, float>();
			if (dataWithPos != null)
			{
				string str;
				float[] cords;
				float x, y;
				int completedCount = 0;
				int currentPage = 1;

				while (completedCount < dataWithPos.Count)
				{
					//Get all the entries that needs to be written on this current page.
					var entriesOnCurrentPage = dataWithPos.Where(ss => ss.Value.LastOrDefault() == currentPage).ToList();

					//Loop through each entry and begin the write.
					foreach (var entry in entriesOnCurrentPage)
					{
						completedCount++;

						str = entry.Key.Trim();
						cords = entry.Value;

						if (str == "")
							continue;

						x = cords[0];
						y = cords[1];

						contentWriter.BeginText();
						contentWriter.ShowTextAligned(0, " X ", x, y, 0);
						contentWriter.EndText();
					}

					currentPage++;

					//Append the next page, but check the count first.
					if (completedCount == dataWithPos.Count)
						break;
					else
					{
						output.NewPage();
						contentWriter.SetFontAndSize(baseFont, 8);
					}
				}
			}

			output.Close();
			fs.Close();
			writer.Close();

			Process.Start("explorer.exe", Path.GetDirectoryName(path));
			return true;
		}




		private static Dictionary<int, KeyValuePair<string,float[]>> OrderDictionaryForWrite(Dictionary<string,float[]> dict)
		{
			Dictionary<int, KeyValuePair<string, float[]>> ret = new Dictionary<int, KeyValuePair<string, float[]>>();

			var ordered = dict.OrderBy(kk => kk.Value.LastOrDefault()).ToList();

			for (int i = 0; i < ordered.Count; i++)
			{
				ret.Add(i, ordered[i]);
			}

			return ret;
		}
	}

}
