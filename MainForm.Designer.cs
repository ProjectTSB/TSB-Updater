
namespace TSB_Updater
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.worldPath = new System.Windows.Forms.TextBox();
            this.infoText = new System.Windows.Forms.Label();
            this.browsButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.worldFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TSB_Updater.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(458, 143);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "ワールドフォルダ";
            // 
            // worldPath
            // 
            this.worldPath.Location = new System.Drawing.Point(12, 242);
            this.worldPath.Name = "worldPath";
            this.worldPath.Size = new System.Drawing.Size(387, 27);
            this.worldPath.TabIndex = 3;
            // 
            // infoText
            // 
            this.infoText.AutoSize = true;
            this.infoText.Location = new System.Drawing.Point(24, 158);
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(436, 40);
            this.infoText.TabIndex = 4;
            this.infoText.Text = "TSB Updater(簡易版)にようこそ！\r\n本ツールはパッチレベルのアップデートのみに対応する点に注意してください。";
            this.infoText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // browsButton
            // 
            this.browsButton.Location = new System.Drawing.Point(405, 240);
            this.browsButton.Name = "browsButton";
            this.browsButton.Size = new System.Drawing.Size(65, 29);
            this.browsButton.TabIndex = 5;
            this.browsButton.Text = "...";
            this.browsButton.UseVisualStyleBackColor = true;
            this.browsButton.Click += new System.EventHandler(this.browsButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(193, 282);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(94, 29);
            this.updateButton.TabIndex = 6;
            this.updateButton.Text = "更新を確認";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // worldFolderBrowserDialog
            // 
            this.worldFolderBrowserDialog.RootFolder = System.Environment.SpecialFolder.Windows;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 323);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.browsButton);
            this.Controls.Add(this.infoText);
            this.Controls.Add(this.worldPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "TSB Updater beta v1.1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox worldPath;
        private System.Windows.Forms.Label infoText;
        private System.Windows.Forms.Button browsButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.FolderBrowserDialog worldFolderBrowserDialog;
    }
}

