namespace CyberCortex
{
    partial class View
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            this.butSelectSamples = new System.Windows.Forms.Button();
            this.fieldToPathSamples = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numSizeSample = new System.Windows.Forms.NumericUpDown();
            this.numLengthSample = new System.Windows.Forms.NumericUpDown();
            this.butLoad = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldToPathAnswers = new System.Windows.Forms.TextBox();
            this.butSelectAnswers = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldToPathTestSamples = new System.Windows.Forms.TextBox();
            this.butSelectTestSamples = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.fieldToPathTestAnswers = new System.Windows.Forms.TextBox();
            this.butSelectTestAnswers = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.numSizeTestSample = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numClassCount = new System.Windows.Forms.NumericUpDown();
            this.butStart = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numClassifiers = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.butLearn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.butLoadClassifiers = new System.Windows.Forms.Button();
            this.butSaveClassifiers = new System.Windows.Forms.Button();
            this.fieldForContent = new System.Windows.Forms.TextBox();
            this.butClean = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkLogs = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numSizeSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLengthSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSizeTestSample)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClassCount)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClassifiers)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // butSelectSamples
            // 
            this.butSelectSamples.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butSelectSamples.Location = new System.Drawing.Point(200, 37);
            this.butSelectSamples.Name = "butSelectSamples";
            this.butSelectSamples.Size = new System.Drawing.Size(91, 30);
            this.butSelectSamples.TabIndex = 0;
            this.butSelectSamples.Text = "Выбрать...";
            this.butSelectSamples.UseVisualStyleBackColor = true;
            // 
            // fieldToPathSamples
            // 
            this.fieldToPathSamples.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fieldToPathSamples.Location = new System.Drawing.Point(4, 43);
            this.fieldToPathSamples.Name = "fieldToPathSamples";
            this.fieldToPathSamples.ReadOnly = true;
            this.fieldToPathSamples.Size = new System.Drawing.Size(190, 21);
            this.fieldToPathSamples.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(2, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Длина примера:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(100, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Примеры Learn:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // numSizeSample
            // 
            this.numSizeSample.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numSizeSample.Location = new System.Drawing.Point(102, 257);
            this.numSizeSample.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSizeSample.Name = "numSizeSample";
            this.numSizeSample.Size = new System.Drawing.Size(92, 21);
            this.numSizeSample.TabIndex = 7;
            // 
            // numLengthSample
            // 
            this.numLengthSample.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numLengthSample.Location = new System.Drawing.Point(4, 257);
            this.numLengthSample.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLengthSample.Name = "numLengthSample";
            this.numLengthSample.Size = new System.Drawing.Size(92, 21);
            this.numLengthSample.TabIndex = 8;
            // 
            // butLoad
            // 
            this.butLoad.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butLoad.Location = new System.Drawing.Point(83, 333);
            this.butLoad.Name = "butLoad";
            this.butLoad.Size = new System.Drawing.Size(130, 31);
            this.butLoad.TabIndex = 10;
            this.butLoad.Text = "Загрузить данные";
            this.butLoad.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(2, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "Путь к обучающей выборке";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(2, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "Путь к обучающим ответам";
            // 
            // fieldToPathAnswers
            // 
            this.fieldToPathAnswers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fieldToPathAnswers.Location = new System.Drawing.Point(4, 92);
            this.fieldToPathAnswers.Name = "fieldToPathAnswers";
            this.fieldToPathAnswers.ReadOnly = true;
            this.fieldToPathAnswers.Size = new System.Drawing.Size(190, 21);
            this.fieldToPathAnswers.TabIndex = 13;
            // 
            // butSelectAnswers
            // 
            this.butSelectAnswers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butSelectAnswers.Location = new System.Drawing.Point(199, 86);
            this.butSelectAnswers.Name = "butSelectAnswers";
            this.butSelectAnswers.Size = new System.Drawing.Size(92, 31);
            this.butSelectAnswers.TabIndex = 14;
            this.butSelectAnswers.Text = "Выбрать...";
            this.butSelectAnswers.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(2, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "Путь к тестовой выборке";
            // 
            // fieldToPathTestSamples
            // 
            this.fieldToPathTestSamples.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fieldToPathTestSamples.Location = new System.Drawing.Point(4, 142);
            this.fieldToPathTestSamples.Name = "fieldToPathTestSamples";
            this.fieldToPathTestSamples.ReadOnly = true;
            this.fieldToPathTestSamples.Size = new System.Drawing.Size(190, 21);
            this.fieldToPathTestSamples.TabIndex = 16;
            // 
            // butSelectTestSamples
            // 
            this.butSelectTestSamples.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butSelectTestSamples.Location = new System.Drawing.Point(199, 136);
            this.butSelectTestSamples.Name = "butSelectTestSamples";
            this.butSelectTestSamples.Size = new System.Drawing.Size(92, 31);
            this.butSelectTestSamples.TabIndex = 17;
            this.butSelectTestSamples.Text = "Выбрать...";
            this.butSelectTestSamples.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(2, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "Путь к тестовым ответам";
            // 
            // fieldToPathTestAnswers
            // 
            this.fieldToPathTestAnswers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fieldToPathTestAnswers.Location = new System.Drawing.Point(4, 193);
            this.fieldToPathTestAnswers.Name = "fieldToPathTestAnswers";
            this.fieldToPathTestAnswers.ReadOnly = true;
            this.fieldToPathTestAnswers.Size = new System.Drawing.Size(190, 21);
            this.fieldToPathTestAnswers.TabIndex = 19;
            // 
            // butSelectTestAnswers
            // 
            this.butSelectTestAnswers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butSelectTestAnswers.Location = new System.Drawing.Point(199, 187);
            this.butSelectTestAnswers.Name = "butSelectTestAnswers";
            this.butSelectTestAnswers.Size = new System.Drawing.Size(92, 31);
            this.butSelectTestAnswers.TabIndex = 20;
            this.butSelectTestAnswers.Text = "Выбрать...";
            this.butSelectTestAnswers.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(198, 242);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "Примеры Test:";
            // 
            // numSizeTestSample
            // 
            this.numSizeTestSample.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numSizeTestSample.Location = new System.Drawing.Point(200, 257);
            this.numSizeTestSample.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSizeTestSample.Name = "numSizeTestSample";
            this.numSizeTestSample.Size = new System.Drawing.Size(91, 21);
            this.numSizeTestSample.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.numClassCount);
            this.groupBox1.Controls.Add(this.butLoad);
            this.groupBox1.Controls.Add(this.numSizeTestSample);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.butSelectTestAnswers);
            this.groupBox1.Controls.Add(this.butSelectTestSamples);
            this.groupBox1.Controls.Add(this.butSelectSamples);
            this.groupBox1.Controls.Add(this.butSelectAnswers);
            this.groupBox1.Controls.Add(this.numSizeSample);
            this.groupBox1.Controls.Add(this.fieldToPathTestAnswers);
            this.groupBox1.Controls.Add(this.numLengthSample);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.fieldToPathAnswers);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.fieldToPathTestSamples);
            this.groupBox1.Controls.Add(this.fieldToPathSamples);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 383);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор и Загрузка Данных";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(100, 291);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "Классы:";
            // 
            // numClassCount
            // 
            this.numClassCount.Location = new System.Drawing.Point(102, 306);
            this.numClassCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numClassCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numClassCount.Name = "numClassCount";
            this.numClassCount.Size = new System.Drawing.Size(92, 21);
            this.numClassCount.TabIndex = 23;
            this.numClassCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // butStart
            // 
            this.butStart.Enabled = false;
            this.butStart.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butStart.Location = new System.Drawing.Point(172, 27);
            this.butStart.Name = "butStart";
            this.butStart.Size = new System.Drawing.Size(130, 30);
            this.butStart.TabIndex = 11;
            this.butStart.Text = "Старт";
            this.butStart.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.butStart);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Location = new System.Drawing.Point(304, 510);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(477, 75);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Сервер";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(320, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 26);
            this.label9.TabIndex = 10;
            this.label9.Text = "                  © Romanov, 2015\r\ncybercortex.store@gmail.com";
            // 
            // numClassifiers
            // 
            this.numClassifiers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numClassifiers.Location = new System.Drawing.Point(102, 44);
            this.numClassifiers.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numClassifiers.Name = "numClassifiers";
            this.numClassifiers.Size = new System.Drawing.Size(92, 21);
            this.numClassifiers.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(100, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "Классификаторы:";
            // 
            // butLearn
            // 
            this.butLearn.Enabled = false;
            this.butLearn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butLearn.Location = new System.Drawing.Point(83, 70);
            this.butLearn.Name = "butLearn";
            this.butLearn.Size = new System.Drawing.Size(130, 30);
            this.butLearn.TabIndex = 31;
            this.butLearn.Text = "Обучить >>";
            this.butLearn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.butLoadClassifiers);
            this.groupBox2.Controls.Add(this.butSaveClassifiers);
            this.groupBox2.Controls.Add(this.butLearn);
            this.groupBox2.Controls.Add(this.numClassifiers);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(3, 395);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 190);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Управление Алгоритмом";
            // 
            // butLoadClassifiers
            // 
            this.butLoadClassifiers.Enabled = false;
            this.butLoadClassifiers.Location = new System.Drawing.Point(83, 142);
            this.butLoadClassifiers.Name = "butLoadClassifiers";
            this.butLoadClassifiers.Size = new System.Drawing.Size(130, 30);
            this.butLoadClassifiers.TabIndex = 33;
            this.butLoadClassifiers.Text = "Загрузить";
            this.butLoadClassifiers.UseVisualStyleBackColor = true;
            // 
            // butSaveClassifiers
            // 
            this.butSaveClassifiers.Enabled = false;
            this.butSaveClassifiers.Location = new System.Drawing.Point(83, 106);
            this.butSaveClassifiers.Name = "butSaveClassifiers";
            this.butSaveClassifiers.Size = new System.Drawing.Size(130, 30);
            this.butSaveClassifiers.TabIndex = 32;
            this.butSaveClassifiers.Text = "Сохранить";
            this.butSaveClassifiers.UseVisualStyleBackColor = true;
            // 
            // fieldForContent
            // 
            this.fieldForContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fieldForContent.Location = new System.Drawing.Point(6, 19);
            this.fieldForContent.Multiline = true;
            this.fieldForContent.Name = "fieldForContent";
            this.fieldForContent.ReadOnly = true;
            this.fieldForContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.fieldForContent.Size = new System.Drawing.Size(465, 434);
            this.fieldForContent.TabIndex = 1;
            this.fieldForContent.WordWrap = false;
            // 
            // butClean
            // 
            this.butClean.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butClean.Location = new System.Drawing.Point(172, 459);
            this.butClean.Name = "butClean";
            this.butClean.Size = new System.Drawing.Size(130, 30);
            this.butClean.TabIndex = 9;
            this.butClean.Text = "Очистить";
            this.butClean.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkLogs);
            this.groupBox4.Controls.Add(this.butClean);
            this.groupBox4.Controls.Add(this.fieldForContent);
            this.groupBox4.Location = new System.Drawing.Point(304, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(477, 498);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Консоль";
            // 
            // checkLogs
            // 
            this.checkLogs.AutoSize = true;
            this.checkLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkLogs.Location = new System.Drawing.Point(6, 460);
            this.checkLogs.Name = "checkLogs";
            this.checkLogs.Size = new System.Drawing.Size(115, 16);
            this.checkLogs.TabIndex = 10;
            this.checkLogs.Text = "Вывод лога обучения";
            this.checkLogs.UseVisualStyleBackColor = true;
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 590);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "View";
            this.Text = "CyberCortex® Machine Learning for Trading";
            ((System.ComponentModel.ISupportInitialize)(this.numSizeSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLengthSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSizeTestSample)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClassCount)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClassifiers)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butSelectSamples;
        private System.Windows.Forms.TextBox fieldToPathSamples;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numSizeSample;
        private System.Windows.Forms.NumericUpDown numLengthSample;
        private System.Windows.Forms.Button butLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fieldToPathAnswers;
        private System.Windows.Forms.Button butSelectAnswers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox fieldToPathTestSamples;
        private System.Windows.Forms.Button butSelectTestSamples;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox fieldToPathTestAnswers;
        private System.Windows.Forms.Button butSelectTestAnswers;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numSizeTestSample;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butStart;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown numClassifiers;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button butLearn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox fieldForContent;
        private System.Windows.Forms.Button butClean;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button butSaveClassifiers;
        private System.Windows.Forms.Button butLoadClassifiers;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numClassCount;
        private System.Windows.Forms.CheckBox checkLogs;
    }
}

