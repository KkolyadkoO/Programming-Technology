using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6;

public class IndexesStore
{
    private SubjectIndex[] indexes;
    IndexesStore(int size) {
        indexes = new SubjectIndex[size];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = new SubjectIndex();
        }
    }
    public void LoadFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int i = 0;
        foreach (string line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException($"Неправильный формат строки:\n{line}");
            }

            string word = parts[0].Trim();
            int[] pages = parts[1].Split(",")
                                  .Select(p => int.Parse(p.Trim()))
                                  .ToArray();
            indexes[i].Word = word;
            indexes[i].PageNumbers = pages;

        }
    }
}
