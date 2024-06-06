using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OSK.Utils
{
	public static class StringUtils
    {
		public static string[] Split(string strValue, string splitValue)
		{
			return strValue.Split(new string[]{splitValue}, StringSplitOptions.None);
		}
  
        public static int ConvertExcelToInt(string input)
        {
            string cleanedInput = input.Replace(" ", "");
            if (int.TryParse(cleanedInput, out int result))
            {
                return result;
            }
            else
            {
                Debug.LogWarning($"Unable to convert '{cleanedInput}' to an integer.");
                return 0; // Default value if conversion fails
            }
        }

        public static string ConvertNamesLargeNumbers(long number)
        {
            // url: https://en.wikipedia.org/wiki/Names_of_large_numbers
            if (number >= 1000000000)
            {
                return (number / 1000000000).ToString() + "B";
            }
            if (number >= 1000000)
            {
                return (number / 1000000).ToString() + "M";
            }
            if (number >= 100000)
            {
                return (number / 1000).ToString() + "K";
            }
            return number.ToString();
        }

        public static string LongToString(long cash, string prefix = "", string suffixTimeUnit = "")
        {
            string[] suffixes = { "", "k", "m", "b" };
            int suffixIndex;
            int digits;
            if (cash == 0)
            {
                suffixIndex = 0; // log10 of 0 is not valid
                digits = cash.ToString().Length;
            }
            else if (cash > 0)
            {
                suffixIndex = (int)(Mathf.Log10(cash) / 3); // get number of digits and divide by 3
                digits = cash.ToString().Length;
            }
            else
            {
                suffixIndex = (int)(Mathf.Log10(Math.Abs(cash)) / 3);
                digits = Math.Abs(cash).ToString().Length;
            }

            var dividor = Mathf.Pow(10, suffixIndex * 3);  // actual number we print
            var text = "";


            if (digits < 4)
            {
                text = prefix + (cash / dividor).ToString() + suffixes[suffixIndex] + suffixTimeUnit;
            }
            else if (digits >= 4 && digits < 7)
            {
                text = prefix + (cash / dividor).ToString("F2") + suffixes[suffixIndex] + suffixTimeUnit;
            }
            else
            {
                text = prefix + (cash / dividor).ToString("F3") + suffixes[suffixIndex] + suffixTimeUnit;
            }
            return text;
        }
    }
}