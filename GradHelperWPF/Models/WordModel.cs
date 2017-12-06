using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GradHelperWPF.Google;
using GradHelperWPF.Utils;
using System;
using DocumentFormat.OpenXml.Office2010.Word.DrawingShape;

namespace GradHelperWPF.Models
{
	public class WordModel
	{
		private static WordModel _wordModelSingleton;
		public static WordModel GetInstance()
		{
			if (_wordModelSingleton == null || _wordModelSingleton.IsClosed)
				_wordModelSingleton = new WordModel();
			return _wordModelSingleton;
		}


		public static readonly List<string> CourseFullName = new List<string>()
		{
			"Biol 010 The Living World 3",

			"CmpE 102 Assembly Language Programming 3",
			"CmpE 120 Computer Organization and Architecture 3",
			"CmpE 131 Software Engineering I 3",
			"CmpE 133 Software Engineering II 3",
			"CmpE 148 Computer Networks 3",
			"CmpE 165 Software Engineering Process Management 3",
			"CmpE 172 Enterprise Software Platforms 3",
			"CmpE 187 Software Quality Engineering 3",
			"CmpE 195A Senior Design Project I 2",
			"CmpE 195B Senior Design Project  II 3",

			"CS 046A Introduction to Programming 4",
			"CS 046B Introduction to Data Structures 4",
			"CS 146 Data Structures and Algorithms 3",
			"CS 149 Operating Systems 3",
			"CS 151 Object – Oriented Design 3",
			"CS 157A Introduction to Database Management 3",
			"CS 166 Information Security 3",

			"Engr 010 Introduction to Engineering 3",
			"Engr 195A Global and Social Issues in Engineering (S) 1",
			"Engr 195B Global and Social Issues in Engineering (V) 1",
			"Engl 01B Argument & Analysis 3",

			"ISE 130 Engineering Probability & Statistics 3",
			"ISE 164 Computer and Human Interaction 3",

			"Math 030 Calculus I 3",
			"Math 031 Calculus II 3",
			"Math 032 Calculus III 3",
			"Math 042 Discrete Mathematics 3",
			"Math 123 Differential Equations & Linear Algebra 3",

			"Phys 050 Mechanics 4",
			"Phys 051 Electricity and Magnetism 4",
		};

		#region Constants
		private readonly string FontSize = "14";
		private readonly string FontFamily = "Arial";
		private readonly string LastFirstMISidFlag = "#lastName##firstName##mi##studentID#";
		private readonly string YearFlag = "#year#";
		private const int MinNumRequiredTuple = 5;
		private const int RowHeight = 200;

		#endregion

		#region Private Members
		private string																			 _workingPath;
		private WordprocessingDocument															 _doc;
		private StringBuilder																	 _courseFullNameSB;
		private OpenXmlElement																	 _page1Table;
		private RunProperties																	 _textRunProperty;
		private IEnumerable<TableRow>															 _tableRows;
		private IEnumerable<Run>																 _runs;
		private IEnumerable<IGrouping<string, OpenXmlElement>>									 _cellsWithCourseName;
		private Dictionary<string, TableCell>													 _writtenToCells;
		private Dictionary<string, Tuple<TableCell, TableCell, TableCell, TableCell, TableCell>> _lookupGrids;
		private Dictionary<string, Text>														_studentNameTable;
		private Dictionary<string, OpenXmlElement>												_footNoteCells;
		#endregion
		
		#region Public properties
		public string CourseFullNameStr
		{
			get
			{
				if (_courseFullNameSB == null)
				{
					_courseFullNameSB = new StringBuilder();
					for (int i = 0; i < CourseFullName.Count; i++)
						_courseFullNameSB.Append($"{CourseFullName[i]} ");
				}

				return _courseFullNameSB.ToString();
			}
		}

		public string WorkingPath
		{
			get { return _workingPath; }
		}
		public bool IsClosed{ set; get; }
        #endregion

        #region Constructor
        private WordModel()
        {
			Init();
			ReadGridTable();
		}

        public WordModel(string filePath)
		{
			FileLocation = filePath;
			Init();
		}
	
		#endregion

