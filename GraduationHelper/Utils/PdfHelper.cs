using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
namespace GraduationHelper.Utils
{
	public class PdfHelper : PdfWriter
	{
		public static readonly string CourseRegex = @"\d{1,}[AB]$";
		public static readonly string YearRegex = @"\d{4}$";




	}
}
