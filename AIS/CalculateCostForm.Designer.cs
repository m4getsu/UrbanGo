using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AIS
{
    partial class CalculateCostForm
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
            labelCarInfo = new Label();
            labelPricePerHourCaption = new Label();
            labelPricePerHour = new Label();
            labelHoursCaption = new Label();
            numericUpDownHours = new NumericUpDown();
            labelTotalCaption = new Label();
            labelTotalCost = new Label();
            buttonOK = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHours).BeginInit();
            SuspendLayout();
            // 
            // labelCarInfo
            // 
            labelCarInfo.Font = new System.Drawing.Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelCarInfo.Location = new Point(14, 16);
            labelCarInfo.Name = "labelCarInfo";
            labelCarInfo.Size = new Size(358, 25);
            labelCarInfo.TabIndex = 0;
            labelCarInfo.Text = "labelCarInfo";
            labelCarInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            labelPricePerHourCaption.AutoSize = true;
            labelPricePerHourCaption.Location = new Point(14, 56);
            labelPricePerHourCaption.Name = "label1";
            labelPricePerHourCaption.Size = new Size(101, 20);
            labelPricePerHourCaption.TabIndex = 1;
            labelPricePerHourCaption.Text = "Цена за час:";
            // 
            // labelPricePerHour
            // 
            labelPricePerHour.AutoSize = true;
            labelPricePerHour.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelPricePerHour.Location = new Point(121, 56);
            labelPricePerHour.Name = "labelPricePerHour";
            labelPricePerHour.Size = new Size(135, 20);
            labelPricePerHour.TabIndex = 2;
            labelPricePerHour.Text = "labelPricePerHour";
            // 
            // label2
            // 
            labelHoursCaption.AutoSize = true;
            labelHoursCaption.Location = new Point(14, 96);
            labelHoursCaption.Name = "label2";
            labelHoursCaption.Size = new Size(153, 20);
            labelHoursCaption.TabIndex = 3;
            labelHoursCaption.Text = "Количество часов:";
            // 
            // numericUpDownHours
            // 
            numericUpDownHours.Location = new Point(173, 94);
            numericUpDownHours.Margin = new Padding(3, 4, 3, 4);
            numericUpDownHours.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownHours.Name = "numericUpDownHours";
            numericUpDownHours.Size = new Size(137, 27);
            numericUpDownHours.TabIndex = 4;
            numericUpDownHours.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownHours.ValueChanged += numericUpDownHours_ValueChanged;
            // 
            // label3
            // 
            labelTotalCaption.AutoSize = true;
            labelTotalCaption.Font = new System.Drawing.Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelTotalCaption.Location = new Point(14, 144);
            labelTotalCaption.Name = "label3";
            labelTotalCaption.Size = new Size(105, 23);
            labelTotalCaption.TabIndex = 5;
            labelTotalCaption.Text = "Итого к оплате:";
            // 
            // labelTotalCost
            // 
            labelTotalCost.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelTotalCost.Location = new Point(14, 176);
            labelTotalCost.Name = "labelTotalCost";
            labelTotalCost.Size = new Size(358, 31);
            labelTotalCost.TabIndex = 6;
            labelTotalCost.Text = "labelTotalCost";
            labelTotalCost.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(139, 224);
            buttonOK.Margin = new Padding(3, 4, 3, 4);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(117, 36);
            buttonOK.TabIndex = 7;
            buttonOK.Text = "ОК";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // CalculateCostForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(387, 277);
            Controls.Add(buttonOK);
            Controls.Add(labelTotalCost);
            Controls.Add(labelTotalCaption);
            Controls.Add(numericUpDownHours);
            Controls.Add(labelHoursCaption);
            Controls.Add(labelPricePerHour);
            Controls.Add(labelPricePerHourCaption);
            Controls.Add(labelCarInfo);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CalculateCostForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Расчет стоимости аренды";
            ((System.ComponentModel.ISupportInitialize)numericUpDownHours).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelCarInfo;
        private Label labelPricePerHourCaption;
        private Label labelPricePerHour;
        private Label labelHoursCaption;
        private NumericUpDown numericUpDownHours;
        private Label labelTotalCaption;
        private Label labelTotalCost;
        private Button buttonOK;
    }
}