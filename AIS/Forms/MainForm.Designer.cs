using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Linq;

namespace AIS
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridViewCars = new DataGridView();
            panelControls = new Panel();
            buttonCalculate = new Button();
            buttonRent = new Button();
            buttonEdit = new Button();
            buttonDelete = new Button();
            buttonAdd = new Button();
            buttonRefresh = new Button();
            panelSearch = new Panel();
            buttonSearch = new Button();
            textBoxSearch = new TextBox();
            labelSearch = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            ((ISupportInitialize)dataGridViewCars).BeginInit();
            panelControls.SuspendLayout();
            panelSearch.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewCars
            // 
            dataGridViewCars.AllowUserToAddRows = false;
            dataGridViewCars.AllowUserToDeleteRows = false;
            dataGridViewCars.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCars.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCars.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCars.Location = new Point(14, 106);
            dataGridViewCars.Margin = new Padding(3, 4, 3, 4);
            dataGridViewCars.Name = "dataGridViewCars";
            dataGridViewCars.ReadOnly = true;
            dataGridViewCars.RowHeadersWidth = 51;
            dataGridViewCars.Size = new Size(1097, 534);
            dataGridViewCars.TabIndex = 0;
            dataGridViewCars.SelectionChanged += dataGridViewCars_SelectionChanged;
            // 
            // panelControls
            // 
            panelControls.BackColor = Color.FromArgb(244, 246, 249);
            panelControls.Controls.Add(buttonCalculate);
            panelControls.Controls.Add(buttonRent);
            panelControls.Controls.Add(buttonEdit);
            panelControls.Controls.Add(buttonDelete);
            panelControls.Controls.Add(buttonAdd);
            panelControls.Controls.Add(buttonRefresh);
            panelControls.Dock = DockStyle.Top;
            panelControls.Location = new Point(0, 0);
            panelControls.Margin = new Padding(3, 4, 3, 4);
            panelControls.Name = "panelControls";
            panelControls.Size = new Size(1125, 54);
            panelControls.TabIndex = 1;
            // 
            // buttonCalculate
            // 
            buttonCalculate.BackColor = Color.FromArgb(0, 120, 215);
            buttonCalculate.Font = new System.Drawing.Font("Impact", 7.8F);
            buttonCalculate.ForeColor = SystemColors.Control;
            buttonCalculate.ImageAlign = ContentAlignment.MiddleLeft;
            buttonCalculate.Location = new Point(521, 6);
            buttonCalculate.Margin = new Padding(3, 4, 3, 4);
            buttonCalculate.Name = "buttonCalculate";
            buttonCalculate.Size = new Size(131, 40);
            buttonCalculate.TabIndex = 5;
            buttonCalculate.Text = "Рассчитать \U0001f9ee";
            buttonCalculate.UseVisualStyleBackColor = false;
            buttonCalculate.Click += buttonCalculate_Click;
            // 
            // buttonRent
            // 
            buttonRent.BackColor = Color.FromArgb(0, 120, 215);
            buttonRent.Font = new System.Drawing.Font("Impact", 7.8F);
            buttonRent.ForeColor = SystemColors.Control;
            buttonRent.ImageAlign = ContentAlignment.MiddleLeft;
            buttonRent.Location = new Point(380, 6);
            buttonRent.Margin = new Padding(3, 4, 3, 4);
            buttonRent.Name = "buttonRent";
            buttonRent.Size = new Size(135, 40);
            buttonRent.TabIndex = 4;
            buttonRent.Text = "Арендовать 🔑";
            buttonRent.UseVisualStyleBackColor = false;
            buttonRent.Click += buttonRent_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.BackColor = Color.FromArgb(0, 120, 215);
            buttonEdit.Font = new System.Drawing.Font("Impact", 7.8F);
            buttonEdit.ForeColor = SystemColors.Control;
            buttonEdit.ImageAlign = ContentAlignment.MiddleLeft;
            buttonEdit.Location = new Point(129, 6);
            buttonEdit.Margin = new Padding(3, 4, 3, 4);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(110, 40);
            buttonEdit.TabIndex = 2;
            buttonEdit.Text = "Изменить ✏️";
            buttonEdit.UseVisualStyleBackColor = false;
            buttonEdit.Click += buttonEdit_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.BackColor = Color.FromArgb(0, 120, 215);
            buttonDelete.Font = new System.Drawing.Font("Impact", 7.8F);
            buttonDelete.ForeColor = SystemColors.Control;
            buttonDelete.ImageAlign = ContentAlignment.MiddleLeft;
            buttonDelete.Location = new Point(244, 6);
            buttonDelete.Margin = new Padding(3, 4, 3, 4);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(130, 40);
            buttonDelete.TabIndex = 3;
            buttonDelete.Text = "Удалить 🗑️";
            buttonDelete.UseVisualStyleBackColor = false;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // buttonAdd
            // 
            buttonAdd.BackColor = Color.FromArgb(0, 120, 215);
            buttonAdd.BackgroundImageLayout = ImageLayout.Zoom;
            buttonAdd.Font = new System.Drawing.Font("Impact", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonAdd.ForeColor = SystemColors.Control;
            buttonAdd.ImageAlign = ContentAlignment.MiddleLeft;
            buttonAdd.Location = new Point(12, 6);
            buttonAdd.Margin = new Padding(3, 4, 3, 4);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(111, 40);
            buttonAdd.TabIndex = 1;
            buttonAdd.Text = "Добавить ➕";
            buttonAdd.UseVisualStyleBackColor = false;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.BackColor = Color.FromArgb(0, 120, 215);
            buttonRefresh.Font = new System.Drawing.Font("Impact", 7.8F);
            buttonRefresh.ForeColor = SystemColors.Control;
            buttonRefresh.ImageAlign = ContentAlignment.MiddleLeft;
            buttonRefresh.Location = new Point(658, 6);
            buttonRefresh.Margin = new Padding(3, 4, 3, 4);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(113, 40);
            buttonRefresh.TabIndex = 6;
            buttonRefresh.Text = "Обновить 🔄";
            buttonRefresh.UseVisualStyleBackColor = false;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.FromArgb(244, 246, 249);
            panelSearch.Controls.Add(buttonSearch);
            panelSearch.Controls.Add(textBoxSearch);
            panelSearch.Controls.Add(labelSearch);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 54);
            panelSearch.Margin = new Padding(3, 4, 3, 4);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1125, 54);
            panelSearch.TabIndex = 2;
            // 
            // buttonSearch
            // 
            buttonSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSearch.BackColor = Color.FromArgb(0, 120, 215);
            buttonSearch.Font = new System.Drawing.Font("Impact", 7.8F);
            buttonSearch.ForeColor = SystemColors.Control;
            buttonSearch.Location = new Point(988, 8);
            buttonSearch.Margin = new Padding(3, 4, 3, 4);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(125, 34);
            buttonSearch.TabIndex = 2;
            buttonSearch.Text = "Поиск 🔍";
            buttonSearch.UseVisualStyleBackColor = false;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(244, 11);
            textBoxSearch.Margin = new Padding(3, 4, 3, 4);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(130, 27);
            textBoxSearch.TabIndex = 1;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // labelSearch
            // 
            labelSearch.AutoSize = true;
            labelSearch.Location = new Point(11, 11);
            labelSearch.Name = "labelSearch";
            labelSearch.Size = new Size(240, 20);
            labelSearch.TabIndex = 0;
            labelSearch.Text = "Поиск по марке или гос. номеру:";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 646);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1125, 26);
            statusStrip.TabIndex = 3;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(143, 20);
            toolStripStatusLabel.Text = "toolStripStatusLabel";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 672);
            Controls.Add(statusStrip);
            Controls.Add(dataGridViewCars);
            Controls.Add(panelSearch);
            Controls.Add(panelControls);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1140, 708);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UrbanGo";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((ISupportInitialize)dataGridViewCars).EndInit();
            panelControls.ResumeLayout(false);
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DataGridView dataGridViewCars;
        private Panel panelControls;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonRent;
        private Button buttonCalculate;
        private Panel panelSearch;
        private TextBox textBoxSearch;
        private Label labelSearch;
        private Button buttonSearch;
        private Button buttonRefresh;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
    }
}

