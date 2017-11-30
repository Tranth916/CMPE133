using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp;
using iTextSharp.text.pdf;
using System.ComponentModel;
using Prism.Mvvm;

namespace GradHelperWPF.Model
{
    public class GradAppModel : BindableBase
    {
        public string GradAppFilePath
        {
            get
            {
                string path = "";
                try
                {
                    var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

                    var paths = from file in files
                                let f = file.ToLower()
                                where f.Contains("resources") && f.Contains("gradapp")
                                select file as string;

                    path = paths != null ? paths.FirstOrDefault() : "";
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.StackTrace);
                }
                return path;
            }
        }
        
        public PdfReader Reader
        {
            set;
            get;
        }
        public PdfStamper Stamper
        {
            set;
            get;
        }
        public Stream FStream
        {
            set;
            get;
        }

        public GradAppModel()
        {

        }

        public void LoadForm(string path)
        {
            if (!File.Exists(path))
                return;
                        
            Reader = new PdfReader(path);

            // This needs to be the output form.
            FStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            
            
        }

        public void WriteData(Dictionary<string,string> data)
        {

        }

        private string firstName = "Student F";
        private string lastName = "Student L'";

        public string FirstName
        {
            get { return firstName; }

            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    RaisePropertyChanged("FirstName");
                    RaisePropertyChanged("FullName");
                }
            }
        }

        public string LastName
        {
            get { return lastName; }

            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    RaisePropertyChanged("LastName");
                    RaisePropertyChanged("FullName");
                }
            }
        }

        public string FullName
        {
            get
            {
                return firstName + " " + lastName;
            }
        }

    }
}
