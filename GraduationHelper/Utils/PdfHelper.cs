using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using System.Diagnostics;
using GraduationHelper.Models;
using System.IO;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Threading;

namespace GraduationHelper.Utils
{
	public enum ImportedPDF
	{
		MyProgressTranscript = 0,
		CourseHistoryTranscript = 1,
		UnofficalTranscript = 2,
		MissionCollegeTranscript = 3,
		MajorForm = 4,
	};
	public enum CourseDictionaryContent
	{
		CourseAbbreviation,
		CourseNumber,
		CourseTitle,
		Season,
		Unit,
		Year,
		Grade,
		ETC,
	}
	public class PdfHelper
	{
		private static PdfHelper _instance;
		public static PdfHelper Instance
		{
			private set
			{
				_instance = value;
			}
			get
			{
				if (_instance == null)
					_instance = new PdfHelper();

				return _instance;
			}
		}
		private PdfHelper()
		{

		}
		
		#region Public Members
		public static readonly char[] SplitPatternVertBar = new char[] { '|' };
        public static readonly char[] SplitPatternRCarriage = new char[] { '\r' };
        public static readonly char[] SplitPatternNewLine = new char[] { '\n' };
        public static readonly string SplitPatternCarriage = "\r";
        public static readonly string RegexPatternCourseNum = @"\d{1,}[AB]$";
        public static readonly string RegexPatternGradePlusMinus = "((?:[a-z][a-z0-9_]*))";
        public static readonly string RegexPatternYEAR = @"\d{4}$";
        public static readonly string RegexPatternMMDDYEAR = "((?:[0]?[1-9]|[1][012])[-:\\/.](?:(?:[0-2]?\\d{1})|(?:[3][01]{1}))[-:\\/.](?:(?:[1]{1}\\d{1}\\d{1}\\d{1})|(?:[2]{1}\\d{3})))(?![\\d])";
        public static readonly string RegexPatternClassUnitFloat = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";
        public static readonly string RegexPatternDigits = "";
        public static readonly string BeginFilterHttp = "https:";
		public static readonly string SplitDesignationStatus = "Designation Status";
		public static readonly string SplitVertBarCourseComplete = "| First 1 of 1 Last";
		public static readonly List<string> LetterGradesList = new List<string>()
		{
			"A", "B", "C", "D", "F",
			"A+", "B+", "C+", "D+", "F",
			"A-", "B-", "C-", "D-", "F",
			"P", "NP",
			"W"
		};
		public static readonly List<string> FiltersMyProgressTranscript = new List<string>()
        {
            "My Academic Requirements",
            "Course Descr iption Units When Grade Requi rement",
            "Units When Grade Status",
            "The following courses may",
            "be used to satisfy this requi rement:",
            "Designation Status",
            "Course Descr iption",
            "Search Plan Enroll My Academics go to",
            "The following courses were used to satisfy this requi rement:",
            "Units When Grade Notes Requi rement",
            "   ",
            "  ",
        };
		#endregion

		public static object ParsePDF(ImportedPDF file, StringBuilder data = null, string fileName = null)
        {
            switch (file)
            {
                case ImportedPDF.MyProgressTranscript:
					if(data != null)
					{
						Dictionary<string, Course> retDictionary = new Dictionary<string, Course>();
						ParseProgressTranscript(data, ref retDictionary);
						return retDictionary;
					}
					break;
                case ImportedPDF.CourseHistoryTranscript:

                    break;

                case ImportedPDF.MissionCollegeTranscript:

                    break;

                case ImportedPDF.UnofficalTranscript:

                    break;

				case ImportedPDF.MajorForm:
					if (fileName != null)
						return ParseMajorFormCords(fileName);				
					break;
            }
			return null;
        }

