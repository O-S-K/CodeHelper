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

		public static string SplitStringByMaxChars(string strValue, int maxChars)
		{
			const string splittedChars = "...";
			maxChars += splittedChars.Length;

			if ( strValue.Length+splittedChars.Length > maxChars )
			{
				strValue = strValue.Substring(0, maxChars-splittedChars.Length )+splittedChars;
			}
			return strValue;
		}

		public static string SplitStringByStartEnd (string strValue, string start, string end)
		{
			if (strValue.Contains(start))
			{
				string[] array = strValue.Split(new string[]{start}, StringSplitOptions.None);
				strValue = array[array.Length-1];
			}

			if (strValue.Contains(end))
			{
				strValue = strValue.Split(new string[]{end}, StringSplitOptions.None)[0];
			}

			return strValue;
		}

		public static string UppercaseFirstLetter(string value)
		{
			if (string.IsNullOrEmpty(value) || value.Length == 0)
				return value;
			
			if (value.Length == 1)
				return value.ToUpper();
			
			return string.Format("{0}{1}", value.Substring(0, 1).ToUpper(), value.Substring(1, value.Length-1).ToLower());
		}

        public static string LowercaseFirstLetter(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length == 0)
                return value;

            if (value.Length == 1)
                return value.ToLower();

            return string.Format("{0}{1}", value.Substring(0, 1).ToLower(), value.Substring(1, value.Length - 1));
        }

		public static string ReplaceHtmlTags(string value)
		{
			string result = value.Replace("<", "\u3008");
			result = result.Replace(">", "\u3009");
			return result;
		}
		
		public static string PrepareTextForBubble(string text, int maxLines, int maxLineLength, string ellipsis)
        {
            string[] words = text.Split(' ');
            List<string> result = new List<string>();
            int index = 0, length = 0;
            bool wordCut = true;
            for (int i = 0; i <= maxLines; i++)
            {
                if (i == maxLines)
                {
                    if (!wordCut)
                    {
                        result.Add(" ");
                        length++;
                        while (true)
                        {
                            if (length == 0 || length + ellipsis.Length <= maxLineLength)
                            {
                                result.Add(ellipsis);
                                break;
                            }
                            length -= result[result.Count - 2].Length + 1;
                            result.RemoveRange(result.Count - 2, 2);
                        }
                    }
                    break;
                }
                if (i > 0)
                {
                    result.Add("\n");
                }
                length = 0;
                wordCut = false;
                while (index < words.Length)
                {
                    string word = words[index];
                    bool extraSpace = length > 0 && word.Length > 0;
                    if (length + (extraSpace ? 1 : 0) + word.Length <= maxLineLength)
                    {
                        if (extraSpace)
                        {
                            result.Add(" ");
                            length++;
                        }
                        result.Add(word);
                        length += word.Length;
                    }
                    else
                    {
                        break;
                    }
                    index++;
                }
                if (length == 0 && index < words.Length)
                {
                    result.Add(words[index].Substring(0, Math.Max(1, maxLineLength - ellipsis.Length)));
                    result.Add(ellipsis);
                    index++;
                    wordCut = true;
                }
                if (index == words.Length)
                {
                    break;
                }
            }
            return String.Join("", result.ToArray());
        }

	    private readonly static StringBuilder _stringBuilder = new StringBuilder();

        public static string Concat(params string[] texts)
        {
            _stringBuilder.Length = 0;

            for (int i = 0; i < texts.Length; i++)
            {
                _stringBuilder.Append(texts[i]);
            }
            return _stringBuilder.ToString();
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