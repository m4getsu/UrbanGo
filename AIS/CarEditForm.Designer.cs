using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AIS
{
    partial class CarEditForm
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
            components = new System.ComponentModel.Container();
            labelBrand = new Label();
            textBoxBrand = new TextBox();
            labelModel = new Label();
            textBoxModel = new TextBox();
            labelLicensePlate = new Label();
            textBoxLicensePlate = new TextBox();
            labelYear = new Label();
            numericUpDownYear = new NumericUpDown();
            labelMileage = new Label();
            numericUpDownMileage = new NumericUpDown();
            labelPrice = new Label();
            numericUpDownPrice = new NumericUpDown();
            labelStatus = new Label();
            comboBoxStatus = new ComboBox();
            buttonOK = new Button();
            buttonCancel = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)numericUpDownYear).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMileage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            labelBrand.AutoSize = true;
            labelBrand.Location = new Point(14, 16);
            labelBrand.Name = "label1";
            labelBrand.Size = new Size(52, 20);
            labelBrand.TabIndex = 0;
            labelBrand.Text = "Марка";
            // 
            // textBoxBrand
            // 
            textBoxBrand.Location = new Point(14, 39);
            textBoxBrand.Margin = new Padding(3, 4, 3, 4);
            textBoxBrand.Name = "textBoxBrand";
            textBoxBrand.Size = new Size(242, 27);
            textBoxBrand.TabIndex = 1;
            textBoxBrand.Validating += textBoxBrand_Validating;
            // 
            // label2
            // 
            labelModel.AutoSize = true;
            labelModel.Location = new Point(14, 80);
            labelModel.Name = "label2";
            labelModel.Size = new Size(61, 20);
            labelModel.TabIndex = 2;
            labelModel.Text = "Модель";
            // 
            // textBoxModel
            // 
            textBoxModel.Location = new Point(14, 104);
            textBoxModel.Margin = new Padding(3, 4, 3, 4);
            textBoxModel.Name = "textBoxModel";
            textBoxModel.Size = new Size(242, 27);
            textBoxModel.TabIndex = 3;
            textBoxModel.Validating += textBoxModel_Validating;
            // 
            // label3
            // 
            labelLicensePlate.AutoSize = true;
            labelLicensePlate.Location = new Point(14, 144);
            labelLicensePlate.Name = "label3";
            labelLicensePlate.Size = new Size(105, 20);
            labelLicensePlate.TabIndex = 4;
            labelLicensePlate.Text = "Гос. номер";
            // 
            // textBoxLicensePlate
            // 
            textBoxLicensePlate.Location = new Point(14, 168);
            textBoxLicensePlate.Margin = new Padding(3, 4, 3, 4);
            textBoxLicensePlate.Name = "textBoxLicensePlate";
            textBoxLicensePlate.Size = new Size(242, 27);
            textBoxLicensePlate.TabIndex = 5;
            textBoxLicensePlate.Validating += textBoxLicensePlate_Validating;
            // 
            // label4
            // 
            labelYear.AutoSize = true;
            labelYear.Location = new Point(14, 208);
            labelYear.Name = "label4";
            labelYear.Size = new Size(33, 20);
            labelYear.TabIndex = 6;
            labelYear.Text = "Год";
            // 
            // numericUpDownYear
            // 
            numericUpDownYear.Location = new Point(14, 232);
            numericUpDownYear.Margin = new Padding(3, 4, 3, 4);
            numericUpDownYear.Maximum = new decimal(new int[] { 2030, 0, 0, 0 });
            numericUpDownYear.Minimum = new decimal(new int[] { 1900, 0, 0, 0 });
            numericUpDownYear.Name = "numericUpDownYear";
            numericUpDownYear.Size = new Size(242, 27);
            numericUpDownYear.TabIndex = 7;
            numericUpDownYear.Value = new decimal(new int[] { 2023, 0, 0, 0 });
            numericUpDownYear.Validating += numericUpDownYear_Validating;
            // 
            // label5
            // 
            labelMileage.AutoSize = true;
            labelMileage.Location = new Point(14, 272);
            labelMileage.Name = "label5";
            labelMileage.Size = new Size(63, 20);
            labelMileage.TabIndex = 8;
            labelMileage.Text = "Пробег";
            // 
            // numericUpDownMileage
            // 
            numericUpDownMileage.Location = new Point(14, 296);
            numericUpDownMileage.Margin = new Padding(3, 4, 3, 4);
            numericUpDownMileage.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numericUpDownMileage.Name = "numericUpDownMileage";
            numericUpDownMileage.Size = new Size(242, 27);
            numericUpDownMileage.TabIndex = 9;
            numericUpDownMileage.Validating += numericUpDownMileage_Validating;
            // 
            // label6
            // 
            labelPrice.AutoSize = true;
            labelPrice.Location = new Point(14, 336);
            labelPrice.Name = "label6";
            labelPrice.Size = new Size(134, 20);
            labelPrice.TabIndex = 10;
            labelPrice.Text = "Цена за час (руб)";
            // 
            // numericUpDownPrice
            // 
            numericUpDownPrice.DecimalPlaces = 2;
            numericUpDownPrice.Location = new Point(14, 360);
            numericUpDownPrice.Margin = new Padding(3, 4, 3, 4);
            numericUpDownPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDownPrice.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownPrice.Name = "numericUpDownPrice";
            numericUpDownPrice.Size = new Size(242, 27);
            numericUpDownPrice.TabIndex = 11;
            numericUpDownPrice.Value = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDownPrice.Validating += numericUpDownPrice_Validating;
            // 
            // label7
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(14, 400);
            labelStatus.Name = "label7";
            labelStatus.Size = new Size(51, 20);
            labelStatus.TabIndex = 12;
            labelStatus.Text = "Статус";
            // 
            // comboBoxStatus
            // 
            comboBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStatus.FormattingEnabled = true;
            comboBoxStatus.Location = new Point(14, 424);
            comboBoxStatus.Margin = new Padding(3, 4, 3, 4);
            comboBoxStatus.Name = "comboBoxStatus";
            comboBoxStatus.Size = new Size(242, 28);
            comboBoxStatus.TabIndex = 13;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(14, 472);
            buttonOK.Margin = new Padding(3, 4, 3, 4);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(117, 36);
            buttonOK.TabIndex = 14;
            buttonOK.Text = "ОК";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.CausesValidation = false;
            buttonCancel.Location = new Point(139, 472);
            buttonCancel.Margin = new Padding(3, 4, 3, 4);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(117, 36);
            buttonCancel.TabIndex = 15;
            buttonCancel.Text = "Отмена";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // CarEditForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(272, 525);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(comboBoxStatus);
            Controls.Add(labelStatus);
            Controls.Add(numericUpDownPrice);
            Controls.Add(labelPrice);
            Controls.Add(numericUpDownMileage);
            Controls.Add(labelMileage);
            Controls.Add(numericUpDownYear);
            Controls.Add(labelYear);
            Controls.Add(textBoxLicensePlate);
            Controls.Add(labelLicensePlate);
            Controls.Add(textBoxModel);
            Controls.Add(labelModel);
            Controls.Add(textBoxBrand);
            Controls.Add(labelBrand);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CarEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Данные автомобиля";
            ((System.ComponentModel.ISupportInitialize)numericUpDownYear).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMileage).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelBrand;
        private TextBox textBoxBrand;
        private Label labelModel;
        private TextBox textBoxModel;
        private Label labelLicensePlate;
        private TextBox textBoxLicensePlate;
        private Label labelYear;
        private NumericUpDown numericUpDownYear;
        private Label labelMileage;
        private NumericUpDown numericUpDownMileage;
        private Label labelPrice;
        private NumericUpDown numericUpDownPrice;
        private Label labelStatus;
        private ComboBox comboBoxStatus;
        private Button buttonOK;
        private Button buttonCancel;
        private ErrorProvider errorProvider;
    }
}