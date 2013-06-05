namespace RecoHuman
{
	partial class FrmRecoHuman
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private delegate void BoolEH(bool b);

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new BoolEH(this.Dispose), disposing);
				return;
			}
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
			this.gbInput = new System.Windows.Forms.GroupBox();
			this.chkDrawInput = new System.Windows.Forms.CheckBox();
			this.vcCapturedImage = new RecoHuman.VideoControl();
			this.gbOutput = new System.Windows.Forms.GroupBox();
			this.vcRecognitionResult = new RecoHuman.VideoControl();
			this.btnExport = new System.Windows.Forms.FlowLayoutPanel();
			this.tcControls = new System.Windows.Forms.TabControl();
			this.tpMain = new System.Windows.Forms.TabPage();
			this.gbControls = new System.Windows.Forms.GroupBox();
			this.chkAutoFind = new System.Windows.Forms.CheckBox();
			this.txtHumanName = new System.Windows.Forms.TextBox();
			this.btnRememberHuman = new System.Windows.Forms.Button();
			this.btnFindHuman = new System.Windows.Forms.Button();
			this.lblHumanName = new System.Windows.Forms.Label();
			this.gbLog = new System.Windows.Forms.GroupBox();
			this.txtConsole = new System.Windows.Forms.TextBox();
			this.tpInputSource = new System.Windows.Forms.TabPage();
			this.gbImageSource = new System.Windows.Forms.GroupBox();
			this.rbImgSrcFile = new System.Windows.Forms.RadioButton();
			this.lblImgSrcFileName = new System.Windows.Forms.Label();
			this.lblImgSrcServerPort = new System.Windows.Forms.Label();
			this.lblImgSrcServerAddress = new System.Windows.Forms.Label();
			this.txtImgSrcFileName = new System.Windows.Forms.TextBox();
			this.txtImgSrcServerPort = new System.Windows.Forms.TextBox();
			this.txtImgSrcServerAddress = new System.Windows.Forms.TextBox();
			this.rbImgSrcSocket = new System.Windows.Forms.RadioButton();
			this.rbImgSrcCamera = new System.Windows.Forms.RadioButton();
			this.ctrlCameraSettings = new RecoHuman.CtrlCamSettings();
			this.gbDetectedCameras = new System.Windows.Forms.GroupBox();
			this.lstDetectedCameras = new System.Windows.Forms.ListBox();
			this.btnFindCameras = new System.Windows.Forms.Button();
			this.tpSettings = new System.Windows.Forms.TabPage();
			this.settingsPannel = new RecoHuman.CtrlSettingsPannel();
			this.tpKnownFaces = new System.Windows.Forms.TabPage();
			this.tscKnownFaces = new System.Windows.Forms.ToolStripContainer();
			this.flpKnownFaces = new System.Windows.Forms.FlowLayoutPanel();
			this.tsKnownFacesTools = new System.Windows.Forms.ToolStrip();
			this.tsbLoadKnownFaces = new System.Windows.Forms.ToolStripButton();
			this.tsbImportKnownFaces = new System.Windows.Forms.ToolStripButton();
			this.tsbExportKnownFaces = new System.Windows.Forms.ToolStripButton();
			this.tsbClearKnownFaces = new System.Windows.Forms.ToolStripButton();
			this.gbRecognitionResult = new System.Windows.Forms.GroupBox();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.lblMatcherStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblExtractorStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblServerStarted = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCurrentInputPort = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblClientConnected = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCurrentAddres = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCurrentOutputPort = new System.Windows.Forms.ToolStripStatusLabel();
			this.dlgExportKnownFaces = new System.Windows.Forms.SaveFileDialog();
			this.dlgLoadKnownFaces = new System.Windows.Forms.OpenFileDialog();
			this.dlgImportKnownFaces = new System.Windows.Forms.OpenFileDialog();
			this.gbInput.SuspendLayout();
			this.gbOutput.SuspendLayout();
			this.tcControls.SuspendLayout();
			this.tpMain.SuspendLayout();
			this.gbControls.SuspendLayout();
			this.gbLog.SuspendLayout();
			this.tpInputSource.SuspendLayout();
			this.gbImageSource.SuspendLayout();
			this.gbDetectedCameras.SuspendLayout();
			this.tpSettings.SuspendLayout();
			this.tpKnownFaces.SuspendLayout();
			this.tscKnownFaces.ContentPanel.SuspendLayout();
			this.tscKnownFaces.RightToolStripPanel.SuspendLayout();
			this.tscKnownFaces.SuspendLayout();
			this.tsKnownFacesTools.SuspendLayout();
			this.gbRecognitionResult.SuspendLayout();
			this.statusBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbInput
			// 
			this.gbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbInput.Controls.Add(this.chkDrawInput);
			this.gbInput.Controls.Add(this.vcCapturedImage);
			this.gbInput.Location = new System.Drawing.Point(12, 12);
			this.gbInput.MaximumSize = new System.Drawing.Size(320, 240);
			this.gbInput.MinimumSize = new System.Drawing.Size(320, 240);
			this.gbInput.Name = "gbInput";
			this.gbInput.Size = new System.Drawing.Size(320, 240);
			this.gbInput.TabIndex = 0;
			this.gbInput.TabStop = false;
			this.gbInput.Text = "Input";
			// 
			// chkDrawInput
			// 
			this.chkDrawInput.AutoSize = true;
			this.chkDrawInput.Location = new System.Drawing.Point(234, 0);
			this.chkDrawInput.Name = "chkDrawInput";
			this.chkDrawInput.Size = new System.Drawing.Size(78, 17);
			this.chkDrawInput.TabIndex = 12;
			this.chkDrawInput.Text = "Draw Input";
			this.chkDrawInput.UseVisualStyleBackColor = true;
			// 
			// vcCapturedImage
			// 
			this.vcCapturedImage.DetectionDetails = null;
			this.vcCapturedImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vcCapturedImage.Faces = null;
			this.vcCapturedImage.Image = null;
			this.vcCapturedImage.Location = new System.Drawing.Point(3, 16);
			this.vcCapturedImage.Name = "vcCapturedImage";
			this.vcCapturedImage.Size = new System.Drawing.Size(314, 221);
			this.vcCapturedImage.String = null;
			this.vcCapturedImage.TabIndex = 0;
			// 
			// gbOutput
			// 
			this.gbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbOutput.Controls.Add(this.vcRecognitionResult);
			this.gbOutput.Location = new System.Drawing.Point(338, 12);
			this.gbOutput.MaximumSize = new System.Drawing.Size(320, 240);
			this.gbOutput.MinimumSize = new System.Drawing.Size(320, 240);
			this.gbOutput.Name = "gbOutput";
			this.gbOutput.Size = new System.Drawing.Size(320, 240);
			this.gbOutput.TabIndex = 0;
			this.gbOutput.TabStop = false;
			this.gbOutput.Text = "Output";
			// 
			// vcRecognitionResult
			// 
			this.vcRecognitionResult.DetectionDetails = null;
			this.vcRecognitionResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vcRecognitionResult.Faces = null;
			this.vcRecognitionResult.Image = null;
			this.vcRecognitionResult.Location = new System.Drawing.Point(3, 16);
			this.vcRecognitionResult.Name = "vcRecognitionResult";
			this.vcRecognitionResult.Size = new System.Drawing.Size(314, 221);
			this.vcRecognitionResult.String = null;
			this.vcRecognitionResult.TabIndex = 1;
			// 
			// btnExport
			// 
			this.btnExport.AutoScroll = true;
			this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnExport.Location = new System.Drawing.Point(3, 16);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(308, 405);
			this.btnExport.TabIndex = 11;
			// 
			// tcControls
			// 
			this.tcControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tcControls.Controls.Add(this.tpMain);
			this.tcControls.Controls.Add(this.tpInputSource);
			this.tcControls.Controls.Add(this.tpSettings);
			this.tcControls.Controls.Add(this.tpKnownFaces);
			this.tcControls.Location = new System.Drawing.Point(0, 258);
			this.tcControls.Name = "tcControls";
			this.tcControls.SelectedIndex = 0;
			this.tcControls.Size = new System.Drawing.Size(658, 186);
			this.tcControls.TabIndex = 10;
			// 
			// tpMain
			// 
			this.tpMain.Controls.Add(this.gbControls);
			this.tpMain.Controls.Add(this.gbLog);
			this.tpMain.Location = new System.Drawing.Point(4, 22);
			this.tpMain.Name = "tpMain";
			this.tpMain.Padding = new System.Windows.Forms.Padding(3);
			this.tpMain.Size = new System.Drawing.Size(650, 160);
			this.tpMain.TabIndex = 0;
			this.tpMain.Text = "Main";
			this.tpMain.UseVisualStyleBackColor = true;
			// 
			// gbControls
			// 
			this.gbControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbControls.Controls.Add(this.chkAutoFind);
			this.gbControls.Controls.Add(this.txtHumanName);
			this.gbControls.Controls.Add(this.btnRememberHuman);
			this.gbControls.Controls.Add(this.btnFindHuman);
			this.gbControls.Controls.Add(this.lblHumanName);
			this.gbControls.Location = new System.Drawing.Point(444, 6);
			this.gbControls.Name = "gbControls";
			this.gbControls.Size = new System.Drawing.Size(200, 148);
			this.gbControls.TabIndex = 0;
			this.gbControls.TabStop = false;
			this.gbControls.Text = "Controls";
			// 
			// chkAutoFind
			// 
			this.chkAutoFind.AutoSize = true;
			this.chkAutoFind.Location = new System.Drawing.Point(6, 19);
			this.chkAutoFind.Name = "chkAutoFind";
			this.chkAutoFind.Size = new System.Drawing.Size(102, 17);
			this.chkAutoFind.TabIndex = 4;
			this.chkAutoFind.Text = "Autofind Human";
			this.chkAutoFind.UseVisualStyleBackColor = true;
			this.chkAutoFind.CheckedChanged += new System.EventHandler(this.chkAutoFind_CheckedChanged);
			// 
			// txtHumanName
			// 
			this.txtHumanName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHumanName.Location = new System.Drawing.Point(6, 64);
			this.txtHumanName.Name = "txtHumanName";
			this.txtHumanName.Size = new System.Drawing.Size(188, 20);
			this.txtHumanName.TabIndex = 3;
			// 
			// btnRememberHuman
			// 
			this.btnRememberHuman.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnRememberHuman.Location = new System.Drawing.Point(6, 119);
			this.btnRememberHuman.Name = "btnRememberHuman";
			this.btnRememberHuman.Size = new System.Drawing.Size(188, 23);
			this.btnRememberHuman.TabIndex = 2;
			this.btnRememberHuman.Text = "Remember Human";
			this.btnRememberHuman.UseVisualStyleBackColor = true;
			this.btnRememberHuman.Click += new System.EventHandler(this.btnRememberHuman_Click);
			// 
			// btnFindHuman
			// 
			this.btnFindHuman.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindHuman.Location = new System.Drawing.Point(6, 90);
			this.btnFindHuman.Name = "btnFindHuman";
			this.btnFindHuman.Size = new System.Drawing.Size(188, 23);
			this.btnFindHuman.TabIndex = 1;
			this.btnFindHuman.Text = "Find Human";
			this.btnFindHuman.UseVisualStyleBackColor = true;
			this.btnFindHuman.Click += new System.EventHandler(this.btnFindHuman_Click);
			// 
			// lblHumanName
			// 
			this.lblHumanName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblHumanName.AutoSize = true;
			this.lblHumanName.Location = new System.Drawing.Point(6, 48);
			this.lblHumanName.Name = "lblHumanName";
			this.lblHumanName.Size = new System.Drawing.Size(75, 13);
			this.lblHumanName.TabIndex = 0;
			this.lblHumanName.Text = "Human Name:";
			// 
			// gbLog
			// 
			this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbLog.Controls.Add(this.txtConsole);
			this.gbLog.Location = new System.Drawing.Point(8, 6);
			this.gbLog.Name = "gbLog";
			this.gbLog.Size = new System.Drawing.Size(430, 148);
			this.gbLog.TabIndex = 0;
			this.gbLog.TabStop = false;
			this.gbLog.Text = "Log";
			// 
			// txtConsole
			// 
			this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtConsole.Location = new System.Drawing.Point(3, 16);
			this.txtConsole.Multiline = true;
			this.txtConsole.Name = "txtConsole";
			this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtConsole.Size = new System.Drawing.Size(424, 129);
			this.txtConsole.TabIndex = 0;
			this.txtConsole.TextChanged += new System.EventHandler(this.txtConsole_TextChanged);
			// 
			// tpInputSource
			// 
			this.tpInputSource.Controls.Add(this.gbImageSource);
			this.tpInputSource.Controls.Add(this.ctrlCameraSettings);
			this.tpInputSource.Controls.Add(this.gbDetectedCameras);
			this.tpInputSource.Location = new System.Drawing.Point(4, 22);
			this.tpInputSource.Name = "tpInputSource";
			this.tpInputSource.Padding = new System.Windows.Forms.Padding(3);
			this.tpInputSource.Size = new System.Drawing.Size(650, 160);
			this.tpInputSource.TabIndex = 1;
			this.tpInputSource.Text = "Input Source";
			this.tpInputSource.UseVisualStyleBackColor = true;
			// 
			// gbImageSource
			// 
			this.gbImageSource.Controls.Add(this.rbImgSrcFile);
			this.gbImageSource.Controls.Add(this.lblImgSrcFileName);
			this.gbImageSource.Controls.Add(this.lblImgSrcServerPort);
			this.gbImageSource.Controls.Add(this.lblImgSrcServerAddress);
			this.gbImageSource.Controls.Add(this.txtImgSrcFileName);
			this.gbImageSource.Controls.Add(this.txtImgSrcServerPort);
			this.gbImageSource.Controls.Add(this.txtImgSrcServerAddress);
			this.gbImageSource.Controls.Add(this.rbImgSrcSocket);
			this.gbImageSource.Controls.Add(this.rbImgSrcCamera);
			this.gbImageSource.Location = new System.Drawing.Point(8, 6);
			this.gbImageSource.Name = "gbImageSource";
			this.gbImageSource.Size = new System.Drawing.Size(200, 148);
			this.gbImageSource.TabIndex = 2;
			this.gbImageSource.TabStop = false;
			this.gbImageSource.Text = "Image Source";
			// 
			// rbImgSrcFile
			// 
			this.rbImgSrcFile.AutoSize = true;
			this.rbImgSrcFile.Location = new System.Drawing.Point(6, 65);
			this.rbImgSrcFile.Name = "rbImgSrcFile";
			this.rbImgSrcFile.Size = new System.Drawing.Size(41, 17);
			this.rbImgSrcFile.TabIndex = 0;
			this.rbImgSrcFile.TabStop = true;
			this.rbImgSrcFile.Text = "File";
			this.rbImgSrcFile.UseVisualStyleBackColor = true;
			this.rbImgSrcFile.CheckedChanged += new System.EventHandler(this.rbImgSrcFile_CheckedChanged);
			// 
			// lblImgSrcFileName
			// 
			this.lblImgSrcFileName.AutoSize = true;
			this.lblImgSrcFileName.Location = new System.Drawing.Point(22, 91);
			this.lblImgSrcFileName.Name = "lblImgSrcFileName";
			this.lblImgSrcFileName.Size = new System.Drawing.Size(57, 13);
			this.lblImgSrcFileName.TabIndex = 3;
			this.lblImgSrcFileName.Text = "File Name:";
			// 
			// lblImgSrcServerPort
			// 
			this.lblImgSrcServerPort.AutoSize = true;
			this.lblImgSrcServerPort.Location = new System.Drawing.Point(22, 94);
			this.lblImgSrcServerPort.Name = "lblImgSrcServerPort";
			this.lblImgSrcServerPort.Size = new System.Drawing.Size(80, 13);
			this.lblImgSrcServerPort.TabIndex = 3;
			this.lblImgSrcServerPort.Text = "Conection Port:";
			// 
			// lblImgSrcServerAddress
			// 
			this.lblImgSrcServerAddress.AutoSize = true;
			this.lblImgSrcServerAddress.Location = new System.Drawing.Point(22, 68);
			this.lblImgSrcServerAddress.Name = "lblImgSrcServerAddress";
			this.lblImgSrcServerAddress.Size = new System.Drawing.Size(82, 13);
			this.lblImgSrcServerAddress.TabIndex = 3;
			this.lblImgSrcServerAddress.Text = "Server Address:";
			// 
			// txtImgSrcFileName
			// 
			this.txtImgSrcFileName.Location = new System.Drawing.Point(85, 88);
			this.txtImgSrcFileName.Name = "txtImgSrcFileName";
			this.txtImgSrcFileName.Size = new System.Drawing.Size(109, 20);
			this.txtImgSrcFileName.TabIndex = 2;
			// 
			// txtImgSrcServerPort
			// 
			this.txtImgSrcServerPort.Location = new System.Drawing.Point(110, 91);
			this.txtImgSrcServerPort.Name = "txtImgSrcServerPort";
			this.txtImgSrcServerPort.Size = new System.Drawing.Size(84, 20);
			this.txtImgSrcServerPort.TabIndex = 2;
			// 
			// txtImgSrcServerAddress
			// 
			this.txtImgSrcServerAddress.Location = new System.Drawing.Point(110, 65);
			this.txtImgSrcServerAddress.Name = "txtImgSrcServerAddress";
			this.txtImgSrcServerAddress.Size = new System.Drawing.Size(84, 20);
			this.txtImgSrcServerAddress.TabIndex = 1;
			// 
			// rbImgSrcSocket
			// 
			this.rbImgSrcSocket.AutoSize = true;
			this.rbImgSrcSocket.Location = new System.Drawing.Point(6, 42);
			this.rbImgSrcSocket.Name = "rbImgSrcSocket";
			this.rbImgSrcSocket.Size = new System.Drawing.Size(131, 17);
			this.rbImgSrcSocket.TabIndex = 0;
			this.rbImgSrcSocket.TabStop = true;
			this.rbImgSrcSocket.Text = "Image Server (Socket)";
			this.rbImgSrcSocket.UseVisualStyleBackColor = true;
			this.rbImgSrcSocket.CheckedChanged += new System.EventHandler(this.rbImgSrcSocket_CheckedChanged);
			// 
			// rbImgSrcCamera
			// 
			this.rbImgSrcCamera.AutoSize = true;
			this.rbImgSrcCamera.Location = new System.Drawing.Point(6, 19);
			this.rbImgSrcCamera.Name = "rbImgSrcCamera";
			this.rbImgSrcCamera.Size = new System.Drawing.Size(61, 17);
			this.rbImgSrcCamera.TabIndex = 0;
			this.rbImgSrcCamera.TabStop = true;
			this.rbImgSrcCamera.Text = "Camera";
			this.rbImgSrcCamera.UseVisualStyleBackColor = true;
			this.rbImgSrcCamera.CheckedChanged += new System.EventHandler(this.rbImgSrcCamera_CheckedChanged);
			// 
			// ctrlCameraSettings
			// 
			this.ctrlCameraSettings.Camera = null;
			this.ctrlCameraSettings.Location = new System.Drawing.Point(430, 6);
			this.ctrlCameraSettings.MinimumSize = new System.Drawing.Size(210, 148);
			this.ctrlCameraSettings.Name = "ctrlCameraSettings";
			this.ctrlCameraSettings.Size = new System.Drawing.Size(210, 148);
			this.ctrlCameraSettings.TabIndex = 1;
			// 
			// gbDetectedCameras
			// 
			this.gbDetectedCameras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.gbDetectedCameras.Controls.Add(this.lstDetectedCameras);
			this.gbDetectedCameras.Controls.Add(this.btnFindCameras);
			this.gbDetectedCameras.Location = new System.Drawing.Point(214, 6);
			this.gbDetectedCameras.MinimumSize = new System.Drawing.Size(200, 148);
			this.gbDetectedCameras.Name = "gbDetectedCameras";
			this.gbDetectedCameras.Size = new System.Drawing.Size(200, 148);
			this.gbDetectedCameras.TabIndex = 0;
			this.gbDetectedCameras.TabStop = false;
			this.gbDetectedCameras.Text = "Detected Cameras";
			// 
			// lstDetectedCameras
			// 
			this.lstDetectedCameras.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstDetectedCameras.FormattingEnabled = true;
			this.lstDetectedCameras.Location = new System.Drawing.Point(6, 19);
			this.lstDetectedCameras.Name = "lstDetectedCameras";
			this.lstDetectedCameras.Size = new System.Drawing.Size(188, 95);
			this.lstDetectedCameras.TabIndex = 1;
			this.lstDetectedCameras.SelectedIndexChanged += new System.EventHandler(this.lstDetectedCameras_SelectedIndexChanged);
			// 
			// btnFindCameras
			// 
			this.btnFindCameras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindCameras.Location = new System.Drawing.Point(6, 119);
			this.btnFindCameras.Name = "btnFindCameras";
			this.btnFindCameras.Size = new System.Drawing.Size(188, 23);
			this.btnFindCameras.TabIndex = 1;
			this.btnFindCameras.Text = "Find Cameras";
			this.btnFindCameras.UseVisualStyleBackColor = true;
			this.btnFindCameras.Click += new System.EventHandler(this.btnFindCameras_Click);
			// 
			// tpSettings
			// 
			this.tpSettings.Controls.Add(this.settingsPannel);
			this.tpSettings.Location = new System.Drawing.Point(4, 22);
			this.tpSettings.Name = "tpSettings";
			this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tpSettings.Size = new System.Drawing.Size(650, 160);
			this.tpSettings.TabIndex = 2;
			this.tpSettings.Text = "Settings";
			this.tpSettings.UseVisualStyleBackColor = true;
			// 
			// settingsPannel
			// 
			this.settingsPannel.Location = new System.Drawing.Point(6, 9);
			this.settingsPannel.MinimumSize = new System.Drawing.Size(618, 148);
			this.settingsPannel.Name = "settingsPannel";
			this.settingsPannel.Settings = null;
			this.settingsPannel.Size = new System.Drawing.Size(638, 148);
			this.settingsPannel.TabIndex = 0;
			// 
			// tpKnownFaces
			// 
			this.tpKnownFaces.Controls.Add(this.tscKnownFaces);
			this.tpKnownFaces.Location = new System.Drawing.Point(4, 22);
			this.tpKnownFaces.Name = "tpKnownFaces";
			this.tpKnownFaces.Padding = new System.Windows.Forms.Padding(3);
			this.tpKnownFaces.Size = new System.Drawing.Size(650, 160);
			this.tpKnownFaces.TabIndex = 3;
			this.tpKnownFaces.Text = "Known Faces";
			this.tpKnownFaces.UseVisualStyleBackColor = true;
			// 
			// tscKnownFaces
			// 
			// 
			// tscKnownFaces.ContentPanel
			// 
			this.tscKnownFaces.ContentPanel.Controls.Add(this.flpKnownFaces);
			this.tscKnownFaces.ContentPanel.Size = new System.Drawing.Size(584, 129);
			this.tscKnownFaces.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tscKnownFaces.Location = new System.Drawing.Point(3, 3);
			this.tscKnownFaces.Name = "tscKnownFaces";
			// 
			// tscKnownFaces.RightToolStripPanel
			// 
			this.tscKnownFaces.RightToolStripPanel.Controls.Add(this.tsKnownFacesTools);
			this.tscKnownFaces.Size = new System.Drawing.Size(644, 154);
			this.tscKnownFaces.TabIndex = 1;
			this.tscKnownFaces.Text = "KnownFaces";
			// 
			// flpKnownFaces
			// 
			this.flpKnownFaces.AutoScroll = true;
			this.flpKnownFaces.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpKnownFaces.Location = new System.Drawing.Point(0, 0);
			this.flpKnownFaces.Margin = new System.Windows.Forms.Padding(0);
			this.flpKnownFaces.Name = "flpKnownFaces";
			this.flpKnownFaces.Size = new System.Drawing.Size(584, 129);
			this.flpKnownFaces.TabIndex = 0;
			// 
			// tsKnownFacesTools
			// 
			this.tsKnownFacesTools.Dock = System.Windows.Forms.DockStyle.None;
			this.tsKnownFacesTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsKnownFacesTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoadKnownFaces,
            this.tsbImportKnownFaces,
            this.tsbExportKnownFaces,
            this.tsbClearKnownFaces});
			this.tsKnownFacesTools.Location = new System.Drawing.Point(0, 3);
			this.tsKnownFacesTools.Name = "tsKnownFacesTools";
			this.tsKnownFacesTools.Size = new System.Drawing.Size(60, 94);
			this.tsKnownFacesTools.TabIndex = 0;
			// 
			// tsbLoadKnownFaces
			// 
			this.tsbLoadKnownFaces.Image = global::RecoHuman.Properties.Resources.openHS;
			this.tsbLoadKnownFaces.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbLoadKnownFaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbLoadKnownFaces.Name = "tsbLoadKnownFaces";
			this.tsbLoadKnownFaces.Size = new System.Drawing.Size(58, 20);
			this.tsbLoadKnownFaces.Text = "Load";
			this.tsbLoadKnownFaces.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbLoadKnownFaces.Click += new System.EventHandler(this.tsbLoadKnownFaces_Click);
			// 
			// tsbImportKnownFaces
			// 
			this.tsbImportKnownFaces.Image = global::RecoHuman.Properties.Resources.MoveToFolderHS;
			this.tsbImportKnownFaces.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbImportKnownFaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbImportKnownFaces.Name = "tsbImportKnownFaces";
			this.tsbImportKnownFaces.Size = new System.Drawing.Size(58, 20);
			this.tsbImportKnownFaces.Text = "Import";
			this.tsbImportKnownFaces.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbImportKnownFaces.Click += new System.EventHandler(this.tsbImportKnownFaces_Click);
			// 
			// tsbExportKnownFaces
			// 
			this.tsbExportKnownFaces.Image = global::RecoHuman.Properties.Resources.SaveAllHS;
			this.tsbExportKnownFaces.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbExportKnownFaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbExportKnownFaces.Name = "tsbExportKnownFaces";
			this.tsbExportKnownFaces.Size = new System.Drawing.Size(58, 20);
			this.tsbExportKnownFaces.Text = "Export";
			this.tsbExportKnownFaces.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbExportKnownFaces.Click += new System.EventHandler(this.tsbExportKnownFaces_Click);
			// 
			// tsbClearKnownFaces
			// 
			this.tsbClearKnownFaces.Image = global::RecoHuman.Properties.Resources.DeleteFolderHS;
			this.tsbClearKnownFaces.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbClearKnownFaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbClearKnownFaces.Name = "tsbClearKnownFaces";
			this.tsbClearKnownFaces.Size = new System.Drawing.Size(58, 20);
			this.tsbClearKnownFaces.Text = "Clear";
			this.tsbClearKnownFaces.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbClearKnownFaces.Click += new System.EventHandler(this.tsbClearKnownFaces_Click);
			// 
			// gbRecognitionResult
			// 
			this.gbRecognitionResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbRecognitionResult.Controls.Add(this.btnExport);
			this.gbRecognitionResult.Location = new System.Drawing.Point(665, 13);
			this.gbRecognitionResult.Name = "gbRecognitionResult";
			this.gbRecognitionResult.Size = new System.Drawing.Size(314, 424);
			this.gbRecognitionResult.TabIndex = 11;
			this.gbRecognitionResult.TabStop = false;
			this.gbRecognitionResult.Text = "Last Recognition Results";
			// 
			// statusBar
			// 
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMatcherStatus,
            this.lblExtractorStatus,
            this.lblServerStarted,
            this.lblCurrentInputPort,
            this.lblClientConnected,
            this.lblCurrentAddres,
            this.lblCurrentOutputPort});
			this.statusBar.Location = new System.Drawing.Point(0, 444);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(983, 22);
			this.statusBar.TabIndex = 10;
			this.statusBar.Text = "statusStrip1";
			// 
			// lblMatcherStatus
			// 
			this.lblMatcherStatus.Image = global::RecoHuman.Properties.Resources.ok16;
			this.lblMatcherStatus.Name = "lblMatcherStatus";
			this.lblMatcherStatus.Size = new System.Drawing.Size(167, 17);
			this.lblMatcherStatus.Text = "Verilook Matcher is Registered";
			// 
			// lblExtractorStatus
			// 
			this.lblExtractorStatus.Image = global::RecoHuman.Properties.Resources.ok16;
			this.lblExtractorStatus.Name = "lblExtractorStatus";
			this.lblExtractorStatus.Size = new System.Drawing.Size(173, 17);
			this.lblExtractorStatus.Text = "Verilook Extractor is Registered";
			// 
			// lblServerStarted
			// 
			this.lblServerStarted.Name = "lblServerStarted";
			this.lblServerStarted.Size = new System.Drawing.Size(78, 17);
			this.lblServerStarted.Text = "Server Started";
			this.lblServerStarted.Visible = false;
			// 
			// lblCurrentInputPort
			// 
			this.lblCurrentInputPort.Name = "lblCurrentInputPort";
			this.lblCurrentInputPort.Size = new System.Drawing.Size(93, 17);
			this.lblCurrentInputPort.Text = "Input Port: 65536";
			this.lblCurrentInputPort.Visible = false;
			// 
			// lblClientConnected
			// 
			this.lblClientConnected.Name = "lblClientConnected";
			this.lblClientConnected.Size = new System.Drawing.Size(94, 17);
			this.lblClientConnected.Text = "Client Conncected";
			this.lblClientConnected.Visible = false;
			// 
			// lblCurrentAddres
			// 
			this.lblCurrentAddres.Name = "lblCurrentAddres";
			this.lblCurrentAddres.Size = new System.Drawing.Size(91, 17);
			this.lblCurrentAddres.Text = "255.255.255.255";
			this.lblCurrentAddres.Visible = false;
			// 
			// lblCurrentOutputPort
			// 
			this.lblCurrentOutputPort.Name = "lblCurrentOutputPort";
			this.lblCurrentOutputPort.Size = new System.Drawing.Size(101, 17);
			this.lblCurrentOutputPort.Text = "Output Port: 65536";
			this.lblCurrentOutputPort.Visible = false;
			// 
			// dlgExportKnownFaces
			// 
			this.dlgExportKnownFaces.DefaultExt = "rh";
			this.dlgExportKnownFaces.FileName = "KnownFaces";
			this.dlgExportKnownFaces.Filter = "RecoHuman Known faces file|*.rh";
			this.dlgExportKnownFaces.Title = "Export Known Faces List";
			// 
			// dlgLoadKnownFaces
			// 
			this.dlgLoadKnownFaces.DefaultExt = "rh";
			this.dlgLoadKnownFaces.FileName = "KnownFaces";
			this.dlgLoadKnownFaces.Filter = "RecoHuman Known faces file|*.rh";
			this.dlgLoadKnownFaces.Title = "Load Known Faces";
			// 
			// dlgImportKnownFaces
			// 
			this.dlgImportKnownFaces.DefaultExt = "rh";
			this.dlgImportKnownFaces.FileName = "KnownFaces";
			this.dlgImportKnownFaces.Filter = "RecoHuman Known faces file|*.rh";
			this.dlgImportKnownFaces.Title = "Import Known Faces";
			// 
			// FrmRecoHuman
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(983, 466);
			this.Controls.Add(this.gbRecognitionResult);
			this.Controls.Add(this.tcControls);
			this.Controls.Add(this.gbOutput);
			this.Controls.Add(this.gbInput);
			this.Controls.Add(this.statusBar);
			this.Icon = global::RecoHuman.Properties.Resources.DigitalFace;
			this.MinimumSize = new System.Drawing.Size(800, 500);
			this.Name = "FrmRecoHuman";
			this.Text = "PRS-FND - RecoHuman";
			this.Load += new System.EventHandler(this.FrmRecoHuman_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRecoHuman_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRecoHuman_FormClosing);
			this.gbInput.ResumeLayout(false);
			this.gbInput.PerformLayout();
			this.gbOutput.ResumeLayout(false);
			this.tcControls.ResumeLayout(false);
			this.tpMain.ResumeLayout(false);
			this.gbControls.ResumeLayout(false);
			this.gbControls.PerformLayout();
			this.gbLog.ResumeLayout(false);
			this.gbLog.PerformLayout();
			this.tpInputSource.ResumeLayout(false);
			this.gbImageSource.ResumeLayout(false);
			this.gbImageSource.PerformLayout();
			this.gbDetectedCameras.ResumeLayout(false);
			this.tpSettings.ResumeLayout(false);
			this.tpKnownFaces.ResumeLayout(false);
			this.tscKnownFaces.ContentPanel.ResumeLayout(false);
			this.tscKnownFaces.RightToolStripPanel.ResumeLayout(false);
			this.tscKnownFaces.RightToolStripPanel.PerformLayout();
			this.tscKnownFaces.ResumeLayout(false);
			this.tscKnownFaces.PerformLayout();
			this.tsKnownFacesTools.ResumeLayout(false);
			this.tsKnownFacesTools.PerformLayout();
			this.gbRecognitionResult.ResumeLayout(false);
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox gbInput;
		private System.Windows.Forms.GroupBox gbOutput;
		private System.Windows.Forms.TabControl tcControls;
		private System.Windows.Forms.TabPage tpMain;
		private System.Windows.Forms.TextBox txtConsole;
		private System.Windows.Forms.TabPage tpInputSource;
		private System.Windows.Forms.GroupBox gbDetectedCameras;
		private System.Windows.Forms.ListBox lstDetectedCameras;
		private System.Windows.Forms.Button btnFindCameras;
		private CtrlCamSettings ctrlCameraSettings;
		private VideoControl vcCapturedImage;
		private VideoControl vcRecognitionResult;
		private System.Windows.Forms.TabPage tpSettings;
		private System.Windows.Forms.FlowLayoutPanel btnExport;
		private System.Windows.Forms.GroupBox gbRecognitionResult;
		private CtrlSettingsPannel settingsPannel;
		private System.Windows.Forms.GroupBox gbControls;
		private System.Windows.Forms.GroupBox gbLog;
		private System.Windows.Forms.CheckBox chkAutoFind;
		private System.Windows.Forms.TextBox txtHumanName;
		private System.Windows.Forms.Button btnRememberHuman;
		private System.Windows.Forms.Button btnFindHuman;
		private System.Windows.Forms.Label lblHumanName;
		private System.Windows.Forms.TabPage tpKnownFaces;
		private System.Windows.Forms.FlowLayoutPanel flpKnownFaces;

		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.ToolStripStatusLabel lblServerStarted;
		private System.Windows.Forms.ToolStripStatusLabel lblClientConnected;
		private System.Windows.Forms.ToolStripStatusLabel lblCurrentAddres;
		private System.Windows.Forms.ToolStripStatusLabel lblCurrentInputPort;
		private System.Windows.Forms.ToolStripStatusLabel lblCurrentOutputPort;
		private System.Windows.Forms.ToolStripStatusLabel lblMatcherStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblExtractorStatus;
		private System.Windows.Forms.CheckBox chkDrawInput;
		private System.Windows.Forms.ToolStripContainer tscKnownFaces;
		private System.Windows.Forms.ToolStrip tsKnownFacesTools;
		private System.Windows.Forms.ToolStripButton tsbLoadKnownFaces;
		private System.Windows.Forms.ToolStripButton tsbImportKnownFaces;
		private System.Windows.Forms.ToolStripButton tsbExportKnownFaces;
		private System.Windows.Forms.ToolStripButton tsbClearKnownFaces;
		private System.Windows.Forms.SaveFileDialog dlgExportKnownFaces;
		private System.Windows.Forms.OpenFileDialog dlgLoadKnownFaces;
		private System.Windows.Forms.OpenFileDialog dlgImportKnownFaces;
		private System.Windows.Forms.GroupBox gbImageSource;
		private System.Windows.Forms.RadioButton rbImgSrcFile;
		private System.Windows.Forms.RadioButton rbImgSrcSocket;
		private System.Windows.Forms.RadioButton rbImgSrcCamera;
		private System.Windows.Forms.Label lblImgSrcServerPort;
		private System.Windows.Forms.Label lblImgSrcServerAddress;
		private System.Windows.Forms.TextBox txtImgSrcServerPort;
		private System.Windows.Forms.TextBox txtImgSrcServerAddress;
		private System.Windows.Forms.Label lblImgSrcFileName;
		private System.Windows.Forms.TextBox txtImgSrcFileName;
	}
}