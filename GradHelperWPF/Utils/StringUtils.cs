using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradHelperWPF.Utils
{
	public class StringUtils
	{
		private static StringBuilder _strBuilder = new StringBuilder();

		public static String CapFirstLetterRemoveNums(string str)
		{
			if (String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str))
				return "";

			var letters = str.Where(letter => Char.IsLetter(letter)).ToArray();

			for (int i = 0; i < letters.Length; i++)
			{
				if (i == 0)
					_strBuilder.Append(letters[i].ToString().ToUpper());
				else
					_strBuilder.Append(letters[i]);
			}

			String value = _strBuilder.ToString();

			_strBuilder.Clear();

			return value;
		}

		public static String RemoveAllLetters(string str)
		{
			var nums = str.Where(n => Char.IsDigit(n)).ToArray();

			for(int i = 0; i < nums.Length; i++)
			{
				_strBuilder.Append($"{nums[i]}");
			}

			string val = _strBuilder.ToString();

			_strBuilder.Clear();

			return val;
		}
	}
}
