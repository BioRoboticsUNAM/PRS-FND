using System;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using Robotics;
using Robotics.API;

using Neurotec;
using Neurotec.Cameras;
using Neurotec.Biometrics;
using Neurotec.Images;
using System.Drawing;
using System.Text;

using RecoHuman.Sources;

namespace RecoHuman
{
	/// <summary>
	/// Represents the method that will handle the FacesLoaded, StatusChanged,  event of a HumanRecognizer object
	/// </summary>
	/// <param name="sender">HumanRecognizer object which rises the event</param>
	public delegate void HumanRecognizerEventHandler(HumanRecognizer sender);

	/// <summary>
	/// Represents the method that will handle the HumanNameChanged event of a HumanRecognizer object
	/// </summary>
	/// <param name="sender">HumanRecognizer object which rises the event</param>
	/// <param name="humanName">The new human name used by the engine</param>
	public delegate void HumanNameChangedEventHandler(HumanRecognizer sender, string humanName);

	/// <summary>
	/// Represents the method that will handle the FaceDetected event of a HumanRecognizer object
	/// </summary>
	/// <param name="sender">HumanRecognizer object which rises the event</param>
	/// <param name="faces">The collection of detected faces</param>
	public delegate void FaceDetectedEventHandler(HumanRecognizer sender, FaceCollection faces);

	/// <summary>
	/// A human recognition engine
	/// </summary>
	public class HumanRecognizer
	{
		#region Variables

		/// <summary>
		/// The source of the imags to process
		/// </summary>
		private IImageSource imageSource;
		/// <summary>
		/// Stores the settings for the face recognition
		/// </summary>
		private RecoHumanSettigs settings;
		/// <summary>
		/// Collection of known faces
		/// </summary>
		private FaceCollection knownFaces;
		/// <summary>
		/// Stores the faces detected in last recognition procedure results
		/// </summary>
		private FaceCollection lastDetectedFaces;
		/// <summary>
		/// Stores the last recognition results
		/// </summary>
		private RecognitionResult[] lastRecognitionResult;
		/// <summary>
		/// flag that indicates if the Vision module is busy.
		/// </summary>
		private bool busy;
		/// <summary>
		/// Indicates if the last recognition result belongs to diferent targets or simple face
		/// </summary>
		private bool isLastRecoResultMultiple;
		/// <summary>
		/// Indicates if the last recognition result has a successfull result
		/// </summary>
		private bool lastRecognitionSucceded;
		/// <summary>
		/// Indicates if AutoFind mode is on
		/// </summary>
		private bool autoFind = false;
		/// <summary>
		/// Indicates the name of the person to match and report when autofind is enabled
		/// </summary>
		private string autoFindName = "human";
		
		/// <summary>
		/// Used to prevent cross thread access
		/// </summary>
		private object reBusy = new object();
		/// <summary>
		/// flag that indicates if the Vision module is ready.
		/// </summary>
		private bool ready;
		/// <summary>
		/// Index of selected camera
		/// </summary>
		private int cameraNumber;
		/// <summary>
		/// Stores the time when the last recognition or train operation was performed
		/// </summary>
		private DateTime lastOperationTime;
		/// <summary>
		/// Indicates if the capture thread must sleep
		/// </summary>
		private bool sleepCapture;
		//private List<string> wildcards = new List<string>(new string[] { "human", "humans", "person", "persons", "all", "any" });
		private List<string> wildcards = new List<string>(new string[] { "human", "humans", "all", "any" });

		#region Verilook Vars

		/// <summary>
		/// Stores the Verilook Face Extractor for face recognition
		/// </summary>
		private VLExtractor vlExtractor;

		/// <summary>
		/// Stores the Verilook Matcher user for compare face templates
		/// </summary>
		private VLMatcher vlMatcher;

		#endregion

		#region Thread Variables

		/// <summary>
		/// Indicates if main thread is running
		/// </summary>
		protected bool running;
		/// <summary>
		/// The main thread
		/// </summary>
		protected Thread mainThread;

		#endregion

		#endregion

		#region Constructors

