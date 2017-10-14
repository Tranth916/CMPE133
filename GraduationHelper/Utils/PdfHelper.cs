using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
namespace GraduationHelper.Utils
{
	public enum ImportedPDF
	{
		MyProgressTranscript = 0,
		CourseHistoryTranscript = 1,
		UnofficalTranscript = 2,
		MissionCollegeTranscript = 3,
	};

	public static class PdfHelper
	{
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

		public static void ParsePDF(ImportedPDF file, StringBuilder data)
		{
			switch (file)
			{
				case ImportedPDF.MyProgressTranscript:
					ParseProgressTranscript(data);
				break;

				case ImportedPDF.CourseHistoryTranscript:

					break;

				case ImportedPDF.MissionCollegeTranscript:

					break;

				case ImportedPDF.UnofficalTranscript:

					break;
			}		
		}
		
		public static void ParseProgressTranscript(StringBuilder textData)
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
				if (HasBarSplitSeperator(line))
				{
					sb.AppendLine();

					sb.Append(line);

					sb.AppendLine();
				}
				else
				{
					sb.Append(line.Replace("\n", "") + " ");
				}
			}

			lines.Clear();
			System.Windows.Forms.MessageBox.Show(sb.ToString());
		}

		private static bool HasBarSplitSeperator(string str)
		{
			return str.Contains("|") && str.Contains("of") && (str.Contains("1") || str.Contains("First"));
		}
		private static bool HasCourseInfoAndGrade(string str)
		{
			

			return true;
		}

		private static bool IsGradeString(string gg)
		{
			if (gg.Length > 2)
				return false;

			else if (gg.Length == 2 && !(gg.EndsWith("+") || gg.EndsWith("-")))
				return false;

			if ((gg[0] == 'A' || gg[0] == 'B' || gg[0] == 'C' || gg[0] == 'D' || gg[0] == 'F'))
				return true;
			else
				return false;
		}
	}
}
