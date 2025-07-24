using System;
using System.Collections.Generic;
using System.Drawing;             // Für grafische Elemente
using System.Windows.Forms;       // Für das UI mit Windows Forms
using System.IO;
using System.Security.Cryptography;

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

            btnAnzeigen.Click += btnAnzeigen_Click;
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
        }

        public void ZeigeDuplikate(List<FileData> duplikate)
        {
            listviewDuplicates.Items.Clear();

            foreach (var file in duplikate)     //Durchläuft Duplikat Liste
            {
                var item = new ListViewItem(new string[] //Erstellt Einträge der Duplikate Liste mit Dateinamen, Dateigröße und Hashwert
                {
                    Path.GetFileName(file.Path),  // ← Nur der Dateiname
                    file.Size.ToString(),
                    file.Hash
                });

                item.Tag = file.Path;
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

            int minHoehe = 150;
            int maxHoehe = 400;
            listviewDuplicates.Height = Math.Max(minHoehe, Math.Min(neueHoehe, maxHoehe));
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

        private void AnalysiereOrdner(string ordnerPfad)    //Erstellt Liste für Duplikate mit Hashwerten
        {
            if (!Directory.Exists(ordnerPfad))  //Überprüfen ob Ordner existiert
            {
                MessageBox.Show("Der gewählte Ordner existiert nicht.");
                return;
            }

            var dateien = Directory.GetFiles(ordnerPfad, SearchOption.AllDirectories);  //alle Dateien im ausgewählten Ordner in Liste dateien sammeln
            var hashMap = new Dictionary<string, List<FileData>>();

            foreach (var datei in dateien)          //durchläuft dateien Liste
            {
                try
                {
                    string hash = BerechneSHA256(datei);     //bildet Hashwert der dateien
                    var info = new FileInfo(datei); 

                    var fileData = new FileData     //Erstellt neue Objekte von FileData mit Dateinamen, Größe und Hashwert
                    {
                        Path = datei,
                        Size = info.Length,
                        Hash = hash
                    };

                    if (!hashMap.ContainsKey(hash))
                        hashMap[hash] = new List<FileData>();

                    hashMap[hash].Add(fileData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler bei Datei {datei}:\n{ex.Message}");
                }
            }

            var duplikate = new List<FileData>();

            foreach (var gruppe in hashMap.Values)  
            {
                if (gruppe.Count > 1)               //Überprüft ob mehrere Hashwerte gleich sind und fügt sie zu duplikate hinzu
                    duplikate.AddRange(gruppe);
            }

            if (duplikate.Count == 0)
            {
                MessageBox.Show("Keine Duplikate gefunden.");
            }

            ZeigeDuplikate(duplikate);          //Zeigt Liste aus Duplikaten an
        }

        private string BerechneSHA256(string dateipfad)
        {
            using (FileStream stream = File.OpenRead(dateipfad))    //Auslesen von Dateiinhalt
            using (SHA256 sha = SHA256.Create())                     
            {
                byte[] hashBytes = sha.ComputeHash(stream);     //Erstellt Hashwert aus Dateiinhalt
                return BitConverter.ToString(hashBytes).Replace("-", "");
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAuswahlOrdner.Text))
            {
                MessageBox.Show("Bitte wähle zuerst einen Ordner aus.");
                return;
            }

            AnalysiereOrdner(txtAuswahlOrdner.Text);
    }

        // Neue Methode: Zeigt alle Dateien im Ordner an (ohne Duplikate filtern)
        private void ZeigeAlleDateien(string ordnerPfad)
        {
            if (!Directory.Exists(ordnerPfad))
            {
                MessageBox.Show("Der gewählte Ordner existiert nicht.");    
                return;
            }

            var dateien = Directory.GetFiles(ordnerPfad, SearchOption.AllDirectories);
            var alleDateien = new List<FileData>();     //Erstellt Liste von allen dateien

            foreach (var datei in dateien)
            {
                try
                {
                    var info = new FileInfo(datei);
                    alleDateien.Add(new FileData
                    {
                        Path = datei,
                        Size = info.Length,
                        Hash = ""  // Hash leer lassen, da hier nicht berechnet
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler bei Datei {datei}:\n{ex.Message}");
                }
            }

            ZeigeDateien(alleDateien);
        }

        // Neue Methode: Allgemeines Anzeigen von Dateien in der ListView
        private void ZeigeDateien(List<FileData> dateien)           //Erstellt Liste von allen Dateien
        {
            listviewDuplicates.Items.Clear();

            foreach (var file in dateien)
            {
                var item = new ListViewItem(new string[]
                {
                    Path.GetFileName(file.Path),
                    file.Size.ToString(),
                    file.Hash
                });

                item.Tag = file.Path;
                listviewDuplicates.Items.Add(item);
            }

            PasseListViewHoeheAn(dateien.Count);

            foreach (ColumnHeader column in listviewDuplicates.Columns)
            {
                column.Width = -2;
            }
        }

        private void btnAnzeigen_Click(object sender, EventArgs e)
        {
    
            {
                if (string.IsNullOrWhiteSpace(txtAuswahlOrdner.Text))       //Überprüfung ob Ordner gewählt wurde
                {
                    MessageBox.Show("Bitte wähle zuerst einen Ordner aus.");
                    return;
                }

                ZeigeAlleDateien(txtAuswahlOrdner.Text);
            }

    }

        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            if (listviewDuplicates.SelectedItems.Count == 0)        //Überprüfung ob Datei zum löschen ausgewählt
            {
                MessageBox.Show("Bitte wähle zuerst eine Datei aus, die gelöscht werden soll.");
                return;
            }

            var selectedItem = listviewDuplicates.SelectedItems[0];
            string dateiName = selectedItem.SubItems[0].Text;

            string kompletterPfad = (string)selectedItem.Tag;
            if (string.IsNullOrEmpty(kompletterPfad) || !File.Exists(kompletterPfad))
            {
                MessageBox.Show("Die Datei konnte nicht gefunden werden oder der Pfad ist ungültig.");
                return;
            }

            var confirmResult = MessageBox.Show($"Willst du die Datei wirklich löschen?\n{kompletterPfad}",
                                                "Datei löschen",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    File.Delete(kompletterPfad);
                    listviewDuplicates.Items.Remove(selectedItem);
                    MessageBox.Show("Datei erfolgreich gelöscht.");

                    // Optional: ListView Höhe anpassen
                    PasseListViewHoeheAn(listviewDuplicates.Items.Count);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Löschen der Datei:\n{ex.Message}");
                }
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