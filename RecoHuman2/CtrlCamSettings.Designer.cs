namespace RecoHuman
{
	partial class CtrlCamSettings
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
			this.gbCameraSettings = new System.Windows.Forms.GroupBox();
			this.chkAutomaticSettings = new System.Windows.Forms.CheckBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.chkMirrorVertical = new System.Windows.Forms.CheckBox();
			this.chkMirrorHorizontal = new System.Windows.Forms.CheckBox();
			this.cmbVideoFormats = new System.Windows.Forms.ComboBox();
			this.lblVideoFormats = new System.Windows.Forms.Label();
			this.gbCameraSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbCameraSettings
			// 
			this.gbCameraSettings.Controls.Add(this.chkAutomaticSettings);
			this.gbCameraSettings.Controls.Add(this.chkEnabled);
			this.gbCameraSettings.Controls.Add(this.chkMirrorVertical);
			this.gbCameraSettings.Controls.Add(this.chkMirrorHorizontal);
			this.gbCameraSettings.Controls.Add(this.cmbVideoFormats);
			this.gbCameraSettings.Controls.Add(this.lblVideoFormats);
			this.gbCameraSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbCameraSettings.Location = new System.Drawing.Point(0, 0);
			this.gbCameraSettings.MinimumSize = new System.Drawing.Size(210, 148);
			this.gbCameraSettings.Name = "gbCameraSettings";
			this.gbCameraSettings.Size = new System.Drawing.Size(210, 148);
			this.gbCameraSettings.TabIndex = 2;
			this.gbCameraSettings.TabStop = false;
			this.gbCameraSettings.Text = "Camera Settings";
			// 
			// chkAutomaticSettings
			// 
			this.chkAutomaticSettings.AutoSize = true;
			this.chkAutomaticSettings.Enabled = false;
			this.chkAutomaticSettings.Location = new System.Drawing.Point(6, 87);
			this.chkAutomaticSettings.Name = "chkAutomaticSettings";
			this.chkAutomaticSettings.Size = new System.Drawing.Size(114, 17);
			this.chkAutomaticSettings.TabIndex = 2;
			this.chkAutomaticSettings.Text = "Automatic Settings";
			this.chkAutomaticSettings.UseVisualStyleBackColor = true;
			this.chkAutomaticSettings.CheckedChanged += new System.EventHandler(this.chkAutomaticSettings_CheckedChanged);
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.chkEnabled.Enabled = false;
			this.chkEnabled.Location = new System.Drawing.Point(6, 110);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 2;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
			// 
			// chkMirrorVertical
			// 
			this.chkMirrorVertical.AutoSize = true;
			this.chkMirrorVertical.Location = new System.Drawing.Point(114, 64);
			this.chkMirrorVertical.Name = "chkMirrorVertical";
			this.chkMirrorVertical.Size = new System.Drawing.Size(90, 17);
			this.chkMirrorVertical.TabIndex = 2;
			this.chkMirrorVertical.Text = "Mirror Vertical";
			this.chkMirrorVertical.UseVisualStyleBackColor = true;
			this.chkMirrorVertical.CheckedChanged += new System.EventHandler(this.chkMirrorVertical_CheckedChanged);
			// 
			// chkMirrorHorizontal
			// 
			this.chkMirrorHorizontal.AutoSize = true;
			this.chkMirrorHorizontal.Location = new System.Drawing.Point(6, 64);
			this.chkMirrorHorizontal.Name = "chkMirrorHorizontal";
			this.chkMirrorHorizontal.Size = new System.Drawing.Size(102, 17);
			this.chkMirrorHorizontal.TabIndex = 2;
			this.chkMirrorHorizontal.Text = "Mirror Horizontal";
			this.chkMirrorHorizontal.UseVisualStyleBackColor = true;
			this.chkMirrorHorizontal.CheckedChanged += new System.EventHandler(this.chkMirrorHorizontal_CheckedChanged);
			// 
			// cmbVideoFormats
			// 
			this.cmbVideoFormats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbVideoFormats.FormattingEnabled = true;
			this.cmbVideoFormats.Location = new System.Drawing.Point(6, 37);
			this.cmbVideoFormats.Name = "cmbVideoFormats";
			this.cmbVideoFormats.Size = new System.Drawing.Size(198, 21);
			this.cmbVideoFormats.TabIndex = 1;
			this.cmbVideoFormats.SelectedIndexChanged += new System.EventHandler(this.cmbVideoFormats_SelectedIndexChanged);
			// 
			// lblVideoFormats
			// 
			this.lblVideoFormats.AutoSize = true;
			this.lblVideoFormats.Location = new System.Drawing.Point(7, 20);
			this.lblVideoFormats.Name = "lblVideoFormats";
			this.lblVideoFormats.Size = new System.Drawing.Size(117, 13);
			this.lblVideoFormats.TabIndex = 0;
			this.lblVideoFormats.Text = "Selected Video Format:";
			// 
			// CtrlCamSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbCameraSettings);
			this.MinimumSize = new System.Drawing.Size(210, 148);
			this.Name = "CtrlCamSettings";
			this.Size = new System.Drawing.Size(210, 148);
			this.gbCameraSettings.ResumeLayout(false);
			this.gbCameraSettings.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbCameraSettings;
		private System.Windows.Forms.CheckBox chkAutomaticSettings;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.CheckBox chkMirrorVertical;
		private System.Windows.Forms.CheckBox chkMirrorHorizontal;
		private System.Windows.Forms.ComboBox cmbVideoFormats;
		private System.Windows.Forms.Label lblVideoFormats;
	}
}
