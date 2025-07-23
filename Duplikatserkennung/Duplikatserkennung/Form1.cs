using System;
using System.Collections.Generic;
using System.Drawing;             // Für grafische Elemente
using System.Windows.Forms;       // Für das UI mit Windows Forms

namespace Duplikatserkennung
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.ToolTip listViewToolTip = new System.Windows.Forms.ToolTip();
        private Timer tooltipTimer = new Timer();
        private ListViewItem pendingTooltipItem = null;
        private Point pendingMousePosition;

        public Form1()
        {
            InitializeComponent();

            listViewToolTip.AutoPopDelay = 50000;
            listViewToolTip.ShowAlways = true;

            listviewDuplicates.MouseMove += ListviewDuplicates_MouseMove;

            tooltipTimer.Interval = 1000; // 1 Sekunden Delay
            tooltipTimer.Tick += TooltipTimer_Tick;
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

            var testdaten = new List<FileData>
            {
                new FileData { Path = @"C:\Test\datei1.txt", Size = 1024, Hash = "ABC123" },
                new FileData { Path = @"C:\Test\datei2.txt", Size = 2048, Hash = "DEF456" },
                new FileData { Path = @"C:\Test\datei3.txt", Size = 512,  Hash = "ABC123" },
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

            PasseListViewHoeheAn(duplikate.Count);

            // Spaltenbreiten automatisch anpassen
            foreach (ColumnHeader column in listviewDuplicates.Columns)
            {
                column.Width = -2;
            }
        }

        //passt die Höhe der ListView an
        private void PasseListViewHoeheAn(int anzahlEintraege)
        {
            int zeilenhoehe = listviewDuplicates.Font.Height + 10;
            int headerhoehe = 25;
            int neueHoehe = headerhoehe + (anzahlEintraege * zeilenhoehe);

            int maxHoehe = 400;
            listviewDuplicates.Height = Math.Min(neueHoehe, maxHoehe);
        }

        private void ListviewDuplicates_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = listviewDuplicates.GetItemAt(e.X, e.Y);

            if (item != pendingTooltipItem)
            {
                tooltipTimer.Stop();
                listViewToolTip.Hide(this);

                pendingTooltipItem = item;
                pendingMousePosition = new Point(e.X, e.Y);

                if (item != null)
                    tooltipTimer.Start();
            }
        }

        private void TooltipTimer_Tick(object sender, EventArgs e)
        {
            tooltipTimer.Stop();

            if (pendingTooltipItem != null)
            {
                string tooltipText = $"Pfad: {pendingTooltipItem.SubItems[0].Text}\nGröße: {pendingTooltipItem.SubItems[1].Text} Bytes\nHash: {pendingTooltipItem.SubItems[2].Text}";

                Point screenPoint = listviewDuplicates.PointToScreen(new Point(pendingMousePosition.X + 15, pendingMousePosition.Y + 15));
                Point clientPoint = this.PointToClient(screenPoint);

                listViewToolTip.Show(tooltipText, this, clientPoint, 5000);
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