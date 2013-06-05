using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RecoHuman
{
	/// <summary>
	/// Represents the method that will handle the FaceAdded and FaceRemoved event of a FaceCollection object.
	/// </summary>
	/// <param name="face"></param>
	public delegate void FaceAddRemoveEH(Face face);

	/// <summary>
	/// Provides a collection of faces
	/// </summary>
	[Serializable]
	public class FaceCollection : IEnumerable<Face>, ICollection<Face>
	{
		#region Variables

		private List<Face> faces;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the FaceCollection class
		/// </summary>
		/// <param name="capacity">The number of elements that the new FaceCollection can initially store</param>
		public FaceCollection(int capacity)
		{
			this.faces = new List<Face>(capacity);
		}

		/// <summary>
		/// Initializes a new instance of the FaceCollection class
		/// </summary>
		public FaceCollection()
			: this(10)
		{
		}

		#endregion

		#region Properties

		#region ICollection<Face> Members

		/// <summary>
		/// Gets the number of Faces in the FaceCollection object
		/// </summary>
		public int Count
		{
			get { return faces.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the FaceCollection object is read-only
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#endregion

		#region Indexers

		/// <summary>
		/// Gets the element at the specified index position
		/// </summary>
		/// <param name="i">The zero based index of the element to get or set</param>
		/// <returns>The Face at position i</returns>
		public Face this[int i]
		{
			get { return faces[i]; }
			set { faces[i] = value; }
		}

		#endregion

		#region Operators

		/// <summary>
		/// Imlicitly converts this collection to a fixed size array
		/// </summary>
		/// <param name="fc">FaceCollection to convert</param>
		/// <returns>A Face[] array with the elements in the collection</returns>
		public static implicit operator Face[](FaceCollection fc)
		{
			return fc.faces.ToArray();
		}

		#endregion

		#region Events

		/// <summary>
		/// Raises when a Face is addedd to the FaceCollection
		/// </summary>
		public event FaceAddRemoveEH FaceAdded;
		/// <summary>
		/// Raises when a Face is removed from the FaceCollection
		/// </summary>
		public event FaceAddRemoveEH FaceRemoved;

		#endregion

		#region Methods

		/// <summary>
		/// Adds the elements of the specified collection to the end of the FaceCollection's List
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the FaceCollection's list.
		/// The collection itself cannot be a null reference (Nothing in Visual Basic),
		/// but it can contain elements that are a null reference (Nothing in Visual Basic)</param>
		public void AddRange(IEnumerable<Face> collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			foreach (Face face in collection)
			{
				if (face != null)
				{
					Add(face);
					if (this.FaceAdded != null)
						FaceAdded(face);
				}
			}
		}

		/// <summary>
		/// Gets an array of elements with the specified type
		/// </summary>
		/// <param name="name">The name of the face element to get</param>
		/// <returns>an array of elements with the specified name</returns>
		public Face[] GetFacesByName(string name)
		{
			List<Face> faces = new List<Face>();
			foreach (Face face in this.faces)
				if (face.Name == name) faces.Add(face);
			return faces.ToArray();
		}

		/// <summary>
		/// Retrieves the index of a specified Face object in the collection
		/// </summary>
		/// <param name="face">The Face for which the index is returned</param>
		/// <returns>The index of the specified Face. If the Face is not currently a member of the collection, it returns -1</returns>
		public virtual int IndexOf(Face face)
		{
			return faces.IndexOf(face);
		}

		/// <summary>
		/// Loads a set of faces from a file and adds it to the collection
		/// </summary>
		/// <param name="filePath">File to load the FaceCollecton from</param>
		public void Import(string filePath)
		{
			BinaryFormatter formatter;
			FileStream stream;
			FaceCollection fc;

			if (!File.Exists(filePath)) return;
			try
			{
				stream = File.OpenRead(filePath);

				formatter = new BinaryFormatter();
				fc = (FaceCollection)formatter.Deserialize(stream);
				stream.Close();
			}
			catch { return; }

			//foreach (Face face in fc)
			//{
			//    if (face == null)
			//        continue;
			//    if (face.Id == face.Id)
			//        continue;
			//    this.Add(face);
			//}

			AddRange(fc);
		}

		/// <summary>
		/// Removes a Face, at the specified index location, from the FaceCollection object
		/// </summary>
		/// <param name="index">The ordinal index of the Face to be removed from the collection</param>
		public virtual void RemoveAt(int index)
		{
			if ((index < 0) || (index >= faces.Count)) throw new ArgumentOutOfRangeException();
			faces.RemoveAt(index);
			if (FaceRemoved != null) FaceRemoved(faces[index]);
		}

		/// <summary>
		/// Copies the elements of the FaceCollection element to a new array
		/// </summary>
		/// <returns>An array containing copies of the elements of the FaceCollection</returns>
		public virtual Face[] ToArray()
		{
			return this.faces.ToArray();
		}

		#region ICollection<Face> Members

		/// <summary>
		/// Adds the specified Face object to the collection.
		/// </summary>
		/// <param name="item">The Face to add to the collection</param>
		public void Add(Face item)
		{
			faces.Add(item);
			if (FaceAdded != null) FaceAdded(item);
		}

		/// <summary>
		/// Removes all elements from the current FaceCollection object
		/// </summary>
		public void Clear()
		{
			faces.Clear();
		}

		/// <summary>
		/// Determines whether the specified Face is in the FaceCollection object
		/// </summary>
		/// <param name="item">The Face to search for in the collection</param>
		/// <returns>true if the specified Face exists in the collection; otherwise, false</returns>
		public bool Contains(Face item)
		{
			return faces.Contains(item);
		}

		/// <summary>
		/// Copies the child controls stored in the FaceCollection object to an Face array object, beginning at the specified index location in the System.Array
		/// </summary>
		/// <param name="array">The Face array to copy the child controls to.</param>
		/// <param name="arrayIndex">The zero-based relative index in array where copying begins</param>
		public void CopyTo(Face[] array, int arrayIndex)
		{
			faces.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the specified Face from the parent FaceCollection object
		/// </summary>
		/// <param name="item">The Face to be removed</param>
		/// <returns>true if the specified Face exists in the collection; otherwise, false</returns>
		public bool Remove(Face item)
		{
			if (faces.Contains(item) && (FaceRemoved != null)) FaceRemoved(item);
			return faces.Remove(item);
		}

		#endregion

		#region IEnumerable<Face> Members
		/// <summary>
		/// Retrieves an enumerator that can iterate through the FaceCollection object
		/// </summary>
		/// <returns>The enumerator to iterate through the collection.</returns>
		public IEnumerator<Face> GetEnumerator()
		{
			return faces.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Retrieves an enumerator that can iterate through the FaceCollection object
		/// </summary>
		/// <returns>The enumerator to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return faces.GetEnumerator();
		}

		#endregion

		#endregion

		#region Static Methods

		/// <summary>
		/// Loads a set of faces from a file
		/// </summary>
		/// <param name="filePath">File to load the FaceCollecton from</param>
		/// <returns>FaceCollection contained in file. Null if not found</returns>
		public static FaceCollection Load(string filePath)
		{
			BinaryFormatter formatter;
			FileStream stream;
			FaceCollection fc;

			if (!File.Exists(filePath)) return null;
			try
			{
				stream = File.OpenRead(filePath);

				formatter = new BinaryFormatter();
				fc = (FaceCollection)formatter.Deserialize(stream);
				stream.Close();
			}
			catch { return null; }
			return fc;
		}

		/// <summary>
		/// Saves a FaceCollecton to a file
		/// </summary>
		/// <param name="filePath">File to save the FaceCollecton in</param>
		/// <param name="collection">FaceCollection to Serialize</param>
		public static bool Save(string filePath, FaceCollection collection)
		{
			BinaryFormatter formatter;
			FileStream stream;

			try
			{
				stream = File.Open(filePath, FileMode.OpenOrCreate);

				formatter = new BinaryFormatter();
				formatter.Serialize(stream, collection);
				stream.Close();

			}
			catch { return false; }
			return true;
		}

		#endregion
	}

}
