using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;

namespace OSK.Save
{
    public static class JsonUtils
    {
        // Todo: Call For Mobile Devices
        // onApplicationPause() => SaveJson

        public static void SaveJson(object data, string fileName, bool isSaveToDocument = true)
        {
            string json = JsonUtility.ToJson(data);
            CustomDebug.Log("Path json: " + fileName);
            FileUtils.WriteToFile(fileName, json, isSaveToDocument);
            refreshEditor();
        }

        public static void LoadJson(object data, string fileName, bool isSaveToDocument = true)
        {
            string json = FileUtils.ReadFromFile(fileName, isSaveToDocument);
            CustomDebug.Log("Load Json: " + json);
            JsonUtility.FromJsonOverwrite(json, data);
            refreshEditor();
        }


        private static void refreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public static string ConvertToJsonString(object data, bool addQuoteEscapes = false)
        {
            string jsonString = "";

            if (data is IDictionary)
            {
                Dictionary<string, object> dic = data as Dictionary<string, object>;

                jsonString += "{";

                List<string> keys = new List<string>(dic.Keys);

                for (int i = 0; i < keys.Count; i++)
                {
                    if (i != 0)
                    {
                        jsonString += ",";
                    }

                    if (addQuoteEscapes)
                    {
                        jsonString += string.Format("\\\"{0}\\\":{1}", keys[i], ConvertToJsonString(dic[keys[i]], addQuoteEscapes));
                    }
                    else
                    {
                        jsonString += string.Format("\"{0}\":{1}", keys[i], ConvertToJsonString(dic[keys[i]], addQuoteEscapes));
                    }
                }

                jsonString += "}";
            }
            else if (data is IList)
            {
                IList list = data as IList;

                jsonString += "[";

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != 0)
                    {
                        jsonString += ",";
                    }

                    jsonString += ConvertToJsonString(list[i], addQuoteEscapes);
                }

                jsonString += "]";
            }
            else if (data is string)
            {
                // If the data is a string then we need to inclose it in quotation marks
                if (addQuoteEscapes)
                {
                    jsonString += "\\\"" + data + "\\\"";
                }
                else
                {
                    jsonString += "\"" + data + "\"";
                }
            }
            else if (data is bool)
            {
                jsonString += (bool)data ? "true" : "false";
            }
            else
            {
                // Else just return what ever data is as a string
                jsonString += data.ToString();
            }

            return jsonString;
        }
    }
}
