using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OSK.Utils
{
    public static class StringUtils
    {
        public static string[] Split(string strValue, string splitValue)
        {
            return strValue.Split(new string[] { splitValue }, StringSplitOptions.None);
        }

        public static string[] SplitParams(string strValue)
        {
            return strValue.Split(new char[] { ',' }, StringSplitOptions.None);
        }

        public static List<int[]> GetNumbersFromString(string input)
        {
            List<int[]> resultList = new List<int[]>(); // List to store arrays

            // Split input by comma to get individual parts
            string[] parts = input.Split(',');

            // Iterate through each part
            foreach (string part in parts)
            {
                // Clean part to remove non-numeric characters
                string cleaned = System.Text.RegularExpressions.Regex.Replace(part, @"[^0-9]", "");

                // Split cleaned part into number strings
                string[] numberStrings = cleaned.Split('n', 'c');

                // Create list to store numbers in this part
                List<int> numbers = new List<int>();

                // Convert number strings to integers and add to list
                foreach (string numString in numberStrings)
                {
                    if (int.TryParse(numString, out int number))
                    {
                        numbers.Add(number);
                    }
                    else
                    {
                        // Handle invalid number if needed
                        Console.WriteLine($"Cannot convert '{numString}' to integer.");
                    }
                }

                // Convert list of numbers to array and add to result list
                resultList.Add(numbers.ToArray());
            }

            return resultList;
        }

        // public static string[] GetNumbersFromString(string input)
        // {
        //     //  Remove all non-digit characters
        //     string cleaned = System.Text.RegularExpressions.Regex.Replace(input, @"[^0-9,]", "");
        //     string[] numbers = cleaned.Split(',');
        //     return numbers;
        // }
  
        public static string RandomString(string characters, int length)
        {
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = characters[UnityEngine.Random.Range(0, characters.Length)];
            }

            return new string(stringChars);
        }

        public static int ConvertExcelToInt(string input)
        {
            string cleanedInput = Regex.Replace(input.ToString(), @"[^\d.-]", "");

            // Try to parse the cleaned string to a float first
            if (float.TryParse(cleanedInput, out float floatResult))
            {
                // Convert the float to an integer by rounding
                return Mathf.RoundToInt(floatResult);
            }
            else
            {
                Debug.LogWarning($"Unable to convert '{cleanedInput}' to a number.");
                return -999; // Default value if conversion fails
            }
        }

        public static float ConvertExcelToFloat(string input)
        {
            string cleanedInput = Regex.Replace(input.ToString(), @"[^\d.-]", "");

            // Try to parse the cleaned string to a float first
            if (float.TryParse(cleanedInput, out float floatResult))
            {
                // Convert the float to an integer by rounding
                return floatResult;
            }
            else
            {
                Debug.LogWarning($"Unable to convert '{cleanedInput}' to a number.");
                return -999; // Default value if conversion fails
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

            var dividor = Mathf.Pow(10, suffixIndex * 3); // actual number we print
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