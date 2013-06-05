using System;

namespace RecoHuman.Sources
{
	public interface ICamera
	{
		/// <summary>
		/// Gets a value indicating if the camera is capturing
		/// </summary>
		bool IsCapturing { get; }

		/// <summary>
		/// Stops the video capturing
		/// </summary>
		void StopCapturing();
	}
}