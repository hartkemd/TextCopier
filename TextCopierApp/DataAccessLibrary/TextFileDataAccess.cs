using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccessLibrary
{
    public class TextFileDataAccess
    {
        public List<TextItemModel> ReadAllRecords(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return new List<TextItemModel>();
            }

            var lines = File.ReadAllLines(filePath);
            List<TextItemModel> output = new();

            foreach (var line in lines)
            {
                TextItemModel ti = new();

                var vals = line.Split(',');

                if (vals.Length < 2)
                {
                    throw new Exception($"Invalid row of data: {line}");
                }

                ti.Description = vals[0];
                ti.Text = vals[1];

                output.Add(ti);
            }

            return output;
        }

        public void WriteAllRecords(IEnumerable<TextItemModel> textItems, string filePath)
        {
            List<string> lines = new();

            foreach (var ti in textItems)
            {
                lines.Add($"{ti.Description},{ti.Text}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
