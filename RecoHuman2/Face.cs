using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;


using Neurotec.Biometrics;
using Neurotec.Images;

namespace RecoHuman
{
	/// <summary>
	/// Represents a recognized face
	/// </summary>
	[Serializable]
	public class Face
	{
		#region Variables

		#region Required Variables

		/// <summary>
		/// Stores the features template that identifies this face
		/// </summary>
		private byte[] features;
		/// <summary>
		/// Stores the features template (compressed) that identifies this face
		/// </summary>
		private byte[] compressedFeatures;
		/// <summary>
		/// Stores the Verilook detection details obtained as result of a face recognition
		/// </summary>
		[NonSerialized]
		private VleDetectionDetails detectionDetails;
		/// <summary>
		/// Verilook extractor used for compress features
		/// </summary>
		[NonSerialized]
		public static VLExtractor Extractor = new VLExtractor();

		#endregion

		#region Optional Variables
		
		/// <summary>
		/// Stores the unique ID of the face object, used for database storage
		/// </summary>
		private int id;
		/// <summary>
		/// Stores the name asociated to the face
		/// </summary>
		private string name;
		/// <summary>
		/// Stores the original bitmap in which recognition was based
		/// </summary>
		private Bitmap originalBitmap;
		/// <summary>
		/// Stores the Neurotec Grayscale Image used for recognition
		/// </summary>
		[NonSerialized]
		private NGrayscaleImage recognitionImage;
		/// <summary>
		/// Stores the Verilook VleFace obtained as result of face detection
		/// </summary>
		[NonSerialized]
		private VleFace vlFace;

		/// <summary>
		/// Stores the vertical field of view
		/// The angle is proportional to the centroid of the detected face respect to the ceter of the image.
		/// The vFOV for the entire image height is 0.7330 radians (42 degrees)
		/// The angle is provided in radians
		/// </summary>
		[NonSerialized]
		private double vFov;

		/// <summary>
		/// Stores the horizontal field of view
		/// The angle is proportional to the centroid of the detected face respect to the ceter of the image.
		/// The hFOV for the entire image width is 0.9774 radians (56 degrees)
		/// The angle is provided in radians
		/// </summary>
		[NonSerialized]
		private double hFov;

		/// <summary>
		/// Indicates if hFOV and vFOV has been calculated
		/// </summary>
		[NonSerialized]
		private bool hasFOV = false;

		/// <summary>
		/// Stores the aproximated x position of the detected face respect to the ceter of the image (camera lens)
		/// This value is calculated using an estimated distance of 6cm between the eyes
		/// The distance is provided in meters
		/// </summary>
		[NonSerialized]
		private double x = 0;

		/// <summary>
		/// Stores the aproximated y position of the detected face respect to the ceter of the image (camera lens)
		/// This value is calculated using an estimated distance of 6cm between the eyes
		/// The distance is provided in meters
		/// </summary>
		[NonSerialized]
		private double y = 0;

		/// <summary>
		/// Stores the aproximated z position of the detected face respect to the ceter of the image (camera lens)
		/// This value is calculated using an estimated distance of 6cm between the eyes
		/// The distance is provided in meters
		/// </summary>
		[NonSerialized]
		private double z = 0;

		/// <summary>
		/// Indicates if x, y, z coordinates has been calculated
		/// </summary>
		[NonSerialized]
		private bool hasCoords = false;

		#endregion

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="vlFace">The Verilook VleFace obtained as result of face detection</param>
		internal Face(VleFace vlFace)
		{
			this.vlFace = vlFace;
			this.id = -1;
			this.name = "unknown";
			this.originalBitmap = null;
			this.recognitionImage = null;
			this.features = null;
			this.hFov = 0;
			this.vFov = 0;
		}

		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		public Face(byte[] features, VleDetectionDetails detectionDetails)
		{
			this.id = -1;
			this.name = "unknown";
			this.originalBitmap = null;
			this.recognitionImage = null;
			this.detectionDetails = detectionDetails;
			this.vlFace = detectionDetails.Face;
			this.features = new byte[features.Length];
			Array.Copy(features, this.features, features.Length);
			this.compressedFeatures = Extractor.Compress(features);
			this.hFov = 0;
			this.vFov = 0;
		}

		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		/// <param name="originalBitmap">The original bitmap in which recognition was based</param>
		public Face(byte[] features, VleDetectionDetails detectionDetails, Bitmap originalBitmap)
			: this(features, detectionDetails)
		{
			this.originalBitmap = (Bitmap)new Bitmap(originalBitmap);
		}

		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		/// <param name="originalBitmap">The original bitmap in which recognition was based</param>
		/// <param name="vlFace">The Verilook VleFace obtained as result of face detection</param>
		public Face(byte[] features, VleDetectionDetails detectionDetails, Bitmap originalBitmap, VleFace vlFace)
			: this(features, detectionDetails, originalBitmap)
		{
			this.vlFace = vlFace;
		}
		
		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="name">Name asociated to the face</param>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		public Face(string name, byte[] features, VleDetectionDetails detectionDetails)
			: this(features, detectionDetails)
		{
			this.name = name;
		}

		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="name">Name asociated to the face</param>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		/// <param name="originalBitmap">The original bitmap in which recognition was based</param>
		public Face(string name, byte[] features, VleDetectionDetails detectionDetails, Bitmap originalBitmap)
			: this(features, detectionDetails, originalBitmap)
		{
			this.name = name;
		}

