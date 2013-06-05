using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using WIA;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Image acquisition using WIA
	/// </summary>
	public class WIACameraAdapter : CameraAdapter
	{
		#region Variables

		/// <summary>
		/// Asynchronously capture images and stores it into a Queue
		/// </summary>
		private Thread mainThread;

		/// <summary>
		/// flag that indicates if the adapter is ready.
		/// </summary>
		private bool ready;

		/// <summary>
		/// Indicates whether the camera is being reconfigured
		/// </summary>
		private bool settingUpCamera;

		/// <summary>
		/// Stores the last captured image
		/// </summary>
		private Bitmap lastCapturedImage;

		/// <summary>
		/// Event to wait until a new image is captured
		/// </summary>
		private AutoResetEvent imageCapturedEvent;

		#region Camera Variables

		/// <summary>
		/// Index of selected camera
		/// </summary>
		private int cameraNumber;

		/// <summary>
		/// Strores the collection of detected cameras
		/// </summary>
		private Device[] detectedCameras;

		/// <summary>
		/// Stores the camera selected for capturing
		/// </summary>
		private Device selectedCamera;

		/// <summary>
		/// Indicates if the capture thread must sleep
		/// </summary>
		private bool sleepCapture;

		#endregion

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of VeriLookWebCamAdapter
		/// </summary>
		public WIACameraAdapter()
		{
			mainThread = new Thread(new ThreadStart(MainThreadTask));
			mainThread.IsBackground = true;
			mainThread.Priority = ThreadPriority.BelowNormal;
			imageCapturedEvent = new AutoResetEvent(false);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the collection of cameras changes
		/// </summary>
		public event WIACameraAdapterEventHandler CameraCollectionChanged;

		/*

		/// <summary>
		/// Occurs when an image is captured
		/// </summary>
		public event WIACameraAdapterImageCapturedEventHandler ImageCaptured;

		/// <summary>
		/// Occurs when a new image is received and becomes availiable
		/// </summary>
		public event ImageProducedEventHandler ImageProduced;

		/// <summary>
		/// Occurs when the SelectedCamera changes
		/// </summary>
		public event WIACameraAdapterEventHandler SelectedCameraChanged;

		/// <summary>
		/// Occurs when the VeriLookWebCamAdapter adapter is started
		/// </summary>
		public event WIACameraAdapterEventHandler Started;

		/// <summary>
		/// Occurs when the VeriLookWebCamAdapter adapter is stopped
		/// </summary>
		public event WIACameraAdapterEventHandler Stopped;

		*/

		#endregion

		#region Properties

		/*
		/// <summary>
		/// Gets or sets a value indicating if the camera uses automatic settings
		/// </summary>
		public bool AutomaticSettings
		{
			get { return (selectedCamera != null) && selectedCamera.AutomaticSettings; }
			set {
				if (selectedCamera == null)return;
				settingUpCamera = true;
				while (selectedCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				selectedCamera.AutomaticSettings = value;
				settingUpCamera = false;
			}
		}
		*/

		/// <summary>
		/// Gets the camera availiable video formats
		/// </summary>
		public override VideoFormat[] AvailableVideoFormats
		{
			get
			{
				// return selectedCamera.GetAvailableVideoFormats(); 
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the index of the camera initialized by default
		/// </summary>
		public int CameraNumber
		{
			get { return cameraNumber; }
			set
			{
				if (cameraNumber < 0) cameraNumber = 0;
				else cameraNumber = value;
			}
		}

		/// <summary>
		/// Gets the collection of detected cameras
		/// </summary>
		public Device[] DetectedCameras
		{
			get { return detectedCameras; }
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera is capturing
		/// </summary>
		public override bool IsCapturing
		{
			get
			{
				// return (selectedCamera != null) ? selectedCamera.IsCapturing : false;
				return false;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored horizontally
		/// </summary>
		public override bool MirrorHorizontal
		{
			get
			{
				//return (selectedCamera != null) ? selectedCamera.MirrorHorizontal : false; 
				return false;
			}
			set
			{
				//if (selectedCamera == null) return;
				//settingUpCamera = true;
				//while (selectedCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				//selectedCamera.MirrorHorizontal = value;
				//settingUpCamera = false;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored vertically
		/// </summary>
		public override bool MirrorVertical
		{
			get
			{
				//return (selectedCamera != null) ? selectedCamera.MirrorVertical : false; 
				return false;
			}
			set
			{
				//if (selectedCamera == null) return;
				//settingUpCamera = true;
				//while (selectedCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				//selectedCamera.MirrorVertical = value;
				//settingUpCamera = false;
			}
		}

		/// <summary>
		/// Gets or sets the video format used for capturing
		/// </summary>
		public override VideoFormat VideoFormat
		{
			get {
				// return selectedCamera.VideoFormat;
				return new VideoFormat();
			}
			set
			{
				//settingUpCamera = true;
				//while (selectedCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				//selectedCamera.VideoFormat = value;
				//settingUpCamera = false;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Rises the CameraCollectionChanged event
		/// </summary>
		protected void OnCameraCollectionChanged()
		{
			if (this.CameraCollectionChanged != null)
				this.CameraCollectionChanged(this);
		}

		#region Thread Methods

		protected override void SetupCamera()
		{
			/*
			int cameraCount;
			int lastCameraCount = 0;
			List<CameraVideoFormat> formats;

			if (cameraManager != null)
				cameraManager.Dispose();
			try
			{
				cameraManager = new CameraMan(null);
			}
			catch { }

			cameraCount = (detectedCameras = cameraManager.Cameras).Count;
			if (cameraCount < 1)
			{
				selectedCamera = null;
				Ready = false;
				Thread.Sleep(1000);
				return;
			}
			if (lastCameraCount != cameraCount)
			{
				OnCameraCollectionChanged();
				lastCameraCount = cameraCount;
			}
			if (selectedCamera == null)
			{
				if (cameraNumber < detectedCameras.Count)
					selectedCamera = detectedCameras[cameraNumber];
				else
					selectedCamera = detectedCameras[0];
				Console("Selecting camera: " + selectedCamera.ID + ". Availiable formats:");
				if (selectedCamera.IsCapturing) selectedCamera.StopCapturing();
				// Set default resolution 320x240
				formats = new List<CameraVideoFormat>();
				foreach (CameraVideoFormat f in selectedCamera.GetAvailableVideoFormats())
				{
					Console("\t" + f.FrameWidth.ToString() + " x " + f.FrameHeight.ToString() + " @" + f.FrameRate.ToString() + "fps");
					//if ((f.FrameWidth == 320) || (f.FrameHeight == 240))
					if ((f.FrameWidth == 320) || (f.FrameHeight == 240) || (f.FrameWidth == 640) || (f.FrameHeight == 480))
					//if ((f.FrameWidth == 640) || (f.FrameHeight == 480))
					{
						formats.Add(f);
						//selectedCamera.VideoFormat = f;
						//break;
					}
				}
				formats.Sort(new Comparison<CameraVideoFormat>(VideoFormatComparer));
				if (formats.Count > 0)
				{
					selectedCamera.VideoFormat = formats[0];
					Console("Selected Format: " + selectedCamera.VideoFormat.FrameWidth.ToString() + " x " + selectedCamera.VideoFormat.FrameHeight.ToString() + " @" + selectedCamera.VideoFormat.FrameRate.ToString() + "fps");
				}
				
				OnSelectedCameraChanged();
			}
			*/
		}

		protected override void CaptureFrame()
		{
			/*
			Bitmap currentFrame;
			int captureDelay = 16;

			// Do nothing if the parameters of the camera is being changed
			while (settingUpCamera)
			{
				if (selectedCamera.IsCapturing) selectedCamera.StopCapturing();
				if (!running) return;
				Thread.Sleep(10);
			}

			if (sleepCapture)
			{
				if (selectedCamera.IsCapturing)
					selectedCamera.StopCapturing();
				//capturedImages.Clear();
				while (sleepCapture)
				{
					if (!running) return;
					Thread.Sleep(20);
				}
			}

			// Start capturing if it has not been started
			if (!selectedCamera.IsCapturing)
			{
				captureDelay = (int)(1000 / selectedCamera.VideoFormat.FrameRate) - 1;
				selectedCamera.StartCapturing();
			}
			Thread.Sleep(captureDelay);
			// Get captured image
			currentFrame = selectedCamera.GetCurrentFrame();
			OnImageCaptured(currentFrame);
			*/
		}

		#endregion

		#endregion
	}
}
