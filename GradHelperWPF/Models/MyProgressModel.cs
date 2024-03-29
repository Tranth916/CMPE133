﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GradHelperWPF.Models
{
    public enum ImportedPdf
    {
        MyProgressTranscript = 0,
        CourseHistoryTranscript = 1,
        UnofficalTranscript = 2,
        MissionCollegeTranscript = 3,
        MajorForm = 4
    }

    public enum CourseDictionaryContent
    {
        CourseAbbreviation,
        CourseNumber,
        CourseTitle,
        Season,
        Unit,
        Year,
        Grade,
        ETC
    }

    public class MyProgressModel
    {
        #region Public Members

        public static readonly char[] SplitPatternVertBar = {'|'};
        public static readonly char[] SplitPatternRCarriage = {'\r'};
        public static readonly char[] SplitPatternNewLine = {'\n'};
        public static readonly string SplitPatternCarriage = "\r";
        public static readonly string RegexPatternCourseNum = @"\d{1,}[AB]$";
        public static readonly string RegexPatternGradePlusMinus = "((?:[a-z][a-z0-9_]*))";
        public static readonly string RegexPatternYEAR = @"\d{4}$";

        public static readonly string RegexPatternMMDDYEAR =
                "((?:[0]?[1-9]|[1][012])[-:\\/.](?:(?:[0-2]?\\d{1})|(?:[3][01]{1}))[-:\\/.](?:(?:[1]{1}\\d{1}\\d{1}\\d{1})|(?:[2]{1}\\d{3})))(?![\\d])"
            ;

        public static readonly string RegexPatternClassUnitFloat = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";
        public static readonly string RegexPatternDigits = "";
        public static readonly string BeginFilterHttp = "https:";
        public static readonly string SplitDesignationStatus = "Designation Status";
        public static readonly string SplitVertBarCourseComplete = "| First 1 of 1 Last";

        public static readonly List<string> LetterGradesList = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "F",
            "A+",
            "B+",
            "C+",
            "D+",
            "F",
            "A-",
            "B-",
            "C-",
            "D-",
            "F",
            "P",
            "NP",
            "W"
        };

        public static readonly List<string> FiltersMyProgressTranscript = new List<string>
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
            "  "
        };

        #endregion Public Members

        public static object ParsePDF( ImportedPdf file, StringBuilder data = null, string fileName = null )
        {
            switch ( file )
            {
                case ImportedPdf.MyProgressTranscript:
                    if ( data != null )
                    {
                        //Dictionary<string, Course> retDictionary = new Dictionary<string, Course>();
                        //ParseProgressTranscript(data, ref retDictionary);
                        //return retDictionary;
                    }
                    break;

                case ImportedPdf.CourseHistoryTranscript:

                    break;

                case ImportedPdf.MissionCollegeTranscript:

                    break;

                case ImportedPdf.UnofficalTranscript:

                    break;

                case ImportedPdf.MajorForm:
                    break;
            }
            return null;
        }

        public static void ParseProgressTranscript( StringBuilder textData )
        {
            var sb = new StringBuilder();

            var lines = textData
                .ToString()
                .Split(SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            lines.RemoveAll( http => http.StartsWith( BeginFilterHttp ) );

            // Filter the text first.
            for ( var i = 0; i < lines.Count; i++ )
            {
                // Remove all dates. mm/dd/yyyy
                lines[i] = Regex.Replace( lines[i], RegexPatternMMDDYEAR, "" ).Trim( );

                foreach ( var filter in FiltersMyProgressTranscript )
                    if ( lines[i].Contains( filter ) )
                        lines[i] = lines[i].Replace( filter, "" );
            }

            // Remove all empty strings
            lines.RemoveAll( emp => emp.Length == 0 || emp == "" );

            // Create lines by split with '| 1 of 1'
            foreach ( var line in lines )
                // Try to prevent data lost.
                SplitByVerticalBarSep( line, ref sb );

            lines = sb.ToString( ).Split( SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries ).ToList( );
            sb.Clear( );

            foreach ( var line in lines )
                SpltByDesignationStatus( line, ref sb );

            lines = sb.ToString( ).Split( SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries ).ToList( );

            var vertBarsToReplaceWithNewLine = lines
                .Where(ss => ss.IndexOf("|") > 0)
                .Select(ii => lines.IndexOf(ii));
            sb.Clear( );

            for ( var i = 0; i < lines.Count; i++ )
                if ( vertBarsToReplaceWithNewLine.Contains( i ) )
                {
                    var replacementIndex = lines[i].IndexOf("|");

                    sb.AppendLine( lines[i].Substring( 0, replacementIndex ).Trim( ) );
                    sb.AppendLine( lines[i].Substring( replacementIndex ).Trim( ) );
                }
                else
                {
                    sb.AppendLine( lines[i].Trim( ) );
                }

            lines = sb.ToString( ).Split( SplitPatternNewLine, StringSplitOptions.RemoveEmptyEntries ).ToList( );

            var geCoursesCompleted = StringBetweenLines(ref lines, "Designation Status", "| First");
            var majorCoursesCompleted = StringBeforeLine(ref lines, "| First 1 of");
        }

        #region Progress Transcript Parser Methods

        private static object ExtractCourseInfoAndGrade( string strr )
        {
            var str = "";

            if ( strr.Contains( "Complete" ) && strr.Contains( "(" ) && strr.Contains( ")" ) )
                str = strr.Substring( strr.LastIndexOf( ")" ) + 1 ).Trim( );
            else
                str = strr;

            str = str.Replace( "SFTE ", "" );

            var innerSplit = str.Split(' ').ToList();

            innerSplit.Add( "#" );

            int placeHolder = innerSplit.Count - 1,
                seasonIndex,
                yearIndex,
                courseAbbrIndex,
                gradeIndex,
                courseNumIndex,
                unitIndex;

            var courseInformation = new Dictionary<string, string>
            {
                {CourseDictionaryContent.CourseAbbreviation.ToString(), ""},
                {CourseDictionaryContent.CourseNumber.ToString(), ""},
                {CourseDictionaryContent.CourseTitle.ToString(), ""},
                {CourseDictionaryContent.Season.ToString(), ""},
                {CourseDictionaryContent.Year.ToString(), ""},
                {CourseDictionaryContent.Grade.ToString(), ""},
                {CourseDictionaryContent.Unit.ToString(), ""},
                {CourseDictionaryContent.ETC.ToString(), ""}
            };

            var iSeason = from s in innerSplit
                          where s.Length > 0 && HasSemesterSeason(s)
                          select innerSplit.IndexOf(s);

            seasonIndex = iSeason != null && iSeason.Count( ) > 0 ? iSeason.First( ) : placeHolder;

            courseInformation[CourseDictionaryContent.Season.ToString( )] = innerSplit[seasonIndex];
            innerSplit[seasonIndex] = seasonIndex < placeHolder
                ? ""
                : courseInformation[CourseDictionaryContent.Season.ToString( )];

            var iYear = from y in innerSplit
                        where y.Length > 0 && HasYear(y)
                        let year = innerSplit.IndexOf(y)
                        where year > seasonIndex
                        select year;

            yearIndex = iYear != null && iYear.Count( ) > 0 ? iYear.First( ) : placeHolder;
            courseInformation[CourseDictionaryContent.Year.ToString( )] = innerSplit[yearIndex];
            innerSplit[yearIndex] =
                yearIndex < placeHolder ? "" : courseInformation[CourseDictionaryContent.Year.ToString( )];

            var iGrade = from g in innerSplit
                         let grade = innerSplit.IndexOf(g)
                         where grade > yearIndex && g.Length > 0 && HasGrade(g)
                         select grade;

            gradeIndex = iGrade != null && iGrade.Count( ) > 0 ? iGrade.First( ) : placeHolder;
            courseInformation[CourseDictionaryContent.Grade.ToString( )] = innerSplit[gradeIndex];
            innerSplit[gradeIndex] = gradeIndex < placeHolder
                ? ""
                : courseInformation[CourseDictionaryContent.Grade.ToString( )];

            var iCourseAbbrv = from ca in innerSplit
                               let caIndex = innerSplit.IndexOf(ca)
                               let caWord = ca.ToUpper()
                               where ca.Length > 0 &&
                      caIndex < seasonIndex &&
                      caWord == ca &&
                      char.IsLetter(ca[0]) &&
                      char.IsLetter(ca[ca.Length - 1])
                               select caIndex;

            courseAbbrIndex = iCourseAbbrv != null && iCourseAbbrv.Count( ) > 0 ? iCourseAbbrv.First( ) : placeHolder;
            courseInformation[CourseDictionaryContent.CourseAbbreviation.ToString( )] = innerSplit[courseAbbrIndex];
            innerSplit[courseAbbrIndex] = courseAbbrIndex < placeHolder
                ? ""
                : courseInformation[CourseDictionaryContent.CourseAbbreviation.ToString( )];

            var iCourseUnit = from cn in innerSplit
                              where cn.Length > 0 && Regex.IsMatch(cn, RegexPatternClassUnitFloat)
                              let num = innerSplit.IndexOf(cn)
                              where num > courseAbbrIndex
                              select num;

            unitIndex = iCourseUnit != null && iCourseUnit.Count( ) > 0 ? iCourseUnit.First( ) : placeHolder;
            courseInformation[CourseDictionaryContent.Unit.ToString( )] = innerSplit[unitIndex];
            innerSplit[unitIndex] =
                unitIndex < placeHolder ? "" : courseInformation[CourseDictionaryContent.Unit.ToString( )];

            var iCourseNumber = from cn in innerSplit
                                where cn.Length > 0
                                let cap = cn.ToUpper()
                                let cnIndex = innerSplit.IndexOf(cn)
                                where cn == cap && HasCourseNumber(cn)
                                select cnIndex;

            courseNumIndex = iCourseNumber != null && iCourseNumber.Count( ) > 0 ? iCourseNumber.First( ) : placeHolder;
            courseInformation[CourseDictionaryContent.CourseNumber.ToString( )] = innerSplit[courseNumIndex];
            innerSplit[courseNumIndex] = courseNumIndex < placeHolder
                ? ""
                : courseInformation[CourseDictionaryContent.CourseNumber.ToString( )];

            var possibleTitle = "";
            for ( var i = courseNumIndex; i < unitIndex; i++ )
                try
                {
                    var word = $"{innerSplit[i]}".Trim();
                    if ( HasCourseTitle( word ) )
                    {
                        innerSplit[i] = "";
                        possibleTitle = $"{possibleTitle} {word}";
                        courseInformation[CourseDictionaryContent.CourseTitle.ToString( )] = possibleTitle;
                    }
                }
                catch ( Exception )
                {
                }

            // Append the rest of the lines to the ETC
            for ( var i = 0; i < innerSplit.Count; i++ )
                if ( innerSplit[i] != "" )
                {
                    courseInformation["ETC"] = innerSplit[i] + " ";
                    innerSplit[i] = "";
                }

            return null;
        }

        private static List<string> StringBeforeLine( ref List<string> lines, string after )
        {
            var list = new List<string>();

            for ( var i = lines.Count - 1; i >= 0; i-- )
                if ( lines[i].StartsWith( after ) && i - 1 >= 0 )
                {
                    list.Add( lines[i - 1] );
                    lines[i] = "";
                    lines[i - 1] = "";
                }
            list.Reverse( );
            lines.RemoveAll( s => s.Length == 0 || s == "" );
            return list;
        }

        private static List<string> StringBetweenLines( ref List<string> lines, string before, string after )
        {
            var betweens = new List<string>();
            int stopIndex,
                limit,
                maxLength = 2 << 7;

            for ( var i = 0; i < lines.Count; i++ )
                if ( lines[i].Trim( ) == before )
                {
                    lines[i] = "";
                    stopIndex = i + 1;
                    limit = 10;

                    while ( stopIndex < lines.Count && limit > 0 )
                    {
                        if ( lines[stopIndex].Length > maxLength )
                        {
                            //TODO handle when line is too long.
                        }
                        else if ( !lines[stopIndex].Trim( ).StartsWith( after ) )
                        {
                            betweens.Add( lines[stopIndex] );
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

            lines.RemoveAll( s => s.Length == 0 || s.Trim( ) == "" );

            return betweens;
        }

        private static bool SpltByDesignationStatus( string str, ref StringBuilder sb )
        {
            if ( str == null || str.Length == 0 )
                return false;

            var indexOfDesStatus = str.IndexOf(SplitDesignationStatus, StringComparison.CurrentCultureIgnoreCase);

            if ( indexOfDesStatus < 0 )
            {
                if ( HasBarSplitSeperator( str ) )
                {
                    var subStrBeforeLast = str.IndexOf("Last") + "Last".Length;
                    var beforeLast = str.Substring(0, subStrBeforeLast).Trim();
                    var afterLast = str.Substring(subStrBeforeLast + 1).Trim();

                    if ( beforeLast.Length > 0 )
                        sb.AppendLine( beforeLast );
                    if ( afterLast.Length > 0 )
                        sb.AppendLine( afterLast );
                }
                else
                {
                    sb.Append( str.Trim( ).Replace( "   ", " " ).Replace( "  ", " " ) + " " );
                }
            }
            else if ( indexOfDesStatus >= 0 )
            {
                var indexes = new List<int>();

                while ( indexOfDesStatus > 0 )
                {
                    if ( !indexes.Contains( indexOfDesStatus ) )
                        indexes.Add( indexOfDesStatus );

                    indexOfDesStatus = str.IndexOf( SplitDesignationStatus, indexOfDesStatus + 1 );
                }

                if ( indexes.Count > 1 )
                {
                    var firstHalf = "";
                    int currentInt, nextInt, lengthOfSubString;

                    for ( var i = 0; i < indexes.Count; i++ )
                    {
                        currentInt = indexes[i];
                        if ( i + 1 < indexes.Count )
                        {
                            nextInt = indexes[i + 1];
                            lengthOfSubString = nextInt - currentInt;
                            firstHalf = str.Substring( currentInt, lengthOfSubString );
                        }
                        else
                        {
                            firstHalf = str.Substring( currentInt );
                        }

                        if ( firstHalf.Length > 0 )
                            sb.AppendLine( Environment.NewLine + firstHalf.Trim( ) );
                    }
                }
                else
                {
                    indexOfDesStatus = indexes.FirstOrDefault( );

                    var firstHalf = str.Substring(0, indexOfDesStatus).Trim();
                    var secondHalf = str.Substring(indexOfDesStatus + SplitDesignationStatus.Length).Trim();

                    if ( firstHalf.Length > 0 )
                        sb.AppendLine( firstHalf );

                    if ( secondHalf.Length > 0 )
                    {
                        sb.AppendLine( SplitDesignationStatus );
                        sb.AppendLine( secondHalf );
                    }
                }
            }
            return true;
        }

        private static bool SplitByVerticalBarSep( string str, ref StringBuilder sb )
        {
            if ( str == null || str.Length == 0 )
                return false;

            if ( str.Contains( "|" ) && str.Contains( "of" ) && ( str.Contains( "1" ) || str.Contains( "First" ) ) )
            {
                sb.AppendLine( );
                sb.AppendLine( str );
                return true;
            }
            sb.Append( str.Replace( "\n", "" ).Replace( "   ", "" ).Replace( "  ", "" ) + " " );

            return false;
        }

        private static bool HasCourseTitle( string str )
        {
            if ( str.Length == 0 || str == "" )
                return false;

            if ( str == "and" || str == "&" || str == "I" || str == "II" || str == "III" )
                return true;

            var firstLetterCap = str[0].ToString().ToUpper() == str[0].ToString();

            var lastLetterLowercase = str[str.Length - 1].ToString().ToLower() == str[str.Length - 1].ToString();

            return firstLetterCap && lastLetterLowercase;
        }

        private static bool HasCourseNumber( string sn )
        {
            if ( sn == null || sn.Length == 0 || sn.Length > 10 )
                return false;

            //195A
            var val = 0;
            if ( int.TryParse( sn, out val ) )
                return true;

            var hasCapsLetter = sn.ToUpper() == sn;
            var numsCount = 0;
            var charsCount = 0;

            foreach ( var c in sn )
                if ( char.IsDigit( c ) )
                    numsCount++;
                else if ( char.IsLetter( c ) )
                    charsCount++;

            return hasCapsLetter && charsCount > 0 && numsCount > 0;
        }

        private static bool HasBarSplitSeperator( string str )
        {
            return str.Contains( "|" ) && str.Contains( "of" ) && ( str.Contains( "1" ) || str.Contains( "First" ) );
        }

        private static bool HasSemesterSeason( string ss )
        {
            return ss.StartsWith( "Fall", StringComparison.CurrentCultureIgnoreCase ) ||
                   ss.StartsWith( "Summer", StringComparison.CurrentCultureIgnoreCase ) ||
                   ss.StartsWith( "Spring", StringComparison.CurrentCultureIgnoreCase );
        }

        private static bool HasYear( string yy )
        {
            return Regex.IsMatch( yy, RegexPatternYEAR );
        }

        private static bool HasCourseUnit( string cu )
        {
            return Regex.IsMatch( cu, RegexPatternClassUnitFloat );
        }

        #endregion Progress Transcript Parser Methods

        public static string GetCourseTitle( string str )
        {
            var ret = str;

            return ret;
        }

        public static bool HasGrade( string gg )
        {
            if ( gg == "" || gg.Length == 0 || gg == "#" )
                return false;

            //if (gg.Length == 2 && !(gg.EndsWith("+") || gg.EndsWith("-")))
            //	return false;

            var grade = string.Format(gg[0].ToString()).ToUpper();

            foreach ( var s in LetterGradesList )
                if ( s == gg )
                    return true;

            if ( char.IsLetter( gg[0] ) && LetterGradesList.Contains( grade ) && ( gg.EndsWith( "+" ) || gg.EndsWith( "-" ) ) )
                return true;
            // some grades are given as units
            if ( int.TryParse( gg, out var i ) && i < 1000 )
                return true;
            return false;
        }
    }
}