		/// <summary>
		/// Initializes a new instance of Face object
		/// </summary>
		/// <param name="name">Name asociated to the face</param>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		/// <param name="originalBitmap">The original bitmap in which recognition was based</param>
		/// <param name="vlFace">The Verilook VleFace obtained as result of face detection</param>
		public Face(string name, byte[] features, VleDetectionDetails detectionDetails, Bitmap originalBitmap, VleFace vlFace)
			: this(features, detectionDetails, originalBitmap, vlFace)
		{
			this.name = name;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets the compressed features template that identifies this face
		/// </summary>
		public byte[] CompressedFeatures
		{
			get
			{
				return compressedFeatures;
			}
		}

		/// <summary>
		/// Gets the Verilook VleDetectionDetails obtained as result of face extraction
		/// Returns null if no VleDetectionDetails was provided when object was created
		/// </summary>
		public VleDetectionDetails DetectionDetails
		{
			get { return detectionDetails; }
		}

		/// <summary>
		/// Gets or sets the Name asociated to the face
		/// </summary>
		public string Name
		{
			get
			{
				if ((name == null) || (name.Length < 1)) return "unknown";
				return name;
			}
			set
			{
				//if (value == null) throw new ArgumentNullException();
				if (value == null) name = "unknown";
				else name = value;
			}
		}

		/// <summary>
		/// Gets the features template that identifies this face
		/// </summary>
		public byte[] Features
		{
			get
			{
				return features;
			}
		}

		/// <summary>
		/// Gets a value indicating if x, y, z coordinates has been calculated
		/// </summary>
		public bool HasCoords
		{
			get { return hasCoords; }
		}

		/// <summary>
		/// Gets a value indicating if hFOV and vFOV has been calculated
		/// </summary>
		public bool HasFOV
		{
			get { return hasFOV; }
		}

		/// <summary>
		/// Gets the unique ID of the face object, used for database storage
		/// </summary>
		public int Id
		{
			get
			{
				return id;
			}
			protected set
			{
				id = value;
			}
		}

		/// <summary>
		/// Gets the original bitmap in which recognition was based.
		/// Returns null if no bitmap was provided when object was created
		/// </summary>
		public Bitmap OriginalBitmap
		{
			get { return originalBitmap; }
		}

		/// <summary>
		/// Gets the Neurotec Grayscale Image used for recognition
		/// Returns null if no bitmap was provided when object was created
		/// </summary>
		public NGrayscaleImage RecognitionImage
		{
			get
			{
				if (originalBitmap == null) return null;
				if (recognitionImage == null)
				{
					using (NImage image = NImage.FromBitmap(originalBitmap))
					{
						recognitionImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);
					}
				}
				return recognitionImage;
			}
		}

		/// <summary>
		/// Gets the Verilook VleFace obtained as result of face extraction
		/// Returns null if no VleFace was provided when object was created
		/// </summary>
		public VleFace VlFace
		{
			get { return vlFace; }
		}

		/// <summary>
		/// Gets the vertical field of view
		/// The angle is proportional to the centroid of the detected face respect to the ceter of the image.
		/// The vFOV for the entire image height is 0.7330 radians (42 degrees)
		/// The angle is provided in radians
		/// </summary>
		public double VFoV
		{
			get { return vFov; }
		}

		/// <summary>
		/// Gets the horizontal field of view
		/// The angle is proportional to the centroid of the detected face respect to the ceter of the image.
		/// The hFOV for the entire image width is 0.9774 radians (56 degrees)
		/// The angle is provided in radians
		/// </summary>
		public double HFoV
		{
			get { return hFov; }
		}

		/// <summary>
		/// Gets the aproximated x position of the detected face respect to the ceter of the image (camera lens)
		/// This value is calculated using an estimated distance of 6cm between the eyes
		/// The distance is provided in meters
		/// </summary>
		public double X
		{
			get
			{
				return x;
			}
		}

