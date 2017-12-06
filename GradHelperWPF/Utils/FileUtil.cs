using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace GradHelperWPF.Utils
{
    class FileUtil
    {
        private static string WorkingDirectory
        {
            get
            {
                if (Directory.GetCurrentDirectory() == null)
                    return System.AppDomain.CurrentDomain.BaseDirectory;

                else
                    return Directory.GetCurrentDirectory();
            }
        }

        /// <summary>
        /// Copy the file selected into a working directory.
        /// </summary>
        /// <param name="filePath">Filepath of the copied file.</param>
        /// <returns></returns>
        public static string MakeWorkingCopy(string filePath)
        {
            string retPath = "";

            if (File.Exists(filePath))
            {
                string workingPath = Path.GetFullPath(filePath).Replace(Path.GetExtension(filePath), "");

                // Try to get the extension of the file.
                string fileExtension = Path.GetExtension(filePath)
                                        .Replace("\\", "")
                                        .Replace("/", "");
                // File extension must begin with a period.
                if (!fileExtension.StartsWith("."))
                    fileExtension = filePath.Substring(filePath.LastIndexOf('.'));

                // Prepare the name for the new copy.
                string destinationFileName = WorkingDirectory +
                                                          "\\" +
                                                   Path.GetFileName(filePath.Replace(fileExtension, ""))
                                                    .Replace("\\", "")
                                                    .Replace("/", "") +
                                                    "_copy" +
                                                    fileExtension;

                // The working file needs to be deleted.
                if (File.Exists(destinationFileName))
                {
                    try
                    {
                        File.Delete(destinationFileName);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to delete working file: " + destinationFileName);
                        // Rename it again.
                        destinationFileName = destinationFileName.Replace(fileExtension, "")
                                                + "2" + fileExtension;
                    }
                }

                try
                {
                    File.Copy(filePath, destinationFileName, true);
                    retPath = destinationFileName;
                    //if (Directory.Exists(Path.GetDirectoryName(filePath)))
                    //{
                    //    String pathWithNoExt = filePath.Substring(0, filePath.LastIndexOf(".")) + $"_copy.docx";
                    //    retPath = pathWithNoExt
                    //    File.Copy(filePath, retPath, true);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            return retPath;
        }

		public static string ShowOpenFileDialog(string[] filters)
		{
			StringBuilder sb = new StringBuilder();

			//Text files (*.txt)|*.txt|All files (*.*)|*.*
			for (int i = 0; i < filters.Length; i++)
			{
				sb.Append($" | {filters[i]} ");
			}

			string filePath = "";

			OpenFileDialog ofd = new OpenFileDialog()
			{

			};

			bool? selectedFile = ofd.ShowDialog();

			if (selectedFile.Value && !string.IsNullOrEmpty(ofd.FileName) )
			{
				filePath = ofd.FileName;
			}

			return filePath;
		}
    }
}
