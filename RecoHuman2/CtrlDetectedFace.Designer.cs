namespace RecoHuman
{
	partial class CtrlDetectedFace
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblSize = new System.Windows.Forms.Label();
			this.lblLocation = new System.Windows.Forms.Label();
			this.pbFace = new System.Windows.Forms.PictureBox();
			this.txtLocation = new System.Windows.Forms.TextBox();
			this.txtSize = new System.Windows.Forms.TextBox();
			this.lblFeatures = new System.Windows.Forms.Label();
			this.txtFeatures = new System.Windows.Forms.TextBox();
			this.txtConfidence = new System.Windows.Forms.TextBox();
			this.lblConfidence = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pbFace)).BeginInit();
			this.SuspendLayout();
			// 
			// lblSize
			// 
			this.lblSize.AutoSize = true;
			this.lblSize.Location = new System.Drawing.Point(84, 65);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(30, 13);
			this.lblSize.TabIndex = 3;
			this.lblSize.Text = "Size:";
			this.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(84, 46);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(51, 13);
			this.lblLocation.TabIndex = 4;
			this.lblLocation.Text = "Location:";
			this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbFace
			// 
			this.pbFace.Image = global::RecoHuman.Properties.Resources.cross48;
			this.pbFace.Location = new System.Drawing.Point(3, 3);
			this.pbFace.Name = "pbFace";
			this.pbFace.Size = new System.Drawing.Size(75, 75);
			this.pbFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pbFace.TabIndex = 2;
			this.pbFace.TabStop = false;
			// 
			// txtLocation
			// 
			this.txtLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLocation.Location = new System.Drawing.Point(141, 46);
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.ReadOnly = true;
			this.txtLocation.Size = new System.Drawing.Size(97, 13);
			this.txtLocation.TabIndex = 5;
			// 
			// txtSize
			// 
			this.txtSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSize.Location = new System.Drawing.Point(141, 65);
			this.txtSize.Name = "txtSize";
			this.txtSize.ReadOnly = true;
			this.txtSize.Size = new System.Drawing.Size(97, 13);
			this.txtSize.TabIndex = 5;
			// 
			// lblFeatures
			// 
			this.lblFeatures.AutoSize = true;
			this.lblFeatures.Location = new System.Drawing.Point(84, 27);
			this.lblFeatures.Name = "lblFeatures";
			this.lblFeatures.Size = new System.Drawing.Size(51, 13);
			this.lblFeatures.TabIndex = 4;
			this.lblFeatures.Text = "Features:";
			this.lblFeatures.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFeatures
			// 
			this.txtFeatures.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtFeatures.Location = new System.Drawing.Point(141, 27);
			this.txtFeatures.Name = "txtFeatures";
			this.txtFeatures.ReadOnly = true;
			this.txtFeatures.Size = new System.Drawing.Size(97, 13);
			this.txtFeatures.TabIndex = 5;
			// 
			// txtConfidence
			// 
			this.txtConfidence.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtConfidence.Location = new System.Drawing.Point(141, 8);
			this.txtConfidence.Name = "txtConfidence";
			this.txtConfidence.ReadOnly = true;
			this.txtConfidence.Size = new System.Drawing.Size(97, 13);
			this.txtConfidence.TabIndex = 5;
			// 
			// lblConfidence
			// 
			this.lblConfidence.AutoSize = true;
			this.lblConfidence.Location = new System.Drawing.Point(84, 8);
			this.lblConfidence.Name = "lblConfidence";
			this.lblConfidence.Size = new System.Drawing.Size(40, 13);
			this.lblConfidence.TabIndex = 4;
			this.lblConfidence.Text = "Confid:";
			this.lblConfidence.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CtrlDetectedFace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtSize);
			this.Controls.Add(this.txtConfidence);
			this.Controls.Add(this.txtFeatures);
			this.Controls.Add(this.txtLocation);
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.lblConfidence);
			this.Controls.Add(this.lblFeatures);
			this.Controls.Add(this.lblLocation);
			this.Controls.Add(this.pbFace);
			this.MaximumSize = new System.Drawing.Size(241, 81);
			this.MinimumSize = new System.Drawing.Size(241, 81);
			this.Name = "CtrlDetectedFace";
			this.Size = new System.Drawing.Size(241, 81);
			((System.ComponentModel.ISupportInitialize)(this.pbFace)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.PictureBox pbFace;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.TextBox txtSize;
		private System.Windows.Forms.Label lblFeatures;
		private System.Windows.Forms.TextBox txtFeatures;
		private System.Windows.Forms.TextBox txtConfidence;
		private System.Windows.Forms.Label lblConfidence;
	}
}
