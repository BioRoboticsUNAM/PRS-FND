using System;
using System.Drawing;

namespace RecoHuman.Sources
{
	[Flags]
	public enum ImageSourceType
	{
		None = 0x00,
		Camera = 0x01,
		ImageServer = 0x02,
		File = 0x04,
		Pipe = 0x08,
		All = ImageSourceType.Camera |  ImageSourceType.ImageServer | ImageSourceType.Pipe | ImageSourceType.File
	};

	/// <summary>
	/// Represents the method that will handle the ElementAdded and ElementRemoved of a ImageSourceCollection object
	/// </summary>
	/// <param name="source">The ImageSourceCollection which rises the event</param>
	/// <param name="element">The element which has been added or remoed from the collection</param>
	public delegate void ImageSourceCollectionChanged(ImageSourceCollection source, IImageSource element);

	/// <summary>
	/// Represents the method that will handle the ImageProduced event of a IImageSource object
	/// </summary>
	/// <param name="source"></param>
	public delegate void ImageProducedEventHandler(IImageSource source);

	/// <summary>
	/// Represents the method that will handle the CameraCollectionChanged event of the VeriLookWebCamAdapter class
	/// </summary>
	/// <param name="sender">The VeriLookWebCamAdapter object which rises the event</param>
	public delegate void VeriLookWebCamAdapterEventHandler(VeriLookWebCamAdapter sender);

	/// <summary>
	/// Represents the method that will handle the ImageCaptured event of the WIACameraAdapter class
	/// </summary>
	/// <param name="sender">The WIACameraAdapter object which rises the event</param>
	/// <param name="capturedImage">The captured image</param>
	public delegate void WIACameraAdapterImageCapturedEventHandler(WIACameraAdapter sender, Bitmap capturedImage);

	/// <summary>
	/// Represents the method that will handle the CameraCollectionChanged event of the WIACameraAdapter class
	/// </summary>
	/// <param name="sender">The WIACameraAdapter object which rises the event</param>
	public delegate void WIACameraAdapterEventHandler(WIACameraAdapter sender);

	/// <summary>
	/// Represents the method that will handle the ImageCaptured event of the CameraAdapter class
	/// </summary>
	/// <param name="sender">The WIACameraAdapter object which rises the event</param>
	/// <param name="capturedImage">The captured image</param>
	public delegate void ImageCapturedEventHandler(CameraAdapter sender, Bitmap capturedImage);

	/// <summary>
	/// Represents the method that will handle the CameraCollectionChanged event of the CameraAdapter class
	/// </summary>
	/// <param name="sender">The WIACameraAdapter object which rises the event</param>
	public delegate void CameraAdapterEventHandler(CameraAdapter sender);


}
