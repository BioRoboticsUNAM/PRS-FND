using System;
using System.Drawing;
using System.Threading;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Video Camera Adapter
	/// </summary>
	public abstract class CameraAdapter : IImageSource
	{
		#region Variables

		/// <summary>
		/// Asynchronously capture images and stores it into a Queue
		/// </summary>
		private Thread mainThread;

		/// <summary>
		/// Object used for locking lastCapturedImage;
		/// </summary>
		private object oLock;

		/// <summary>
		/// Indicates if main thread is running
		/// </summary>
		protected bool running;

		/// <summary>
		/// Flag that indicates if the adapter is ready.
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

		/// <summary>
		/// Indicates if the capture thread must sleep
		/// </summary>
		private bool sleepCapture;

		/// <summary>
		/// Stores the camera selected for capturing
		/// </summary>
		private ICamera selectedCamera;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of CameraAdapter
		/// </summary>
		public CameraAdapter()
		{
			this.oLock = new Object();
			this.imageCapturedEvent = new AutoResetEvent(false);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when an image is captured
		/// </summary>
		public event ImageCapturedEventHandler ImageCaptured;

		/// <summary>
		/// Occurs when a new image is received and becomes availiable
		/// </summary>
		public event ImageProducedEventHandler ImageProduced;

		/// <summary>
		/// Occurs when the SelectedCamera changes
		/// </summary>
		public event CameraAdapterEventHandler SelectedCameraChanged;

		/// <summary>
		/// Occurs when the CameraAdapter adapter is started
		/// </summary>
		public event CameraAdapterEventHandler Started;

		/// <summary>
		/// Occurs when the CameraAdapter adapter is stopped
		/// </summary>
		public event CameraAdapterEventHandler Stopped;

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
		public abstract VideoFormat[] AvailableVideoFormats { get; }

		/// <summary>
		/// Gets or sets a value indicating if the camera is capturing
		/// </summary>
		public abstract bool IsCapturing{ get; }

		/// <summary>
		/// Gets or sets the last captured image
		/// </summary>
		protected Bitmap LastCapturedImage
		{
			get {
				Bitmap retVal;
				lock (oLock)
				{
					retVal = lastCapturedImage;
					lastCapturedImage = null;
				}
				return retVal;
			}
			set
			{
				lock (oLock)
				{
					if (lastCapturedImage != null)
						lastCapturedImage.Dispose();
					if ((lastCapturedImage = value) != null)
						imageCapturedEvent.Set();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored horizontally
		/// </summary>
		public abstract bool MirrorHorizontal
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored vertically
		/// </summary>
		public abstract bool MirrorVertical
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a value that indicates if the module is ready
		/// </summary>
		public virtual bool Ready
		{
			get { return ready; }
			protected set
			{
				if (this.ready == value) return;
				this.ready = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the the capture thread must sleep
		/// </summary>
		public virtual bool SleepCapture
		{
			get { return sleepCapture; }
			set
			{
				this.sleepCapture = value;
			}
		}

		/// <summary>
		/// Gets the type of the Image Source
		/// </summary>
		public ImageSourceType SourceType { get { return ImageSourceType.Camera; } }

		/// <summary>
		/// Gets or sets the video format used for capturing
		/// </summary>
		public abstract VideoFormat VideoFormat { get; set; }

		/// <summary>
		/// Gets the camera selected for capturing
		/// </summary>
		public ICamera SelectedCamera
		{
			get { return this.selectedCamera; }
			protected set { this.selectedCamera = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the last captured image. if no image is availiable, it blocks the thread call untill an image arrives
		/// </summary>
		/// <returns>An image from the source</returns>
		public Bitmap GetImage()
		{
			if (LastCapturedImage == null)
				imageCapturedEvent.WaitOne();
			return LastCapturedImage;
		}

		/// <summary>
		/// Gets the last captured image. if no image is availiable, it blocks the thread call untill an image arrives or the specified time elapses. If there is a timeout null is returned
		/// </summary>
		/// <param name="timeout">The maximum amount of time to wait for an image</param>
		/// <returns>An image from the source or null if timedout</returns>
		public Bitmap GetImage(int timeout)
		{
			if (lastCapturedImage == null)
				imageCapturedEvent.WaitOne(timeout);
			return LastCapturedImage;
		}

		/// <summary>
		/// Rises the ImageCaptured and ImageProduced events,t and updates the LastCapturedImage variable
		/// </summary>
		/// <param name="capturedImage">The captured image</param>
		protected void OnImageCaptured(Bitmap capturedImage)
		{
			LastCapturedImage = capturedImage;
			if (this.ImageProduced != null)
				this.ImageProduced(this);
			if (this.ImageCaptured != null)
				this.ImageCaptured(this, capturedImage);

		}

		/// <summary>
		/// Rises the SelectedCameraChanged event
		/// </summary>
		protected void OnSelectedCameraChanged()
		{
			if (this.SelectedCameraChanged != null)
				this.SelectedCameraChanged(this);
		}

		/// <summary>
		/// Rises the Stopped event
		/// </summary>
		protected void OnStart()
		{
			if (this.Started != null)
				this.Started(this);
		}

		/// <summary>
		/// Rises the Started event
		/// </summary>
		protected void OnStop()
		{
			if (this.Stopped != null)
				this.Stopped(this);
		}

		/// <summary>
		/// Starts the WebCam adapter
		/// </summary>
		public void Start()
		{
			if (running)
				return;
			running = true;
			if ((mainThread != null) && mainThread.IsAlive)
			{
				mainThread.Abort();
				mainThread.Join(100);
				mainThread = null;
			}
			mainThread = new Thread(new ThreadStart(MainThreadTask));
			mainThread.IsBackground = true;
			mainThread.Start();
		}

		/// <summary>
		/// Stops the  WebCam adapter
		/// </summary>
		public void Stop()
		{
			if (!running || (mainThread == null) || !mainThread.IsAlive)
				return;
			running = false;
			mainThread.Join(100);
			if ((mainThread != null) && mainThread.IsAlive)
			{
				mainThread.Abort();
				mainThread.Join(100);
				mainThread = null;
			}
		}

		protected void Console(string s)
		{
		}

		#region Thread Methods

		/// <summary>
		/// Executes async capture tasks
		/// </summary>
		protected virtual void MainThreadTask()
		{
			running = true;
			OnStart();
			while (running)
			{
				#region Camera verification
				while (running && (selectedCamera == null))
				{
					try
					{
						SetupCamera();
					}
					catch { Thread.Sleep(100); }
					Thread.Sleep(500);
				}
				#endregion

				#region Image Adquisition

				while (running)
				{
					try
					{
						CaptureFrame();
						Ready = true;
					}
					catch (ThreadAbortException)
					{
						//	Console("Error: " + ex.Message);
						if (selectedCamera.IsCapturing) selectedCamera.StopCapturing();
						selectedCamera = null;
						return;
					}
					catch (Exception ex)
					{
						if (running)
						{
							Console("Capture Thread error: " + ex.Message);
							Ready = false;
							if (selectedCamera != null)
							{
								if (selectedCamera.IsCapturing)
									selectedCamera.StopCapturing();
								selectedCamera = null;
							}
							break;
						}
					}
					//Thread.Sleep(captureDelay);
				}
			}
			//EndVideoClient();
				#endregion

			if (selectedCamera != null)
			{
				if (selectedCamera.IsCapturing) selectedCamera.StopCapturing();
				selectedCamera = null;
			}
			OnStop();
		}

		protected abstract void SetupCamera();
		protected abstract void CaptureFrame();

		/*
		protected virtual void SetupCamera()
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
		}

		protected virtual void CaptureFrame()
		{
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
		}
		*/

		#endregion

		#endregion
	}
}
