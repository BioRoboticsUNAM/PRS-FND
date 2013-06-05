using System;

namespace RecoHuman.Sources
{
	public class VerilookWebCam : ICamera
	{
		/// <summary>
		/// Stores the selected camera for capturing
		/// </summary>
		private Neurotec.Cameras.Camera camera;

		public VerilookWebCam(Neurotec.Cameras.Camera camera)
		{
			this.camera = camera;
		}

		/// <summary>
		/// Gets the selected camera for capturing
		/// </summary>
		public Neurotec.Cameras.Camera Camera
		{
			get { return this.camera; }
		}

		#region ICamera Members

		public bool IsCapturing
		{
			get { return this.camera.IsCapturing; }
		}

		public void StopCapturing()
		{
			this.camera.StopCapturing();
		}

		public static implicit operator VerilookWebCam(Neurotec.Cameras.Camera camera)
		{
			return new VerilookWebCam(camera);
		}

		#endregion
	}
}
