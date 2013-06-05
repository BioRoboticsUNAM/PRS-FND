//#define FILE
#define PIPES

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Text;

using Robotics;
using Robotics.API;
using Robotics.API.MiscSharedVariables;
using Robotics.HAL.Sensors;
using Robotics.Controls;

using Neurotec;
using Neurotec.Cameras;
using Neurotec.Biometrics;
using Neurotec.Images;

using RecoHuman.CommandExecuters;
using RecoHuman.Sources;

using Timer = System.Threading.Timer;

namespace RecoHuman
{
	/// <summary>
	/// Represents the method that will handle an event that receives an string as param 
	/// </summary>
	/// <param name="str">String to pass</param>
	public delegate void StringEventHandler(string str);

	/// <summary>
	/// Represents the method that will handle an event that receives no parameters
	/// </summary>
	public delegate void VoidEventHandler();

	public partial class FrmRecoHuman : Form
	{

		/// <summary>
		/// Stores the module name
		/// </summary>
		const string MODULE_NAME = "PRS-REC";

		#region Variables

		#region Delegate Vars

		/// <summary>
		/// Represents the UpdateControlCameras method. Used for async calls
		/// </summary>
		private VoidEventHandler dlgUpdateControlCameras;
		/// <summary>
		/// Represents the UpdateListCameras method. Used for async calls
		/// </summary>
		private VoidEventHandler dlgListCameras;
		/// <summary>
		/// Represents the VideoControls method. Used for async calls
		/// </summary>
		private VoidEventHandler dlgUpdateVideoControls;
		/// <summary>
		/// Represents the UpdateSettings method. Used for async calls
		/// </summary>
		private VoidEventHandler dlgUpdateSettings;
		/// <summary>
		/// Represents the RecognitionResultUpdate method. Used for async calls
		/// </summary>
		private VideoControlUpdateEH dlgRecognitionResultUpdate;
		/// <summary>
		/// Represents the RecognitionResultUpdate method. Used for async calls
		/// </summary>
		private VideoControlUpdate2EH dlgRecognitionResultUpdate2;
		/// <summary>
		/// Represents the RecognitionResultUpdate method. Used for async calls
		/// </summary>
		private VideoControlUpdate3EH dlgRecognitionResultUpdate3;
		/// <summary>
		/// Represents the RecognitionResultUpdate method. Used for async calls
		/// </summary>
		private VideoControlUpdate4EH dlgRecognitionResultUpdate4;
		/// <summary>
		/// Represents the ShowRecognitionResults method. Used for async calls
		/// </summary>
		private ShowRecognitionResultsEH dlgShowRecognitionResults;
		/// <summary>
		/// Represents the ShowDetectionResults method. Used for async calls
		/// </summary>
		private ShowDetectionResultsEH dlgShowDetectionResults;
		/// <summary>
		/// Represents the UpdateKnownFacesPanel method. Used for async calls
		/// </summary>
		private VoidEventHandler dlgUpdateKnownFacesPanel;
		/// <summary>
		/// Represents the ShowVLComponentsStatus
		/// </summary>
		private VoidEventHandler dlgShowVLComponentsStatus;
		/// <summary>
		/// Represents the CloseRequest
		/// </summary>
		private VoidEventHandler dlgCloseRequest;

		#endregion

		/// <summary>
		/// Stores the settings for the face recognition
		/// </summary>
		private RecoHumanSettigs settings;
		/// <summary>
		/// Collection of known faces
		/// </summary>
		private FaceCollection knownFaces;

		/// <summary>
		/// Manages commands
		/// </summary>
		private CommandManager commandManager;

		/// <summary>
		/// Manages connection with blackboard
		/// </summary>
		private ConnectionManager connectionManager;

		/// <summary>
		/// The image sorce manager
		/// </summary>
		private ImageSourceManager sourceManager;

		/// <summary>
		/// The human recognizer engine
		/// </summary>
		private HumanRecognizer engine;

		/// <summary>
		/// Adapter used for capturing images
		/// </summary>
		private CameraAdapter cameraAdapter;

		/// <summary>
		/// Adapter used for pipe transmited images
		/// </summary>
		private VideoPipeAdapter pipeAdapter;

		/// <summary>
		/// List of received responses.
		/// </summary>
		private List<Response> responsesReceived = new List<Response>();
		/// <summary>
		/// Indicates if AutoFind mode is on
		/// </summary>
		private bool autoFind = false;
		/// <summary>
		/// Regular expression used to extract pf_auto params
		/// </summary>
		private Regex rxAutoFind = new Regex(@"(?<mode>(1|0|enable|disable))(\s+(?<pName>\w+))?", RegexOptions.Compiled);
		/// <summary>
		/// Used to prevent cross thread access
		/// </summary>
		private object reBusy = new object();

		//private List<string> wildcards = new List<string>(new string[] { "human", "humans", "person", "persons", "all", "any" });
		private List<string> wildcards = new List<string>(new string[] { "human", "humans", "all", "any" });

		private TextBoxStreamWriter log;
		/// <summary>
		/// Flag that prevents reentrance to the image source set
		/// </summary>
		private bool settingImageSource;

