using System;
using System.Collections.Generic;
using System.Text;

namespace RecoHuman.Sources
{
	public struct VideoFormat
	{
		#region Variables

		/// <summary>
		/// The height of the frame
		/// </summary>
		private int frameHeight;
		/// <summary>
		/// The frame rate in fps
		/// </summary>
		private double frameRate;
		/// <summary>
		/// The width of the frame
		/// </summary>
		private int frameWidth;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of VideoFormat
		/// </summary>
		/// <param name="height">The height of the frame</param>
		/// <param name="rate">The frame rate in fps</param>
		/// <param name="width">The width of the frame</param>
		public VideoFormat(int width, int height, double rate)
		{
			if (width < 0) throw new ArgumentOutOfRangeException();
			if (height < 0) throw new ArgumentOutOfRangeException();
			if (rate < 0) throw new ArgumentOutOfRangeException();
			frameHeight = height;
			frameRate = rate;
			frameWidth = width;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the height of the frame
		/// </summary>
		public int FrameHeight
		{
			get { return this.frameHeight; }
			set
			{
				if (value < 0) throw new ArgumentOutOfRangeException();
				this.frameHeight = value;
			}
		}

		/// <summary>
		/// Gets the frame rate in fps
		/// </summary>
		public double FrameRate
		{
			get { return this.frameRate; }
			set
			{
				this.frameRate = value;
			}
		}

		/// <summary>
		/// Gets the width of the frame
		/// </summary>
		public int FrameWidth
		{
			get { return this.frameWidth; }
			set
			{
				if (value < 0) throw new ArgumentOutOfRangeException();
				this.frameWidth = value;
			}
		}

		#endregion

		public static implicit operator Neurotec.Cameras.CameraVideoFormat(VideoFormat vf)
		{
			Neurotec.Cameras.CameraVideoFormat cvf = new Neurotec.Cameras.CameraVideoFormat();
			cvf.FrameHeight = vf.FrameHeight;
			cvf.FrameRate = (float)vf.FrameRate;
			cvf.FrameWidth = vf.FrameWidth;
			return cvf;
		}

		public static implicit operator VideoFormat(Neurotec.Cameras.CameraVideoFormat cvf)
		{
			return new VideoFormat(cvf.FrameWidth, cvf.FrameHeight, cvf.FrameRate);
		}

		public static Neurotec.Cameras.CameraVideoFormat[] Cast(VideoFormat[] vf)
		{
			Neurotec.Cameras.CameraVideoFormat[] formats = new Neurotec.Cameras.CameraVideoFormat[vf.Length];
			for (int i = 0; i < formats.Length; ++i)
				formats[i] = vf[i];
			return formats;
		}

		public static VideoFormat[] Cast(Neurotec.Cameras.CameraVideoFormat[] vf)
		{
			VideoFormat[] formats = new VideoFormat[vf.Length];
			for (int i = 0; i < formats.Length; ++i)
				formats[i] = vf[i];
			return formats;
		}
	}
}
