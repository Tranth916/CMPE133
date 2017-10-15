using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using System.Diagnostics;
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
        public static readonly string SplitDesignationStatus = "Designation Status";
        public static readonly string SplitVertBarCourseComplete = "| First 1 of 1 Last";
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

            foreach (string s in geCoursesCompleted)
            {
                ExtractCourseInfoAndGrade(s);
            }

            foreach (string t in majorCoursesCompleted)
                Debug.WriteLine(t);


       
        }

        private static bool ExtractCourseInfoAndGrade(string str)
        {
            //COMM 20 Public Speaking 3.00 Fall 2016 B GE A1: Oral Communication
            List<string> innerSplit = str.Split(' ').ToList();
            innerSplit.Add("#");

            int placeHolder = innerSplit.Count - 1;
            int seasonIndex, yearIndex, courseAbbrIndex, courseTitleIndex, gradeIndex,
                courseNumIndex, unitIndex;

            Dictionary<string, string> courses = new Dictionary<string, string>()
            {
                { "CourseAbbreviation", "" },
                { "CourseNumber", "" },
                { "Season", "" },
                { "Year", "" },
                { "Grade", "" },
                { "Unit", "" },
                {"CourseTitle","" }
                
            };

            var iSeason = from s in innerSplit
                          where s.Length > 0 && HasSemesterSeason(s)
                          let season = innerSplit.IndexOf(s)
                          select season;

            seasonIndex = iSeason != null ? iSeason.FirstOrDefault() : placeHolder;
            courses["Season"] = innerSplit[seasonIndex];
            innerSplit[seasonIndex] = "";

            var iYear = from y in innerSplit
                        where y.Length > 0 && HasYear(y)
                        let year = innerSplit.IndexOf(y)
                        where year > seasonIndex
                        select year;

            yearIndex = iYear != null ? iYear.First() : placeHolder;
            courses["Year"] = innerSplit[yearIndex];
            innerSplit[yearIndex] = "";
            
            var iGrade = from g in innerSplit
                         where g.Length > 0 && HasGrade(g)
                         let grade = innerSplit.IndexOf(g)
                         where grade > yearIndex
                         select grade;

            gradeIndex = iGrade != null ? iGrade.FirstOrDefault() : placeHolder;
            courses["Grade"] = innerSplit[gradeIndex];
            innerSplit[gradeIndex] = "";

            var iCourseAbbrv = from ca in innerSplit
                               where ca.Length > 0
                               let caIndex = innerSplit.IndexOf(ca)
                               let caWord = ca.ToUpper()
                               where caIndex < seasonIndex && 
                                               caWord == ca && 
                                               Char.IsLetter(ca[0]) &&
                                               Char.IsLetter(ca[ca.Length - 1])
                               select caIndex;

            courseAbbrIndex = iCourseAbbrv != null ? iCourseAbbrv.FirstOrDefault() : placeHolder;
            courses["CourseAbbreviation"] = innerSplit[courseAbbrIndex];
            innerSplit[courseAbbrIndex] = "";


            var iCourseUnit = from cn in innerSplit
                             where cn.Length > 0 && Regex.IsMatch(cn, RegexPatternClassUnitFloat)
                             let num = innerSplit.IndexOf(cn)
                             where num > courseAbbrIndex
                             select num;

            unitIndex = iCourseUnit != null ? iCourseUnit.FirstOrDefault() : placeHolder;
            courses["Unit"] = innerSplit[unitIndex];
            innerSplit[unitIndex] = "";

            var iCourseNumber = from cn in innerSplit
                               where cn.Length > 0
                               let cap = cn.ToUpper()
                               let cnIndex = innerSplit.IndexOf(cn)
                               where cn == cap && HasCourseNumber(cn)
                               select cnIndex;

            courseNumIndex = iCourseNumber != null ? iCourseNumber.FirstOrDefault() : placeHolder;
            courses["CourseNumber"] = innerSplit[courseNumIndex];
            innerSplit[courseNumIndex] = "";

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
                        courses["CourseTitle"] = possibleTitle;
                    }
                }
                catch (Exception) { }
            }

            return true;
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

        public static string GetCourseTitle(string str)
        {
            string ret = str;


            return ret;
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
		private static bool HasGrade(string gg)
		{
			if (gg.Length == 2 && !(gg.EndsWith("+") || gg.EndsWith("-")))
				return false;

            if ((gg[0] == 'A' || gg[0] == 'B' || gg[0] == 'C' || gg[0] == 'D' || gg[0] == 'F'))
                return true;
            // some grades are given as units
            else if (int.TryParse(gg, out var i))
                return true;
            else
                return false;
		}
        private static bool HasCourseUnit(string cu)
        {
            return Regex.IsMatch(cu, RegexPatternClassUnitFloat);
        }
	}
}
