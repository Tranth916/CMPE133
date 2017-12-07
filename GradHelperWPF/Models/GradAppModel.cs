using GradHelperWPF.Utils;
using iTextSharp.text.pdf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GradHelperWPF.Models
{
    public class GradAppModel : BindableBase
    {
        private static GradAppModel _gradApp;
        private string _gradAppFilePath;
        private string _outputFilePath;
        private string _readFromFilePath;
        public string apartmentNumber;
        public string city;
        public string[] currentEnrolledCourses;
        public string degreeObjective;
        public string email;

        public string firstName;
        public string gradSemester;
        public string gradYear;
        public string lastName;
        public string majorName = "Software Engineering";

        private readonly Dictionary<string, string> map = new Dictionary<string, string>
        {
            {"firstName", "First name"},
            {"middleName", "Middle name"},
            {"lastName", "Last name"},
            {"email", "E-mail address"},
            {"phoneNumber", "Home phone number"},
            {"sidName", "SJSU ID"},
            {"majorName", "Major"},
            {"streetNumber", "Street number"},
            {"streetName", "Street name"},
            {"apartmentNumber", "Apartment number"},
            {"city", "City"},
            {"state", "State"},
            {"zipcode", "Zip code"},
            {"nonSJSUNotCompleted", "Non SJSU course #"},
            {"currentEnrolledCourses", "current SJSU course #"}
        };

        public string middleName;
        public string[] nonSJSUNotCompleted;
        public string phoneNumber;
        public string state;
        public string streetName;
        public string streetNumber;
        public string studentID;
        public string zipcode;

        private GradAppModel( )
        {
            nonSJSUNotCompleted = new string[8];
            currentEnrolledCourses = new string[8];
            for ( var i = 0; i < nonSJSUNotCompleted.Length; i++ )
            {
                nonSJSUNotCompleted[i] = "";
                currentEnrolledCourses[i] = "";
            }
            Init( );
        }

        public GradAppModel( string filePath )
        {
            SourceFilePath = filePath;
            Init( );
        }

        public string SourceFilePath
        {
            set => _gradAppFilePath = value;
            get
            {
                if ( !string.IsNullOrEmpty( _gradAppFilePath ) )
                    return _gradAppFilePath;

                var path = "";
                try
                {
                    var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf",
                        SearchOption.AllDirectories);

                    var paths = from file in files
                                let f = file.ToLower()
                                where f.Contains("resources") && f.Contains("gradapp")
                                select file;

                    path = paths != null ? paths.FirstOrDefault( ) : "";
                }
                catch ( Exception ex )
                {
                    throw new Exception( ex.StackTrace );
                }
                return path;
            }
        }

        private string OutputFilePath
        {
            set
            {
                // the file should not exist, if it does , then delete it.
                try
                {
                    //Delete existing working copy.
                    if ( !string.IsNullOrEmpty( _outputFilePath ) && File.Exists( _outputFilePath ) )
                        File.Delete( _outputFilePath );

                    if ( File.Exists( value ) )
                        File.Delete( value );
                }
                catch ( Exception ex )
                {
                    throw new Exception( "Failed to delete PDF working file!" + ex.StackTrace );
                }
                _outputFilePath = value;
            }
            get => _outputFilePath ?? "";
        }

        public PdfReader Reader { set; get; }

        public PdfStamper Stamper { set; get; }

        public Stream FStream { set; get; }

        public static GradAppModel GetInstance( )
        {
            if ( _gradApp == null )
                _gradApp = new GradAppModel( );
            return _gradApp;
        }

        private void Init( )
        {
            _readFromFilePath = FileUtil.MakeWorkingCopy( SourceFilePath );
            LoadReader( _readFromFilePath );

            OutputFilePath = Directory.GetCurrentDirectory( ) + "\\" + "temp_gp.pdf";
            LoadStamper( OutputFilePath );

            //	System.Diagnostics.Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }

        private void LoadStamper( string outputPath )
        {
            if ( string.IsNullOrEmpty( outputPath ) )
                return;
            try
            {
                if ( Reader == null )
                    LoadReader( _gradAppFilePath );

                FStream = new FileStream( outputPath, FileMode.OpenOrCreate );
                Stamper = new PdfStamper( Reader, FStream );
            }
            catch ( Exception ex )
            {
                throw new Exception( "Exception while loading stamper " + ex.StackTrace );
            }
        }

        public void LoadReader( string path )
        {
            if ( !File.Exists( path ) )
                return;
            try
            {
                Reader = new PdfReader( path );
            }
            catch ( Exception ex )
            {
                throw new Exception( "Exception while loading PDF Reader" + ex.StackTrace );
            }
        }

        public bool LoadForm( )
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

            if ( files == null || files.Count( ) == 0 )
                files = Directory.EnumerateFiles( Directory.GetCurrentDirectory( ), "*.PDF", SearchOption.AllDirectories );

            if ( files == null || files.Count( ) == 0 )
                return false;

            var filePath = files.FirstOrDefault(f => f.ToLower().Contains("gradapp"));

            try
            {
                Reader = new PdfReader( filePath );
            }
            catch ( Exception ex )
            {
                throw new Exception( "Failed to find the gradapp.pdf, was it deleted from the ~/Templates folder? " +
                                    ex.StackTrace );
            }

            return true;
        }

        public bool CheckSemester( string semester )
        {
            //Check Box for Summer
            if ( semester.Contains( "Spring" ) || semester.Contains( "Fall" ) || semester.Contains( "Summer" ) )
            {
                var checkBoxKey = Stamper.AcroFields.Fields.Keys.Where(k => k.Contains("Check Box for ")).ToList();
                foreach ( var key in checkBoxKey )
                    if ( key.Contains( semester ) )
                    {
                        Stamper.AcroFields.SetField( key, "Yes", true );
                    }
                    else
                    {
                        if ( key.EndsWith( "Summer" ) )
                            Stamper.AcroFields.SetField( "Year 1", "", true );
                        else if ( key.EndsWith( "Fall" ) )
                            Stamper.AcroFields.SetField( "Year 2", "", true );
                        else if ( key.EndsWith( "Spring" ) )
                            Stamper.AcroFields.SetField( "Year 3", "", true );

                        Stamper.AcroFields.SetField( key, "No", true );
                    }
            }

            // sum fall spr
            if ( !string.IsNullOrEmpty( gradYear ) )
            {
                var yearKey = "";

                switch ( gradSemester )
                {
                    case "Summer":
                        yearKey = "Year 1";
                        break;

                    case "Fall":
                        yearKey = "Year 2";
                        break;

                    case "Spring":
                        yearKey = "Year 3";
                        break;
                }
                Stamper.AcroFields.SetField( yearKey, gradYear, true );
            }

            return true;
        }

        public bool WriteField( string key, string val = null )
        {
            //this is not working correctly.
            return false;

            var writtenCount = 0;

            if ( Stamper == null )
                return false;

            var fieldValue = string.IsNullOrEmpty(val) ? "" : val;

            if ( key == "Semester" || key == "Year" && !string.IsNullOrEmpty( gradSemester ) )
                return CheckSemester( val );

            switch ( key )
            {
                case "First name":
                    fieldValue = firstName ?? "";
                    break;

                case "Middle name":
                    fieldValue = middleName ?? "";
                    break;

                case "Last name":
                    fieldValue = lastName ?? "";
                    break;

                case "E-mail address":
                    fieldValue = email ?? "";
                    break;

                case "Home phone number":
                    fieldValue = phoneNumber ?? "";
                    break;

                case "SJSU ID":
                    fieldValue = studentID ?? "";
                    break;

                case "Major":
                    fieldValue = majorName ?? "";
                    break;

                case "Street number":
                    fieldValue = streetNumber ?? "";
                    break;

                case "Street name":
                    fieldValue = streetName ?? "";
                    break;

                case "Apartment number":
                    fieldValue = apartmentNumber ?? "";
                    break;

                case "City":
                    fieldValue = city ?? "";
                    break;

                case "State":
                    fieldValue = state ?? "";
                    break;

                case "Zip code":
                    fieldValue = zipcode ?? "";
                    break;
            }

            if ( string.IsNullOrEmpty( fieldValue ) )
            {
                var currentEnrollKey = "current SJSU course #";
                var unCompleteKey = "Non SJSU course #";

                var isCurrentEnrolled = key.StartsWith(currentEnrollKey.Substring(0, currentEnrollKey.Length - 2));
                var isUnCompletedKey = key.StartsWith(unCompleteKey.Substring(0, currentEnrollKey.Length - 2));

                if ( isCurrentEnrolled || isUnCompletedKey )
                {
                    var n = key.Trim().LastOrDefault().ToString();
                    switch ( n )
                    {
                        //nonSJSUNotCompleted
                        case "1":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[0];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[0];
                            break;

                        case "2":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[1];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[1];
                            break;

                        case "3":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[2];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[2];
                            break;

                        case "4":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[3];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[3];
                            break;

                        case "5":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[4];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[4];
                            break;

                        case "6":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[5];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[5];
                            break;

                        case "7":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[6];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[6];
                            break;

                        case "8":
                            if ( isCurrentEnrolled ) fieldValue = currentEnrolledCourses[7];
                            else if ( isUnCompletedKey ) fieldValue = nonSJSUNotCompleted[7];
                            break;

                        default:
                            fieldValue = "";
                            break;
                    }
                }
            }

            if ( !string.IsNullOrEmpty( fieldValue ) )
            {
                Stamper.AcroFields.SetField( key, fieldValue, true );
                writtenCount++;
            }

            return writtenCount > 0;
        }

        public bool WriteAllFields( string outputPath = null )
        {
            if ( Reader == null )
                if ( !LoadForm( ) )
                    return false;
            //string path = outputPath != null ? outputPath : Directory.GetCurrentDirectory() + $"\\{firstName}_{lastName}_gradapp.pdf";
            var fields = Stamper.AcroFields.Fields;
            Stamper.AcroFields.SetField( "First name", firstName ?? "", true );
            Stamper.AcroFields.SetField( "Middle name", middleName ?? "", true );
            Stamper.AcroFields.SetField( "Last name", lastName ?? "", true );
            Stamper.AcroFields.SetField( "E-mail address", email ?? "", true );
            Stamper.AcroFields.SetField( "Home phone number", phoneNumber ?? "", true );
            Stamper.AcroFields.SetField( "SJSU ID", studentID ?? "", true );
            Stamper.AcroFields.SetField( "Major", majorName ?? "", true );
            Stamper.AcroFields.SetField( "Street number", streetNumber ?? "", true );
            Stamper.AcroFields.SetField( "Street name", streetName ?? "", true );
            Stamper.AcroFields.SetField( "Apartment number", apartmentNumber ?? "", true );
            Stamper.AcroFields.SetField( "City", city ?? "", true );
            Stamper.AcroFields.SetField( "State", state ?? "", true );
            Stamper.AcroFields.SetField( "Zip code", zipcode ?? "", true );

            var currentEnrollKey = "current SJSU course #";
            var unCompleteKey = "Non SJSU course #";
            string key1, key2, val1, val2;

            for ( var i = 0; i < currentEnrolledCourses.Length; i++ )
            {
                key1 = currentEnrollKey.Replace( "#", $"{i + 1}" );
                key2 = unCompleteKey.Replace( "#", $"{i + 1}" );
                val1 = currentEnrolledCourses[i] ?? "";
                val2 = nonSJSUNotCompleted[i] ?? "";
                Stamper.AcroFields.SetField( key1, val1, true );
                Stamper.AcroFields.SetField( key2, val2, true );
            }

            CheckSemester( gradSemester );

            return true;
        }

        public void StampDataToPDF( string path, Dictionary<string, string> data )
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

            if ( files == null )
                throw new Exception( );

            var pdfFile = files.FirstOrDefault(ff => ff.Contains("gradapp"));

            var reader = new PdfReader(pdfFile);

            var fs = new FileStream(path, FileMode.OpenOrCreate);

            var stamper = new PdfStamper(reader, fs);

            var fields = stamper.AcroFields.Fields;

            string fieldName;
            foreach ( var f in fields )
            {
                fieldName = f.Key;

                var textBoxName = map.Where(m => m.Value == fieldName).Select(m => m.Key).FirstOrDefault();

                if ( textBoxName == null || !data.ContainsKey( textBoxName ) )
                    continue;

                stamper.AcroFields.SetField( fieldName, data[textBoxName], true );
            }

            stamper.Close( );
            fs.Close( );
            reader.Close( );

            Process.Start( "explorer.exe", path );
        }

        public void Close( )
        {
            try
            {
                Stamper.Close( );
                FStream.Close( );
                Reader.Close( );
            }
            catch ( Exception ex )
            {
            }

            //if (File.Exists(OutputFilePath))
            //	System.Diagnostics.Process.Start("explorer.exe", OutputFilePath);
        }
    }
}