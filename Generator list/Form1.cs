using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Generator_list
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private ListGenerator listGenerators;

        private void generateButton_Click(object sender, EventArgs e)
        {
            try
            {
                listGenerators = new ListGenerator(inputTextBox, (string)nameClassBox.Text, (string)nameListBox.Text, (string)categoryNameBox.Text);
                copyButton.Visible = true;
                clearListButton.Visible = true;
            }
            catch(IndexOutOfRangeException)
            {
               MessageBox.Show("Ilość wyrażeń jest mniejsza niż liczba kategorii", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wystąpił nieznany błąd w programie. Informacje ułatwiające rozpoznanie błędu zostały zapisane" + Environment.NewLine + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!File.Exists(@"error_listGenerator.txt"))
                {
                     File.Create(@"error_listGenerator.txt");
                }
                using (StreamWriter sw = new StreamWriter(@"error_listGenerator.txt", true))
                {
                    sw.WriteLine(DateTime.Now + " " + ex.Message);                    
                }                    
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            //listGenerators = new ListGenerator(inputTextBox, (string)nameClassBox.Text, (string)nameListBox.Text, (string)categoryNameBox.Text);
            //listGenerators.prepareList();
            Clipboard.SetText(listGenerators.ListTextGenerator);
        }

        private void clearListButton_Click(object sender, EventArgs e)
        {
            nameClassBox.Text = "";
            nameListBox.Text = "";
            categoryNameBox.Text = "";
            inputTextBox.Text = "";
            copyButton.Visible = false;
            clearListButton.Visible = false;
            ///listGenerators.compareLines();
        }
    }
}
