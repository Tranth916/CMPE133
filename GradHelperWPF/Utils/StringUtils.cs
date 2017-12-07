using System.Linq;
using System.Text;

namespace GradHelperWPF.Utils
{
    public class StringUtils
    {
        private static readonly StringBuilder _strBuilder = new StringBuilder();

        public static string CapFirstLetterRemoveNums( string str )
        {
            _strBuilder.Clear( );

            if ( string.IsNullOrEmpty( str ) || string.IsNullOrWhiteSpace( str ) )
                return "";

            if ( str.ToLower( ) == "to" )
                return "to";

            var letters = str.Where(letter => char.IsLetter(letter) || letter == ' ').ToArray();

            var nextLetterNeedsCap = 1;

            for ( var i = 0; i < letters.Length; i++ )
                if ( letters[i] == ' ' )
                {
                    _strBuilder.Append( letters[i] );
                    nextLetterNeedsCap = 1;
                }
                else if ( nextLetterNeedsCap == 1 )
                {
                    _strBuilder.Append( letters[i].ToString( ).ToUpper( ) );
                    nextLetterNeedsCap = 0;
                }
                else
                {
                    _strBuilder.Append( letters[i] );
                }

            var value = _strBuilder.ToString().Trim();

            _strBuilder.Clear( );

            return value;
        }

        public static string FormatPhoneNumber( string str )
        {
            _strBuilder.Clear( );

            var numbers = str.Where(n => char.IsDigit(n)).ToArray();

            if ( numbers == null || numbers.Count( ) == 0 )
                return "";

            _strBuilder.Append( "(" );

            // ( n n n )   n n n - n n n n
            // 0 1 2 3 4 5 6 7 8 9
            for ( var i = 0; i < numbers.Length; i++ )
            {
                if ( i == 3 )
                    _strBuilder.Append( ") " );

                if ( i == 6 )
                    _strBuilder.Append( "-" );

                _strBuilder.Append( numbers[i] );
            }

            var val = _strBuilder.ToString().Trim();

            _strBuilder.Clear( );

            if ( val == "(" )
                return "";

            return val;
        }

        public static string RemoveAllLetters( string str )
        {
            var nums = str.Where(n => char.IsDigit(n)).ToArray();

            for ( var i = 0; i < nums.Length; i++ )
                _strBuilder.Append( $"{nums[i]}" );

            var val = _strBuilder.ToString();

            _strBuilder.Clear( );

            return val;
        }
    }
}