		public static Dictionary<string, float[]> ParseMajorFormCords(string fileName)
		{
			try
			{
				MajorFormExtractor mfe = new MajorFormExtractor(fileName);
				return mfe.ExtractStringsByRectangle();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception thrown while parsing major form." + ex.StackTrace);
			}
		
			return null;
		}
        public static void ParseProgressTranscript(StringBuilder textData, ref Dictionary<string,Course> dict)
        {
            StringBuilder sb = new StringBuilder();

            List<string> lines = textData
                                .ToString()
                                .Split(SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries)
                                .ToList();

            lines.RemoveAll(http => http.StartsWith(BeginFilterHttp));

            // Filter the text first.
            for (int i = 0; i < lines.Count; i++)
            {
                // Remove all dates. mm/dd/yyyy
                lines[i] = Regex.Replace(lines[i], RegexPatternMMDDYEAR, "").Trim();

                foreach (string filter in FiltersMyProgressTranscript)
                {
                    if (lines[i].Contains(filter))
                        lines[i] = lines[i].Replace(filter, "");
                }
            }

            // Remove all empty strings
            lines.RemoveAll(emp => emp.Length == 0 || emp == "");

            // Create lines by split with '| 1 of 1'
            foreach (string line in lines)
            {
                // Try to prevent data lost.
                SplitByVerticalBarSep(line, ref sb);
            }

            lines = sb.ToString().Split(SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            sb.Clear();

            foreach (string line in lines)
            {
                SpltByDesignationStatus(line, ref sb);
            }

            lines = sb.ToString().Split(SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

            var vertBarsToReplaceWithNewLine = lines
                                                .Where(ss => ss.IndexOf("|") > 0)
                                                .Select(ii => lines.IndexOf(ii));
            sb.Clear();

            for (int i = 0; i < lines.Count; i++)
            {
                if (vertBarsToReplaceWithNewLine.Contains(i))
                {
                    int replacementIndex = lines[i].IndexOf("|");

                    sb.AppendLine(lines[i].Substring(0, replacementIndex).Trim());
                    sb.AppendLine(lines[i].Substring(replacementIndex).Trim());
                }
                else
                    sb.AppendLine(lines[i].Trim());
            }

            lines = sb.ToString().Split(SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

            var geCoursesCompleted = StringBetweenLines(ref lines, "Designation Status", "| First");
            var majorCoursesCompleted = StringBeforeLine(ref lines, "| First 1 of");

			Course cc;
            foreach (string s in geCoursesCompleted)
            {
				try
				{
					cc = ExtractCourseInfoAndGrade(s);

					if (!dict.ContainsKey(cc.EntryKey))
						dict.Add(cc.EntryKey, cc);
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
				}
            }
            foreach (string t in majorCoursesCompleted)
			{
				try
				{
					cc = ExtractCourseInfoAndGrade(t);

					if(!dict.ContainsKey(cc.EntryKey))
						dict.Add(cc.EntryKey, cc);
				}
				catch(Exception e)
				{
					Console.WriteLine(e.StackTrace);
				}
			}
			
        }
		#region Progress Transcript Parser Methods
		private static Course ExtractCourseInfoAndGrade(string strr)
        {
			string str = "";

			if (strr.Contains("Complete") && strr.Contains("(") && strr.Contains(")"))
				str = strr.Substring(strr.LastIndexOf(")") + 1).Trim();
			else
				str = strr;

			str = str.Replace("SFTE ", "");
			
			List<string> innerSplit = str.Split(' ').ToList();

            innerSplit.Add("#");

            int placeHolder = innerSplit.Count - 1,
				seasonIndex, yearIndex, 
				courseAbbrIndex, gradeIndex,
                courseNumIndex, unitIndex;

            Dictionary<string, string> courseInformation = new Dictionary<string, string>()
            {
                { CourseDictionaryContent.CourseAbbreviation.ToString(), "" },
                { CourseDictionaryContent.CourseNumber.ToString(), "" },        
				{ CourseDictionaryContent.CourseTitle.ToString(), "" },
				{ CourseDictionaryContent.Season.ToString(), "" },
				{ CourseDictionaryContent.Year.ToString(), "" },
                { CourseDictionaryContent.Grade.ToString(), "" },
                { CourseDictionaryContent.Unit.ToString(), "" },
				{ CourseDictionaryContent.ETC.ToString(), "" }
            };

            var iSeason = from s in innerSplit						  
						  where s.Length > 0 && HasSemesterSeason(s)
                          select innerSplit.IndexOf(s);
			
            seasonIndex = (iSeason != null && iSeason.Count() > 0) ? iSeason.First() : placeHolder;

			courseInformation[CourseDictionaryContent.Season.ToString()] = innerSplit[seasonIndex];
            innerSplit[seasonIndex] = seasonIndex < placeHolder ? 
									  "" : courseInformation[CourseDictionaryContent.Season.ToString()];

            var iYear = from y in innerSplit
                        where y.Length > 0 && HasYear(y)
                        let year = innerSplit.IndexOf(y)
                        where year > seasonIndex
                        select year;

            yearIndex = (iYear != null && iYear.Count() > 0) ? iYear.First() : placeHolder;
            courseInformation[CourseDictionaryContent.Year.ToString()] = innerSplit[yearIndex];
            innerSplit[yearIndex] = yearIndex < placeHolder ? 
									"" : courseInformation[CourseDictionaryContent.Year.ToString()];
            
            var iGrade = from g in innerSplit
						 let grade = innerSplit.IndexOf(g)
						 where grade > yearIndex && g.Length > 0 && HasGrade(g)
                         select grade;

            gradeIndex = (iGrade != null && iGrade.Count() > 0) ? iGrade.First() : placeHolder;
            courseInformation[CourseDictionaryContent.Grade.ToString()] = innerSplit[gradeIndex];
            innerSplit[gradeIndex] = gradeIndex < placeHolder ? 
									 "" : courseInformation[CourseDictionaryContent.Grade.ToString()];

            var iCourseAbbrv = from ca in innerSplit
							   let caIndex = innerSplit.IndexOf(ca)
							   let caWord = ca.ToUpper()
                               where  ca.Length > 0 &&
									  caIndex < seasonIndex && 
                                      caWord == ca && 
                                      Char.IsLetter(ca[0]) &&
                                      Char.IsLetter(ca[ca.Length - 1])
                               select caIndex;

            courseAbbrIndex = (iCourseAbbrv != null && iCourseAbbrv.Count() > 0) ? iCourseAbbrv.First() : placeHolder;
            courseInformation[CourseDictionaryContent.CourseAbbreviation.ToString()] = innerSplit[courseAbbrIndex];
            innerSplit[courseAbbrIndex] = courseAbbrIndex < placeHolder ? 
										  "" : courseInformation[CourseDictionaryContent.CourseAbbreviation.ToString()];
			
            var iCourseUnit = from cn in innerSplit
                             where cn.Length > 0 && Regex.IsMatch(cn, RegexPatternClassUnitFloat)
                             let num = innerSplit.IndexOf(cn)
                             where num > courseAbbrIndex
                             select num;

            unitIndex = (iCourseUnit != null && iCourseUnit.Count() > 0) ? iCourseUnit.First() : placeHolder;
            courseInformation[CourseDictionaryContent.Unit.ToString()] = innerSplit[unitIndex];
            innerSplit[unitIndex] = unitIndex < placeHolder ?
									"" : courseInformation[CourseDictionaryContent.Unit.ToString()];

            var iCourseNumber = from cn in innerSplit
                               where cn.Length > 0
                               let cap = cn.ToUpper()
                               let cnIndex = innerSplit.IndexOf(cn)
                               where cn == cap && HasCourseNumber(cn)
                               select cnIndex;

            courseNumIndex = (iCourseNumber != null && iCourseNumber.Count() > 0) ? iCourseNumber.First() : placeHolder;
            courseInformation[CourseDictionaryContent.CourseNumber.ToString()] = innerSplit[courseNumIndex];
            innerSplit[courseNumIndex] = courseNumIndex < placeHolder ? 
										"" : courseInformation[CourseDictionaryContent.CourseNumber.ToString()];

            string possibleTitle = "";
            for(int i = courseNumIndex; i < unitIndex; i++)
            {
                try
                {
                    string word = $"{innerSplit[i]}".Trim();
                    if(HasCourseTitle(word))
                    {
                        innerSplit[i] = "";
                        possibleTitle = $"{possibleTitle} {word}";
                        courseInformation[CourseDictionaryContent.CourseTitle.ToString()] = possibleTitle;
                    }
                }
                catch (Exception) { }
            }

			// Append the rest of the lines to the ETC
			for(int i = 0; i < innerSplit.Count; i++)
			{
				if(innerSplit[i] != "")
				{
					courseInformation["ETC"] = innerSplit[i] + " ";
					innerSplit[i] = "";
				}
			}
			
			return new Course(courseInformation);
        } 		
        private static List<string> StringBeforeLine(ref List<string> lines, string after)
        {
            List<string> list = new List<string>();

            for(int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].StartsWith(after) && i - 1 >= 0)
                {
                    list.Add(lines[i - 1]);
                    lines[i] = "";
                    lines[i - 1] = "";
                }
            }
            list.Reverse();
            lines.RemoveAll(s => s.Length == 0 || s == "");
            return list;
        }
        private static List<string> StringBetweenLines(ref List<string> lines, string before, string after)
        {
            List<string> betweens = new List<string>();
            int stopIndex, 
                limit, 
                maxLength = 2 << 7;

            for(int i = 0; i < lines.Count; i++)
            {
                if(lines[i].Trim() == before)
                {
                    lines[i] = "";
                    stopIndex = i + 1;
                    limit = 10;

                    while(stopIndex < lines.Count && limit > 0)
                    {
                        if (lines[stopIndex].Length > maxLength)
                        {
                            //TODO handle when line is too long.
                        }
                        else if (!lines[stopIndex].Trim().StartsWith(after))
                        {
                            betweens.Add(lines[stopIndex]);
                            lines[stopIndex] = "";
                        }
                        else
                        {
                            i = stopIndex;
                            lines[stopIndex] = "";
                            break;
                        }
                        stopIndex++;
                        limit--;
                    }
                }
            }
            
            lines.RemoveAll(s => s.Length == 0 || s.Trim() == "");

            return betweens;
        }
        private static bool SpltByDesignationStatus(string str, ref StringBuilder sb)
        {
            if (str == null || str.Length == 0)
                return false;

            int indexOfDesStatus = str.IndexOf(SplitDesignationStatus, StringComparison.CurrentCultureIgnoreCase);
           
            if (indexOfDesStatus < 0)
            {
                if (HasBarSplitSeperator(str))
                {
                    int subStrBeforeLast = str.IndexOf("Last") + "Last".Length;
                    string beforeLast = str.Substring(0, subStrBeforeLast).Trim();
                    string afterLast = str.Substring(subStrBeforeLast + 1).Trim();

                    if (beforeLast.Length > 0)
                        sb.AppendLine(beforeLast);
                    if(afterLast.Length > 0)
                        sb.AppendLine(afterLast);
                }
                else
                    sb.Append(str.Trim().Replace("   "," ").Replace("  "," ") + " ");
            }
            else if(indexOfDesStatus >= 0)
            {
                List<int> indexes = new List<int>();
                
                while(indexOfDesStatus > 0)
                {
                    if(!indexes.Contains(indexOfDesStatus))
                        indexes.Add(indexOfDesStatus);

                    indexOfDesStatus = str.IndexOf(SplitDesignationStatus, indexOfDesStatus + 1);
                }
          
                if(indexes.Count > 1)
                {
                    string firstHalf = "";
                    int currentInt, nextInt, lengthOfSubString;

                    for(int i = 0; i < indexes.Count; i++)
                    {
                        currentInt = indexes[i];
                        if ((i+1) < indexes.Count)
                        {
                            nextInt = indexes[i + 1];
                            lengthOfSubString = nextInt - currentInt;
                            firstHalf = str.Substring(currentInt, lengthOfSubString);
                        }
                        else
                        {
                            firstHalf = str.Substring(currentInt);
                        }

                        if(firstHalf.Length > 0)
                        {
                            sb.AppendLine(Environment.NewLine + firstHalf.Trim());
                        }
                    }
                }
                else
                {
                    indexOfDesStatus = indexes.FirstOrDefault();

                    string firstHalf = str.Substring(0, indexOfDesStatus).Trim();
                    string secondHalf = str.Substring(indexOfDesStatus + SplitDesignationStatus.Length).Trim();

                    if (firstHalf.Length > 0)
                        sb.AppendLine(firstHalf);

                    if (secondHalf.Length > 0)
                    {
                        sb.AppendLine(SplitDesignationStatus);
                        sb.AppendLine(secondHalf);
                    }
                }
            }
            return true;
        }     
        private static bool SplitByVerticalBarSep(string str, ref StringBuilder sb)
        {
            if (str == null || str.Length == 0)
                return false;

            if(str.Contains("|") && str.Contains("of") && (str.Contains("1") || str.Contains("First")))
            {
                sb.AppendLine();
                sb.AppendLine(str);
                return true;
            }
            else
            {
                sb.Append(str.Replace("\n", "").Replace("   ","").Replace("  ","") + " ");
            }

            return false;
        }  
		private static bool HasCourseTitle(string str)
        {
            if (str.Length == 0 || str == "")
                return false;

            if (str == "and" || str=="&" || str == "I" || str =="II" || str=="III")
                return true;

            bool firstLetterCap = str[0].ToString().ToUpper() == str[0].ToString();

            bool lastLetterLowercase = str[str.Length - 1].ToString().ToLower() == str[str.Length - 1].ToString();
            
            return firstLetterCap && lastLetterLowercase;
        }
        private static bool HasCourseNumber(string sn)
        {
            if (sn == null || sn.Length == 0 || sn.Length > 10)
                return false;

            //195A
            int val = 0;
            if (int.TryParse(sn, out val))
                return true;

            bool hasCapsLetter = sn.ToUpper() == sn;
            int numsCount = 0;
            int charsCount = 0;

            foreach(char c in sn.ToCharArray())
            {
                if (Char.IsDigit(c))
                    numsCount++;
                else if (Char.IsLetter(c))
                    charsCount++;
            }
           
            return hasCapsLetter && charsCount > 0 && numsCount > 0;
        }
		private static bool HasBarSplitSeperator(string str)
		{
			return str.Contains("|") && str.Contains("of") && (str.Contains("1") || str.Contains("First"));
		}
        private static bool HasSemesterSeason(string ss)
        {
            return ss.StartsWith("Fall", StringComparison.CurrentCultureIgnoreCase) ||
                   ss.StartsWith("Summer", StringComparison.CurrentCultureIgnoreCase) ||
                   ss.StartsWith("Spring", StringComparison.CurrentCultureIgnoreCase);
        }
        private static bool HasYear(string yy)
        {
            return Regex.IsMatch(yy, RegexPatternYEAR);
        }
		private static bool HasCourseUnit(string cu)
		{
			return Regex.IsMatch(cu, RegexPatternClassUnitFloat);
		}
		#endregion
		public static string GetCourseTitle(string str)
		{
			string ret = str;


			return ret;
		}
		public static bool HasGrade(string gg)
		{
			if (gg == "" || gg.Length == 0 || gg == "#")
				return false;

			//if (gg.Length == 2 && !(gg.EndsWith("+") || gg.EndsWith("-")))
			//	return false;

			string grade = String.Format(gg[0].ToString()).ToUpper();

			foreach(string s in LetterGradesList)
			{
				if (s == gg)
					return true;
			}




			if (Char.IsLetter(gg[0]) && LetterGradesList.Contains(grade) && (gg.EndsWith("+") || gg.EndsWith("-")))
				return true;
			// some grades are given as units
			else if (int.TryParse(gg, out var i) && i < 1000)
			{
				return true;
			}
			else
				return false;
		}
	}

