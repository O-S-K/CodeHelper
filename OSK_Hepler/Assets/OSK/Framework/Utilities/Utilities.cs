using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK.Utils
{
    public static class Utilities
    { 

        public delegate TResult MapFunc<out TResult, TArg>(TArg arg);
        public delegate bool FilterFunc<TArg>(TArg arg);


        public static List<TOut> Map<TIn, TOut>(List<TIn> list, MapFunc<TOut, TIn> func)
        {
            List<TOut> newList = new List<TOut>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                newList.Add(func(list[i]));
            }

            return newList;
        }

        public static void Filter<T>(List<T> list, FilterFunc<T> func)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (func(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }

        public static void SwapValue<T>(ref T value1, ref T value2)
        {
            T temp = value1;
            value1 = value2;
            value2 = temp;
        }

        /// <summary>
        /// Returns to mouse position
        /// </summary>
        public static Vector2 MousePosition()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            return (Vector2)Input.mousePosition;
#else
			if (Input.touchCount > 0)
			{
				return Input.touches[0].position;
			}

			return Vector2.zero;
#endif
        }

        /// <summary>
        /// Returns true if a mouse down event happened, false otherwise
        /// </summary>
        public static bool MouseDown()
        {
            return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began);
        }

        /// <summary>
        /// Returns true if a mouse up event happened, false otherwise
        /// </summary>
        public static bool MouseUp()
        {
            return (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended));
        }

        /// <summary>
        /// Returns true if no mouse events are happening, false otherwise
        /// </summary>
        public static bool MouseNone()
        {
            return (!Input.GetMouseButton(0) && Input.touchCount == 0);
        }

        public static char CharToLower(char c)
        {
            return (c >= 'A' && c <= 'Z') ? (char)(c + ('a' - 'A')) : c;
        }

        public static int GCD(int a, int b)
        {
            int start = Mathf.Min(a, b);

            for (int i = start; i > 1; i--)
            {
                if (a % i == 0 && b % i == 0)
                {
                    return i;
                }
            }

            return 1;
        }

        public static string CalculateMD5Hash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            CustomDebug.Log("CalculateMD5Hash", sb.ToString());
            return sb.ToString();
        }

        public static bool CompareLists<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = list1.Count - 1; i >= 0; i--)
            {
                bool found = false;

                for (int j = 0; j < list2.Count; j++)
                {
                    if (list1[i].Equals(list2[j]))
                    {
                        found = true;
                        list1.RemoveAt(i);
                        list2.RemoveAt(j);
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        public static void PrintList<T>(List<T> list)
        {
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                {
                    str += ", ";
                }
                str += list[i].ToString();
            }
            Debug.Log(str);
        }
    }
}
