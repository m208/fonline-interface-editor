
namespace FOIE
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cfgFileNameLabel = new System.Windows.Forms.Label();
            this.settingsButton1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Configuration file:";
            // 
            // cfgFileNameLabel
            // 
            this.cfgFileNameLabel.AutoSize = true;
            this.cfgFileNameLabel.Location = new System.Drawing.Point(16, 63);
            this.cfgFileNameLabel.Name = "cfgFileNameLabel";
            this.cfgFileNameLabel.Size = new System.Drawing.Size(92, 13);
            this.cfgFileNameLabel.TabIndex = 1;
            this.cfgFileNameLabel.Text = "cfgFileNameLabel";
            // 
            // settingsButton1
            // 
            this.settingsButton1.Location = new System.Drawing.Point(169, 58);
            this.settingsButton1.Name = "settingsButton1";
            this.settingsButton1.Size = new System.Drawing.Size(75, 23);
            this.settingsButton1.TabIndex = 2;
            this.settingsButton1.Text = "Browse";
            this.settingsButton1.UseVisualStyleBackColor = true;
            this.settingsButton1.Click += new System.EventHandler(this.settingsButton1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(91, 154);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(97, 41);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 207);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.settingsButton1);
            this.Controls.Add(this.cfgFileNameLabel);
            this.Controls.Add(this.label1);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label cfgFileNameLabel;
        private System.Windows.Forms.Button settingsButton1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOk;
    }
}