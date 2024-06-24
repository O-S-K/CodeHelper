using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK.Save
{
    public static class PathFile
    {
        public static string Path(string fileName, bool isPathToDocument)
        {
            if(isPathToDocument)
            {
#if UNITY_ANDROID
          return  GetAndroidDocumentsPath(fileName);
#elif UNITY_IPHONE
                return GetEditorDocumentsPath(fileName);
#else
                return GetEditorDocumentsPath(fileName);
#endif
            }
            else
            {
                return $"Assets/{fileName}";
            }
        } 

        public static string PathFolder
        {
#if UNITY_ANDROID
            get => Application.dataPath.ToString();
#elif UNITY_IPHONE
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            get => (path + "/Documents").ToString();
#else
            get => Application.persistentDataPath.ToString();
#endif
        }

        public static string GetEditorDocumentsPath(string fileName)
        {
            string path = Application.dataPath + fileName;
            return path;
        }

        public static string GetIPhoneDocumentsPath(string fileName)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents" + fileName;
        }

        public static string GetAndroidDocumentsPath(string fileName)
        {
            string path = Application.persistentDataPath + fileName;
            return path;
        }
    }
}
