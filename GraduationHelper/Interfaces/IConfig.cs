using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationHelper.Interfaces
{
	public interface IConfig
	{
		string ConfigFilePath
		{
			set; get;
		}
		DateTime LastDateSave
		{
			set; get;
		}
	}
}
