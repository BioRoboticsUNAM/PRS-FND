using System;
using System.Drawing;

using Robotics.HAL.Sensors;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Represents a source of image
	/// </summary>
	public interface IImageSource
	{
		#region Properties

		/// <summary>
		/// Gets the type of the Image Source
		/// </summary>
		ImageSourceType SourceType { get; }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when a new image is received and becomes availiable
		/// </summary>
		event ImageProducedEventHandler ImageProduced;

		#endregion

		#region Methods

		/// <summary>
		/// Gets an image. It blocks the thread call untill an image arrives
		/// </summary>
		/// <returns>An image from the source</returns>
		Bitmap GetImage();

		/// <summary>
		/// Gets an image. It blocks the thread call untill an image arrives or the specified time elapses. If there is a timeout null is returned
		/// </summary>
		/// <param name="timeout">The maximum amount of time to wait for an image</param>
		/// <returns>An image from the source or null if timedout</returns>
		Bitmap GetImage(int timeout);

		#endregion
	}
}
