namespace RecoHuman
{
	partial class CtrlRecognitionResult
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
			this.pbBaseFace = new System.Windows.Forms.PictureBox();
			this.pbComparedFace = new System.Windows.Forms.PictureBox();
			this.lblMatch = new System.Windows.Forms.Label();
			this.lblAffinity = new System.Windows.Forms.Label();
			this.lblBaseFace = new System.Windows.Forms.Label();
			this.lblComparedFace = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pbBaseFace)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbComparedFace)).BeginInit();
			this.SuspendLayout();
			// 
			// pbBaseFace
			// 
			this.pbBaseFace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.pbBaseFace.Image = global::RecoHuman.Properties.Resources.cross48;
			this.pbBaseFace.Location = new System.Drawing.Point(3, 3);
			this.pbBaseFace.Name = "pbBaseFace";
			this.pbBaseFace.Size = new System.Drawing.Size(100, 100);
			this.pbBaseFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pbBaseFace.TabIndex = 0;
			this.pbBaseFace.TabStop = false;
			// 
			// pbComparedFace
			// 
			this.pbComparedFace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pbComparedFace.Image = global::RecoHuman.Properties.Resources.cross48;
			this.pbComparedFace.Location = new System.Drawing.Point(163, 3);
			this.pbComparedFace.Name = "pbComparedFace";
			this.pbComparedFace.Size = new System.Drawing.Size(100, 100);
			this.pbComparedFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pbComparedFace.TabIndex = 0;
			this.pbComparedFace.TabStop = false;
			// 
			// lblMatch
			// 
			this.lblMatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMatch.AutoSize = true;
			this.lblMatch.Location = new System.Drawing.Point(109, 40);
			this.lblMatch.Name = "lblMatch";
			this.lblMatch.Size = new System.Drawing.Size(48, 13);
			this.lblMatch.TabIndex = 1;
			this.lblMatch.Text = "Matches";
			this.lblMatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAffinity
			// 
			this.lblAffinity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblAffinity.AutoSize = true;
			this.lblAffinity.Location = new System.Drawing.Point(109, 53);
			this.lblAffinity.Name = "lblAffinity";
			this.lblAffinity.Size = new System.Drawing.Size(48, 13);
			this.lblAffinity.TabIndex = 1;
			this.lblAffinity.Text = "100.00%";
			this.lblAffinity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblBaseFace
			// 
			this.lblBaseFace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lblBaseFace.BackColor = System.Drawing.Color.Transparent;
			this.lblBaseFace.Location = new System.Drawing.Point(3, 90);
			this.lblBaseFace.Name = "lblBaseFace";
			this.lblBaseFace.Size = new System.Drawing.Size(100, 13);
			this.lblBaseFace.TabIndex = 1;
			this.lblBaseFace.Text = "Matches";
			this.lblBaseFace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblComparedFace
			// 
			this.lblComparedFace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblComparedFace.BackColor = System.Drawing.Color.Transparent;
			this.lblComparedFace.Location = new System.Drawing.Point(163, 90);
			this.lblComparedFace.Name = "lblComparedFace";
			this.lblComparedFace.Size = new System.Drawing.Size(100, 13);
			this.lblComparedFace.TabIndex = 1;
			this.lblComparedFace.Text = "Matches";
			this.lblComparedFace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CtrlRecognitionResult
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblAffinity);
			this.Controls.Add(this.lblComparedFace);
			this.Controls.Add(this.lblBaseFace);
			this.Controls.Add(this.lblMatch);
			this.Controls.Add(this.pbComparedFace);
			this.Controls.Add(this.pbBaseFace);
			this.MaximumSize = new System.Drawing.Size(266, 106);
			this.MinimumSize = new System.Drawing.Size(266, 106);
			this.Name = "CtrlRecognitionResult";
			this.Size = new System.Drawing.Size(266, 106);
			((System.ComponentModel.ISupportInitialize)(this.pbBaseFace)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbComparedFace)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbBaseFace;
		private System.Windows.Forms.PictureBox pbComparedFace;
		private System.Windows.Forms.Label lblMatch;
		private System.Windows.Forms.Label lblAffinity;
		private System.Windows.Forms.Label lblBaseFace;
		private System.Windows.Forms.Label lblComparedFace;
	}
}