		/// <summary>
		/// Image for video control
		/// </summary>
		private Bitmap vcImage;

		#region Command Executers

		private AsyncCommandExecuter cxFind;
		private AsyncCommandExecuter cxRemember;
		private SyncCommandExecuter cxAuto;
		private SyncCommandExecuter cxShutdown;
		private SyncCommandExecuter cxSleep;

		#endregion

		/// <summary>
		/// Eventhandler for console update
		/// </summary>
		private StringEventHandler updateConsoleEH;

		#region Shared Variables

		/// <summary>
		/// 
		/// </summary>
		private KnownHumanFaces svKnownHumans;
		/// <summary>
		/// 
		/// </summary>
		private DetectedHumanFaces svDetectedHumans;

		#endregion

		#region Socket Variables

		/// <summary>
		/// Stores the IP Address of the sender of the last packet received trough socket server
		/// </summary>
		protected IPAddress senderAddress;
		/// <summary>
		/// IP Address of the remote computer to connect using the socket client
		/// </summary>
		protected IPAddress serverIP;
		/// <summary>
		/// Port for incoming data used by Tcp Server
		/// </summary>
		protected int portIn;
		/// <summary>
		/// Port for outgoing data used by Tcp Client
		/// </summary>
		protected int portOut;

		#endregion

		#endregion

		#region Constructors

