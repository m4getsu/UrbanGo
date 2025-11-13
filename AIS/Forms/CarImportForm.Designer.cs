namespace AIS.Forms
{
    partial class CarImportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelSelectFile = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBoxFormat = new System.Windows.Forms.GroupBox();
            this.rbJson = new System.Windows.Forms.RadioButton();
            this.rbCsv = new System.Windows.Forms.RadioButton();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.labelResults = new System.Windows.Forms.Label();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxFormat.SuspendLayout();
            this.SuspendLayout();
            //
            // labelTitle
            //
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(300, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Импорт автомобилей из файла";
            //
            // labelSelectFile
            //
            this.labelSelectFile.AutoSize = true;
            this.labelSelectFile.Location = new System.Drawing.Point(12, 50);
            this.labelSelectFile.Name = "labelSelectFile";
            this.labelSelectFile.Size = new System.Drawing.Size(137, 15);
            this.labelSelectFile.TabIndex = 1;
            this.labelSelectFile.Text = "Выберите файл импорта:";
            //
            // txtFilePath
            //
            this.txtFilePath.Location = new System.Drawing.Point(12, 70);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(560, 23);
            this.txtFilePath.TabIndex = 2;
            //
            // btnBrowse
            //
            this.btnBrowse.Location = new System.Drawing.Point(578, 69);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(94, 25);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Обзор...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //
            // groupBoxFormat
            //
            this.groupBoxFormat.Controls.Add(this.rbJson);
            this.groupBoxFormat.Controls.Add(this.rbCsv);
            this.groupBoxFormat.Location = new System.Drawing.Point(12, 105);
            this.groupBoxFormat.Name = "groupBoxFormat";
            this.groupBoxFormat.Size = new System.Drawing.Size(200, 80);
            this.groupBoxFormat.TabIndex = 4;
            this.groupBoxFormat.TabStop = false;
            this.groupBoxFormat.Text = "Формат файла";
            //
            // rbJson
            //
            this.rbJson.AutoSize = true;
            this.rbJson.Location = new System.Drawing.Point(15, 47);
            this.rbJson.Name = "rbJson";
            this.rbJson.Size = new System.Drawing.Size(52, 19);
            this.rbJson.TabIndex = 1;
            this.rbJson.Text = "JSON";
            this.rbJson.UseVisualStyleBackColor = true;
            this.rbJson.CheckedChanged += new System.EventHandler(this.rbFormat_CheckedChanged);
            //
            // rbCsv
            //
            this.rbCsv.AutoSize = true;
            this.rbCsv.Checked = true;
            this.rbCsv.Location = new System.Drawing.Point(15, 22);
            this.rbCsv.Name = "rbCsv";
            this.rbCsv.Size = new System.Drawing.Size(45, 19);
            this.rbCsv.TabIndex = 0;
            this.rbCsv.TabStop = true;
            this.rbCsv.Text = "CSV";
            this.rbCsv.UseVisualStyleBackColor = true;
            this.rbCsv.CheckedChanged += new System.EventHandler(this.rbFormat_CheckedChanged);
            //
            // btnValidate
            //
            this.btnValidate.Location = new System.Drawing.Point(230, 115);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(140, 30);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "Проверить файл";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            //
            // btnImport
            //
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnImport.Location = new System.Drawing.Point(230, 151);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(140, 30);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Импортировать";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            //
            // labelResults
            //
            this.labelResults.AutoSize = true;
            this.labelResults.Location = new System.Drawing.Point(12, 195);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(152, 15);
            this.labelResults.TabIndex = 7;
            this.labelResults.Text = "Результаты проверки/импорта:";
            //
            // txtResults
            //
            this.txtResults.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtResults.Location = new System.Drawing.Point(12, 215);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResults.Size = new System.Drawing.Size(660, 250);
            this.txtResults.TabIndex = 8;
            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(12, 471);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(660, 23);
            this.progressBar.TabIndex = 9;
            //
            // btnClose
            //
            this.btnClose.Location = new System.Drawing.Point(578, 500);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // CarImportForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 542);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.labelResults);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.groupBoxFormat);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.labelSelectFile);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CarImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Импорт автомобилей";
            this.groupBoxFormat.ResumeLayout(false);
            this.groupBoxFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSelectFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBoxFormat;
        private System.Windows.Forms.RadioButton rbJson;
        private System.Windows.Forms.RadioButton rbCsv;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label labelResults;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnClose;
    }
}
