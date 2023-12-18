using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    class Dictionary
    {
        private Dictionary<string, List<DictionaryEntry>> index;
        private string termsFilePath;
        private string definitionsFilePath;

        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();

            foreach (string key in index.Keys)
            {
                keys.Add(key);
            }

            return keys;
        }

        public Dictionary(string termsFilePath, string definitionsFilePath)
        {
            index = new Dictionary<string, List<DictionaryEntry>>();
            this.termsFilePath = termsFilePath;
            this.definitionsFilePath = definitionsFilePath;

            OpenFiles(); // Открываем файлы словаря при инициализации класса
        }

        public void CreateFiles()
        {
            // Создание файлов словаря, только если они не существуют
            if (!File.Exists(termsFilePath))
                File.WriteAllText(termsFilePath, string.Empty);

            if (!File.Exists(definitionsFilePath))
                File.WriteAllText(definitionsFilePath, string.Empty);
        }

        public void OpenFiles()
        {
            // Открытие файлов словаря
            if (!File.Exists(termsFilePath) || !File.Exists(definitionsFilePath))
            {
                MessageBox.Show("Файлы словаря не существуют!");
                return;
            }

            // Загрузка индекса из файла с терминами
            string[] termLines = File.ReadAllLines(termsFilePath);
            foreach (string termLine in termLines)
            {
                string[] parts = termLine.Split('|');
                if (parts.Length == 3)
                {
                    string term = parts[0];
                    long position = long.Parse(parts[1]);
                    int chainLength = int.Parse(parts[2]);

                    if (!index.ContainsKey(term))
                        index[term] = new List<DictionaryEntry>();

                    index[term].Add(new DictionaryEntry(position, chainLength));
                }
            }
        }

        public void FillDictionary(string term, string definition)
        {
            // Заполнение словаря
            long position;
            using (StreamWriter termsWriter = File.AppendText(termsFilePath))
            using (StreamWriter definitionsWriter = File.AppendText(definitionsFilePath))
            {
                position = definitionsWriter.BaseStream.Position;

                int chainLength = 0;
                if (index.ContainsKey(term.ToLower()))
                    chainLength = index[term.ToLower()].Count;

                if (!index.ContainsKey(term.ToLower()))
                    index[term.ToLower()] = new List<DictionaryEntry>();

                index[term.ToLower()].Add(new DictionaryEntry(position, chainLength));

                termsWriter.WriteLine($"{term.ToLower()}|{position}|{chainLength}");

                definitionsWriter.WriteLine(definition);
            }
        }

        public List<string> SearchTerm(string term)
        {
            // Поиск определения заданного слова (термина)
            List<string> definitions = new List<string>();

            if (index.ContainsKey(term.ToLower()))
            {
                List<DictionaryEntry> entries = index[term.ToLower()];
                using (StreamReader definitionsReader = new StreamReader(definitionsFilePath))
                {
                    foreach (DictionaryEntry entry in entries)
                    {
                        definitionsReader.BaseStream.Seek(entry.Position, SeekOrigin.Begin);

                        string currentDefinition = definitionsReader.ReadLine();

                            definitions.Add($"{term.ToLower()} - {currentDefinition}");
                        

                        definitionsReader.DiscardBufferedData();
                    }
                }
            }
            else
            {
                MessageBox.Show($"Термин '{term}' не найден в словаре.");
            }

            return definitions;
        }


    }

    class DictionaryEntry
    {
        public long Position { get; }
        public int ChainLength { get; }

        public DictionaryEntry(long position, int chainLength)
        {
            Position = position;
            ChainLength = chainLength;
        }
    }
}
