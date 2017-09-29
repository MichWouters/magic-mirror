using System;
using System.IO;

namespace Acme.Generic
{
    public class FileWriter
    {
        public static void WriteJsonToFile(string json, string fileName, string path)
        {
            try
            {
                File.WriteAllText($"{path}/{fileName}", json);
            }
            catch (Exception e)
            {
                throw new IOException("Unable to write text to file", e);
            }
        }

        public static string ReadFromFile(string path, string fileName)
        {
            try
            {
                string result = File.ReadAllText($"{path}/{fileName}");
                return result;
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException($"{fileName} does not exist", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not read text from {fileName}", e);
            }
        }

        public static void AppendToFile(string path, string fileName)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new IOException("Could not append text to file", e);
            }
        }

        public static void DeleteFile(string path, string fileName)
        {
            try
            {
                File.Delete($"{path}/{fileName}");
            }
            catch (Exception e)
            {
                throw new IOException("Could not delete file", e);
            }
        }
    }
}