	public class PDFFileWriter
	{
		public static bool WriteDataToPdf(string path, PdfReader template = null, Dictionary<string,float[]> dataWithPos = null)
		{
			if (!File.Exists(path))
				return false;

			string originalPath = path;
			string orgRootPath = System.IO.Path.GetDirectoryName(path);
			string orgFileName = System.IO.Path.GetFileNameWithoutExtension(path);
			string orgExtension = System.IO.Path.GetExtension(path);
			
			// make a copy of the file.
			string copyOfFile = System.IO.Path.Combine(orgRootPath, orgFileName + "_copy" + orgExtension);

			if (File.Exists(copyOfFile))
				File.Delete(copyOfFile);

			File.Copy(path, copyOfFile);
			
			path = copyOfFile;

			// Need the size of the pages.
			PdfReader reader = File.Exists(path) ? new PdfReader(path) : template;

			if (reader == null)
				return false;
			
			var _rect = new Rectangle(reader.GetPageSize(1));
			reader.Close();

			int fontSize = 15;
			int topOfPage = 792;
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
				contentWriter.AddTemplate(page, 0, 0);
			}

			var docFonts = BaseFont.GetDocumentFonts(reader);

			var fontStrs = docFonts.Select(ss=> ss.FirstOrDefault()).Cast<String>();

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
	
