using System;
using System.Drawing;
using System.Threading;

using Robotics;

using Neurotec;
using Neurotec.Cameras;
using Neurotec.Biometrics;
using Neurotec.Images;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Provides access to video capturing using the VeriLook capture engine
	/// </summary>
	public class VeriLookWebCamAdapter : CameraAdapter, IWebCamAdapter
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
		/// Camera manager manages all installed camera
		/// </summary>
		private CameraMan cameraManager;

		/// <summary>
		/// Strores the collection of detected cameras
		/// </summary>
		private CameraMan.CameraCollection detectedCameras;

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
		public VeriLookWebCamAdapter()
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
		public event VeriLookWebCamAdapterEventHandler CameraCollectionChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating if the camera uses automatic settings
		/// </summary>
		public bool AutomaticSettings
		{
			get { return (NCamera != null) && NCamera.AutomaticSettings; }
			set {
				if (NCamera == null) return;
				settingUpCamera = true;
				while (NCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				NCamera.AutomaticSettings = value;
				settingUpCamera = false;
			}
		}

		/// <summary>
		/// Gets the camera availiable video formats
		/// </summary>
		public override VideoFormat[] AvailableVideoFormats
		{
			get {
				if(SelectedCamera == null)
					return null;
				CameraVideoFormat[] avf = NCamera.GetAvailableVideoFormats();
				VideoFormat[] formats = new VideoFormat[avf.Length];
				for(int i = 0; i < formats.Length; ++i)
					formats[i] = new VideoFormat(avf[i].FrameWidth, avf[i].FrameHeight, avf[i].FrameRate);
				return formats;
			}
		}

		/// <summary>
		/// Gets the camera availiable video formats
		/// </summary>
		CameraVideoFormat[] IWebCamAdapter.AvailableVideoFormats
		{
			get { return NCamera.GetAvailableVideoFormats(); }
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
		public CameraMan.CameraCollection DetectedCameras
		{
			get{return detectedCameras;}
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera is capturing
		/// </summary>
		public override bool IsCapturing
		{
			get { return (SelectedCamera != null) ? SelectedCamera.IsCapturing : false; }
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored horizontally
		/// </summary>
		public override bool MirrorHorizontal
		{
			get { return (NCamera != null) ? NCamera.MirrorHorizontal : false; }
			set
			{
				if (SelectedCamera == null) return;
				settingUpCamera = true;
				while (NCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				NCamera.MirrorHorizontal = value;
				settingUpCamera = false;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored vertically
		/// </summary>
		public override bool MirrorVertical
		{
			get { return (NCamera != null) ? NCamera.MirrorVertical : false; }
			set {
				if (SelectedCamera == null) return;
				settingUpCamera = true;
				while (NCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				NCamera.MirrorVertical = value;
				settingUpCamera = false;
			}
		}

		public Neurotec.Cameras.Camera NCamera
		{
			get { return (this.SelectedCamera == null) ? null : ((VerilookWebCam)this.SelectedCamera).Camera; }
			set
			{
				this.SelectedCamera = (value == null) ? null : new VerilookWebCam(value);
			}
		}

		/// <summary>
		/// Gets a value that indicates if the module is ready
		/// </summary>
		public override bool Ready
		{
			get { return ready; }
			protected set
			{
				if (ready == value) return;
				ready = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the the capture thread must sleep
		/// </summary>
		public override bool SleepCapture
		{
			get { return sleepCapture; }
			set
			{
				sleepCapture = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the video format used for capturing
		/// </summary>
		public override VideoFormat VideoFormat
		{
			get { return NCamera.VideoFormat; }
			set
			{
				settingUpCamera = true;
				while (NCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				NCamera.VideoFormat = value;
				settingUpCamera = false;
			}
		}

		/// <summary>
		/// Gets or sets the video format used for capturing
		/// </summary>
		CameraVideoFormat IWebCamAdapter.VideoFormat
		{
			get { return NCamera.VideoFormat; }
			set
			{
				settingUpCamera = true;
				while (SelectedCamera.IsCapturing) System.Threading.Thread.Sleep(0);
				NCamera.VideoFormat = value;
				settingUpCamera = false;
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

		protected int VideoFormatComparer(CameraVideoFormat f1, CameraVideoFormat f2)
		{
			int diag1, diag2;

			diag1 = (int)Math.Sqrt(f1.FrameWidth * f1.FrameWidth + f1.FrameHeight * f1.FrameHeight);
			diag2 = (int)Math.Sqrt(f2.FrameWidth * f2.FrameWidth + f2.FrameHeight * f2.FrameHeight);

			return (diag2 - diag1);
		}

		#region Thread Methods

		protected override void SetupCamera()
		{
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
				SelectedCamera = null;
				Ready = false;
				Thread.Sleep(1000);
				return;
			}
			if (lastCameraCount != cameraCount)
			{
				OnCameraCollectionChanged();
				lastCameraCount = cameraCount;
			}
			if (NCamera == null)
			{
				if (cameraNumber < detectedCameras.Count)
					NCamera = detectedCameras[cameraNumber];
				else
					NCamera = detectedCameras[0];
				Console("Selecting camera: " + NCamera.ID + ". Availiable formats:");
				if (SelectedCamera.IsCapturing) NCamera.StopCapturing();
				// Set default resolution 320x240
				formats = new List<CameraVideoFormat>();
				foreach (CameraVideoFormat f in NCamera.GetAvailableVideoFormats())
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
					NCamera.VideoFormat = formats[0];
					Console("Selected Format: " + NCamera.VideoFormat.FrameWidth.ToString() + " x " + NCamera.VideoFormat.FrameHeight.ToString() + " @" + NCamera.VideoFormat.FrameRate.ToString() + "fps");
				}
				
				OnSelectedCameraChanged();
			}
		}

		protected override void CaptureFrame()
		{
			NImage currentFrame;
			int captureDelay = 16;

			// Do nothing if the parameters of the camera is being changed
			while (settingUpCamera)
			{
				if (SelectedCamera.IsCapturing) SelectedCamera.StopCapturing();
				if (!running) return;
				Thread.Sleep(10);
			}

			if (sleepCapture)
			{
				if (SelectedCamera.IsCapturing)
					SelectedCamera.StopCapturing();
				//capturedImages.Clear();
				while (sleepCapture)
				{
					if (!running) return;
					Thread.Sleep(20);
				}
			}

			// Start capturing if it has not been started
			if (!SelectedCamera.IsCapturing)
			{
				captureDelay = (int)(1000 / NCamera.VideoFormat.FrameRate) - 1;
				NCamera.StartCapturing();
			}
			Thread.Sleep(captureDelay);
			// Get captured image
			currentFrame = NCamera.GetCurrentFrame();
			OnImageCaptured(currentFrame.ToBitmap());
			currentFrame.Dispose();
		}

		#endregion

		#endregion
	}
}
