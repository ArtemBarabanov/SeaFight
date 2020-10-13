namespace SeaFightInterface
{
    partial class GameForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.startButton = new System.Windows.Forms.PictureBox();
            this.lazyPctr = new System.Windows.Forms.PictureBox();
            this.closePctr = new System.Windows.Forms.PictureBox();
            this.minimizePctr = new System.Windows.Forms.PictureBox();
            this.WavesTimer = new System.Windows.Forms.Timer(this.components);
            this.PlayerLettersBox = new System.Windows.Forms.PictureBox();
            this.OpponentLettersBox = new System.Windows.Forms.PictureBox();
            this.OpponentNumbersBox = new System.Windows.Forms.PictureBox();
            this.PlayerNumbersBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.startButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lazyPctr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closePctr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizePctr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerLettersBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentLettersBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentNumbersBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerNumbersBox)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.Transparent;
            this.startButton.BackgroundImage = global::SeaFightInterface.Properties.Resources.Fight;
            this.startButton.Location = new System.Drawing.Point(565, 210);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 100);
            this.startButton.TabIndex = 2;
            this.startButton.TabStop = false;
            this.startButton.Visible = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            this.startButton.MouseEnter += new System.EventHandler(this.startButton_MouseEnter);
            this.startButton.MouseLeave += new System.EventHandler(this.startButton_MouseLeave);
            // 
            // lazyPctr
            // 
            this.lazyPctr.BackColor = System.Drawing.Color.Transparent;
            this.lazyPctr.Image = global::SeaFightInterface.Properties.Resources.Лень1;
            this.lazyPctr.Location = new System.Drawing.Point(565, 358);
            this.lazyPctr.Name = "lazyPctr";
            this.lazyPctr.Size = new System.Drawing.Size(100, 80);
            this.lazyPctr.TabIndex = 5;
            this.lazyPctr.TabStop = false;
            this.lazyPctr.Click += new System.EventHandler(this.lazyPctr_Click);
            this.lazyPctr.MouseEnter += new System.EventHandler(this.lazyPctr_MouseEnter);
            this.lazyPctr.MouseLeave += new System.EventHandler(this.lazyPctr_MouseLeave);
            // 
            // closePctr
            // 
            this.closePctr.BackColor = System.Drawing.Color.Transparent;
            this.closePctr.BackgroundImage = global::SeaFightInterface.Properties.Resources.Close;
            this.closePctr.Location = new System.Drawing.Point(1138, 2);
            this.closePctr.Name = "closePctr";
            this.closePctr.Size = new System.Drawing.Size(40, 40);
            this.closePctr.TabIndex = 9;
            this.closePctr.TabStop = false;
            this.closePctr.Click += new System.EventHandler(this.closePctr_Click);
            this.closePctr.MouseEnter += new System.EventHandler(this.closePctr_MouseEnter);
            this.closePctr.MouseLeave += new System.EventHandler(this.closePctr_MouseLeave);
            // 
            // minimizePctr
            // 
            this.minimizePctr.BackColor = System.Drawing.Color.Transparent;
            this.minimizePctr.BackgroundImage = global::SeaFightInterface.Properties.Resources.Minimize;
            this.minimizePctr.Location = new System.Drawing.Point(1079, 2);
            this.minimizePctr.Name = "minimizePctr";
            this.minimizePctr.Size = new System.Drawing.Size(40, 40);
            this.minimizePctr.TabIndex = 10;
            this.minimizePctr.TabStop = false;
            this.minimizePctr.Click += new System.EventHandler(this.minimizePctr_Click);
            this.minimizePctr.MouseEnter += new System.EventHandler(this.minimizePctr_MouseEnter);
            this.minimizePctr.MouseLeave += new System.EventHandler(this.minimizePctr_MouseLeave);
            // 
            // WavesTimer
            // 
            this.WavesTimer.Enabled = true;
            this.WavesTimer.Tick += new System.EventHandler(this.Waves_Tick);
            // 
            // PlayerLettersBox
            // 
            this.PlayerLettersBox.BackColor = System.Drawing.Color.Transparent;
            this.PlayerLettersBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PlayerLettersBox.Image = global::SeaFightInterface.Properties.Resources.Letters;
            this.PlayerLettersBox.Location = new System.Drawing.Point(80, 40);
            this.PlayerLettersBox.Name = "PlayerLettersBox";
            this.PlayerLettersBox.Size = new System.Drawing.Size(400, 40);
            this.PlayerLettersBox.TabIndex = 11;
            this.PlayerLettersBox.TabStop = false;
            // 
            // OpponentLettersBox
            // 
            this.OpponentLettersBox.BackColor = System.Drawing.Color.Transparent;
            this.OpponentLettersBox.Image = global::SeaFightInterface.Properties.Resources.Letters;
            this.OpponentLettersBox.Location = new System.Drawing.Point(760, 40);
            this.OpponentLettersBox.Name = "OpponentLettersBox";
            this.OpponentLettersBox.Size = new System.Drawing.Size(400, 40);
            this.OpponentLettersBox.TabIndex = 12;
            this.OpponentLettersBox.TabStop = false;
            // 
            // OpponentNumbersBox
            // 
            this.OpponentNumbersBox.BackColor = System.Drawing.Color.Transparent;
            this.OpponentNumbersBox.BackgroundImage = global::SeaFightInterface.Properties.Resources.Numbers;
            this.OpponentNumbersBox.Location = new System.Drawing.Point(720, 80);
            this.OpponentNumbersBox.Name = "OpponentNumbersBox";
            this.OpponentNumbersBox.Size = new System.Drawing.Size(40, 400);
            this.OpponentNumbersBox.TabIndex = 13;
            this.OpponentNumbersBox.TabStop = false;
            // 
            // PlayerNumbersBox
            // 
            this.PlayerNumbersBox.BackColor = System.Drawing.Color.Transparent;
            this.PlayerNumbersBox.BackgroundImage = global::SeaFightInterface.Properties.Resources.Numbers;
            this.PlayerNumbersBox.Location = new System.Drawing.Point(40, 80);
            this.PlayerNumbersBox.Name = "PlayerNumbersBox";
            this.PlayerNumbersBox.Size = new System.Drawing.Size(40, 400);
            this.PlayerNumbersBox.TabIndex = 14;
            this.PlayerNumbersBox.TabStop = false;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1200, 760);
            this.Controls.Add(this.PlayerNumbersBox);
            this.Controls.Add(this.OpponentNumbersBox);
            this.Controls.Add(this.OpponentLettersBox);
            this.Controls.Add(this.PlayerLettersBox);
            this.Controls.Add(this.minimizePctr);
            this.Controls.Add(this.closePctr);
            this.Controls.Add(this.lazyPctr);
            this.Controls.Add(this.startButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.GameForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.startButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lazyPctr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closePctr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizePctr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerLettersBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentLettersBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentNumbersBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerNumbersBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox startButton;
        private System.Windows.Forms.PictureBox lazyPctr;
        private System.Windows.Forms.PictureBox closePctr;
        private System.Windows.Forms.PictureBox minimizePctr;
        private System.Windows.Forms.Timer WavesTimer;
        private System.Windows.Forms.PictureBox PlayerLettersBox;
        private System.Windows.Forms.PictureBox OpponentLettersBox;
        private System.Windows.Forms.PictureBox OpponentNumbersBox;
        private System.Windows.Forms.PictureBox PlayerNumbersBox;
    }
}

