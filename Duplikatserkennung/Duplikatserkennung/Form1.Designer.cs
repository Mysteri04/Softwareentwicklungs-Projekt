﻿namespace Duplikatserkennung
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWaehlen = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtAuswahlOrdner = new System.Windows.Forms.RichTextBox();
            this.listviewDuplicates = new System.Windows.Forms.ListView();
            this.btnAnzeigen = new System.Windows.Forms.Button();
            this.btnLoeschen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(741, 319);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnWaehlen
            // 
            this.btnWaehlen.Location = new System.Drawing.Point(44, 12);
            this.btnWaehlen.Name = "btnWaehlen";
            this.btnWaehlen.Size = new System.Drawing.Size(97, 23);
            this.btnWaehlen.TabIndex = 1;
            this.btnWaehlen.Text = "Ordner wählen";
            this.btnWaehlen.UseVisualStyleBackColor = true;
            this.btnWaehlen.Click += new System.EventHandler(this.btnWaehlen_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(589, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Duplikate anzeigen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtAuswahlOrdner
            // 
            this.txtAuswahlOrdner.Location = new System.Drawing.Point(12, 41);
            this.txtAuswahlOrdner.Name = "txtAuswahlOrdner";
            this.txtAuswahlOrdner.ReadOnly = true;
            this.txtAuswahlOrdner.Size = new System.Drawing.Size(170, 163);
            this.txtAuswahlOrdner.TabIndex = 5;
            this.txtAuswahlOrdner.Text = "";
            // 
            // listviewDuplicates
            // 
            this.listviewDuplicates.FullRowSelect = true;
            this.listviewDuplicates.GridLines = true;
            this.listviewDuplicates.HideSelection = false;
            this.listviewDuplicates.Location = new System.Drawing.Point(202, 41);
            this.listviewDuplicates.Name = "listviewDuplicates";
            this.listviewDuplicates.Size = new System.Drawing.Size(614, 163);
            this.listviewDuplicates.TabIndex = 6;
            this.listviewDuplicates.UseCompatibleStateImageBehavior = false;
            this.listviewDuplicates.View = System.Windows.Forms.View.Details;
            // 
            // btnAnzeigen
            // 
            this.btnAnzeigen.Location = new System.Drawing.Point(312, 12);
            this.btnAnzeigen.Name = "btnAnzeigen";
            this.btnAnzeigen.Size = new System.Drawing.Size(108, 23);
            this.btnAnzeigen.TabIndex = 7;
            this.btnAnzeigen.Text = "Inhalte Anzeigen";
            this.btnAnzeigen.UseVisualStyleBackColor = true;
            this.btnAnzeigen.Click += new System.EventHandler(this.btnAnzeigen_Click);
            // 
            // btnLoeschen
            // 
            this.btnLoeschen.Location = new System.Drawing.Point(310, 210);
            this.btnLoeschen.Name = "btnLoeschen";
            this.btnLoeschen.Size = new System.Drawing.Size(110, 23);
            this.btnLoeschen.TabIndex = 8;
            this.btnLoeschen.Text = "Duplikat Löschen";
            this.btnLoeschen.UseVisualStyleBackColor = true;
            this.btnLoeschen.Click += new System.EventHandler(this.btnLoeschen_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(827, 354);
            this.Controls.Add(this.btnLoeschen);
            this.Controls.Add(this.btnAnzeigen);
            this.Controls.Add(this.listviewDuplicates);
            this.Controls.Add(this.txtAuswahlOrdner);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnWaehlen);
            this.Controls.Add(this.btnClose);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWaehlen;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.RichTextBox txtAuswahlOrdner;
        private System.Windows.Forms.ListView listviewDuplicates;
        private System.Windows.Forms.Button btnAnzeigen;
        private System.Windows.Forms.Button btnLoeschen;
    }
}

