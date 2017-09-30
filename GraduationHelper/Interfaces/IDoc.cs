using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationHelper.Interfaces
{
	public interface IDoc
	{
		string FileName { set; get; }
		string FileLocation { set; get; }
		string FileVersion { set; get; }
	}
}
