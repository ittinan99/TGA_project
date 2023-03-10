using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TGA.LocalFile
{
    public static class LocalFileManager
    {

        public static void SaveToLocal<T>(T dataObject, string folder, string filename)
        {
            var jsonString = JsonConvert.SerializeObject(dataObject);
            SaveToLocal(jsonString, folder, filename);
        }

        public static void SaveToLocal(string jsonData, string folder, string filename)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, $"{filename}");

            using (var stringWriter = new StreamWriter(path))
            {
                stringWriter.Write(jsonData);
                stringWriter.Flush();
            }
        }

        public static string LoadFromPersistentDataPath(string filename)
        {
            var filepath = Path.Combine(Application.persistentDataPath, filename);

            if (!File.Exists(filepath)) return null;

            return File.ReadAllText(filepath);
        }

    }
}
