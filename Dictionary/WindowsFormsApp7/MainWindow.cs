using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class MainWindow : Form
    {
        private Dictionary dictionary;

        private TextBox termTextBox;
        private TextBox definitionTextBox;
        private Button addButton;
        private ListBox termListBox;
        private TextBox searchTextBox;
        private Button searchButton;
        private TextBox definitionsTextBox;

        public MainWindow()
        {
            string termsFilePath = "terms.txt";
            string definitionsFilePath = "definitions.txt";
            dictionary = new Dictionary(termsFilePath, definitionsFilePath);

            CreateUI();
            RefreshTermList();
        }

        private void CreateUI()
        {
            this.termTextBox = new TextBox();
            this.definitionTextBox = new TextBox();
            this.addButton = new Button();
            this.termListBox = new ListBox();
            this.searchTextBox = new TextBox();
            this.searchButton = new Button();
            this.definitionsTextBox = new TextBox();

            // Term Label
            Label termLabel = new Label();
            termLabel.Text = "Термин:";
            termLabel.Location = new Point(12, 12);
            termLabel.AutoSize = true;

            // Term TextBox
            this.termTextBox.Location = new Point(12, 32);
            this.termTextBox.Size = new Size(200, 20);

            // Definition Label
            Label definitionLabel = new Label();
            definitionLabel.Text = "Определение:";
            definitionLabel.Location = new Point(12, 58);
            definitionLabel.AutoSize = true;

            // Definition TextBox
            this.definitionTextBox.Location = new Point(12, 78);
            this.definitionTextBox.Size = new Size(200, 60);
            this.definitionTextBox.Multiline = true;

            // Add Button
            this.addButton.Location = new Point(12, 144);
            this.addButton.Size = new Size(75, 23);
            this.addButton.Text = "Добавить";
            this.addButton.Click += new EventHandler(AddButton_Click);

            // Term List Label
            Label termListLabel = new Label();
            termListLabel.Text = "Список терминов:";
            termListLabel.Location = new Point(12, 174);
            termListLabel.AutoSize = true;

            // Term ListBox
            this.termListBox.Location = new Point(12, 190);
            this.termListBox.Size = new Size(200, 200);
            this.termListBox.SelectedIndexChanged += new EventHandler(termListBox_SelectedIndexChanged);

            // Search Label
            Label searchLabel = new Label();
            searchLabel.Text = "Поиск:";
            searchLabel.Location = new Point(218, 12);
            searchLabel.AutoSize = true;

            // Search TextBox
            this.searchTextBox.Location = new Point(218, 32);
            this.searchTextBox.Size = new Size(300, 20);

            // Search Button
            this.searchButton.Location = new Point(218, 58);
            this.searchButton.Size = new Size(75, 23);
            this.searchButton.Text = "Поиск";
            this.searchButton.Click += new EventHandler(SearchButton_Click);

            // Definitions Label
            Label definitionsLabel = new Label();
            definitionsLabel.Text = "Определения:";
            definitionsLabel.Location = new Point(218, 90);
            definitionsLabel.AutoSize = true;

            // Definitions TextBox
            this.definitionsTextBox.Location = new Point(218, 110);
            this.definitionsTextBox.Size = new Size(300, 280);
            this.definitionsTextBox.Multiline = true;
            this.definitionsTextBox.ScrollBars = ScrollBars.Vertical;
            this.definitionsTextBox.ReadOnly = true;

            // MainForm
            this.ClientSize = new Size(530, 400);
            this.Controls.Add(termLabel);
            this.Controls.Add(this.termTextBox);
            this.Controls.Add(definitionLabel);
            this.Controls.Add(this.definitionTextBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(termListLabel);
            this.Controls.Add(this.termListBox);
            this.Controls.Add(searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(definitionsLabel);
            this.Controls.Add(this.definitionsTextBox);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Словарь";
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string term = termTextBox.Text.ToLower();
            string definition = definitionTextBox.Text;

            if (!string.IsNullOrEmpty(term) && !string.IsNullOrEmpty(definition))
            {
                dictionary.FillDictionary(term, definition);

                termTextBox.Clear();
                definitionTextBox.Clear();

                RefreshTermList();

                MessageBox.Show("Термин добавлен в словарь.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите термин и его описание.");
            }
        }

        private void RefreshTermList()
        {
            termListBox.Items.Clear();

            foreach (string key in dictionary.GetKeys().OrderBy(k => k))
            {
                termListBox.Items.Add(key);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string term = searchTextBox.Text;

            if (!string.IsNullOrEmpty(term))
            {
                List<string> definitions = dictionary.SearchTerm(term);
                DisplayDefinitions(definitions);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите термин для поиска.");
            }
        }

        private void DisplayDefinitions(List<string> definitions)
        {
            definitionsTextBox.Text = string.Join(Environment.NewLine, definitions);
        }

        private void termListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTerm = termListBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedTerm))
            {
                List<string> definitions = dictionary.SearchTerm(selectedTerm).Select(d => $"{d}").ToList();
                DisplayDefinitions(definitions);
            }
        }
    }
}