		/// <summary>
		/// Initiates a new instance of FrmRecoHuman
		/// </summary>
		public HumanRecognizer(IImageSource imageSource)
		{
			if (imageSource == null)
				throw new ArgumentNullException();
			this.imageSource = imageSource;
			LoadKnownFaces();
			lastDetectedFaces = new FaceCollection();

			settings = RecoHumanSettigs.Load("Settings.xml");
			if (settings == null) settings = RecoHumanSettigs.Default;

			//capturedImages = new ProducerConsumer<NImage>(10);

			mainThread = new Thread(new ThreadStart(MainThreadTask));
			mainThread.IsBackground = true;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gest or sets a value indicating if AutoFind is enabled
		/// </summary>
		public bool AutoFindHuman
		{
			get { return autoFind; }
			set
			{
				if (autoFind == value) return;
				autoFind = value;

				/*
				if (this.InvokeRequired)
				{
					try
					{
						if (this.IsHandleCreated && !this.Disposing && !this.IsDisposed)
							this.BeginInvoke(new VoidEventHandler(delegate() { chkAutoFind.Checked = value; }));
					}
					catch { }
					return;
				}
				else chkAutoFind.Checked = value;
				*/
			}
		}

		/// <summary>
		/// Gets a value that indicates if the module is busy
		/// </summary>
		public bool Busy
		{
			get { return busy; }
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
		/// Gets a value indicating if the last recognition result belongs to diferent targets or simple face
		/// </summary>
		public bool IsLastRecoResultMultiple
		{
			get { return this.isLastRecoResultMultiple; }
		}

		/// <summary>
		/// Gets a value indicating if the last recognition result has a successfull result
		/// </summary>
		public bool LastRecognitionSucceded
		{
			get { return this.lastRecognitionSucceded; }
		}

		/// <summary>
		/// Gets a value that indicates if the module is ready
		/// </summary>
		public bool Ready
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
		public bool SleepCapture
		{
			get { return sleepCapture; }
			set {
				sleepCapture = value;
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when a face is detected
		/// </summary>
		public event FaceDetectedEventHandler FaceDetected;

		/// <summary>
		/// Occurs when a known faces file is loaded
		/// </summary>
		public event HumanRecognizerEventHandler FacesLoaded;

		/// <summary>
		/// Occurs when the human name used by the engine, changes
		/// </summary>
		public event HumanNameChangedEventHandler HumanNameChanged;

		/// <summary>
		/// Occurs when the HumanRecognizer engine is started
		/// </summary>
		public event HumanRecognizerEventHandler Started;

		/// <summary>
		/// Occurs when the HumanRecognizer engine is stopped
		/// </summary>
		public event HumanRecognizerEventHandler Stopped;

		#endregion

		#region Command Management

		/// <summary>
		/// Parses a received Command
		/// </summary>
		/// <param name="command">Command received</param>
		protected void ParseCommand()
		{
			/*
			switch (1)
			{

				case "pf_auto":
					Console("Received: " + command.ToString());
					CmdAutoFindHuman(command);
					break;

				case "pf_find":
					Console("Received: " + command.ToString());
					#region No params provided
					if (!command.HasParams)
					{
						SendResponse(false, command);
						break;
					}
					#endregion
					FindHuman(command);
					break;

				case "pf_remember":
					Console("Received: " + command.ToString());
					#region No params provided
					if (!command.HasParams)
					{
						SendResponse(false, command);
						break;
					}
					#endregion
					RememberHuman(command);
					break;

				case "pf_shutdown":
					Console("Received: " + command.ToString());
					running = false;
					this.Close();
					break;

				case "pf_sleep":
					Console("Received: " + command.ToString());
					CmdSleep(command);
					break;
			}
			*/
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Rises the FaceDetected event
		/// </summary>
		/// <param name="faces">The collection of detected faces</param>
		public void OnFaceDetected(FaceCollection faces)
		{
			if (this.FaceDetected != null)
				this.FaceDetected(this, faces);
		}

		/// <summary>
		/// Rises the FacesLoaded event
		/// </summary>
		public void OnFacesLoaded()
		{
			if (this.FacesLoaded != null)
				this.FacesLoaded(this);
		}

		/// <summary>
		/// Rises the HumanNameChanged event
		/// </summary>
		/// <param name="humanName">The new human name used by the engine</param>
		public void OnHumanNameChanged(string humanName)
		{
			if (this.HumanNameChanged != null)
				this.HumanNameChanged(this, humanName);
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
		/// Starts the recognition engine
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
		/// Stops the recognition engine
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

		#region Command Execution Methods

		public void SetupAutoFindHuman(bool enable, string name)
		{
			AutoFindHuman = enable;
			autoFindName = name;
		}

		public bool FindHuman(ref string personName, out double hFoV, out double vFoV)
		{
			Face recognizedFace;
			bool result;


			sleepCapture = false;
			lastOperationTime = DateTime.Now;

			hFoV = Double.NaN;
			vFoV = Double.NaN;
			if (busy)
				return false;

			try
			{
				OnHumanNameChanged(personName);
			}
			catch { }
			result = FindHuman(ref personName, out recognizedFace);
			if (result && (recognizedFace != null))
			{
				if (recognizedFace.HasCoords)
				{
					//sbParams.Append(' ');
					//sbParams.Append(recognizedFace.X.ToString("0.000"));
					//sbParams.Append(' ');
					//sbParams.Append(recognizedFace.Y.ToString("0.000"));
					//sbParams.Append(' ');
					//sbParams.Append(recognizedFace.Z.ToString("0.000"));
					hFoV = recognizedFace.HFoV;
					vFoV = recognizedFace.VFoV;
				}
				else if (recognizedFace.HasFOV)
				{
					hFoV = recognizedFace.HFoV;
					vFoV = recognizedFace.VFoV;
				}
			}

			try
			{
				OnHumanNameChanged(personName);
			}
			catch { }
			lastOperationTime = DateTime.Now;
			busy = false;
			return result;
		}

		public bool RememberHuman(string personName, int attempts)
		{
			bool result;


			sleepCapture = false;
			lastOperationTime = DateTime.Now;

			if (busy || (attempts < 1) || (attempts > 100))
				return false;

			try
			{
				OnHumanNameChanged(personName);
			}
			catch { }
			result = false;
			for (int i = 0; i < attempts; ++i)
			{
				if (result = RememberHuman(personName))
					break;
			}
			lastOperationTime = DateTime.Now;
			busy = false;
			return result;
		}

		protected void Console(string text)
		{
		}

		#endregion

		#region Core Methods

		/// <summary>
		/// Finds a human
		/// </summary>
		/// <param name="person">Name of the person to find out</param>
		/// <returns>true if person found. false otherwise</returns>
		private bool FindHuman(ref string person)
		{
			Face result;
			return FindHuman(ref person, out result);
		}

		/// <summary>
		/// Finds a human
		/// </summary>
		/// <param name="person">Name of the person to find out</param>
		/// <param name="attempts">The maximum number of attempts while matching</param>
		/// <param name="result">The Face object that represents the detected face if any</param>
		/// <returns>true if person found. false otherwise</returns>
		private bool FindHuman(ref string person, out Face result)
		{
			return FindHuman(ref person, wildcards.Contains(person) ? settings.MatchingAttempts : 1, out result);
		}

		/// <summary>
		/// Finds a human
		/// </summary>
		/// <param name="person">Name of the person to find out</param>
		/// <param name="maxAttempts">The maximum number of attempts while matching</param>
		/// <param name="result">The Face object that represents the detected face if any</param>
		/// <returns>true if person found. false otherwise</returns>
		private bool FindHuman(ref string person, int maxAttempts, out Face result)
		{
			// Count for match attempts
			int attempts = 0;
			sleepCapture = false;
			lastOperationTime = DateTime.Now;
			// Detected faces. Used for avoiding cross thread access
			FaceCollection detectedFaces;
			//NImage image;
			result = null;

			for (attempts = 0; attempts < maxAttempts; ++attempts)
			{
				// Realize recognition
				Detect(out detectedFaces);

				// Check if I'm looking someonw
				if (detectedFaces.Count < 1) continue;

				// Check who I am looking at
				for (int i = 0; i < detectedFaces.Count; ++i)
				{
					if (detectedFaces[i] == null) continue;

					// If no specific person requested
					if (wildcards.Contains(person))
					{
						// If detected person is known, then return it.
						if (detectedFaces[i].Name != "unknown")
						{
							person = detectedFaces[i].Name;
							result = detectedFaces[i];
							return true;
						}
						// Else, three attempts to try to guess who is her.
						else
						{
							Console("Detected unknown. Trying to identify.");
							FaceCollection df;
							for (int whoCount = 1; whoCount <= 3; ++whoCount)
							{
								// Detect again
								Console("\tAttempt " + whoCount.ToString() + " of 3");
								Detect(out df);
								if (df.Count < 1) continue;
								for (int j = 0; j < df.Count; ++j)
								{
									// If detection successfull and person is known; done!
									if ((df[j] != null) && (df[j].Name != "unknown"))
									{
										person = df[j].Name;
										result = df[j];
										Console("\t\tFound " + person);
										return true;
									}
								}
							}
							person = detectedFaces[i].Name;
							result = detectedFaces[i];
							return true;
						}
					}
					// If specific person is requested
					else if (detectedFaces[i].Name == person)
						return true;
				}
				// I didnt find any named person and requested for any. I return unknown
				//if ((person == "person") || (person == "persons"))
				if (wildcards.Contains(person))
				//if (person == "human")
				{
					if (detectedFaces[0] == null) return false;
					person = detectedFaces[0].Name;
					result = detectedFaces[0];
					return true;
				}
			}

			// I didnt find any person
			return false;
		}

		#region Recognition Methods

		/// <summary>
		/// Recognizes a single face from a single image frame
		/// </summary>
		/// <param name="image">Neurotec Image in which is based the face recognition</param>
		/// <returns>The best match in all known faces. Null if not found</returns>
		private RecognitionResult RecognizeFace(NImage image)
		{
			RecognitionResult[] recognitionResults;
			Face face = null;
			return RecognizeFace(image, ref face, out recognitionResults);
		}

		/// <summary>
		/// Recognizes a single face from a single image frame
		/// </summary>
		/// <param name="image">Neurotec Image in which is based the face recognition</param>
		/// <param name="vlFace">Face detected in which region is the face to recognize</param>
		/// <returns>The best match in all known faces. Null if not found</returns>
		private RecognitionResult RecognizeFace(NImage image, VleFace vlFace)
		{
			RecognitionResult[] recognitionResults;
			Face face;
			return RecognizeFace(image, vlFace, out face, out recognitionResults);
		}

		/// <summary>
		/// Recognizes a single face from a single image frame
		/// </summary>
		/// <param name="image">Neurotec Image in which is based the face recognition</param>
		/// <param name="vlFace">Face detected in which region is the face to recognize</param>
		/// <param name="face">Face object used to realize a recognition</param>
		/// <param name="recognitionResults">An array containing all recognition results for the recognized face</param>
		/// <returns>The best match in all known faces. Null if not found</returns>
		private RecognitionResult RecognizeFace(NImage image, VleFace vlFace, out Face face, out RecognitionResult[] recognitionResults)
		{
			// Bitmap to draw in the detected face region
			Bitmap croppedBitmap;
			// Graphics used to copy the face detected region
			Graphics g;
			// Rectangle used to copy the scaled region of face detected
			Rectangle rect;
			// The recognition result obtained
			RecognitionResult result;

			// Get a rectangle a bit larger than the one the face has been recognized.
			// Its because some times in the exact area of the face the face cannot be recognized again
			//rect = new Rectangle(vlFace.Rectangle.X - 50, vlFace.Rectangle.Y - 50, vlFace.Rectangle.Width + 100, vlFace.Rectangle.Height + 100);
			rect = new Rectangle(vlFace.Rectangle.X - vlFace.Rectangle.Width / 2, vlFace.Rectangle.Y - vlFace.Rectangle.Height / 2, vlFace.Rectangle.Width * 2, vlFace.Rectangle.Height * 2);
			croppedBitmap = new Bitmap(rect.Width, rect.Height);
			g = Graphics.FromImage(croppedBitmap);
			g.DrawImage(image.ToBitmap(), 0, 0, rect, GraphicsUnit.Pixel);
			face = new Face(vlFace);
			result = RecognizeFace(NImage.FromBitmap(croppedBitmap), ref face, out recognitionResults);

			return result;
		}

		/// <summary>
		/// Recognizes a single face from a single image frame
		/// </summary>
		/// <param name="image">Neurotec Image in which is based the face recognition</param>
		/// <param name="face">Face object used to realize a recognition</param>
		/// <param name="recognitionResults">An array containing all recognition results for the recognized face</param>
		/// <returns>The best match in all known faces. Null if not found</returns>
		private RecognitionResult RecognizeFace(NImage image, ref Face face, out RecognitionResult[] recognitionResults)
		{
			// Nurotec Image required in the process of recognize the face
			NGrayscaleImage gray;
			// The face features result of a face recognition
			byte[] features;
			// Verilook Detetion Details as result of face recognition
			VleDetectionDetails detectionDetails;
			// Stores the face object to realize a recognition
			//Face face;

			// Get gray image for face detection
			gray = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);

			// Extract the face and extract its template
			UseResources();
			features = vlExtractor.Extract(gray, out detectionDetails);
			ReleaseResources();
			gray.Dispose();
			recognitionResults = null;
			//if ((features == null) || (!detectionDetails.FaceAvailable)) return null;
			if (!detectionDetails.FaceAvailable)
			{
				face = null;
				return null;
			}
			if (face == null)
				face = new Face(features, detectionDetails, image.ToBitmap());
			else
			{
				face.SetRecognitionData(features, detectionDetails, image.ToBitmap());
				Console("found face: location = (" + detectionDetails.Face.Rectangle.X + ", " + detectionDetails.Face.Rectangle.Y + "), width = " + detectionDetails.Face.Rectangle.Width + ", height = " + detectionDetails.Face.Rectangle.Height + ", confidence = " + detectionDetails.Face.Confidence);
			}
			if (features == null)
				return null;

			return Recognize(face, out recognitionResults);

			//if (!detectionDetails.FaceAvailable) return null;
			//return new Face(features, detectionDetails, image.ToBitmap());
		}

		/// <summary>
		/// Recognizes multiple faces from a single image frame
		/// </summary>
		/// <param name="image">Neurotec Image in which is based the face recognition</param>
		/// <param name="vlFaces">Array of faces detected</param>
		/// <returns>An array containing best match in all known faces.</returns>
		private RecognitionResult[] RecognizeMultipleFaces(NImage image, VleFace[] vlFaces)
		{
			RecognitionResult[][] MultipleRecognitionResults;
			FaceCollection recognizedFaces;
			return RecognizeMultipleFaces(image, vlFaces, out recognizedFaces, out MultipleRecognitionResults);
		}

		/// <summary>
		/// Recognizes multiple faces from a single image frame
		/// </summary>
		/// <param name="image">Neurotec Image in which is based the face recognition</param>
		/// <param name="vlFaces">Array of faces detected</param>
		/// <param name="recognizedFaces">Array of Face objects used to realize each recognition</param>
		/// <param name="MultipleRecognitionResults">An array containing all recognition results for each recognized face</param>
		/// <returns>An array containing best match in all known faces.</returns>
		private RecognitionResult[] RecognizeMultipleFaces(NImage image, VleFace[] vlFaces, out FaceCollection detectedFaces, out RecognitionResult[][] MultipleRecognitionResults)
		{
			#region Variables
			// Stores the original image as bitmap
			Bitmap bmp;
			// Bitmap to draw in the detected face region
			Bitmap croppedBitmap;
			// Graphics used to copy the face detected region
			Graphics g;
			// Rectangle used to copy the scaled region of face detected
			Rectangle rect;
			// Nurotec Image required in the process of recognize the face detected region
			NGrayscaleImage gray;
			// Verilook Detetion Details as result of face recognition
			VleDetectionDetails detectionDetails;
			// The face template result of a face recognition
			byte[][] templates = new byte[vlFaces.Length][];
			// The face features result of a face recognition
			byte[] features;
			// Stores the current recognition face
			Face currentFace;
			// Stores the recognized faces
			//FaceCollection recognizedFaces = new FaceCollection(vlFaces.Length);
			detectedFaces = new FaceCollection(vlFaces.Length);
			// Stores the best recognition result for current face
			RecognitionResult currentResult;
			// Stores the recognition results for current face
			RecognitionResult[] currentRecognitionResults;
			// Stores all Recognition results
			List<RecognitionResult[]> recognitionResults = new List<RecognitionResult[]>();
			// Stores the best recognition result matches
			List<RecognitionResult> selectedResults = new List<RecognitionResult>();
			#endregion

			// Get the original image as bitmap
			bmp = new Bitmap(image.ToBitmap());

			// Extract each face, and get its template
			foreach (VleFace vlFace in vlFaces)
			{
				// Get a rectangle a bit larger than the one the face has been recognized.
				// Its because some times in the exact area of the face the face cannot be recognized again
				//rect = new Rectangle(vlFace.Rectangle.X - 50, vlFace.Rectangle.Y - 50, vlFace.Rectangle.Width + 100, vlFace.Rectangle.Height + 100);
				rect = new Rectangle(vlFace.Rectangle.X - vlFace.Rectangle.Width / 2, vlFace.Rectangle.Y - vlFace.Rectangle.Height / 2, vlFace.Rectangle.Width * 2, vlFace.Rectangle.Height * 2);
				// Get the face bitmap
				croppedBitmap = new Bitmap(rect.Width, rect.Height);
				g = Graphics.FromImage(croppedBitmap);
				g.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
				// Get gray image for face detection
				gray = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, NImage.FromBitmap(croppedBitmap));

				// Extract the face and extract its template
				currentFace = new Face(vlFace);
				features = vlExtractor.Extract(gray, out detectionDetails);
				if (!detectionDetails.FaceAvailable) continue;
				UseResources();
				currentFace.SetRecognitionData(features, detectionDetails, croppedBitmap);
				ReleaseResources();
				currentFace.CalculateFovAndCoords((int)image.Width, (int)image.Height);
				detectedFaces.Add(currentFace);
				Console("Found face: location = (" + detectionDetails.Face.Rectangle.X + ", " + detectionDetails.Face.Rectangle.Y + "), width = " + detectionDetails.Face.Rectangle.Width + ", height = " + detectionDetails.Face.Rectangle.Height + ", confidence = " + detectionDetails.Face.Confidence);

				try
				{
					croppedBitmap.Dispose();
					g.Dispose();
					gray.Dispose();
				}
				catch { }

			}
			if (detectedFaces.Count > 0) Console(detectedFaces.Count.ToString() + " faces found.");
			if (knownFaces.Count > 0)
			{
				Console("Initializing recognition");
				// Recognize each detected face
				for (int i = 0; i < detectedFaces.Count; ++i)
				{
					if (detectedFaces[i].Features == null) continue;
					currentFace = detectedFaces[i];

					// Start recognition
					currentResult = Recognize(currentFace, out currentRecognitionResults);
					if (currentResult == null) continue;
					selectedResults.Add(currentResult);
					recognitionResults.Add(currentRecognitionResults);
				}
			}

			MultipleRecognitionResults = recognitionResults.ToArray();
			return selectedResults.ToArray();
		}

		/// <summary>
		/// Performs a face recognition based in provided features
		/// </summary>
		/// <param name="features">Face to match with</param>
		/// <param name="recognitionResults">An array containing all the recognition results</param>
		/// <returns>The best match in all known faces. Null if not found</returns>
		private RecognitionResult Recognize(Face face, out RecognitionResult[] recognitionResults)
		{
			recognitionResults = null;
			if (knownFaces.Count < 1) return null;

			// Stores current match result
			double matchResult = -1;
			// Stores highest match result
			double highestMatchResult = -1;
			// The index of the highestMatchResult owner face 
			int highestMatchResultOwner = -1;
			// List of recognition results
			List<RecognitionResult> results = new List<RecognitionResult>(knownFaces.Count);

			UseResources();

			vlMatcher.MatchingThreshold = settings.MatchingThreshold;
			vlMatcher.IdentifyStart(face.Features);
			for (int i = 0; i < knownFaces.Count; ++i)
			{
				matchResult = vlMatcher.IdentifyNext(knownFaces[i].CompressedFeatures);
				results.Add(new RecognitionResult(face, knownFaces[i], matchResult));
				if ((matchResult >= vlMatcher.MatchingThreshold) && (matchResult > highestMatchResult))
				{
					highestMatchResult = matchResult;
					highestMatchResultOwner = i;
				}
			}
			vlMatcher.IdentifyEnd();

			ReleaseResources();

			if (highestMatchResultOwner == -1) return null;
			results.Sort();
			for (int i = 0; i < results.Count; ++i)
				results[i].BaseFace.Name = results[0].ComparedFace.Name;
			recognitionResults = results.ToArray();
			//return new RecognitionResult(face, knownFaces[highestMatchResultOwner], highestMatchResult);
			return recognitionResults[0];
		}

		/// <summary>
		/// Performs a face recognition based in provided features
		/// </summary>
		/// <param name="features">Face features to match with</param>
		/// <returns>The best match in all known faces. Null if not found</returns>
		private Face Recognize(byte[] features)
		{
			if (knownFaces.Count < 1) return null;

			// Stores current match result
			double matchResult = -1;
			// Stores highest match result
			double highestMatchResult = -1;
			// The index of the highestMatchResult owner face 
			int highestMatchResultOwner = -1;

			UseResources();

			vlMatcher.MatchingThreshold = settings.MatchingThreshold;
			vlMatcher.IdentifyStart(features);
			for (int i = 0; i < knownFaces.Count; ++i)
			{
				matchResult = vlMatcher.IdentifyNext(knownFaces[i].Features);
				if ((matchResult >= vlMatcher.MatchingThreshold) && (matchResult > highestMatchResult))
				{
					highestMatchResult = matchResult;
					highestMatchResultOwner = i;
				}
			}
			vlMatcher.IdentifyEnd();

			ReleaseResources();

			if (highestMatchResultOwner == -1) return null;
			return knownFaces[highestMatchResultOwner];
		}

		void test()
		{
			/*
			VleFace[] faces;
			NGrayscaleImage grayImage;

			try
			{
				Bitmap[] image = new Bitmap[]
					{
						(Bitmap)Bitmap.FromFile(@"C:\Documents and Settings\Mau\My Documents\My Pictures\Lenna.jpg"),
						(Bitmap)Bitmap.FromFile(@"C:\Documents and Settings\Mau\My Documents\My Pictures\FR1_01.jpg"),
						(Bitmap)Bitmap.FromFile(@"C:\Documents and Settings\Mau\My Documents\My Pictures\FR2_03.jpg"),
						(Bitmap)Bitmap.FromFile(@"C:\Documents and Settings\Mau\My Documents\My Pictures\FR3_06.jpg"),
						(Bitmap)Bitmap.FromFile(@"C:\Documents and Settings\Mau\My Documents\My Pictures\FR4_08.jpg"),
						(Bitmap)Bitmap.FromFile(@"C:\Documents and Settings\Mau\My Documents\My Pictures\FR5_08.jpg")
					};
				DateTime startTime;
				DateTime stopTime;
				TimeSpan diff;
				double size;
				double rate;

				for (int i = 0; i < image.Length; ++i)
				{
					//if (image[i].Width > 1024)
					//	image[i] = new Bitmap(image[i], 1024, image[i].Height * 1024 / image[i].Width);
					//if(image[i].Height > 1024)
					//	image[i] = new Bitmap(image[i], image[i].Width * 1024 / image[i].Height, 1024);

					startTime = DateTime.Now;
					grayImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, NImage.FromBitmap(image[i]));
					faces = vlExtractor.DetectFaces(grayImage);
					stopTime = DateTime.Now;
					diff = stopTime.Subtract(startTime);
					size = image[i].Width * image[i].Height;
					size = size / 1048576.0;
					rate = 1048576.0 * size / diff.TotalMilliseconds;
					Console("Image " + i.ToString() + ": Recognized " + faces.Length.ToString() + " in " + diff.TotalMilliseconds.ToString() + "msec (" + diff.Ticks.ToString() + "ticks).  Size: " + size.ToString("0.00") + " Rate: " + rate.ToString("0.00"));
					RecognizeMultipleFaces(NImage.FromBitmap(image[i]), faces);
				}
			}
			catch { }

			running = false;
			*/
		}

		void foo()
		{
			/*
			if (!VLExtractor.IsRegistered) return;

			VleFace[] faces;
			NImage image;
			NGrayscaleImage grayImage;
			VleDetectionDetails details;

			image = capturedImages.Consume();
			grayImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);

			try
			{
				faces = vlExtractor.DetectFaces(grayImage);
				//vlExtractor.ExtractStart(settings.AttemptsWhileMatching);
				//vlExtractor.ExtractNext()
				byte[] data = vlExtractor.Extract(grayImage, out details);
				if (details.FaceAvailable)
				{
					Console("found face: location = (" + details.Face.Rectangle.X + ", " + details.Face.Rectangle.Y + "), width = " + details.Face.Rectangle.Width + ", height = " + details.Face.Rectangle.Height + ", confidence = " + details.Face.Confidence);
				}

				//if ((faces != null) && (faces.Length > 0))
				//	MessageBox.Show("Yeah!");
				if (!this.Disposing && !this.IsDisposed)
					this.BeginInvoke(dlgRecognitionResultUpdate, new object[] { new Bitmap(image.ToBitmap()), faces, details });
			}
			catch (NeurotecException ex)
			{
				MessageBox.Show(ex.Message + ". StackTrace: " + ex.StackTrace.ToString());
			}

			grayImage.Dispose();
			image.Dispose();
			*/
		}

		#endregion

		#endregion

		#region Thread Methods

		/// <summary>
		/// Executes async tasks
		/// </summary>
		protected void MainThreadTask()
		{
			int i = 0;
			running = true;
			ready = false;
			Console("RecoHuman 2.0: Main thread started");

			Console("Initializing VeriLook");
			InitVeriLook();
			Console("Done!");
			OnStart();

			lastOperationTime = DateTime.Now;
			Ready = running && VLExtractor.IsRegistered && VLMatcher.IsRegistered;
			while (running)
			{
				if (autoFind && (i++ >= 25))
				{
					AutoFind();
					lastOperationTime = DateTime.Now;
					i = 0;
				}

				//if (!sleepCapture && (((TimeSpan)DateTime.Now.Subtract(lastOperationTime)).TotalMinutes >= 2))
				//	sleepCapture = true;

				Thread.Sleep(10);
			}
			//if (capturedImages != null)
			//	capturedImages.Clear();
			if (vlExtractor != null)
				vlExtractor.Dispose();
			if (vlMatcher != null)
				vlMatcher.Dispose();

			OnStop();
		}

		/// <summary>
		///  Waits for Verilook resources to be free, and takes control of it
		/// </summary>
		private void UseResources()
		{
			//// Wait for free resources
			//while (reBusy)
			//    Thread.Sleep(1);
			//// Mark to use resources
			//reBusy = true;
			Monitor.Enter(reBusy);
		}

		/// <summary>
		/// Release Verilook resources
		/// </summary>
		private void ReleaseResources()
		{
			//// Free resources
			//reBusy = false;
			Monitor.Pulse(reBusy);
			Monitor.Exit(reBusy);
		}

		#endregion

		#region Detection and Enrollment methods

		/// <summary>
		/// Auto finds human
		/// </summary>
		private void AutoFind()
		{
			// Stores the detected faces. Used to prefecn cross thread access
			FaceCollection detectedFaces;

			Detect();
			if (lastDetectedFaces.Count < 1) return;
			detectedFaces = lastDetectedFaces;
			OnFaceDetected(detectedFaces);
		}

		/// <summary>
		/// Analyzes current frame for availiable faces.
		/// If faces are found it tries to find a match record for each face;
		/// </summary>
		protected bool Detect()
		{
			FaceCollection fc;
			return Detect(out fc);
		}

		/// <summary>
		/// Analyzes current frame for availiable faces.
		/// If faces are found it tries to find a match record for each face;
		/// </summary>
		/// <param name="detectedFaces">Collection of detected faces</param>
		protected bool Detect(out FaceCollection detectedFaces)
		{
			Bitmap bmp;
			detectedFaces = new FaceCollection();
			if (!VLExtractor.IsRegistered) return false;
			if (!VLMatcher.IsRegistered) return false;
			sleepCapture = false;
			//NImage image = capturedImages.Consume();
			if ((bmp = imageSource.GetImage(100)) == null)
				return false;
			NImage image = NImage.FromBitmap(bmp);
			if (image == null)
				return false;

			VleFace[] vlFaces;
			NGrayscaleImage grayImage;
			// Stores the succeded recognition result for the single face detected
			RecognitionResult sRecoResult;
			// Stores all the recognition result (successfull and failed) for the single face detected
			RecognitionResult[] sRecoResults;
			// Stores the succeded recognition result for all the detected faces
			RecognitionResult[] mRecoResult;
			// Stores all the recognition result (successfull and failed) for all the detected faces
			RecognitionResult[][] mRecoResults;
			// Image recognition result
			Bitmap lciBmp;

			// Get gray image for face detection
			try
			{
				grayImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);
			}
			catch
			{
				image.Dispose();
				return false;
			}

			UseResources();

			// Find all faces in frame
			vlFaces = vlExtractor.DetectFaces(grayImage);

			ReleaseResources();

			#region No faces detected

			if ((vlFaces == null) || (vlFaces.Length < 1))
			{
				grayImage.Dispose();
				image.Dispose();
				this.lastDetectedFaces.Clear();
				return false;
			}

			#endregion

			lciBmp = image.ToBitmap();
			// RecognitionResultUpdate(lciBmp, vlFaces);

			#region Only one face detected

			if (vlFaces.Length == 1)
			{
				Face detectedFace;
				sRecoResult = RecognizeFace(image, vlFaces[0], out detectedFace, out sRecoResults);
				if (detectedFace == null)
				{
					RecognitionResultUpdate(lciBmp, vlFaces);
					image.Dispose();
					grayImage.Dispose();
					return false;
				}
				detectedFace.CalculateFovAndCoords((int)image.Width, (int)image.Height);
				RecognitionResultUpdate(lciBmp, detectedFace);
				detectedFaces.Add(detectedFace);
				if (sRecoResult == null)
				{
					isLastRecoResultMultiple = false;
					lastRecognitionSucceded = false;
					lastRecognitionResult = null;
					ShowDetectionResults(lastDetectedFaces);
				}
				else
				{
					isLastRecoResultMultiple = false;
					lastRecognitionSucceded = true;
					lastRecognitionResult = null;
					ShowRecognitionResults(sRecoResults);
				}
			}
			#endregion

			#region Multiple faces detected
			else
			{
				// Array of detected face objects used during recognition
				mRecoResult = RecognizeMultipleFaces(image, vlFaces, out detectedFaces, out mRecoResults);
				if ((mRecoResult == null) || (mRecoResult.Length < 1))
				{
					isLastRecoResultMultiple = true;
					lastRecognitionSucceded = false;
					lastRecognitionResult = null;
					ShowDetectionResults(detectedFaces);
					//ShowRecognitionResults(mRecoResult);
				}
				else
				{
					isLastRecoResultMultiple = true;
					lastRecognitionSucceded = true;
					lastRecognitionResult = mRecoResult;
					ShowRecognitionResults(mRecoResult);
				}
			}
			#endregion
			this.lastDetectedFaces = detectedFaces;
			image.Dispose();
			grayImage.Dispose();
			return true;
		}

		/// <summary>
		/// Memorizes a human face
		/// </summary>
		/// <param name="person">Name to asign the recognized human</param>
		/// <returns>True if a person was detected and name could be stores. False otherwise</returns>
		private bool RememberHuman(string person)
		{
			// Captured image
			NImage image = null;
			// Grayscale image used for face recognition
			NGrayscaleImage gray = null;
			// Copy of captured image bitmap
			Bitmap bitmap;
			// Variable used to not wait forever for an image 
			int count = 0;
			// attempts while trying to enroll
			int attempts = 0;
			// Extraction has succeded
			bool extractionSucceded = false;
			// Stores the detection details of detected face
			VleDetectionDetails details = new VleDetectionDetails();
			// Stores the features of detected face
			byte[] features = null;
			// Stores detected and recognized face
			Face face = null;


			sleepCapture = false;
			lastOperationTime = DateTime.Now;
			UseResources();

			// Start enrolling
			vlExtractor.ExtractStart(settings.AttemptsWhileEnrolling);
			// Attemp counter
			for (attempts = 0; running && (attempts < settings.AttemptsWhileEnrolling); ++attempts)
			{
				// Convert image to a gray one
				if (image != null) image.Dispose();
				if (gray != null) gray.Dispose();
				image = NImage.FromBitmap(imageSource.GetImage(100));
				if (image == null) continue;
				gray = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);

				// Extract face details (if any)
				try
				{
					extractionSucceded = vlExtractor.ExtractNext(gray, out details, out features);
				}
				catch { }
				// Check if we have all data (extraction succeded)
				//if (extractionSucceded && (details != null) && (details.FaceAvailable != null) && (features != null) && (features.Length > 0))
				if (extractionSucceded && !details.Equals(null) && details.FaceAvailable && (features != null) && (features.Length > 0))
				{
					// Create the face
					face = new Face(features, details);
					break;
				}
			}
			vlExtractor.Reset();
			ReleaseResources();

			// If there is no face, enrollment failed
			if (face == null)
			{
				if (image != null) image.Dispose();
				if (gray != null) gray.Dispose();
				return false;
			}

			// Update video output
			bitmap = image.ToBitmap();
			RecognitionResultUpdate(bitmap, face);

			// Set the image and name

			// Bitmap to draw in the detected face region
			Bitmap croppedBitmap;
			// Graphics used to copy the face detected region
			Graphics g;
			// Rectangle used to copy the scaled region of face detected
			Rectangle rect;

			// Get a rectangle a bit larger than the one the face has been recognized.
			// Its because some times in the exact area of the face the face cannot be recognized again
			rect = new Rectangle(face.VlFace.Rectangle.X - face.VlFace.Rectangle.Width / 2, face.VlFace.Rectangle.Y - face.VlFace.Rectangle.Height / 2, face.VlFace.Rectangle.Width * 2, face.VlFace.Rectangle.Height * 2);
			croppedBitmap = new Bitmap(rect.Width, rect.Height);
			g = Graphics.FromImage(croppedBitmap);
			bitmap = image.ToBitmap();
			g.DrawImage(bitmap, 0, 0, rect, GraphicsUnit.Pixel);
			face.SetBitmap(croppedBitmap);
			face.Name = person;

			// Register and save db
			knownFaces.Add(face);
			SaveKnownFaces();
			LoadKnownFaces();
			if (image != null) image.Dispose();
			if (gray != null) gray.Dispose();
			return true;
		}

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
			OnFacesLoaded();
		}

		/// <summary>
		/// Initialize Verilook environment variables
		/// </summary>
		private void InitVeriLook()
		{
			bool ready = true;

			// Setup variables
			try
			{
				//vlDetectionDetails = new VleDetectionDetails();
				vlExtractor = new VLExtractor();
				vlMatcher = new VLMatcher();
			}
			catch (NeurotecException ex)
			{
				ready = false;
				Console(ex.Message + ". StackTrace: " + ex.StackTrace.ToString());
			}

			if ((vlExtractor == null) && (vlMatcher == null))
			{
				ready = false;
				return;
			}

			Console("VLExtractor " + (VLExtractor.IsRegistered ? "is" : "IS NOT") + " registered");
			Console("VLMatcher " + (VLMatcher.IsRegistered ? "is" : "IS NOT") + " registered");

			if (!VLExtractor.IsRegistered || !VLMatcher.IsRegistered)
			{
				ready = false;
			}

			// Configure verilook parameters
			#region Configure verilook parameters

			// Set IOD
			vlExtractor.MinIod = settings.MinimalInterOcularDistance;
			vlExtractor.MaxIod = settings.MaximumInterOcularDistance;

			// Set Generalization Threshold
			vlExtractor.GeneralizationThreshold = settings.GeneralizationThreshold;

			// Set Matching Threshold
			vlMatcher.MatchingThreshold = settings.MatchingThreshold;

			#endregion

			this.ready = ready;
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
			OnFacesLoaded();
		}


		/// <summary>
		/// Updates the output video control
		/// </summary>
		/// <param name="image">Base Image</param>
		/// <param name="faces">List of faces detected</param>
		private void RecognitionResultUpdate(Bitmap image, VleFace[] faces)
		{
			/*
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
			*/
		}

		/// <summary>
		/// Updates the output video control
		/// </summary>
		/// <param name="image">Base Image</param>
		/// <param name="faces">List of faces detected</param>
		private void RecognitionResultUpdate(Bitmap image, Face face)
		{
			/*
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
			*/
		}

		/// <summary>
		/// Shows the recognition results in the recognition results pannel
		/// </summary>
		/// <param name="recognitionResults">Array of recognition results to display</param>
		public void ShowRecognitionResults(RecognitionResult[] recognitionResults)
		{
			/*
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
			*/
		}

		/// <summary>
		/// Shows the detected faces in the recognition results pannel
		/// </summary>
		/// <param name="detectedFaces">Array of detected faces to display</param>
		public void ShowDetectionResults(Face[] detectedFaces)
		{
			/*
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
			*/
		}

		/*

		

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

		

		*/

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
		/// Updates the settings controls
		/// </summary>
		private void UpdateSettings()
		{
			//settingsPannel.UpdateSettings();
			throw new NotImplementedException();
		}

		#endregion

		#endregion

	}
}