			if( dataWithPos != null)
			{
				string str;
				float[] cords;
				float x, y;
				
				foreach(var entry in dataWithPos)
				{
					if (entry.Key == "")
						continue;

					str = entry.Key;
					cords = entry.Value;
					x = cords[0];
					y = cords[1];



					if (writtenPos.ContainsKey(x) && writtenPos[x] == y)
						continue;



					contentWriter.BeginText();
					contentWriter.ShowTextAligned(0, str, x, y, 0);
					contentWriter.EndText();	
				}
			}
			
			output.Close();
			fs.Close();
			writer.Close();

			Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(path));
			return true;
		}
	}

	class MajorFormExtractor
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
				if(File.Exists(filePath) && filePath.EndsWith("pdf",StringComparison.CurrentCultureIgnoreCase))
				{
					_reader = new PdfReader(filePath);
				}
			}
			catch(Exception ex)
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

			// Starting cordinates
			float lowerLeftX_MovingRect = 0;
			float lowerLeftY_MovingRect = pageHeight - deltaY;
			float upperRightX_MovingRect = pageWidth;
			float upperRightY_MovingRect = pageHeight;

			var data = new Dictionary<string, float[]>();
			
			var movingYY_Rect = new Rectangle(lowerLeftX_MovingRect, lowerLeftY_MovingRect, upperRightX_MovingRect, upperRightY_MovingRect);

			// Declare variables before going into the loop.
			RegionTextRenderFilter renderFilter;
			LocationTextExtractionStrategy locStrat;
			FilteredTextRenderListener textRect;
			string result;
			int yPtr;

			// The PdfReader pages[] starts at 1.
			for(int currentPage = 1; currentPage <= numOfPages; currentPage++)
			{
				try
				{
					//Total height of this page.
					yPtr = (int)pageHeight;

					//Taverse downwards.
					while (yPtr >= deltaY && (yPtr - deltaY) >= 0)
					{
						locStrat = new LocationTextExtractionStrategy();
						renderFilter = new RegionTextRenderFilter(movingYY_Rect);
						textRect = new FilteredTextRenderListener(locStrat, renderFilter);
						result = PdfTextExtractor.GetTextFromPage(_reader, currentPage, textRect);

						//Hit a string value vertically.
						if (result != null && (result = result.Trim()).Length > 0)
						{
							if (!data.ContainsKey(result))
								data.Add(result, new float[] { movingYY_Rect.Left, movingYY_Rect.Bottom, currentPage });
						}
						yPtr -= deltaY;
						movingYY_Rect.Top = yPtr;
						movingYY_Rect.Bottom = movingYY_Rect.Top - deltaY;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
				}
			}
			// Got a list of the data by horizontal rectangles;

			Dictionary<string, float[]> dataByHorizontals = new Dictionary<string, float[]>();

			foreach (var entry in data.OrderBy(s => s.Value.LastOrDefault()))
			{



				locStrat = new LocationTextExtractionStrategy();
				renderFilter = new RegionTextRenderFilter(movingYY_Rect);				
				textRect = new FilteredTextRenderListener(locStrat, renderFilter);
				result = PdfTextExtractor.GetTextFromPage(_reader, (int)entry.Value.LastOrDefault(), textRect);
				
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
