using System;
using System.Collections.Generic;
using System.Text;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Represents a collection of IImageSource objects.
	/// </summary>
	/// <remarks>pending to implement multithread sync</remarks>
	public class ImageSourceCollection : ICollection<IImageSource>
	{

		#region Variables

		/// <summary>
		/// Stores the list of image sources
		/// </summary>
		private List<IImageSource> imageSourceList;

		/// <summary>
		/// The ImageSourceManager object to which this ImageSourceCollection is bound to
		/// </summary>
		private ImageSourceManager imageSourceManager;

		/// <summary>
		/// Lock for thread safe operations
		/// </summary>
		private System.Threading.ReaderWriterLock rwLock;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of ImageSourceCollection
		/// <param name="imageSourceManager"></param>
		/// </summary>
		public ImageSourceCollection(ImageSourceManager imageSourceManager)
		{
			this.imageSourceList = new List<IImageSource>();
			this.imageSourceManager = imageSourceManager;
			this.rwLock = new System.Threading.ReaderWriterLock();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when an element is added to the collection
		/// </summary>
		public event ImageSourceCollectionChanged ElementAdded;
		/// <summary>
		/// Occurs when an element is removed the collection
		/// </summary>
		public event ImageSourceCollectionChanged ElementRemoved;

		#endregion

		#region Properties and Indexers

		/// <summary>
		/// Gets the IImageSource at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the IImageSource object to retrieve</param>
		/// <returns>The IImageSource at the specified index.</returns>
		public IImageSource this[int index]
		{
			get
			{
				IImageSource result;
				this.rwLock.AcquireReaderLock(-1);
				if ((index < 0) || (index > this.imageSourceList.Count))
				{
					this.rwLock.ReleaseReaderLock();
					throw new ArgumentOutOfRangeException();
				}
				result = this.imageSourceList[index];
				this.rwLock.ReleaseReaderLock();
				return result;
			}
		}

		/// <summary>
		/// Gets the ImageSourceManager object to which this ImageSourceCollection is bound to
		/// </summary>
		public ImageSourceManager ImageSourceManager
		{
			get { return this.imageSourceManager; }
		}

		#region ICollection<IImageSource> Members

		/// <summary>
		/// Gets the number of elements actually contained in the ImageSourceCollection.
		/// </summary>
		public int Count
		{
			get
			{
				int result;
				this.rwLock.AcquireReaderLock(-1);
				result = this.imageSourceList.Count;
				this.rwLock.ReleaseReaderLock();
				return result;
			}
		}

		/// <summary>
		/// This property always returns false
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#endregion

		#region Methodos

		/// <summary>
		/// Rises the ElementAdded event
		/// </summary>
		/// <param name="element">The element which has been added to the collection</param>
		protected void OnElementAdded(IImageSource element)
		{
			try
			{
				if (this.ElementAdded != null)
					this.ElementAdded(this, element);
			}
			catch { }
		}

		/// <summary>
		/// Rises the ElementRemoved event
		/// </summary>
		/// <param name="element">The element which has been remoed from the collection</param>
		protected void OnElementRemoved(IImageSource element)
		{
			try
			{
				if (this.ElementRemoved != null)
					this.ElementRemoved(this, element);
			}
			catch { }
		}

		/// <summary>
		/// Searches for the specified object and returns the zero-based index of the element in the collection
		/// </summary>
		/// <param name="element">The object to locate in the collection</param>
		/// <returns>The zero-based index of the element in the collection if found; otherwise, -1.</returns>
		public int IndexOf(IImageSource element)
		{
			return this.imageSourceList.IndexOf(element);
		}

		#region ICollection<IImageSource> Members

		/// <summary>
		/// Adds a IImageSource to the ImageSourceCollection
		/// </summary>
		/// <param name="imageSource">The IImageSource to be added to the end of the ImageSourceCollection.</param>
		public void Add(IImageSource imageSource)
		{
			if (imageSource == null) throw new ArgumentNullException();
			this.rwLock.AcquireWriterLock(-1);
			if (this.imageSourceList.Contains(imageSource))
			{
				this.rwLock.ReleaseWriterLock();
				throw new ArgumentException("The provided IImageSource already exists in the collection", "imageSource");
			}
			this.imageSourceList.Add(imageSource);
			this.rwLock.ReleaseWriterLock();
			OnElementAdded(imageSource);
		}

		/// <summary>
		/// Removes all elements from the ImageSourceCollection
		/// </summary>
		public void Clear()
		{
			IImageSource[] elements;
			this.rwLock.AcquireWriterLock(-1);
			elements = this.imageSourceList.ToArray();
			this.imageSourceList.Clear();
			this.rwLock.ReleaseWriterLock();
			foreach (IImageSource source in elements)
				OnElementRemoved(source);
		}

		/// <summary>
		/// Determines whether an IImageSource is in the ImageSourceCollection.
		/// </summary>
		/// <param name="item">The object to locate in the ImageSourceCollection.</param>
		/// <returns>true if item is found in the ImageSourceCollection; otherwise, false</returns>
		public bool Contains(IImageSource item)
		{
			bool result;
			this.rwLock.AcquireReaderLock(-1);
			result = this.imageSourceList.Contains(item);
			this.rwLock.ReleaseReaderLock();
			return result;
		}

		/// <summary>
		/// Copies the entire ImageSourceCollection to a compatible one-dimensional array, starting at the specified index of the target array
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from ImageSourceCollection. The Array must have zero-based indexing</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
		public void CopyTo(IImageSource[] array, int arrayIndex)
		{
			this.rwLock.AcquireReaderLock(-1);
			this.imageSourceList.CopyTo(array, arrayIndex);
			this.rwLock.ReleaseReaderLock();
		}

		/// <summary>
		/// Removes the specified IImageSource from the ImageSourceCollection
		/// </summary>
		/// <param name="imageSource">The object to remove from the ImageSourceCollection. The value can be a null reference (Nothing in Visual Basic) for reference types</param>
		/// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the List</returns>
		public bool Remove(IImageSource imageSource)
		{
			bool result;
			if (imageSource == null) return false;
			this.rwLock.AcquireWriterLock(-1);
			result = Remove(imageSource);
			this.rwLock.ReleaseWriterLock();
			if (result)
				OnElementAdded(imageSource);
			return result;
		}

		#endregion

		#region IEnumerable<IImageSource> Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection</returns>
		public IEnumerator<IImageSource> GetEnumerator()
		{
			return this.imageSourceList.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.imageSourceList.GetEnumerator();
		}

		#endregion

		#endregion
	}
}