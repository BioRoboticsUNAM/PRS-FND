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
	/// Represents a Serializable Face object.
	/// Should be used only for serialization
	/// </summary>
	[Serializable]
	internal class SerializableFace
	{
		#region Variables

		/// <summary>
		/// Stores the features template that identifies this face
		/// </summary>
		private byte[] features;
		/// <summary>
		/// Stores the features template (compressed) that identifies this face
		/// </summary>
		private byte[] compressedFeatures;

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
		/// Stores the Verilook VleFace obtained as result of face detection
		/// </summary>
		private VleFace vlFace;

		#endregion

		/// <summary>
		/// Initializes a new instance of SerializableFace object
		/// </summary>
		/// <param name="face">Base face</param>
		public SerializableFace(Face face)
		{
			this.compressedFeatures = face.CompressedFeatures;
			this.features = face.Features;
			this.id = face.Id;
			this.name = face.Name;
			this.originalBitmap = face.OriginalBitmap;
			this.vlFace = face.VlFace;
		}

		/// <summary>
		/// Converts a SerializableFace object into a Face object
		/// </summary>
		/// <param name="sf">SerializableFace to connvert</param>
		/// <returns></returns>
		public static implicit operator Face(SerializableFace sf)
		{
			return new Face(sf.name, sf.features, new VleDetectionDetails(), sf.originalBitmap);
		}

		/*
		private class SerializableDetectionDetails
		{
			public SerializableDetectionDetails(VleDetectionDetails details)
			{
				details.
			}
		}
		*/
	}
}
