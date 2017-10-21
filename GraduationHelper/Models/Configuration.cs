using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper.Interfaces;

namespace GraduationHelper.Models
{
	public class Configuration : IConfig
	{
		string IConfig.ConfigFilePath
		{
			set; get;
		}

		DateTime IConfig.LastDateSave
		{
			set; get;
		}
		
		public Configuration()
		{

		}	
	}
}
