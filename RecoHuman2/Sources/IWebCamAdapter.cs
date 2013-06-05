using System;
using System.Collections.Generic;
using System.Text;
using Neurotec.Cameras;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Represents an adapter for video Capturing
	/// </summary>
	public interface IWebCamAdapter : IImageSource
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating if the camera uses automatic settings
		/// </summary>
		bool AutomaticSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the camera availiable video formats
		/// </summary>
		CameraVideoFormat[] AvailableVideoFormats
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating if the camera is capturing
		/// </summary>
		bool IsCapturing
		{
			get;
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored horizontally
		/// </summary>
		bool MirrorHorizontal
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating if the camera image is mirrored vertically
		/// </summary>
		bool MirrorVertical
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the video format used for capturing
		/// </summary>
		CameraVideoFormat VideoFormat
		{
			get;
			set;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Starts the WebCam adapter
		/// </summary>
		void Start();

		/// <summary>
		/// Stops the WebCam adapter
		/// </summary>
		void Stop();

		#endregion
	}
}