		private void Init()
		{
			try
			{
				if( string.IsNullOrEmpty(FileLocation) )
				{
					var docFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.docx", SearchOption.AllDirectories);

					if (docFiles != null && docFiles.Count() > 0)
					{
						FileLocation = docFiles.Where( f => !f.Contains("copy")).FirstOrDefault();
					}
					
				}

				if (FileLocation == null)
					throw new Exception("Cannot find the majorform.docx, did someone delete it?");

				_workingPath = FileUtil.MakeWorkingCopy(FileLocation) ?? "";

				if (string.IsNullOrEmpty(_workingPath))
					throw new IOException();

				_doc = WordprocessingDocument.Open(_workingPath, true, new OpenSettings() { AutoSave = true });
			}
			catch (IOException iox)
			{
				// File is currently opened. Then use a stream reader to open the doc.
				FileStream fs = new FileStream(_workingPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
				_doc = WordprocessingDocument.Open(fs, true);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}

		public void ShowDoc()
		{
			System.Diagnostics.Process.Start("explorer.exe", _workingPath);
		}

		public bool Close()
		{
			try
			{
				if (_doc != null)
				{
					_doc.Close();
					IsClosed = true;
					return true;
				}
			}
			catch
			{

			}
			if (_workingPath != null)
			{
				System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(FileLocation));
			}
			return false;
		}

		/// <summary>
		/// Parse the word document and create a lookup table for each course.
		/// </summary>
		/// <returns></returns>
		public bool ReadGridTable()
		{
			if (_doc == null)
				return false;

			bool result = false;

			_page1Table = _doc.MainDocumentPart
							  .Document.Body
							  .ChildElements.FirstOrDefault(tt => tt.InnerText.Contains("LastFirst") && tt is Table);
			
			_tableRows =			from tr in _page1Table.ChildElements
									where tr is TableRow
									select tr as TableRow;

			_studentNameTable = GetNameTextCells(ref _page1Table);
			_footNoteCells = GetFootNoteCell(ref _page1Table);

			// prep the form;
			PrepareMajorForm(ref _tableRows);

			_cellsWithCourseName =	from tr in _tableRows
									where tr.ChildElements.Count() > 10 && !string.IsNullOrEmpty(tr.InnerText)
									from trChild in tr.ChildElements
									where trChild is TableCell
									group trChild by tr.InnerText.Replace(" ", "");

			_runs =					from rowx in _tableRows
									from cell in rowx.ChildElements
									from paragraph in cell.ChildElements
									where paragraph is Paragraph
									from run in paragraph.ChildElements
									where run is Run
									select run as Run;
			//BuildWritableCellsTable();
			return result = _page1Table != null && _tableRows != null && _cellsWithCourseName != null && _runs != null;
		}
        
		/// <summary>
        /// This should be called for transferred courses only. Clone the RUN children of the Top row and insert them into the bottom.
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        private bool CloneTopAndWriteIntoBot(TableCell top, TableCell bot, string val)
		{
			var paragraphs = bot.ChildElements
							.Where(s => s is Paragraph)
							.Select(s => s as Paragraph);

			foreach (var pg in paragraphs)
			{
				// its empty and will need a Run
				var runChilds = pg.ChildElements.Where(rr => rr is Run);

				if (runChilds == null || runChilds.Count() == 0)
				{
					// since the top row has the child elements needed, then clone it and emtpy the text..
					var clone = top.ChildElements
										.Where(rr => rr is Paragraph)
										.Select(rr => rr as Paragraph)
										.FirstOrDefault();

					var childClones = clone.ChildElements.Where(cc => cc is Run);

					// if this is null then its the grade's cell and is initially empty.
					if (childClones == null || childClones.Count() == 0)
					{
						Run r = new Run();

						if (_textRunProperty != null)
							r.RunProperties = _textRunProperty;
						else
						{
							// select the parent nearest sibling.
							var p = pg.Parent.Parent.ChildElements
										.Where(pp => pp is TableCell)
										.Select(pp => pp as TableCell);

							var sibs = from rows in p
									   from row in rows.ChildElements
									   where row is Paragraph
									   from run in row.ChildElements
									   where run is Run && !string.IsNullOrEmpty(run.InnerText)
									   select run as Run;

							if (sibs != null && sibs.Count() > 0)
							{
								_textRunProperty = sibs.FirstOrDefault().RunProperties.Clone() as RunProperties;
								r.RunProperties = _textRunProperty;
							}
						}
						Text t = new Text();
						t.Text = val;
						r.AppendChild(t);
						pg.AppendChild(r);
						break;
					}

					Run clonedRun = null;
					foreach (var cc in childClones)
					{
						clonedRun = cc.CloneNode(true) as Run;
						// The paragraph often have 2 runs.
						var textsOfRun = clonedRun.ChildElements.Where(txx => txx is Text).Cast<Text>().ToArray();

						// clear all the text of the run.
						for (int i = 0; i < textsOfRun.Length; i++)
						{
							textsOfRun[i].Text = "";
						}

						pg.AppendChild(clonedRun);
					}
					// set the text of the very first clone.
					if (clonedRun != null)
					{
						var txx = clonedRun.ChildElements.Where(tx => tx is Text).Select(tx => tx as Text).FirstOrDefault();
						if (txx != null)
							txx.Text = val + " ";

						// okay if this has an actual text, then lets save a reference to its' runs property.
						if (_textRunProperty == null)
						{
							_textRunProperty = clonedRun.CloneNode(true) as RunProperties;
						}
					}
				}
			}
			return true;
		}

        /// <summary>
        /// Prepare the major form by appending empty rows under existing text cells.
        /// </summary>
        /// <param name="tableRows"></param>
        private void PrepareMajorForm(ref IEnumerable<TableRow> tableRows)
		{
			// Get a reference to a table row that does not have any text.
			// Use that empty row to fix up the entire table.
			TableRow emptyStringTableRow = tableRows
								.Where(ww => ww.PreviousSibling() != null
									   && ww.PreviousSibling().InnerText != null
									   && ww.PreviousSibling().InnerText.Contains("Introduction to Engineering"))
								.FirstOrDefault(tt => tt.InnerText != null && tt.InnerText.Trim() == "");

			TableRow introEngrRow = emptyStringTableRow.PreviousSibling<TableRow>();

			// The vertical gray separator on the page is different...
			emptyStringTableRow = tableRows.Where(ww => ww.PreviousSibling() != null
										&& ww.PreviousSibling().InnerText != null
										&& ww.PreviousSibling().InnerText.Contains("Assembly Language Programming"))
								.FirstOrDefault(tt => tt.InnerText != null && tt.InnerText.Trim() == "");

			TableRow currentRow = introEngrRow;
			TableRow nextRow;
			while (currentRow != null)
			{
				if (currentRow != null && !string.IsNullOrEmpty(currentRow.InnerText) && currentRow.NextSibling() != null)
				{
					if (currentRow.InnerText.ToUpper().Contains("SIGNATURE"))
					{
						currentRow = null;
						break;
					}

					nextRow = currentRow.NextSibling() as TableRow;
					if (nextRow == null)
						break;

					if (string.IsNullOrEmpty(nextRow.InnerText) || string.IsNullOrWhiteSpace(nextRow.InnerText))
					{
						currentRow = nextRow.NextSibling() as TableRow;
						continue;
					}
					else if (currentRow.InnerText.ToUpper().Contains("REQUIRED COURSES")
							|| currentRow.InnerText.ToUpper().Contains("TECHNICAL ELECTIVES")
							|| currentRow.InnerText.ToUpper().Contains("COURSES REQUIRED IN PREPARATION FOR THE MAJOR")
							|| currentRow.InnerText.ToUpper().Contains("TOTAL"))
					{
						currentRow = nextRow.NextSibling() as TableRow;
						continue;
					}
					else
					{
						currentRow.InsertAfterSelf(emptyStringTableRow.Clone() as TableRow);
						var tableRowHeight = from ro in currentRow.ChildElements
											 from rowPro in ro
											 where rowPro is TableRowProperties
											 from rowHeight in rowPro
											 where rowHeight is TableRowHeight
											 select rowHeight as TableRowHeight;
						foreach (var rowHeight in tableRowHeight)
						{
							if (rowHeight.Val.HasValue)
								rowHeight.Val = RowHeight;
						}
						var currentRowBorders = from ro in currentRow.ChildElements
												where ro is TableCell
												from tc in ro.ChildElements
												where tc is TableCellProperties
												from border in tc.ChildElements
												where border is TableCellBorders
												select border as TableCellBorders;
						foreach (var border in currentRowBorders)
							border.BottomBorder = null;
						// remove the bottom black cell border.                            
						if (currentRow.InnerText.Contains("Argument"))
						{
							currentRow = null;
							break;
						}
						currentRow = currentRow.NextSibling() as TableRow;
					}
				}
				currentRow = currentRow.NextSibling() as TableRow;
			}
		}
       
        /// <summary>
        /// Build a dictionary for the cells to edit in the future.
        /// </summary>
        /// <returns></returns>
        public bool BuildWritableCellsTable()
        {
            if (_cellsWithCourseName == null || _cellsWithCourseName.Count() == 0)
                return false;

            bool result = false;

            _lookupGrids = new Dictionary<string, Tuple<TableCell, TableCell, TableCell, TableCell, TableCell>>();

            IEnumerable<string> query;
            string key;

            foreach (var group in _cellsWithCourseName)
            {
                key = group.Key;

                // the key contains the entire text of the row
                query = from course in CourseFullName
                        let name = course.Replace(" ", "")
                        where key.Contains(name)
                        select course;

                // no matches
                if (query == null || query.Count() == 0)
                    continue;

                var rowOfCells = group.Cast<TableCell>().ToList();
                var nextRowOfCells = GetNextTableRow(key);

                foreach (string name in query)
                {
                    int startIndex = key.LastIndexOf(name.Replace(" ", ""));
                    if (startIndex >= 0)
                    {
                        // The first 5 tablecell belongs to this course.
                        if (rowOfCells == null || rowOfCells.Count < 5 || _lookupGrids.ContainsKey(name))
                            continue;

                        // Each cell is going to be mapped to each word in the name.
                        var splitOfName = name.Split(' ');

                        var beginTuple = from cell in rowOfCells
                                         let i = 0
                                         where cell.InnerText.Contains(splitOfName[i])
                                         select rowOfCells.IndexOf(cell);

                        if (beginTuple == null || beginTuple.Count() == 0)
                            continue;

                        int tt = beginTuple.FirstOrDefault();

                        _lookupGrids.Add(name,
                            Tuple.Create(
                                                rowOfCells[tt++],   //5
                                                rowOfCells[tt++],   //6
                                                rowOfCells[tt++],   //7
                                                rowOfCells[tt++],   //8
                                                rowOfCells[tt]      //9
                                        ));

                        tt = beginTuple.FirstOrDefault();

                        try
                        {
                            // This is throwing an exception for se - fall2015
                            if (tt + 5 < nextRowOfCells.Count && nextRowOfCells.Count > 5)
                            {
                                _lookupGrids.Add(name + "|EMPTY", Tuple.Create(
                                    nextRowOfCells[tt++],
                                    nextRowOfCells[tt++],
                                    nextRowOfCells[tt++],
                                    nextRowOfCells[tt++],
                                    nextRowOfCells[tt]
                                ));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }
                    }
                }
            }
            return result;
        }

        public string GetBestMatchCourseName(string searchKey)
        {
            string key = searchKey;

            if (key.StartsWith("TRLD") || key.Contains("Precalculus"))
                return "";

            key = key.Replace("Engr ", "Engineering ");
            key = key.Replace("Org & Arch", "Organization and Architecture");
            key = key.Replace("Strc", "Structures");
            key = key.Replace("Progrmng", "Programming");
            key = key.Replace("Comp ", "Computer ");
            key = key.Replace("Inter ", "Interaction ");
            key = key.Replace("Struct & Alg", "Structures and Algorithms");
            key = key.Replace("Soft ", "Software ");
            key = key.Replace("SW ", "Software");
            key = key.Replace("Diff Eq ", "Differential Equations ");
            key = key.Replace("Intro ", "Introduction ");
            key = key.Replace("Orntd ", "Oriented");
            key = key.Replace("Dsgn", "Design");
			
            // okay try to search by course name then course #.
            string[] splitKey = key.Split(' ');
            if (splitKey.Length > 3)
            {
                // search for the course #.
                var query = from number in CourseFullName
                            where number.Contains(splitKey[1])
                            select number as string;

                if (query != null)
                {
                    if (query.Count() == 1)
                        return query.FirstOrDefault();
                    else
                    {
                        var query2 = from name in query
                                     let first = name.Split(' ').FirstOrDefault()
                                     where !string.IsNullOrEmpty(first) &&
                                           ("SECMPECS".Contains(first) || key.StartsWith(first))
                                     select name as string;

                        if (query2 != null && query2.Count() == 1)
                            return query2.FirstOrDefault();
                    }
                }
            }

            // if its an SE course, it might fall under CMPE.
            if (key.Contains("SE"))
            {
                string cmpeKey = key.Replace("SE", "CMPE");
                string csKey = key.Replace("SE ", "CS ");
                string found = CourseFullName.Where(s => s.Equals(cmpeKey) || s.Equals(csKey)).FirstOrDefault();

                if (!string.IsNullOrEmpty(found))
                    return found;
            }

            diff_match_patch google = new diff_match_patch();

            Dictionary<string, int> leviDistances = new Dictionary<string, int>();

            for (int i = 0; i < CourseFullName.Count; i++)
            {
                var diff1 = google.diff_main(key, CourseFullName[i]);

                int leviDistance1 = google.diff_levenshtein(diff1);

                if (!leviDistances.ContainsKey(CourseFullName[i]))
                    leviDistances.Add(CourseFullName[i], leviDistance1);

                if (key.StartsWith("SE "))
                {
                    string cmpeKey = key.Replace("SE ", "CMPE ");

                    var diff2 = google.diff_main(cmpeKey, CourseFullName[i]);
                    int leviDistance2 = google.diff_levenshtein(diff2);

                    if (!leviDistances.ContainsKey(cmpeKey))
                        leviDistances.Add(cmpeKey, leviDistance2);

                    string csKey = key.Replace("SE ", "CS ");
                    var diff3 = google.diff_main(csKey, CourseFullName[i]);
                    int leviDistance3 = google.diff_levenshtein(diff3);

                    if (!leviDistances.ContainsKey(csKey))
                        leviDistances.Add(csKey, leviDistance3);

                }
            }

            string courseAbbr = key.Split(' ').FirstOrDefault();

            var sorted = leviDistances.Where(s => s.Key.StartsWith(courseAbbr, StringComparison.CurrentCultureIgnoreCase))
                                      .OrderBy(s => s.Value)
                                      .FirstOrDefault();

            if (sorted.Key == null && key.StartsWith("SE "))
            {
                sorted = leviDistances.Where(s => s.Key.StartsWith("CMPE ", StringComparison.CurrentCultureIgnoreCase))
                        .OrderBy(s => s.Value)
                        .FirstOrDefault();
            }

            if (sorted.Key != null)
            {
                return sorted.Key;
            }

            return "";
        }

		/// <summary>
		/// Clean up and get the row to write into student's the First, Last, Middle, Year name.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
        private Dictionary<string, Text> GetNameTextCells(ref OpenXmlElement table)
		{
			if (_doc == null)
				return null;

			Dictionary<string, Text> textCells = new Dictionary<string, Text>();

			var rowForName = from row in table.ChildElements
							 where row.InnerText.Contains(LastFirstMISidFlag) || row.InnerText.Contains(YearFlag)
							 from rowChild in row.ChildElements
							 where rowChild is TableCell && 
							 (LastFirstMISidFlag.Contains(rowChild.InnerText) || YearFlag.Equals(rowChild.InnerText))
							 from par in rowChild.ChildElements
							 where par is Paragraph
							 from run in par.ChildElements
							 where run is Run
							 from text in run.ChildElements
							 where text is Text
							 select text as Text;
			
			string textVal;
			foreach( var text in rowForName)
			{
				textVal = text.InnerText.Replace("#","");
				text.Text = textVal;

				if (string.IsNullOrEmpty(textVal))
					continue;

				if (!textCells.ContainsKey(textVal))
					textCells.Add(textVal, text);
			}
			return textCells;
		}

		/// <summary>
		/// Get the text box to write in the foot note.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
        private Dictionary<string, OpenXmlElement> GetFootNoteCell(ref OpenXmlElement table)
		{
			Dictionary<string, OpenXmlElement> footnote = new Dictionary<string, OpenXmlElement>();
			string footStr = "footnote";
            // THERES GOT TO BE A BETTER WAY TO DO THIS!!!!
			var footNoteAlternateContents = from tableRow in table.ChildElements
											where tableRow.InnerText.Contains(footStr)
											from tableCell in tableRow.ChildElements
											where tableCell.InnerText.Contains(footStr)
											from para in tableCell.ChildElements
											where para.InnerText.Contains(footStr)
											from run in para.ChildElements
											where run.InnerText.Contains(footStr)
											from ac in run.ChildElements
											where ac is AlternateContent
											select ac as AlternateContent;

			var textboxesFromGraphics = from ac in footNoteAlternateContents										
										from acChoice in ac.ChildElements
										from drawing in acChoice.ChildElements
										from anchor in drawing.ChildElements
										from graphic in anchor.ChildElements
										where graphic is DocumentFormat.OpenXml.Drawing.Graphic
										select graphic as DocumentFormat.OpenXml.Drawing.Graphic;

			var graphicData = from graphic in textboxesFromGraphics
							  from gData in graphic.ChildElements
							  where gData is DocumentFormat.OpenXml.Drawing.GraphicData
							  from wProcShape in gData.ChildElements
							  where wProcShape is WordprocessingShape
							  select wProcShape as WordprocessingShape;

            var textBoxParagraphs = from wordProcShape in graphicData
                                    from textBoxInfo2 in wordProcShape.ChildElements
                                    where textBoxInfo2 is TextBoxInfo2
                                    from content in textBoxInfo2.ChildElements
                                    where content.InnerText.Contains(footStr) && content is TextBoxContent
                                    from para in content.ChildElements
                                    where para is Paragraph
                                    select para as Paragraph;

            var textRuns = from para in textBoxParagraphs
                           from run in para.ChildElements
                           where run is Run
                           from text in run.ChildElements
                           where text is Text
                           select text as Text;

            string textVal;
            int i = 0;
            foreach(var text in textRuns)
            {
                textVal = text.Text;

                text.Text = "";

                if( i++ % 2 == 0)
                    footnote.Add($"footNote{i}", text);
            }
			return footnote;
		}

        /// <summary>
        /// Get the tablecells of the next sibling row.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private List<TableCell> GetNextTableRow(string key)
        {
            List<TableCell> ret = new List<TableCell>();
            try
            {
                // select the empty row that follows this row.
                var rowOfEmptyCells = (from rr in _tableRows
                                       where !string.IsNullOrEmpty(rr.InnerText)
                                           && rr.InnerText.Replace(" ", "") == key
                                       select rr.NextSibling() as TableRow
                                       ).FirstOrDefault();

                var emptyValueCells = from cell in rowOfEmptyCells.ChildElements
                                      where cell is TableCell &&
                                     (string.IsNullOrEmpty(cell.InnerText)
                                   || string.IsNullOrWhiteSpace(cell.InnerText))
                                      select cell as TableCell;

                ret = emptyValueCells.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return ret;
        }

		/// <summary>
		/// Get the cells ...
		/// </summary>
		/// <param name="texts"></param>
		/// <returns></returns>
		private List<TableCell> GetTargetColumnCells(List<string> texts, out String innerTextOfRow)
		{
			List<TableCell> list = new List<TableCell>();
			innerTextOfRow = "";

			var targetTableRow = from row in _tableRows
								 where !string.IsNullOrEmpty(row.InnerText)
								 let count = SearchInnerTextList(row.InnerText, texts)
								 where count > 2 // need at least 3 of the 4 matching...
								 orderby count descending
								 select row;

			if (targetTableRow != null && targetTableRow.Count() > 0)
			{
				innerTextOfRow = targetTableRow.FirstOrDefault().InnerText;
				//Get the columns of that row.
				var rowColumns = targetTableRow
									.FirstOrDefault().ChildElements
									.Where(c => c is TableCell)
									.Select(c => c as TableCell);

				if (rowColumns != null && rowColumns.Count() > 0)
					list = rowColumns.ToList();
			}
			
			return list;
		}


		/// <summary>
		/// Strikes out all text are children of the passed in parameter.
		/// </summary>
		/// <param name="item">Text, TableCell, Run, or Paragraph</param>
		/// <returns></returns>
        private bool StrikeOutText(OpenXmlElement item)
        {
            List<Text> texts = new List<Text>();

            int processedText = 0;

            if (item is Text)
            {
                texts.Add(item as Text);
            }
            else if (item is TableCell)
            {
                var itemTexts = from pg in item.ChildElements
                                where pg is Paragraph
                                from rr in pg.ChildElements
                                where rr is Run
                                from tt in rr.ChildElements
                                where tt is Text
                                select tt as Text;

                foreach (var t in itemTexts)
                {
                    texts.Add(t);
                }
            }
            else if (item is Run)
            {
                var itemTexts = from t in item.ChildElements
                                where t is Text
                                select t as Text;
                foreach (var t in itemTexts)
                {
                    texts.Add(t);
                }
            }
			else if (item is Paragraph)
			{
				var para = item as Paragraph;
				var runs = para.ChildElements.Where(c => c is Run);
				foreach ( var run in runs )
				{
					var tx = run.ChildElements.Where(t => t is Text).Select(t => t as Text);
					if (tx != null)
						texts.AddRange(tx);
				}
			}

			foreach (var txt in texts)
            {
                if (string.IsNullOrEmpty(txt.InnerText))
                    continue;

                Run parent = txt.Parent as Run;

                if (parent == null)
                    continue;

                parent.RunProperties.Strike = new Strike();
                ++processedText;
            }

            return processedText > 0;
        }

		/// <summary>
		/// Automatically append the asterisks and write in the instituion that the course was taken at.
		/// </summary>
		/// <param name="value">Institution's name</param>
		/// <returns>Count of asterisks preappended to the name</returns>
        public int WriteFootNote(string value)
        {
			if (_footNoteCells == null)
				_footNoteCells = GetFootNoteCell(ref _page1Table);
			string name = value.Replace("*", ""); // remove existing asterisks.
            int count = 0;		
            if( _footNoteCells != null )
            {
                try
                {
                    var keys = _footNoteCells.Keys.ToList();
                    var keyToInsert = from text in _footNoteCells
                                      let textBox = text.Value as Text
                                      where string.IsNullOrEmpty(textBox.Text)
                                      let indexOfTextBox = keys.IndexOf(text.Key)
                                      select indexOfTextBox;
                    string key = keys[keyToInsert.FirstOrDefault()];
                    int numOfAskerists = keys.IndexOf(key) + 1;
                    (_footNoteCells[key] as Text).Text = new string('*', numOfAskerists) + name;
                    count++;
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while writing to foot note. " + ex.StackTrace);
                }
            }

            return count;
        }

        public bool WriteGradeToSJSUCourse(string key, string grade)
        {
            if (_writtenToCells == null)
                _writtenToCells = new Dictionary<string, TableCell>();

            if (_lookupGrids == null)
            {
                ReadGridTable();
                BuildWritableCellsTable();
            }

            var exactMatch = CourseFullName.FirstOrDefault(k => k.ToLower().Trim() == key.ToLower().Trim());

            if (string.IsNullOrEmpty(exactMatch))
            {
                string bestMatchKey = GetBestMatchCourseName(key);

                if (!string.IsNullOrWhiteSpace(bestMatchKey))
                    WriteGrade(bestMatchKey, grade);
            }
            else
            {
                if (_lookupGrids.ContainsKey(key))
                    WriteGrade(key, grade);
            }

            return false;
        }

        public bool WriteGrade(string key, string grade)
        {
            if (_lookupGrids == null || !_lookupGrids.ContainsKey(key))
                return false;

            var value = _lookupGrids[key];

            var gradeText = value.Item5.ChildElements.FirstOrDefault(p => p is Paragraph);

            var neighborRun = (from item in value.Item4.ChildElements
                               where item is Paragraph
                               from ru in item.ChildElements
                               where ru is Run
                               select ru as Run).FirstOrDefault();

            string fontSizeOfRun = neighborRun != null &&
                                   neighborRun.RunProperties != null &&
                                   neighborRun.RunProperties.FontSize != null ? neighborRun.RunProperties.FontSize.Val.ToString() :
                                   "14";

            Text t = new Text();

            string gradeVal = grade;

            if (grade.Length > 1)
            {
                // get a count of occurences.
                var occurences = from g in gradeVal
                                 group g by g;

                foreach (var group in occurences)
                {
                    var letter = group.Key;

                    if (group.Count() > 1)
                    {
                        int indexOfLetter = gradeVal.IndexOf(letter);

                        string firstHalf = gradeVal.Substring(0, indexOfLetter);
                        string secondHalf = grade.Substring(indexOfLetter + 1);
                        secondHalf = secondHalf.Replace(letter.ToString(), "");
                        gradeVal = firstHalf + secondHalf;
                    }
                }
            }

            t.Text = gradeVal;

            Run r = new Run();
            r.Append(t);
            r.RunProperties = new RunProperties();
            r.RunProperties.FontSize = new FontSize() { Val = fontSizeOfRun };

            gradeText.AppendChild(r);

            if (_writtenToCells != null && !_writtenToCells.ContainsKey(key))
            {
                _writtenToCells.Add(key, _lookupGrids[key].Item5);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Key-> {key} Grade-> {grade}");
            }

            return true;
        }

        /// <summary>
        /// Write first, middle, last name.
        /// </summary>
        /// <param name="key">first, fn, middle, mi, last, ln</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteNameYear(string key, string value)
        {
            int writtenCount = 0;

            string dest = key.ToLower();
            string dictKey = "";

            try
            {
                if (dest.Contains("first") || dest.Contains("fn"))
                    dictKey = _studentNameTable.Keys.FirstOrDefault(fn => fn.Contains("first"));
                else if (dest.Contains("last") || dest.Contains("ln"))
                    dictKey = _studentNameTable.Keys.FirstOrDefault(fn => fn.Contains("last"));
                else if (dest.Contains("middle") || dest.Contains("mi"))
                    dictKey = _studentNameTable.Keys.FirstOrDefault(fn => fn.Contains("mi"));
                else if (dest.Contains("year"))
                    dictKey = _studentNameTable.Keys.FirstOrDefault(fn => fn.Contains("year"));
                else if (dest.Contains("studentid") || dest.Contains("id"))
                    dictKey = _studentNameTable.Keys.FirstOrDefault(fn => fn.Contains("student"));

                if (!string.IsNullOrEmpty(dictKey))
                {
                    _studentNameTable[dictKey].Text = value;
                    writtenCount++;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }

            return writtenCount > 0;
        }

        public bool WriteRow(string key, string[] vals)
        {
            if (vals.Length < 4)
                return false;

            return WriteValueToRow(key, vals[0], vals[1], vals[2], vals[3], vals[4]);
        }

        /// <summary>
        /// Strike out the SJSU course and write the value of the transfered course into the bottom row.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val1">Course Abbreviation</param>
        /// <param name="val2">Course Number</param>
        /// <param name="val3">Course Title</param>
        /// <param name="val4">Course Units</param>
        /// <param name="val5">Grade</param>
        /// <returns></returns>
        private bool WriteValueToRow(string key, string val1, string val2, string val3, string val4, string val5)
        {
            bool result = false;

            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                return result;

            string dictKey = key.Replace("|EMPTY", "");
            string dictKeyEmpty = key + "|EMPTY";

            var rowTop = _lookupGrids[dictKey];
            var rowBot = _lookupGrids[dictKeyEmpty];

            /* To write values for "transfer courses"
			 * 
			 *		----------------------------------------------------------
			 *		[ some sjsu course			]   |
			 *		[ transfer course           ]   |
			 *      -----------------------------------------------------------
			 *  The transfer course row is initially empty.
			 *	To handle this programmatically:
			 *	1. Create the transfer course by cloning the elements in the top row.
			 *  2. Fill in the values
			 *  3. Strike out the top row ONLY after all the elements in the top row is filled out.
			 * 
			 */
            CloneTopAndWriteIntoBot(rowTop.Item1, rowBot.Item1, val1);
            CloneTopAndWriteIntoBot(rowTop.Item2, rowBot.Item2, val2);
            CloneTopAndWriteIntoBot(rowTop.Item3, rowBot.Item3, val3);
            CloneTopAndWriteIntoBot(rowTop.Item4, rowBot.Item4, val4);
            CloneTopAndWriteIntoBot(rowTop.Item5, rowBot.Item5, val5);

            StrikeOutText(rowTop.Item1);
            StrikeOutText(rowTop.Item2);
            StrikeOutText(rowTop.Item3);
            StrikeOutText(rowTop.Item4);
            StrikeOutText(rowTop.Item5);

            return result;
        }

		private int SearchInnerTextList(string text, List<string> list)
		{
			int count = 0;
			for (int i = 0; i  < list.Count; i++)
			{
				int found = text.IndexOf(list[i], StringComparison.CurrentCultureIgnoreCase);
				if (found > 0)
					count++;
			}
			return count;
		}

		public bool WriteCourseToRow(CourseModel cm)
		{
			string textOfTargetRow = "";
			int processed = 0;
			/* Cases
			 * 1. SJSU course - if the CourseModel is not flagged as IsTransferCourse 
			 *		1.a Some CmpE & CS course abbreviation must be striked and SE written to the cell below it.
			 *		1.b For all others, just write the grade in.
			 * 
			 *
			 * 2. Transfer course -  
			 *
			 */
			List<string> texts = new List<string>()
			{
				cm.CourseAbbreviation,
				cm.CourseNumber,
				cm.CourseTitle,
				cm.CourseUnit,
			};

			//Find the target TableRow.			
			var rowColumns = GetTargetColumnCells(texts, out textOfTargetRow);
			if ( rowColumns.Count == 0 && cm.IsTransferCourse )
			{
				// did not find a match, then it could be a transfer course.
				CourseModel sjsuCM = cm.SJSUCourse;
				texts = new List<string>()
				{
					sjsuCM.CourseAbbreviation,
					sjsuCM.CourseNumber,
					sjsuCM.CourseTitle,
					sjsuCM.CourseUnit
				};
				rowColumns = GetTargetColumnCells(texts, out textOfTargetRow);
			}

			if (rowColumns.Count == 0)
				throw new Exception("Failed to get column cells to write : " + cm.ToString());

			//A row has 10 columns, need to select only the columns that belong to this CourseModel.						   
			int startingIndexOfCourseName = -1;
			for( int i = 0; i < rowColumns.Count; i++)
			{
				if (string.IsNullOrEmpty(rowColumns[i].InnerText))
					continue;

				if (rowColumns[i].InnerText.StartsWith(cm.CourseAbbreviation, StringComparison.CurrentCultureIgnoreCase))
				{
					startingIndexOfCourseName = i;
					break;
				}
				else if(cm.SJSUCourse != null && rowColumns[i].InnerText.StartsWith(cm.SJSUCourse.CourseAbbreviation, StringComparison.CurrentCultureIgnoreCase))
				{
					startingIndexOfCourseName = i;
					break;
				}
			}

			if (startingIndexOfCourseName < 0)
				throw new Exception("Exception while searching for columns to insert " + cm.ToString());

			//If the startingIndexIs at the mid point, then the ending point is the count of columns.
			startingIndexOfCourseName = startingIndexOfCourseName >= rowColumns.Count / 2 ? startingIndexOfCourseName : 0;
			int end = startingIndexOfCourseName == 0 ? rowColumns.Count / 2: rowColumns.Count - 1;

			//Have the target columns:
			var targetColumns = rowColumns
								.Where( s => rowColumns.IndexOf(s) >= startingIndexOfCourseName && rowColumns.IndexOf(s) <= end)
								.ToList();
			//Need to write the course abbr, no, title, units, and grade into the row below the existing similar course.
			if( cm.IsTransferCourse )
			{
				//write in the foot note.
				int numOfAsterisk = WriteFootNote(cm.Institution);
				//todo get a better way to find the indexes of enumerators.
				int indexOfNextRow = 0;
				for(int i = 0; i < _tableRows.Count(); i++)
				{
					if( _tableRows.ElementAt(i).InnerText == textOfTargetRow )
					{
						indexOfNextRow = i + 1;
						break;
					}
				}

				var nextTableRow = (_tableRows.ElementAt(indexOfNextRow) as TableRow)
									.ChildElements
									.Where( c => c is TableCell)
									.Select( c=> c as TableCell).ToList();
				var nextTableColumns = nextTableRow
										.Where(c => nextTableRow.IndexOf(c) >= startingIndexOfCourseName &&
													nextTableRow.IndexOf(c) >= startingIndexOfCourseName).ToList();
				
				for(int i = 0; i < nextTableColumns.Count; i++)
				{
					string val = "";
					switch (i)
					{
						case 0:
							val = cm.CourseAbbreviation;
							break;
						case 1:
							val = cm.CourseNumber;
							break;
						case 2:
							val = new string('*', numOfAsterisk) + cm.CourseTitle; // need to preappend the asterisks.
							break;
						case 3:
							val = cm.CourseUnit;
							break;
						case 4:
							val = cm.CourseGrade;
							break;
					}

					var paragraph = nextTableColumns[i].ChildElements.Where(c => c is Paragraph).FirstOrDefault();
					Run run = GetRunForText(val);
					paragraph.Append(run);

					if( targetColumns[i] != null )
					{
						// strike out the top row.
						var strikePara = targetColumns[i].ChildElements
										.Where(c => c is Paragraph)
										.Select(c => c as Paragraph)
										.FirstOrDefault();
						StrikeOutText(strikePara);
					}


					processed++;
				}
			}			
			return processed > 0;
		}

		private Run GetRunForText(string text)
		{
			Text t = new Text(text);
			Run r = new Run(t);

			r.RunProperties = new RunProperties()
			{
				FontSize = new FontSize() { Val = "14" },
				RunFonts = new RunFonts() { Ascii = "Arial" }
			};

			return r;
		}

        public string FileName { get; set; }
		public string FileLocation { get; set; }
		public string FileVersion { get; set; }
	}
}
