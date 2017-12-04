using iTextSharp.text.pdf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GradHelperWPF.Models
{
    public class GradAppModel : BindableBase
    {
        public string firstName;
        public string middleName;
        public string lastName;
        public string studentID;
        public string email;
        public string phoneNumber;
        public string majorName = "Software Engineering";
        public string gradYear;
        public string gradSemester;
        public string streetNumber;
        public string streetName;
        public string apartmentNumber;
        public string city;
        public string state;
        public string zipcode;
        public string degreeObjective;
        public string[] nonSJSUNotCompleted;
        public string[] currentEnrolledCourses;

        Dictionary<string, string> map = new Dictionary<string, string>()
            {
                { "firstName"                                       ,"First name"               },
                { "middleName"                                      ,"Middle name"              },
                { "lastName"                                        ,"Last name"                },
                { "email"                                           ,"E-mail address"           },
                { "phoneNumber"                                     ,"Home phone number"        },
                { "sidName"                                         ,"SJSU ID"                  },
                { "majorName"                                       ,"Major"                    },
                { "streetNumber"                                    ,"Street number"            },
                { "streetName"                                      ,"Street name"              },
                { "apartmentNumber"                                 ,"Apartment number"         },
                { "city"                                            ,"City"                     },
                { "state"                                           ,"State"                    },
                { "zipcode"                                         ,"Zip code"                 },
                { "nonSJSUNotCompleted"                             ,"Non SJSU course #"        },
                { "currentEnrolledCourses"                          ,"current SJSU course #"    } ,
            };
       
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
            nonSJSUNotCompleted = new string[8];
            currentEnrolledCourses = new string[8];

            for (int i = 0; i < nonSJSUNotCompleted.Length; i++)
            {
                nonSJSUNotCompleted[i] = "";
                currentEnrolledCourses[i] = "";
            }
        }

        public void LoadForm(string path)
        {
            if (!File.Exists(path))
                return;
                        
            Reader = new PdfReader(path);

            // This needs to be the output form.
            FStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);            
        }

        public bool LoadForm()
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

            if (files == null || files.Count() == 0)
                files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.PDF", SearchOption.AllDirectories);

            if (files == null || files.Count() == 0)
                return false;

            var filePath = files.FirstOrDefault( f => f.ToLower().Contains("gradapp") );

            try
            {
                Reader = new PdfReader(filePath);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to find the gradapp.pdf, was it deleted from the ~/Templates folder? " + ex.StackTrace);
            }

            return true;
        }

        public bool WriteToForm(string outputPath = null)
        {
            if (Reader == null)
            {
                if (!LoadForm())
                    return false;
            }

            string path = outputPath != null ? outputPath : Directory.GetCurrentDirectory() + $"\\{firstName}_{lastName}_gradapp.pdf";

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            Stamper = new PdfStamper(Reader, fs);

            var fields = Stamper.AcroFields.Fields;

            Stamper.AcroFields.SetField(  "First name"              ,firstName        ?? "" , true );
            Stamper.AcroFields.SetField(  "Middle name"             ,middleName       ?? "" , true );
            Stamper.AcroFields.SetField(  "Last name"               ,lastName         ?? "" , true );
            Stamper.AcroFields.SetField(  "E-mail address"          ,email            ?? "" , true );
            Stamper.AcroFields.SetField(  "Home phone number"       ,phoneNumber      ?? "" , true );
            Stamper.AcroFields.SetField(  "SJSU ID"                 ,studentID        ?? "" , true );
            Stamper.AcroFields.SetField(  "Major"                   ,majorName        ?? "" , true );
            Stamper.AcroFields.SetField(  "Street number"           ,streetNumber     ?? "" , true );
            Stamper.AcroFields.SetField(  "Street name"             ,streetName       ?? "" , true );
            Stamper.AcroFields.SetField(  "Apartment number"        ,apartmentNumber  ?? "" , true );
            Stamper.AcroFields.SetField(  "City"                    ,city             ?? "" , true );
            Stamper.AcroFields.SetField(  "State"                   ,state            ?? "" , true );
            Stamper.AcroFields.SetField(  "Zip code",                zipcode          ?? "" , true);

            string currentEnrollKey = "current SJSU course #";
            string unCompleteKey = "Non SJSU course #";
            string key1, key2, val1, val2;

            for(int i = 0; i < currentEnrolledCourses.Length; i++)
            {
                key1 = currentEnrollKey.Replace("#", $"{i + 1}");
                key2 = unCompleteKey.Replace("#", $"{i + 1}");

                val1 = currentEnrolledCourses[i] ?? "";
                val2 = nonSJSUNotCompleted[i] ?? "";

                Stamper.AcroFields.SetField(key1, val1, true);
                Stamper.AcroFields.SetField(key2, val2, true);
            }

            fs.Close();
            Stamper.Close();

            return true;
        }


        public void StampDataToPDF(string path, Dictionary<string, string> data)
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

            if (files == null)
                throw new Exception();

            var pdfFile = files.FirstOrDefault(ff => ff.Contains("gradapp"));

            var reader = new PdfReader(pdfFile);

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            var stamper = new PdfStamper(reader, fs);

            var fields = stamper.AcroFields.Fields;

            string fieldName;
            foreach (var f in fields)
            {
                fieldName = f.Key;

                var textBoxName = map.Where(m => m.Value == fieldName).Select(m => m.Key).FirstOrDefault();

                if (textBoxName == null || !data.ContainsKey(textBoxName))
                    continue;

                stamper.AcroFields.SetField(fieldName, data[textBoxName], true);
            }

            stamper.Close();
            fs.Close();
            reader.Close();

            System.Diagnostics.Process.Start("explorer.exe", path);
        }

    }
}
