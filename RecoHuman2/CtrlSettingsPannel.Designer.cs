namespace RecoHuman
{
	partial class CtrlSettingsPannel
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
			this.gbFeaturesExtraction = new System.Windows.Forms.GroupBox();
			this.lblMaxIOD = new System.Windows.Forms.Label();
			this.lblMinIOD = new System.Windows.Forms.Label();
			this.lblAttemptsWhileMatching = new System.Windows.Forms.Label();
			this.lblAttemptsWhileEnrolling = new System.Windows.Forms.Label();
			this.nudMaxIOD = new System.Windows.Forms.NumericUpDown();
			this.nudMinIOD = new System.Windows.Forms.NumericUpDown();
			this.nudAttemptsWhileMatching = new System.Windows.Forms.NumericUpDown();
			this.nudAttemptsWhileEnrolling = new System.Windows.Forms.NumericUpDown();
			this.gbEnollmentWGen = new System.Windows.Forms.GroupBox();
			this.nudImageCount = new System.Windows.Forms.NumericUpDown();
			this.nudGeneralizationTreshold = new System.Windows.Forms.NumericUpDown();
			this.lblImageCount = new System.Windows.Forms.Label();
			this.lblGeneralizationTreshold = new System.Windows.Forms.Label();
			this.gbMatching = new System.Windows.Forms.GroupBox();
			this.nudMaxMatchingResults = new System.Windows.Forms.NumericUpDown();
			this.nudMatchingAttempts = new System.Windows.Forms.NumericUpDown();
			this.nudMatchingTreshold = new System.Windows.Forms.NumericUpDown();
			this.lblMaxMatchingResults = new System.Windows.Forms.Label();
			this.lblMatchingAttempts = new System.Windows.Forms.Label();
			this.lblMatchingTreshold = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.gbFeaturesExtraction.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxIOD)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinIOD)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAttemptsWhileMatching)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAttemptsWhileEnrolling)).BeginInit();
			this.gbEnollmentWGen.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudImageCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudGeneralizationTreshold)).BeginInit();
			this.gbMatching.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxMatchingResults)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMatchingAttempts)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMatchingTreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// gbFeaturesExtraction
			// 
			this.gbFeaturesExtraction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.gbFeaturesExtraction.Controls.Add(this.label1);
			this.gbFeaturesExtraction.Controls.Add(this.lblMaxIOD);
			this.gbFeaturesExtraction.Controls.Add(this.lblMinIOD);
			this.gbFeaturesExtraction.Controls.Add(this.lblAttemptsWhileMatching);
			this.gbFeaturesExtraction.Controls.Add(this.lblAttemptsWhileEnrolling);
			this.gbFeaturesExtraction.Controls.Add(this.nudMaxIOD);
			this.gbFeaturesExtraction.Controls.Add(this.nudMinIOD);
			this.gbFeaturesExtraction.Controls.Add(this.nudAttemptsWhileMatching);
			this.gbFeaturesExtraction.Controls.Add(this.nudAttemptsWhileEnrolling);
			this.gbFeaturesExtraction.Location = new System.Drawing.Point(3, 3);
			this.gbFeaturesExtraction.Name = "gbFeaturesExtraction";
			this.gbFeaturesExtraction.Size = new System.Drawing.Size(200, 142);
			this.gbFeaturesExtraction.TabIndex = 1;
			this.gbFeaturesExtraction.TabStop = false;
			this.gbFeaturesExtraction.Text = "Features Extraction";
			// 
			// lblMaxIOD
			// 
			this.lblMaxIOD.AutoSize = true;
			this.lblMaxIOD.Location = new System.Drawing.Point(6, 99);
			this.lblMaxIOD.Name = "lblMaxIOD";
			this.lblMaxIOD.Size = new System.Drawing.Size(80, 13);
			this.lblMaxIOD.TabIndex = 0;
			this.lblMaxIOD.Text = "Maximum IOD*:";
			// 
			// lblMinIOD
			// 
			this.lblMinIOD.AutoSize = true;
			this.lblMinIOD.Location = new System.Drawing.Point(6, 73);
			this.lblMinIOD.Name = "lblMinIOD";
			this.lblMinIOD.Size = new System.Drawing.Size(71, 13);
			this.lblMinIOD.TabIndex = 0;
			this.lblMinIOD.Text = "Minimal IOD*:";
			// 
			// lblAttemptsWhileMatching
			// 
			this.lblAttemptsWhileMatching.AutoSize = true;
			this.lblAttemptsWhileMatching.Location = new System.Drawing.Point(6, 47);
			this.lblAttemptsWhileMatching.Name = "lblAttemptsWhileMatching";
			this.lblAttemptsWhileMatching.Size = new System.Drawing.Size(124, 13);
			this.lblAttemptsWhileMatching.TabIndex = 0;
			this.lblAttemptsWhileMatching.Text = "Attempts while matching:";
			// 
			// lblAttemptsWhileEnrolling
			// 
			this.lblAttemptsWhileEnrolling.AutoSize = true;
			this.lblAttemptsWhileEnrolling.Location = new System.Drawing.Point(6, 21);
			this.lblAttemptsWhileEnrolling.Name = "lblAttemptsWhileEnrolling";
			this.lblAttemptsWhileEnrolling.Size = new System.Drawing.Size(120, 13);
			this.lblAttemptsWhileEnrolling.TabIndex = 0;
			this.lblAttemptsWhileEnrolling.Text = "Attempts while enrolling:";
			// 
			// nudMaxIOD
			// 
			this.nudMaxIOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudMaxIOD.Location = new System.Drawing.Point(136, 97);
			this.nudMaxIOD.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.nudMaxIOD.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.nudMaxIOD.Name = "nudMaxIOD";
			this.nudMaxIOD.Size = new System.Drawing.Size(58, 20);
			this.nudMaxIOD.TabIndex = 3;
			this.nudMaxIOD.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.nudMaxIOD.ValueChanged += new System.EventHandler(this.nudMaxIOD_ValueChanged);
			// 
			// nudMinIOD
			// 
			this.nudMinIOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudMinIOD.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudMinIOD.Location = new System.Drawing.Point(136, 71);
			this.nudMinIOD.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.nudMinIOD.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.nudMinIOD.Name = "nudMinIOD";
			this.nudMinIOD.Size = new System.Drawing.Size(58, 20);
			this.nudMinIOD.TabIndex = 2;
			this.nudMinIOD.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.nudMinIOD.ValueChanged += new System.EventHandler(this.nudMinIOD_ValueChanged);
			// 
			// nudAttemptsWhileMatching
			// 
			this.nudAttemptsWhileMatching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudAttemptsWhileMatching.Location = new System.Drawing.Point(136, 45);
			this.nudAttemptsWhileMatching.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAttemptsWhileMatching.Name = "nudAttemptsWhileMatching";
			this.nudAttemptsWhileMatching.Size = new System.Drawing.Size(58, 20);
			this.nudAttemptsWhileMatching.TabIndex = 1;
			this.nudAttemptsWhileMatching.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.nudAttemptsWhileMatching.ValueChanged += new System.EventHandler(this.nudAttemptsWhileMatching_ValueChanged);
			// 
			// nudAttemptsWhileEnrolling
			// 
			this.nudAttemptsWhileEnrolling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudAttemptsWhileEnrolling.Location = new System.Drawing.Point(136, 19);
			this.nudAttemptsWhileEnrolling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAttemptsWhileEnrolling.Name = "nudAttemptsWhileEnrolling";
			this.nudAttemptsWhileEnrolling.Size = new System.Drawing.Size(58, 20);
			this.nudAttemptsWhileEnrolling.TabIndex = 0;
			this.nudAttemptsWhileEnrolling.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudAttemptsWhileEnrolling.ValueChanged += new System.EventHandler(this.nudAttemptsWhileEnrolling_ValueChanged);
			// 
			// gbEnollmentWGen
			// 
			this.gbEnollmentWGen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbEnollmentWGen.Controls.Add(this.nudImageCount);
			this.gbEnollmentWGen.Controls.Add(this.nudGeneralizationTreshold);
			this.gbEnollmentWGen.Controls.Add(this.lblImageCount);
			this.gbEnollmentWGen.Controls.Add(this.lblGeneralizationTreshold);
			this.gbEnollmentWGen.Location = new System.Drawing.Point(209, 3);
			this.gbEnollmentWGen.Name = "gbEnollmentWGen";
			this.gbEnollmentWGen.Size = new System.Drawing.Size(200, 142);
			this.gbEnollmentWGen.TabIndex = 2;
			this.gbEnollmentWGen.TabStop = false;
			this.gbEnollmentWGen.Text = "Enrollment with Generalization";
			// 
			// nudImageCount
			// 
			this.nudImageCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudImageCount.Location = new System.Drawing.Point(133, 45);
			this.nudImageCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.nudImageCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.nudImageCount.Name = "nudImageCount";
			this.nudImageCount.Size = new System.Drawing.Size(61, 20);
			this.nudImageCount.TabIndex = 5;
			this.nudImageCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.nudImageCount.ValueChanged += new System.EventHandler(this.nudImageCount_ValueChanged);
			// 
			// nudGeneralizationTreshold
			// 
			this.nudGeneralizationTreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudGeneralizationTreshold.DecimalPlaces = 3;
			this.nudGeneralizationTreshold.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
			this.nudGeneralizationTreshold.Location = new System.Drawing.Point(133, 19);
			this.nudGeneralizationTreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudGeneralizationTreshold.Name = "nudGeneralizationTreshold";
			this.nudGeneralizationTreshold.Size = new System.Drawing.Size(61, 20);
			this.nudGeneralizationTreshold.TabIndex = 4;
			this.nudGeneralizationTreshold.ValueChanged += new System.EventHandler(this.nudGeneralizationTreshold_ValueChanged);
			// 
			// lblImageCount
			// 
			this.lblImageCount.AutoSize = true;
			this.lblImageCount.Location = new System.Drawing.Point(6, 47);
			this.lblImageCount.Name = "lblImageCount";
			this.lblImageCount.Size = new System.Drawing.Size(70, 13);
			this.lblImageCount.TabIndex = 0;
			this.lblImageCount.Text = "Image Count:";
			// 
			// lblGeneralizationTreshold
			// 
			this.lblGeneralizationTreshold.AutoSize = true;
			this.lblGeneralizationTreshold.Location = new System.Drawing.Point(6, 21);
			this.lblGeneralizationTreshold.Name = "lblGeneralizationTreshold";
			this.lblGeneralizationTreshold.Size = new System.Drawing.Size(121, 13);
			this.lblGeneralizationTreshold.TabIndex = 0;
			this.lblGeneralizationTreshold.Text = "Generalization Treshold:";
			// 
			// gbMatching
			// 
			this.gbMatching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbMatching.Controls.Add(this.nudMaxMatchingResults);
			this.gbMatching.Controls.Add(this.nudMatchingAttempts);
			this.gbMatching.Controls.Add(this.nudMatchingTreshold);
			this.gbMatching.Controls.Add(this.lblMaxMatchingResults);
			this.gbMatching.Controls.Add(this.lblMatchingAttempts);
			this.gbMatching.Controls.Add(this.lblMatchingTreshold);
			this.gbMatching.Location = new System.Drawing.Point(415, 3);
			this.gbMatching.Name = "gbMatching";
			this.gbMatching.Size = new System.Drawing.Size(200, 142);
			this.gbMatching.TabIndex = 3;
			this.gbMatching.TabStop = false;
			this.gbMatching.Text = "Matching";
			// 
			// nudMaxMatchingResults
			// 
			this.nudMaxMatchingResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudMaxMatchingResults.Location = new System.Drawing.Point(145, 71);
			this.nudMaxMatchingResults.Name = "nudMaxMatchingResults";
			this.nudMaxMatchingResults.Size = new System.Drawing.Size(49, 20);
			this.nudMaxMatchingResults.TabIndex = 8;
			this.nudMaxMatchingResults.ValueChanged += new System.EventHandler(this.nudMaxMatchingResults_ValueChanged);
			// 
			// nudMatchingAttempts
			// 
			this.nudMatchingAttempts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudMatchingAttempts.Location = new System.Drawing.Point(145, 45);
			this.nudMatchingAttempts.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.nudMatchingAttempts.Name = "nudMatchingAttempts";
			this.nudMatchingAttempts.Size = new System.Drawing.Size(49, 20);
			this.nudMatchingAttempts.TabIndex = 7;
			this.nudMatchingAttempts.ValueChanged += new System.EventHandler(this.nudMatchingAttempts_ValueChanged);
			// 
			// nudMatchingTreshold
			// 
			this.nudMatchingTreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nudMatchingTreshold.DecimalPlaces = 3;
			this.nudMatchingTreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.nudMatchingTreshold.Location = new System.Drawing.Point(145, 19);
			this.nudMatchingTreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudMatchingTreshold.Name = "nudMatchingTreshold";
			this.nudMatchingTreshold.Size = new System.Drawing.Size(49, 20);
			this.nudMatchingTreshold.TabIndex = 6;
			this.nudMatchingTreshold.ValueChanged += new System.EventHandler(this.nudMatchingTreshold_ValueChanged);
			// 
			// lblMaxMatchingResults
			// 
			this.lblMaxMatchingResults.AutoSize = true;
			this.lblMaxMatchingResults.Location = new System.Drawing.Point(6, 73);
			this.lblMaxMatchingResults.Name = "lblMaxMatchingResults";
			this.lblMaxMatchingResults.Size = new System.Drawing.Size(133, 13);
			this.lblMaxMatchingResults.TabIndex = 0;
			this.lblMaxMatchingResults.Text = "Maximum matching results:";
			// 
			// lblMatchingAttempts
			// 
			this.lblMatchingAttempts.AutoSize = true;
			this.lblMatchingAttempts.Location = new System.Drawing.Point(6, 47);
			this.lblMatchingAttempts.Name = "lblMatchingAttempts";
			this.lblMatchingAttempts.Size = new System.Drawing.Size(98, 13);
			this.lblMatchingAttempts.TabIndex = 0;
			this.lblMatchingAttempts.Text = "Matching Attempts:";
			// 
			// lblMatchingTreshold
			// 
			this.lblMatchingTreshold.AutoSize = true;
			this.lblMatchingTreshold.Location = new System.Drawing.Point(6, 21);
			this.lblMatchingTreshold.Name = "lblMatchingTreshold";
			this.lblMatchingTreshold.Size = new System.Drawing.Size(94, 13);
			this.lblMatchingTreshold.TabIndex = 0;
			this.lblMatchingTreshold.Text = "Matching treshold:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 126);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(166, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "* IOD means Inter ocular distance";
			// 
			// CtrlSettingsPannel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbMatching);
			this.Controls.Add(this.gbEnollmentWGen);
			this.Controls.Add(this.gbFeaturesExtraction);
			this.MinimumSize = new System.Drawing.Size(618, 148);
			this.Name = "CtrlSettingsPannel";
			this.Size = new System.Drawing.Size(618, 148);
			this.gbFeaturesExtraction.ResumeLayout(false);
			this.gbFeaturesExtraction.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxIOD)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinIOD)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAttemptsWhileMatching)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAttemptsWhileEnrolling)).EndInit();
			this.gbEnollmentWGen.ResumeLayout(false);
			this.gbEnollmentWGen.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudImageCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudGeneralizationTreshold)).EndInit();
			this.gbMatching.ResumeLayout(false);
			this.gbMatching.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxMatchingResults)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMatchingAttempts)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMatchingTreshold)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbFeaturesExtraction;
		private System.Windows.Forms.Label lblMaxIOD;
		private System.Windows.Forms.Label lblMinIOD;
		private System.Windows.Forms.Label lblAttemptsWhileMatching;
		private System.Windows.Forms.Label lblAttemptsWhileEnrolling;
		private System.Windows.Forms.NumericUpDown nudMaxIOD;
		private System.Windows.Forms.NumericUpDown nudMinIOD;
		private System.Windows.Forms.NumericUpDown nudAttemptsWhileMatching;
		private System.Windows.Forms.NumericUpDown nudAttemptsWhileEnrolling;
		private System.Windows.Forms.GroupBox gbEnollmentWGen;
		private System.Windows.Forms.NumericUpDown nudImageCount;
		private System.Windows.Forms.NumericUpDown nudGeneralizationTreshold;
		private System.Windows.Forms.Label lblImageCount;
		private System.Windows.Forms.Label lblGeneralizationTreshold;
		private System.Windows.Forms.GroupBox gbMatching;
		private System.Windows.Forms.NumericUpDown nudMaxMatchingResults;
		private System.Windows.Forms.NumericUpDown nudMatchingAttempts;
		private System.Windows.Forms.NumericUpDown nudMatchingTreshold;
		private System.Windows.Forms.Label lblMaxMatchingResults;
		private System.Windows.Forms.Label lblMatchingAttempts;
		private System.Windows.Forms.Label lblMatchingTreshold;
		private System.Windows.Forms.Label label1;
	}
}