		/// <summary>
		/// Initiates a new instance of FrmRecoHuman
		/// </summary>
		public FrmRecoHuman()
		{
			InitializeComponent();
			SetImageSource(ImageSourceType.Camera);
			log = new TextBoxStreamWriter(txtConsole);
			//VideoClientPort = 2001;
			//VideoClientAddress = IPAddress.Parse("127.0.0.1");

			//LoadKnownFaces();
			//lastDetectedFaces = new FaceCollection();

			settings = RecoHumanSettigs.Load("Settings.xml");
			if (settings == null) settings = RecoHumanSettigs.Default;
			settingsPannel.Settings = settings;

			SetupAdapters();
			SetupSourceManager();
			SetupEngine();
			SetupCommandManager();
			SetupConnectionManager();
			SetupSharedVariables();
			chkAutoFind.Checked = autoFind;
			SetupEventHandlers();
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Represents the VideoControlUpdate method
		/// </summary>
		/// <param name="image">Image to update de video control with</param>
		/// <param name="faces">Array of faces recognized in the image</param>
		public delegate void VideoControlUpdateEH(Bitmap image, VleFace[] faces);
		/// <summary>
		/// Represents the VideoControlUpdate method
		/// </summary>
		/// <param name="image">Image to update de video control with</param>
		/// <param name="faces">Array of faces recognized in the image</param>
		/// <param name="detectionDetails">Details asociated to face detection</param>
		public delegate void VideoControlUpdate2EH(Bitmap image, VleFace[] faces, VleDetectionDetails[] detectionDetails);
		/// <summary>
		/// Represents the VideoControlUpdate method
		/// </summary>
		/// <param name="image">Image to update de video control with</param>
		/// <param name="faces">Face object that represents face recognized in the image</param>
		public delegate void VideoControlUpdate3EH(Bitmap image, Face faces);
		/// <summary>
		/// Represents the VideoControlUpdate method
		/// </summary>
		/// <param name="image">Image to update de video control with</param>
		/// <param name="faces">Array of Face objects that represents faces recognized in the image</param>
		public delegate void VideoControlUpdate4EH(Bitmap image, Face[] faces);
		/// <summary>
		/// Represents the ShowRecognitionResults method
		/// </summary>
		/// <param name="recognitionResults">Array of recognition results to display</param>
		public delegate void ShowRecognitionResultsEH(RecognitionResult[] recognitionResults);
		/// <summary>
		/// Represents the ShowDetectionResults method
		/// </summary>
		/// <param name="recognitionResults">Array of recognition results to display</param>
		public delegate void ShowDetectionResultsEH(Face[] detectedFaces);

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the index of the camera initialized by default
		/// </summary>
		public int CameraNumber
		{
			get {
				VeriLookWebCamAdapter cameraAdapter = this.cameraAdapter as VeriLookWebCamAdapter;
				if (cameraAdapter == null)
					return 0;
				return cameraAdapter.CameraNumber; }
			set
			{
				VeriLookWebCamAdapter cameraAdapter = this.cameraAdapter as VeriLookWebCamAdapter;
				if (cameraAdapter == null)
					return;
				cameraAdapter.CameraNumber = value;
			}
		}

		/*

		/// <summary>
		/// Gets or Sets the VideoClient IP Address
		/// </summary>
		public IPAddress VideoClientAddress
		{
			get { return videoClientAddress; }
			set
			{
				if (value == videoClientAddress) return;
				videoClientAddress = value;
			}
		}

		/// <summary>
		/// Gets or Sets the VideoClient Udp Port
		/// </summary>
		public int VideoClientPort
		{
			get { return videoClientPort; }
			set
			{
				if (value == videoClientPort) return;
				videoClientPort = value;
			}
		}

		*/

		#region Socket related

		/// <summary>
		/// Gets a value indicating if the module is woking in bidirectional mode
		/// </summary>
		public bool Bidirectional
		{
			get { return connectionManager.Bidirectional; }
		}

		/// <summary>
		/// Gets or sets the Tcp port for incoming data used by Tcp Server
		/// </summary>
		public int TcpPortIn
		{
			get { return connectionManager.PortIn; }
			set
			{
				connectionManager.PortIn = value;
				lblCurrentInputPort.Text = "Input port: " + value.ToString();
				lblCurrentInputPort.Visible = true;
			}
		}

		/// <summary>
		/// Gets or sets the Tcp port for outgoing data used by Tcp Client
		/// </summary>
		public int TcpPortOut
		{
			get { return connectionManager.PortOut; }
			set
			{
				connectionManager.PortOut = value;
				lblCurrentOutputPort.Text = "Output port: " + value.ToString();
				lblCurrentOutputPort.Visible = true;
			}
		}

		/// <summary>
		/// Gets or sets the IP Address of the remote computer to connect using the socket client
		/// </summary>
		public IPAddress TcpServerAddress
		{
			get { return connectionManager.TcpServerAddress; }
			set
			{
				connectionManager.TcpServerAddress = value;
				lblCurrentAddres.Text = "Server Address: " + value.ToString();
				lblCurrentAddres.Visible = true;
			}
		}

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Creates and clears the shared variable in the blackboard
		/// </summary>
		private void SetupSharedVariable()
		{
			//tcpSend("create_var \"string faces\" @" + AutoId.ToString());
			//tcpSend("write_var \"faces \" @" + AutoId.ToString());
		}


		#region VideoClient Methods

		/*

		/// <summary>
		/// Initializes the VideoClient Socket variables
		/// </summary>
		private void InitVideoClient()
		{
			if ((videoClientAddress == null) || (videoClientPort <= 1024) || (videoClientAddress == IPAddress.Any))
				return;
			try
			{
				videoClientSocket = new SocketUdp(videoClientPort);
				videoClientSocket.BufferSize = 512000;
				videoClientSocket.Open();
			}
			catch
			{
				videoClientSocket = null;
			}
		}

		/// <summary>
		/// Finalizes the VideoClient Socket variables
		/// </summary>
		private void EndVideoClient()
		{
			if ((videoClientAddress == null) || (videoClientPort <= 1024) || (videoClientAddress == IPAddress.Any))
				return;
			try
			{
				if (videoClientSocket.IsOpen)
					videoClientSocket.Close();
				videoClientSocket = null;
			}
			catch
			{
				videoClientSocket = null;
			}
		}

		/// <summary>
		/// Sends captured image trough socket to VideoClient
		/// </summary>
		/// <param name="image">Image to be sent</param>
		private void SendImage(NImage image)
		{
			if (videoClientSocket == null) return;
			if (!videoClientSocket.IsOpen)
			{
				return;
				try
				{
					videoClientSocket.Open();
					return;
				}
				catch { }
			}

			// Get image
			Bitmap bmp = image.ToBitmap();
			// Stream to store image
			System.IO.MemoryStream ms = new System.IO.MemoryStream(512000);
			// Final buffer
			byte[] buffer;
			// Serialize
			bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
			buffer = ms.ToArray();
			try
			{
				videoClientSocket.SendTo(VideoClientAddress, buffer, 0, buffer.Length);
			}
			catch { }
		}

		*/

		#endregion

		#region Miscelaneous Methods

		/// <summary>
		/// Imports known faces from file
		/// </summary>
		/// <param name="knownFacesFile">The file collection to load</param>
		private void ImportKnownFaces(string knownFacesFile)
		{
			if (knownFaces == null)
				knownFaces = new FaceCollection();
			knownFaces.Import(knownFacesFile);

		}

		/// <summary>
		/// Loads known faces from file
		/// </summary>
		private void LoadKnownFaces()
		{
			LoadKnownFaces("KnownFaces.rh");
		}

		/// <summary>
		/// Loads known faces from file
		/// </summary>
		/// <param name="knownFacesFile">The file collection to load</param>
		private void LoadKnownFaces(string knownFacesFile)
		{
			knownFaces = FaceCollection.Load(knownFacesFile);
			if (knownFaces == null)
				knownFaces = new FaceCollection();
			UpdateKnownFacesPanel();
		}

		/// <summary>
		/// Updates the KnownFaces Panel
		/// </summary>
		private void UpdateKnownFacesPanel()
		{
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.Disposing || this.IsDisposed) return;
				this.BeginInvoke(dlgUpdateKnownFacesPanel);
				return;
			}

