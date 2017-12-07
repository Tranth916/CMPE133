using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ExcelDataReader;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GradHelperWPF.Utils
{
    internal class FileUtil
    {
        public enum FileStatus
        {
            SjsuCourses,
            TransferCourses,
            Corrupted,
            Empty,
            FileIsMissing,
            FileIsLocked,
        }
        private static string WorkingDirectory => Directory.GetCurrentDirectory( ) ?? AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     Copy the file selected into a working directory.
        /// </summary>
        /// <param name="filePath">Filepath of the copied file.</param>
        /// <returns></returns>
        public static string MakeWorkingCopy( string filePath )
        {
            var retPath = "";

            if (!File.Exists(filePath)) return retPath;

            var workingPath = Path.GetFullPath(filePath).Replace(Path.GetExtension(filePath), "");

            // Try to get the extension of the file.
            var fileExtension = Path.GetExtension(filePath)
                .Replace("\\", "")
                .Replace("/", "");
            // File extension must begin with a period.
            if ( !fileExtension.StartsWith( "." ) )
                fileExtension = filePath.Substring( filePath.LastIndexOf( '.' ) );

            // Prepare the name for the new copy.
            var destinationFileName = WorkingDirectory +
                                      "\\" +
                                      Path.GetFileName(filePath.Replace(fileExtension, ""))
                                          .Replace("\\", "")
                                          .Replace("/", "") +
                                      "_copy" +
                                      fileExtension;

            // The working file needs to be deleted.
            if ( File.Exists( destinationFileName ) )
                try
                {
                    File.Delete( destinationFileName );
                }
                catch ( Exception )
                {

                    // Rename it again.
                    destinationFileName = destinationFileName.Replace( fileExtension, "" )
                                          + "2" + fileExtension;
                }

            try
            {
                File.Copy( filePath, destinationFileName, true );
                retPath = destinationFileName;
            }
            catch (Exception)
            {

            }

            return retPath;
        }

        public static string ShowOpenFileDialog( string[] filters )
        {
            var sb = new StringBuilder();
            
            for ( var i = 0; i < filters.Length; i++ )
                sb.Append( $" | {i} " );

            var filePath = "";

            var ofd = new OpenFileDialog();

            var selectedFile = ofd.ShowDialog();

            if ( selectedFile != null && (selectedFile.Value && !string.IsNullOrEmpty( ofd.FileName )) )
                filePath = ofd.FileName;

            return filePath;
        }

        public static FileStatus CheckFileBeforeOpen(string filePath)
        {
            FileStatus status = FileStatus.FileIsMissing;

            if (!File.Exists(filePath)) return status;

            string fileExtension = Path.GetExtension(filePath);

            if (fileExtension != null && (fileExtension.StartsWith(".xls") || fileExtension.StartsWith(".xlsx")))
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var excelReader = ExcelReaderFactory.CreateReader(fs);

                        // file exists, but cannot be opened?
                        if (excelReader == null) return FileStatus.FileIsLocked;

                        if (excelReader.FieldCount == 0) return FileStatus.Empty;

                        List<string> columnNames = new List<string>();
                        int rowCount = 0;
                        do
                        {   // just read the column names.
                            while (excelReader.Read())
                            {
                                for (int i = 0; i < excelReader.FieldCount; i++)
                                {
                                    try
                                    {
                                        if (excelReader.GetValue(i) != null)
                                            columnNames.Add((String) excelReader.GetValue(i) ?? "");
                                    }
                                    catch (InvalidCastException icx)
                                    {
                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Exception throw while reading header column" + ex.StackTrace);
                                    }
                                }
                                rowCount++;

                                if (rowCount > 1)
                                    break;
                            }
                            break;
                        } while (excelReader.NextResult());

                        var hasCourseHeader = columnNames.Any(c => c.StartsWith("Course"));
                        var hasTransferHeader = columnNames.Any(c => c.StartsWith("Transfer"));

                        if (hasCourseHeader)
                        {
                            status = hasTransferHeader ? FileStatus.TransferCourses : FileStatus.SjsuCourses;
                        }
                        excelReader.Close();
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    status = FileStatus.Corrupted;
                }
            }
            return status;
        }
    }
}