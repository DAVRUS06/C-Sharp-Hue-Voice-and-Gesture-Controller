namespace hueController
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLightNames = new System.Windows.Forms.TextBox();
            this.textBoxGroupNames = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDetectedCommands = new System.Windows.Forms.TextBox();
            this.textBoxActionsTaken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxUserKey = new System.Windows.Forms.TextBox();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.textBoxColorChoices = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelVoiceRecognition = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelConnectStatus = new System.Windows.Forms.Label();
            this.pictureBoxKinect = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.labelGestureOnOff = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxKinect)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(32, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Light Names";
            // 
            // textBoxLightNames
            // 
            this.textBoxLightNames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxLightNames.ForeColor = System.Drawing.Color.White;
            this.textBoxLightNames.Location = new System.Drawing.Point(12, 75);
            this.textBoxLightNames.Multiline = true;
            this.textBoxLightNames.Name = "textBoxLightNames";
            this.textBoxLightNames.ReadOnly = true;
            this.textBoxLightNames.Size = new System.Drawing.Size(122, 275);
            this.textBoxLightNames.TabIndex = 1;
            // 
            // textBoxGroupNames
            // 
            this.textBoxGroupNames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxGroupNames.ForeColor = System.Drawing.Color.White;
            this.textBoxGroupNames.Location = new System.Drawing.Point(146, 75);
            this.textBoxGroupNames.Multiline = true;
            this.textBoxGroupNames.Name = "textBoxGroupNames";
            this.textBoxGroupNames.ReadOnly = true;
            this.textBoxGroupNames.Size = new System.Drawing.Size(126, 275);
            this.textBoxGroupNames.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(162, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Group Names";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(144, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Detected commands";
            // 
            // textBoxDetectedCommands
            // 
            this.textBoxDetectedCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxDetectedCommands.ForeColor = System.Drawing.Color.White;
            this.textBoxDetectedCommands.Location = new System.Drawing.Point(12, 378);
            this.textBoxDetectedCommands.Multiline = true;
            this.textBoxDetectedCommands.Name = "textBoxDetectedCommands";
            this.textBoxDetectedCommands.ReadOnly = true;
            this.textBoxDetectedCommands.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDetectedCommands.Size = new System.Drawing.Size(397, 135);
            this.textBoxDetectedCommands.TabIndex = 5;
            this.textBoxDetectedCommands.TextChanged += new System.EventHandler(this.textBoxDetectedCommands_TextChanged);
            // 
            // textBoxActionsTaken
            // 
            this.textBoxActionsTaken.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxActionsTaken.ForeColor = System.Drawing.Color.White;
            this.textBoxActionsTaken.Location = new System.Drawing.Point(12, 539);
            this.textBoxActionsTaken.Multiline = true;
            this.textBoxActionsTaken.Name = "textBoxActionsTaken";
            this.textBoxActionsTaken.ReadOnly = true;
            this.textBoxActionsTaken.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxActionsTaken.Size = new System.Drawing.Size(397, 135);
            this.textBoxActionsTaken.TabIndex = 7;
            this.textBoxActionsTaken.TextChanged += new System.EventHandler(this.textBoxActionsTaken_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(163, 519);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Actions Taken";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(17, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Bridge IP Address:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Bridge User Token: ";
            // 
            // textBoxIP
            // 
            this.textBoxIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxIP.ForeColor = System.Drawing.Color.White;
            this.textBoxIP.Location = new System.Drawing.Point(138, 10);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(202, 20);
            this.textBoxIP.TabIndex = 10;
            // 
            // textBoxUserKey
            // 
            this.textBoxUserKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxUserKey.ForeColor = System.Drawing.Color.White;
            this.textBoxUserKey.Location = new System.Drawing.Point(138, 33);
            this.textBoxUserKey.Name = "textBoxUserKey";
            this.textBoxUserKey.Size = new System.Drawing.Size(202, 20);
            this.textBoxUserKey.TabIndex = 11;
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(346, 10);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(63, 43);
            this.buttonSaveSettings.TabIndex = 12;
            this.buttonSaveSettings.Text = "Save";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // textBoxColorChoices
            // 
            this.textBoxColorChoices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxColorChoices.ForeColor = System.Drawing.Color.White;
            this.textBoxColorChoices.Location = new System.Drawing.Point(283, 75);
            this.textBoxColorChoices.Multiline = true;
            this.textBoxColorChoices.Name = "textBoxColorChoices";
            this.textBoxColorChoices.ReadOnly = true;
            this.textBoxColorChoices.Size = new System.Drawing.Size(126, 275);
            this.textBoxColorChoices.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(303, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "Color Choices";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(576, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "System Status";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(510, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 16);
            this.label10.TabIndex = 17;
            this.label10.Text = "Voice Recognition: ";
            // 
            // labelVoiceRecognition
            // 
            this.labelVoiceRecognition.AutoSize = true;
            this.labelVoiceRecognition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.labelVoiceRecognition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVoiceRecognition.ForeColor = System.Drawing.Color.Green;
            this.labelVoiceRecognition.Location = new System.Drawing.Point(640, 58);
            this.labelVoiceRecognition.Name = "labelVoiceRecognition";
            this.labelVoiceRecognition.Size = new System.Drawing.Size(51, 16);
            this.labelVoiceRecognition.TabIndex = 29;
            this.labelVoiceRecognition.Text = "Active";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(482, 33);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(152, 16);
            this.label15.TabIndex = 30;
            this.label15.Text = "Hue Bridge Connection: ";
            // 
            // labelConnectStatus
            // 
            this.labelConnectStatus.AutoSize = true;
            this.labelConnectStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.labelConnectStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnectStatus.ForeColor = System.Drawing.Color.Red;
            this.labelConnectStatus.Location = new System.Drawing.Point(640, 33);
            this.labelConnectStatus.Name = "labelConnectStatus";
            this.labelConnectStatus.Size = new System.Drawing.Size(218, 16);
            this.labelConnectStatus.TabIndex = 31;
            this.labelConnectStatus.Text = "Inactive - Check IP/UserToken";
            // 
            // pictureBoxKinect
            // 
            this.pictureBoxKinect.Location = new System.Drawing.Point(415, 378);
            this.pictureBoxKinect.Name = "pictureBoxKinect";
            this.pictureBoxKinect.Size = new System.Drawing.Size(443, 295);
            this.pictureBoxKinect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxKinect.TabIndex = 32;
            this.pictureBoxKinect.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(528, 81);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 16);
            this.label12.TabIndex = 19;
            this.label12.Text = "Gesture Control: ";
            // 
            // labelGestureOnOff
            // 
            this.labelGestureOnOff.AutoSize = true;
            this.labelGestureOnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGestureOnOff.ForeColor = System.Drawing.Color.Green;
            this.labelGestureOnOff.Location = new System.Drawing.Point(640, 81);
            this.labelGestureOnOff.Name = "labelGestureOnOff";
            this.labelGestureOnOff.Size = new System.Drawing.Size(51, 16);
            this.labelGestureOnOff.TabIndex = 27;
            this.labelGestureOnOff.Text = "Active";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(872, 686);
            this.Controls.Add(this.pictureBoxKinect);
            this.Controls.Add(this.labelConnectStatus);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.labelVoiceRecognition);
            this.Controls.Add(this.labelGestureOnOff);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxColorChoices);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.textBoxUserKey);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxActionsTaken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDetectedCommands);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxGroupNames);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLightNames);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxKinect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLightNames;
        private System.Windows.Forms.TextBox textBoxGroupNames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDetectedCommands;
        private System.Windows.Forms.TextBox textBoxActionsTaken;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxUserKey;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.TextBox textBoxColorChoices;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelVoiceRecognition;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelConnectStatus;
        private System.Windows.Forms.PictureBox pictureBoxKinect;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelGestureOnOff;
    }
}