			try
			{
				flpKnownFaces.Controls.Clear();
				foreach (Face face in knownFaces)
				{
					CtrlKnownFace ctrlKnownFace = new CtrlKnownFace(face);
					ctrlKnownFace.DeleteClick += new FaceEventHandler(ctrlKnownFace_DeleteClick);
					flpKnownFaces.Controls.Add(ctrlKnownFace);
				}
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		/// <summary>
		/// Updates the output video control
		/// </summary>
		/// <param name="image">Base Image</param>
		/// <param name="faces">List of faces detected</param>
		private void RecognitionResultUpdate(Bitmap image, VleFace[] faces)
		{
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.Disposing || this.IsDisposed)
					return;
				this.BeginInvoke(dlgRecognitionResultUpdate, image, faces);
				return;
			}
			vcRecognitionResult.Image = image;
			vcRecognitionResult.Faces = faces;
			//vcRecognitionResult.DetectionDetails = details;

		}

		/// <summary>
		/// Updates the output video control
		/// </summary>
		/// <param name="image">Base Image</param>
		/// <param name="faces">List of faces detected</param>
		/// <param name="details">Recognition details asociated to face detected</param>
		private void RecognitionResultUpdate(Bitmap image, VleFace[] faces, VleDetectionDetails[] details)
		{
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.Disposing || this.IsDisposed)
					return;
				this.BeginInvoke(dlgRecognitionResultUpdate2, image, faces, details);
				return;
			}
			vcRecognitionResult.Image = image;
			vcRecognitionResult.Faces = faces;
			vcRecognitionResult.DetectionDetails = details;

		}

		/// <summary>
		/// Updates the output video control
		/// </summary>
		/// <param name="image">Base Image</param>
		/// <param name="faces">List of faces detected</param>
		private void RecognitionResultUpdate(Bitmap image, Face face)
		{
			if (face == null)
				return;
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.Disposing || this.IsDisposed)
					return;
				this.BeginInvoke(dlgRecognitionResultUpdate3, image, face);
				return;
			}
			VleFace[] vlFaces;
			VleDetectionDetails[] vlDetails;

			vlFaces = new VleFace[] { face.VlFace };
			vlDetails = new VleDetectionDetails[] { face.DetectionDetails };
			vcRecognitionResult.Image = image;
			vcRecognitionResult.Faces = vlFaces;
			vcRecognitionResult.DetectionDetails = vlDetails;
			if (face.HasFOV)
			{
				vcRecognitionResult.String = "FOV: (" + face.HFoV.ToString("0.00") + "," + face.VFoV.ToString("0.00") + ")";
			}
		}

		/// <summary>
		/// Updates the output video control
		/// </summary>
		/// <param name="image">Base Image</param>
		/// <param name="faces">List of faces detected</param>
		private void RecognitionResultUpdate(Bitmap image, Face[] faces)
		{
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.Disposing || this.IsDisposed)
					return;
				this.BeginInvoke(dlgRecognitionResultUpdate4, image, faces);
				return;
			}
			VleFace[] vlFaces;
			VleDetectionDetails[] vlDetails;

			vlFaces = new VleFace[faces.Length];
			vlDetails = new VleDetectionDetails[faces.Length];
			for (int i = 0; i < faces.Length; ++i)
			{
				if (faces[i] != null)
				{
					vlFaces[i] = faces[i].VlFace;
					vlDetails[i] = faces[i].DetectionDetails;
				}
			}
			vcRecognitionResult.Image = image;
			vcRecognitionResult.Faces = vlFaces;
			vcRecognitionResult.DetectionDetails = vlDetails;
			if (faces[0].HasFOV)
			{
				vcRecognitionResult.String = "(" + faces[0].HFoV.ToString("0.00") + "," + faces[0].VFoV.ToString("0.00") + ")";
			}
		}

		/// <summary>
		/// Saves known faces into a file.
		/// </summary>
		private void SaveKnownFaces()
		{
			SaveKnownFaces("KnownFaces.rh");
		}

		/// <summary>
		/// Saves known faces into a file.
		/// </summary>
		private void SaveKnownFaces(string knownFacesFile)
		{
			try
			{
				FaceCollection.Save(knownFacesFile, knownFaces);
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		/// <summary>
		/// Seths the human name in the form
		/// </summary>
		private void SetHumanName(string name)
		{
			try
			{
				if (this.InvokeRequired)
				{
					if (!this.Disposing && !this.IsDisposed && this.IsHandleCreated)
						this.BeginInvoke(new StringEventHandler(SetHumanName), name);
					return;
				}
				txtHumanName.Text = name;
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		/// <summary>
		/// Sets the source for image acquisition
		/// </summary>
		/// <param name="imageSource">Source for image acquisition</param>
		private void SetImageSource(ImageSourceType imageSource)
		{
			if (settingImageSource)
				return;
			settingImageSource = true;
			rbImgSrcCamera.Checked = (imageSource == ImageSourceType.Camera);
			rbImgSrcFile.Checked = (imageSource == ImageSourceType.File);
			rbImgSrcSocket.Checked = (imageSource == ImageSourceType.ImageServer);
			rbImgSrcFile.Location = (imageSource == ImageSourceType.ImageServer) ? new Point(6, 117) : new Point(6, 65);

			gbDetectedCameras.Enabled = (imageSource == ImageSourceType.Camera);
			ctrlCameraSettings.Enabled = (imageSource == ImageSourceType.Camera);

			lblImgSrcServerAddress.Enabled = (imageSource == ImageSourceType.ImageServer);
			txtImgSrcServerAddress.Enabled = (imageSource == ImageSourceType.ImageServer);
			lblImgSrcServerPort.Enabled = (imageSource == ImageSourceType.ImageServer);
			txtImgSrcServerPort.Enabled = (imageSource == ImageSourceType.ImageServer);
			lblImgSrcServerAddress.Visible = (imageSource == ImageSourceType.ImageServer);
			txtImgSrcServerAddress.Visible = (imageSource == ImageSourceType.ImageServer);
			lblImgSrcServerPort.Visible = (imageSource == ImageSourceType.ImageServer);
			txtImgSrcServerPort.Visible = (imageSource == ImageSourceType.ImageServer);

			txtImgSrcFileName.Enabled = (imageSource == ImageSourceType.File);
			txtImgSrcFileName.Visible = (imageSource == ImageSourceType.File);
			lblImgSrcFileName.Enabled = (imageSource == ImageSourceType.File);
			lblImgSrcFileName.Visible = (imageSource == ImageSourceType.File);
			settingImageSource = false;
		}

		/// <summary>
		/// Shows the recognition results in the recognition results pannel
		/// </summary>
		/// <param name="recognitionResults">Array of recognition results to display</param>
		public void ShowRecognitionResults(RecognitionResult[] recognitionResults)
		{
			if (recognitionResults.Length < 1) return;
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.IsDisposed || this.IsDisposed) return;
				this.BeginInvoke(dlgShowRecognitionResults, new object[] { recognitionResults });
				return;
			}
			try
			{
				btnExport.Controls.Clear();
				for (int i = 0; (i < recognitionResults.Length) && (i < 4); ++i)
				{
					btnExport.Controls.Add(new CtrlRecognitionResult(recognitionResults[i]));
				}
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		/// <summary>
		/// Shows the detected faces in the recognition results pannel
		/// </summary>
		/// <param name="detectedFaces">Array of detected faces to display</param>
		public void ShowDetectionResults(Face[] detectedFaces)
		{
			if (detectedFaces.Length < 1) return;
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.IsDisposed || this.Disposing) return;
				this.BeginInvoke(dlgShowDetectionResults, new object[] { detectedFaces });
				return;
			}
			CtrlDetectedFace[] ctrls = new CtrlDetectedFace[detectedFaces.Length];

			try
			{
				for (int i = 0; (i < detectedFaces.Length) && (i < 4); ++i)
				{
					//flpRecognitionResult.Controls.Add(new CtrlDetectedFace(detectedFaces[i]));
					ctrls[i] = new CtrlDetectedFace(detectedFaces[i]);
				}
			}
			catch (Exception ex)
			{
				Console(ex.Message);
				return;
			}

			try
			{
				btnExport.Controls.Clear();
				btnExport.Controls.AddRange(ctrls);
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		/// <summary>
		/// Updates the settings controls
		/// </summary>
		private void UpdateSettings()
		{
			settingsPannel.UpdateSettings();
		}

		/// <summary>
		/// Updates the input video control
		/// </summary>
		private void UpdateVideoControls()
		{
			if (this.InvokeRequired)
			{
				if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
					this.BeginInvoke(dlgUpdateVideoControls);
				return;
			}
			if ((vcImage == null) || (this.WindowState == FormWindowState.Minimized) || !this.chkDrawInput.Checked || !this.Visible)
				return;
			vcCapturedImage.Image = vcImage;
			vcImage = null;
		}

		/// <summary>
		/// Shows the registered status of Verilook Components in the form
		/// </summary>
		private void ShowVLComponentsStatus()
		{
			try
			{
				if (this.InvokeRequired)
				{
					if (!this.IsHandleCreated || this.IsDisposed || this.Disposing) return;
					this.BeginInvoke(dlgShowVLComponentsStatus);
					return;
				}
				if (VLExtractor.IsRegistered)
				{
					lblExtractorStatus.Text = "Verilook Extractor is registered";
					lblExtractorStatus.Image = Properties.Resources.ok16;
				}
				else
				{
					lblExtractorStatus.Text = "Verilook Extractor IS NOT registered";
					lblExtractorStatus.Image = Properties.Resources.err16;
				}

				if (VLMatcher.IsRegistered)
				{
					lblMatcherStatus.Text = "Verilook Matcher is registered";
					lblMatcherStatus.Image = Properties.Resources.ok16;
				}
				else
				{
					lblMatcherStatus.Text = "Verilook Matcher IS NOT registered";
					lblMatcherStatus.Image = Properties.Resources.err16;
				}
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		/// <summary>
		/// Closes the form and ends application
		/// </summary>
		private void CloseRequest()
		{
			if (this.InvokeRequired)
			{
				if (!this.IsHandleCreated || this.IsDisposed || this.Disposing) return;
				this.BeginInvoke(dlgCloseRequest);
				return;
			}
			this.Close();
			engine.Stop();
			SaveKnownFaces();
			RecoHumanSettigs.Save("Settings.xml", settings);
			commandManager.Stop();
			connectionManager.Stop();
			Application.Exit();
		}

		#endregion

		#region Camera methods

		protected void ListCameras()
		{
			VeriLookWebCamAdapter cameraAdapter = this.cameraAdapter as VeriLookWebCamAdapter;
			if (cameraAdapter == null)
				return;
			if (this.InvokeRequired)
			{
				if (this.IsHandleCreated && !this.Disposing && !this.IsDisposed)
					this.BeginInvoke(dlgListCameras);
				return;
			}
			lstDetectedCameras.Items.Clear();
			if (cameraAdapter.DetectedCameras.Count < 1)
			{
				ctrlCameraSettings.Enabled = false;
				MessageBox.Show("No capture devices found!");
				return;
			}
			ctrlCameraSettings.Enabled = true;

			foreach (Camera cam in cameraAdapter.DetectedCameras)
			{
				lstDetectedCameras.Items.Add(cam.ID);
			}

		}

		protected void UpdateControlCameras()
		{
			if (this.InvokeRequired)
			{
				if (this.IsHandleCreated && !this.Disposing && !this.IsDisposed)
					this.BeginInvoke(dlgUpdateControlCameras);
				return;
			}
			ctrlCameraSettings.Camera = cameraAdapter;
		}

		#endregion

		#region Setup Methods

		private void SetupAdapters()
		{
			cameraAdapter = new VeriLookWebCamAdapter();
			// cameraAdapter.CameraCollectionChanged += new VeriLookWebCamAdapterEventHandler(adapter_CameraCollectionChanged);
			cameraAdapter.ImageCaptured += new ImageCapturedEventHandler(adapter_ImageCaptured);
			cameraAdapter.SelectedCameraChanged += new CameraAdapterEventHandler(adapter_SelectedCameraChanged);
			cameraAdapter.Started += new CameraAdapterEventHandler(adapter_Started);
			cameraAdapter.Stopped += new CameraAdapterEventHandler(adapter_Stopped);

			pipeAdapter = new VideoPipeAdapter("kinectRGB");
		}

		private void SetupCommandManager()
		{
			cxAuto = new PfAuto(engine);
			cxFind = new PfFind(engine);
			cxRemember = new PfRemember(engine);
			cxShutdown = new PfShutdown(engine);
			cxSleep = new PfSleep(engine);

			commandManager = new CommandManager();
			commandManager.CommandExecuters.Add(cxAuto);
			commandManager.CommandExecuters.Add(cxFind);
			commandManager.CommandExecuters.Add(cxRemember);
			commandManager.CommandExecuters.Add(cxShutdown);
			commandManager.CommandExecuters.Add(cxSleep);
		}

		private void SetupConnectionManager()
		{
			connectionManager = new ConnectionManager(MODULE_NAME, 2075, 2075, serverIP, commandManager);
			connectionManager.ClientConnected += new TcpClientConnectedEventHandler(connectionManager_ClientConnected);
			connectionManager.ClientDisconnected += new TcpClientDisconnectedEventHandler(connectionManager_ClientDisconnected);
			connectionManager.Connected += new TcpClientConnectedEventHandler(connectionManager_Connected);
			connectionManager.Disconnected += new TcpClientDisconnectedEventHandler(connectionManager_Disconnected);
#if DEBUG
			connectionManager.DataReceived += new ConnectionManagerDataReceivedEH(connectionManager_DataReceived);
#endif
			TcpPortIn = 2075;
			TcpPortOut = 2075;
			TcpServerAddress = IPAddress.Parse("127.0.0.1");
		}

		private void SetupEventHandlers()
		{
			this.FormClosing += new FormClosingEventHandler(FrmRecoHuman_FormClosing);

			dlgUpdateControlCameras = new VoidEventHandler(UpdateControlCameras);
			dlgListCameras = new VoidEventHandler(ListCameras);
			dlgUpdateSettings = new VoidEventHandler(UpdateSettings);
			dlgUpdateVideoControls = new VoidEventHandler(UpdateVideoControls);
			dlgRecognitionResultUpdate = new VideoControlUpdateEH(RecognitionResultUpdate);
			dlgRecognitionResultUpdate2 = new VideoControlUpdate2EH(RecognitionResultUpdate);
			dlgRecognitionResultUpdate3 = new VideoControlUpdate3EH(RecognitionResultUpdate);
			dlgRecognitionResultUpdate4 = new VideoControlUpdate4EH(RecognitionResultUpdate);
			dlgShowRecognitionResults = new ShowRecognitionResultsEH(ShowRecognitionResults);
			dlgShowDetectionResults = new ShowDetectionResultsEH(ShowDetectionResults);
			dlgUpdateKnownFacesPanel = new VoidEventHandler(UpdateKnownFacesPanel);
			dlgShowVLComponentsStatus = new VoidEventHandler(ShowVLComponentsStatus);
			dlgCloseRequest = new VoidEventHandler(CloseRequest);
			updateConsoleEH = new StringEventHandler(Console);
		}

		private void SetupEngine()
		{
			engine = new HumanRecognizer(sourceManager);
			engine.FaceDetected += new FaceDetectedEventHandler(engine_FaceDetected);
			engine.FacesLoaded += new HumanRecognizerEventHandler(engine_FacesLoaded);
			engine.HumanNameChanged += new HumanNameChangedEventHandler(engine_HumanNameChanged);
			engine.Started += new HumanRecognizerEventHandler(engine_Started);
			engine.Stopped += new HumanRecognizerEventHandler(engine_Stopped);
		}

		private void SetupSharedVariables()
		{
			svDetectedHumans = new DetectedHumanFaces("rhDetectedHumans");
			svKnownHumans = new KnownHumanFaces("rhKnownHumans");
			commandManager.SharedVariablesLoaded += new SharedVariablesLoadedEventHandler(commandManager_SharedVariablesLoaded);
		}

		private void SetupSourceManager()
		{
			sourceManager = new ImageSourceManager();
			sourceManager.Sources.Add(cameraAdapter);
			sourceManager.Sources.Add(pipeAdapter);
			sourceManager.SelectedSource = pipeAdapter;
		}

		#endregion

		#endregion

		#region Adapter Events

		private void adapter_CameraCollectionChanged(VeriLookWebCamAdapter sender)
		{
			ListCameras();
		}

		private void adapter_ImageCaptured(CameraAdapter sender, Bitmap capturedImage)
		{
			// Request update video display
			vcImage = capturedImage;
			UpdateVideoControls();
		}

		private void adapter_SelectedCameraChanged(CameraAdapter sender)
		{
			UpdateControlCameras();
		}

		private void adapter_Started(CameraAdapter sender)
		{
			//throw new NotImplementedException();
		}

		private void adapter_Stopped(CameraAdapter sender)
		{
			//throw new NotImplementedException();
		}

		#endregion

		#region Engine Events

		private void engine_FaceDetected(HumanRecognizer sender, FaceCollection faces)
		{
			foreach (Face f in faces)
			{

			}
			if (sender.AutoFindHuman)
			{
			}
		}

		private void engine_FacesLoaded(HumanRecognizer sender)
		{
			UpdateKnownFacesPanel();
		}

		private void engine_HumanNameChanged(HumanRecognizer sender, string humanName)
		{
			SetHumanName(humanName);
		}

		private void engine_Started(HumanRecognizer sender)
		{
			ShowVLComponentsStatus();
		}

		private void engine_Stopped(HumanRecognizer sender)
		{
			// throw new NotImplementedException();
		}

		#endregion

		#region Form Event Handlers

		private void ctrlKnownFace_DeleteClick(Face face)
		{
			try
			{
				lock (knownFaces)
				{
					knownFaces.Remove(face);
				}
				SaveKnownFaces();
				UpdateKnownFacesPanel();
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		private void lstDetectedCameras_SelectedIndexChanged(object sender, EventArgs e)
		{
			VeriLookWebCamAdapter cameraAdapter = this.cameraAdapter as VeriLookWebCamAdapter;
			if (cameraAdapter == null)
				return;
			try
			{
				if ((lstDetectedCameras.SelectedIndex == -1) || (cameraAdapter.DetectedCameras.Count < lstDetectedCameras.SelectedIndex)) return;
				if (cameraAdapter.SelectedCamera == cameraAdapter.DetectedCameras[lstDetectedCameras.SelectedIndex]) return;
				cameraAdapter.NCamera = cameraAdapter.DetectedCameras[lstDetectedCameras.SelectedIndex];
				UpdateControlCameras();
			}
			catch (Exception ex) { Console(ex.Message); }
		}

		private void btnFindCameras_Click(object sender, EventArgs e)
		{
			ListCameras();
		}

		private void FrmRecoHuman_FormClosing(object sender, FormClosingEventArgs e)
		{

			try
			{
				if (!this.InvokeRequired)
					this.Enabled = false;
			}
			catch { }
			cameraAdapter.Stop();
			engine.Stop();
			SaveKnownFaces();
			RecoHumanSettigs.Save("Settings.xml", settings);
			commandManager.Stop();
			connectionManager.Stop();

		}

		private void FrmRecoHuman_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void txtConsole_TextChanged(object sender, EventArgs e)
		{
			//Clipboard.SetText(txtConsole.Text);
		}

		private void btnFindHuman_Click(object sender, EventArgs e)
		{
			bool result;
			double hFoV;
			double vFoV;
			string person = "unknown";

			Console("Requested FindHuman");
			if (txtHumanName.Text.Length > 0) person = txtHumanName.Text;
			result = engine.FindHuman(ref person, out hFoV, out vFoV);
			Console("FindHuman [" + person + "] " + (result ? "success!" : "failed"));
		}

		private void btnRememberHuman_Click(object sender, EventArgs e)
		{
			bool result;
			string person = "unknown";

			Console("Requested RememberHuman");
			if ((txtHumanName.Text.Trim().Length < 1) || txtHumanName.Text.Trim().ToLower() == "unknown")
			{
				result = false;
				Console("Invalid human name");
			}
			else
			{
				person = txtHumanName.Text;
				result = engine.RememberHuman(person, 3);
			}
			Console("RememberHuman [" + person + "] " + (result ? "success!" : "failed"));
		}

		private void chkAutoFind_CheckedChanged(object sender, EventArgs e)
		{
			engine.AutoFindHuman = chkAutoFind.Checked;
		}

		private void FrmRecoHuman_Load(object sender, EventArgs e)
		{
			engine.Start();
			//foreach (IImageSource source in sourceManager.Sources)
			//	source.Start();
			cameraAdapter.Start();
#if PIPES
			pipeAdapter.Start();
#endif
			connectionManager.Start();
			commandManager.Start();

			//UpdateKnownFacesPanel();
		}

		private void tsbExportKnownFaces_Click(object sender, EventArgs e)
		{
			if (dlgExportKnownFaces.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				SaveKnownFaces(dlgExportKnownFaces.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error saving known faces\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tsbImportKnownFaces_Click(object sender, EventArgs e)
		{
			if (dlgImportKnownFaces.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				ImportKnownFaces(dlgImportKnownFaces.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error importing known faces\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tsbClearKnownFaces_Click(object sender, EventArgs e)
		{
			lock (knownFaces)
			{
				knownFaces.Clear();
			}
			SaveKnownFaces();
			UpdateKnownFacesPanel();
		}

		private void tsbLoadKnownFaces_Click(object sender, EventArgs e)
		{
			if (dlgLoadKnownFaces.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				LoadKnownFaces(dlgLoadKnownFaces.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading known faces\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void rbImgSrcCamera_CheckedChanged(object sender, EventArgs e)
		{
			SetImageSource(ImageSourceType.Camera);
		}

		private void rbImgSrcSocket_CheckedChanged(object sender, EventArgs e)
		{
			SetImageSource(ImageSourceType.ImageServer);
		}

		private void rbImgSrcFile_CheckedChanged(object sender, EventArgs e)
		{
			SetImageSource(ImageSourceType.File);
		}

		#endregion

		#region Command Manager events

		/// <summary>
		/// Manages the SharedVariablesLoaded event of the CommandManager
		/// </summary>
		private void commandManager_SharedVariablesLoaded(CommandManager cmdMan)
		{
			bool written;

			if (!cmdMan.SharedVariables.Contains("rhKnownHumans"))
				cmdMan.SharedVariables.Add(svKnownHumans);
			else svKnownHumans = (KnownHumanFaces)cmdMan.SharedVariables["rhKnownHumans"];

			if (!cmdMan.SharedVariables.Contains("rhDetectedHumans"))
				cmdMan.SharedVariables.Add(svDetectedHumans);
			else svDetectedHumans = (DetectedHumanFaces)cmdMan.SharedVariables["rhDetectedHumans"];

			written = false;
			for (int i = 0; (i < 3) && !written; ++i)
				written = svDetectedHumans.TryWrite(null);

			written = false;
			for (int i = 0; (i < 3) && !written; ++i)
				written = svKnownHumans.TryWrite(null);
		}

		#endregion

		#region Connection Manager events

		/// <summary>
		/// Manages the DataReceived event of the ConnectionManager
		/// </summary>
		/// <param name="p">Received data</param>
		private void connectionManager_DataReceived(ConnectionManager connectionManager, TcpPacket packet)
		{
#if DEBUG
			if (!packet.IsAnsi)
				return;
			for (int i = 0; i < packet.DataStrings.Length; ++i)
			{
				Console("Rcv: " + "[" + packet.RemoteEndPoint.Address.ToString() + "] " + packet.DataStrings[i]);
			}
#endif
		}

		/// <summary>
		/// Manages the Disconnected event of the ConnectionManager
		/// </summary>
		/// <param name="ep">Disconnection endpoint</param>
		private void connectionManager_Disconnected(EndPoint ep)
		{
			Console("Client disconnected");
			//lblClientConnected.Visible = false;
		}

		/// <summary>
		/// Manages the Connected event of the ConnectionManager
		/// </summary>
		/// <param name="s">Socket used for connection</param>
		private void connectionManager_Connected(Socket s)
		{
			Console("Client connected to " + s.RemoteEndPoint.ToString());
			//lblClientConnected.Visible = true;
		}

		/// <summary>
		/// Manages the ClientDisconnected event of the ConnectionManager
		/// </summary>
		/// <param name="ep">Disconnection endpoint</param>
		private void connectionManager_ClientDisconnected(EndPoint ep)
		{
			try
			{
				Console("" + ep.ToString() + " disconnected from local server");
			}
			catch { Console("Client 0.0.0.0:0 disconnected from local server"); }
		}

		/// <summary>
		/// Manages the ClientConnected event of the ConnectionManager
		/// </summary>
		/// <param name="s">Socket used for connection</param>
		private void connectionManager_ClientConnected(Socket s)
		{
			Console(s.RemoteEndPoint.ToString() + " connected to local server");
		}

		#endregion

		#region Console Methods

		/// <summary>
		/// Appends text to the console
		/// </summary>
		/// <param name="text">Text to append</param>
		protected void Console(string text)
		{
			log.WriteLine(text);
		}

		private void updateConsole(string text)
		{
			log.Write(text);
		}

		#endregion

	}
}
