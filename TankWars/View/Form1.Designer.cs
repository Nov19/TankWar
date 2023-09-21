// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
namespace TankWars
{
    partial class Form1
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
            this.playerNameBox = new System.Windows.Forms.TextBox();
            this.serverAddressBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Help = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // playerNameBox
            // 
            this.playerNameBox.Location = new System.Drawing.Point(269, 8);
            this.playerNameBox.Name = "playerNameBox";
            this.playerNameBox.Size = new System.Drawing.Size(100, 21);
            this.playerNameBox.TabIndex = 0;
            this.playerNameBox.TextChanged += new System.EventHandler(this.playerNameBox_TextChanged);
            // 
            // serverAddressBox
            // 
            this.serverAddressBox.Location = new System.Drawing.Point(68, 8);
            this.serverAddressBox.Name = "serverAddressBox";
            this.serverAddressBox.Size = new System.Drawing.Size(100, 21);
            this.serverAddressBox.TabIndex = 1;
            this.serverAddressBox.TextChanged += new System.EventHandler(this.serverAddressBox_TextChanged);
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(396, 6);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 2;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Player Name:";
            // 
            // Help
            // 
            this.Help.Location = new System.Drawing.Point(713, 6);
            this.Help.Name = "Help";
            this.Help.Size = new System.Drawing.Size(75, 23);
            this.Help.TabIndex = 5;
            this.Help.Text = "Help";
            this.Help.UseVisualStyleBackColor = true;
            this.Help.Click += new System.EventHandler(this.Help_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Help);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.serverAddressBox);
            this.Controls.Add(this.playerNameBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HandleKeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HandleMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleMouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox playerNameBox;
        private System.Windows.Forms.TextBox serverAddressBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Help;
    }
}