		/// <summary>
		/// Gets the aproximated y position of the detected face respect to the ceter of the image (camera lens)
		/// This value is calculated using an estimated distance of 6cm between the eyes
		/// The distance is provided in meters
		/// </summary>
		public double Y
		{
			get
			{
				return y;
			}
		}

		/// <summary>
		/// Gets the aproximated z position of the detected face respect to the ceter of the image (camera lens)
		/// This value is calculated using an estimated distance of 6cm between the eyes
		/// The distance is provided in meters
		/// </summary>
		public double Z
		{
			get{return z;}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the recognition data for this Face object
		/// </summary>
		/// <param name="originalBitmap">The original bitmap in which recognition was based</param>
		internal void SetBitmap(Bitmap originalBitmap)
		{
			if (this.originalBitmap != null)
				return;
			this.originalBitmap = (Bitmap)new Bitmap(originalBitmap);
		}

		/// <summary>
		/// Sets the recognition data for this Face object
		/// </summary>
		/// <param name="features">Features template that identifies this face</param>
		/// <param name="detectionDetails">Detection detais generated from a face extraction</param>
		/// <param name="originalBitmap">The original bitmap in which recognition was based</param>
		internal void SetRecognitionData(byte[] features, VleDetectionDetails detectionDetails, Bitmap originalBitmap)
		{
			if (this.features != null)
				return;
			this.originalBitmap = (Bitmap)new Bitmap(originalBitmap);
			this.detectionDetails = detectionDetails;
			if (features == null) return;
			this.features = new byte[features.Length];
			Array.Copy(features, this.features, features.Length);
			this.compressedFeatures = Extractor.Compress(features);
		}

		/// <summary>
		/// Calculates the hFov and vFov
		/// </summary>
		internal void CalculateFovAndCoords(int width, int height)
		{

			if (Object.ReferenceEquals(vlFace, null))
				return;

			double fovV = 0.73303829;
			double fovH = 0.97738438;
			double centerX =  width / 2;
			double centerY = height / 2;
			double faceX = 0;
			double faceY = 0;
			// Eye distance in pixels
			double eyeDistanceX = 0;
			double eyeDistanceY = 0;
			double eyeDistancePx = 0;
			// Eye distance in meters
			double eyeDistanceM = 0.065;
			// Scale Factor
			double scaleX;
			double scaleY;
			double scaleZ;

			// Centroid of the face: Raster to cartesian
			faceX = (this.vlFace.Rectangle.X + this.vlFace.Rectangle.Width / 2) - centerX;
			faceY = centerY - (this.vlFace.Rectangle.Y + this.vlFace.Rectangle.Height / 2);

			// Fov calculation
			this.hFov = -fovH * faceX / width;
			this.vFov = fovV * faceY / height;
			hasFOV = true;

			if (Object.ReferenceEquals(detectionDetails, null) || !this.detectionDetails.FaceAvailable)
				return;

			if (!detectionDetails.EyesAvailable)
				return;
			// Scale factor
			eyeDistanceX = detectionDetails.Eyes.Second.X - detectionDetails.Eyes.First.X;
			eyeDistanceY = detectionDetails.Eyes.Second.Y - detectionDetails.Eyes.First.Y;
			eyeDistancePx = Math.Sqrt(eyeDistanceX*eyeDistanceX + eyeDistanceY*eyeDistanceY);
			scaleX = eyeDistanceM / eyeDistancePx;
			// Supose same scale for X
			scaleY = scaleX;
			// Distance ignoring the spherical deformation of the lens
			// With a hFOV of 56 degrees, a 6.5cm ruler must be a 2.2cm from the lens
			// to cover all with of the image.
			scaleZ = 0.06112361012375579 * width;

			// Calculate coords
			this.z = scaleZ / eyeDistancePx;
			this.x = faceX * scaleX;
			this.y = faceY * scaleY;
			
			hasCoords = true;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Extracts a face from a bitmap
		/// </summary>
		/// <param name="bitmap">Bitmap to extract from</param>
		/// <returns>Face is extraction was successfull, null otherwise</returns>
		public static Face ExtractFromBitmap(Bitmap bitmap)
		{
			NImage image;
			NGrayscaleImage gray;
			VLExtractor extractor;
			VleDetectionDetails details;
			byte[] features;

			image = NImage.FromBitmap(bitmap);
			gray = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);

			extractor = new VLExtractor();
			features = extractor.Extract(gray, out details);
			if ((features == null) || (features.Length == 0) || !details.FaceAvailable)
			{
				image.Dispose();
				gray.Dispose();
				return null;
			}
			return new Face(features, details, bitmap);
		}

		#endregion
	}
}
