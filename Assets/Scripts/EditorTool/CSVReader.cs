using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryptoQuest.EditorTool
{
    public class CSVReader
    {
        public List<string[]> ReadCSVFile(string path)
        {
            List<string[]> data = new List<string[]>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        data.Add(values);
                    }
                }
            }
            catch (IOException e)
            {
                Debug.LogError("Error reading the CSV file: " + e.Message);
            }

            return data;
        }
    }
}