using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Robotics;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Manages multiple image sources
	/// </summary>
	public class ImageSourceManager : IImageSource
	{

		#region Variables

		/// <summary>
		/// Stores the last captured images
		/// </summary>
		private ProducerConsumer<Bitmap> capturedImages;

		/// <summary>
		/// Collection of image sources asociated to this ImageSourceManager object
		/// </summary>
		private ImageSourceCollection sources;

		/// <summary>
		/// The index of the selected image source
		/// </summary>
		private int selectedSourceIndex;

		/// <summary>
		/// Represents the element_ImageProduced method
		/// </summary>
		private ImageProducedEventHandler dlgImageProduced;

		#endregion

		#region Constructors

		public ImageSourceManager()
		{
			this.selectedSourceIndex = -1;
			this.capturedImages = new ProducerConsumer<Bitmap>(30);
			this.sources = new ImageSourceCollection(this);
			this.sources.ElementAdded += new ImageSourceCollectionChanged(sources_ElementAdded);
			this.sources.ElementRemoved += new ImageSourceCollectionChanged(sources_ElementRemoved);
			this.dlgImageProduced = new ImageProducedEventHandler(element_ImageProduced);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the index of the selected image source
		/// </summary>
		public int SelectedSourceIndex
		{
			get { return this.selectedSourceIndex; }
			set
			{
				if (this.selectedSourceIndex == value)
					return;
				if ((value < -1) || (value >= sources.Count))
					throw new ArgumentOutOfRangeException();
				this.selectedSourceIndex = value;
				capturedImages.Clear();
			}
		}

		/// <summary>
		/// Gets or sets the selected image source
		/// </summary>
		public IImageSource SelectedSource
		{
			get { return this.sources[selectedSourceIndex]; }
			set
			{
				int index;
				if(value == null)
					throw new ArgumentNullException();
				index = this.sources.IndexOf(value);
				if (index == -1)
					throw new ArgumentException("The specified IImageSource object does not belong to the collection");
				selectedSourceIndex = index;
			}
		}

		/// <summary>
		/// Gets a collection of image sources asociated to this ImageSourceManager object
		/// </summary>
		public ImageSourceCollection Sources
		{
			get { return this.sources; }
		}

		/// <summary>
		/// Gets the type of the Image Source
		/// </summary>
		public ImageSourceType SourceType { get { return ImageSourceType.All; } }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when a new image is received and becomes availiable
		/// </summary>
		public event ImageProducedEventHandler ImageProduced;

		#endregion

		#region Methods

		/// <summary>
		/// Gets an image. It blocks the thread call untill an image arrives
		/// </summary>
		/// <returns>An image from the source</returns>
		public Bitmap GetImage()
		{
			return capturedImages.Consume();
		}

		/// <summary>
		/// Gets an image. It blocks the thread call untill an image arrives or the specified time elapses. If there is a timeout null is returned
		/// </summary>
		/// <param name="timeout">The maximum amount of time to wait for an image</param>
		/// <returns>An image from the source or null if timedout</returns>
		public Bitmap GetImage(int timeout)
		{
			return capturedImages.Consume(timeout);
		}

		#endregion

		#region EventHandlers

		private void sources_ElementAdded(ImageSourceCollection source, IImageSource element)
		{
			element.ImageProduced += dlgImageProduced;
		}

		private void sources_ElementRemoved(ImageSourceCollection source, IImageSource element)
		{
			element.ImageProduced -= dlgImageProduced;
		}

		private void element_ImageProduced(IImageSource source)
		{
			if ((selectedSourceIndex < 0) || (sources[selectedSourceIndex] != source))
				return;
			Bitmap image = source.GetImage(0);
			if(image != null)
				capturedImages.Produce(image);
		}

		#endregion
	}
}
