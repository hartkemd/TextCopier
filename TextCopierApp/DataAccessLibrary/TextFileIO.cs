using System.IO;

namespace DataAccessLibrary
{
    public static class TextFileIO
    {
        public static void WriteTextToFile(string filePath, string textToWrite)
        {
            File.WriteAllText(filePath, textToWrite);
        }

        public static string ReadTextFromFile(string filePath)
        {
            string output;

            output = File.ReadAllText(filePath);

            return output;
        }

        public static bool FileExists(string filePath)
        {
            bool output;

            output = File.Exists(filePath);

            return output;
        }
    }
}
