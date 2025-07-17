using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Duplikatserkennung
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtAuswahlOrdner_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnWaehlen_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Wähle ein Verzeichnis zur Duplikatsuche aus";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtAuswahlOrdner.Text = dialog.SelectedPath;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listviewDuplicates.View = View.Details;
            listviewDuplicates.FullRowSelect = true;
            listviewDuplicates.GridLines = true;

            listviewDuplicates.Columns.Add("Dateiname", 300, HorizontalAlignment.Left);
            listviewDuplicates.Columns.Add("Dateigröße (Bytes)", 100, HorizontalAlignment.Right);
            listviewDuplicates.Columns.Add("Hashwert", 250, HorizontalAlignment.Left);

            // Testdaten für die Anzeige von Duplikaten
            var testdaten = new List<FileData>
            {
                new FileData { Path = @"C:\Test\datei1.txt", Size = 1024, Hash = "ABC123" },
                new FileData { Path = @"C:\Test\datei2.txt", Size = 2048, Hash = "DEF456" },
                new FileData { Path = @"C:\Test\datei3.txt", Size = 512,  Hash = "ABC123" }
            };

            ZeigeDuplikate(testdaten);
        }

        public void ZeigeDuplikate(List<FileData> duplikate)
        {
            listviewDuplicates.Items.Clear();

            foreach (var file in duplikate)
            {
                var item = new ListViewItem(new string[]
                {
                    file.Path,
                    file.Size.ToString(),
                    file.Hash
                });

                listviewDuplicates.Items.Add(item);
            }
        }
    }

    public class FileData
    {
        public string Path { get; set; }
        public long Size { get; set; }
        public string Hash { get; set; }
    }
}
