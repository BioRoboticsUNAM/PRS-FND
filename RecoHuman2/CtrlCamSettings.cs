using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Neurotec.Cameras;

using RecoHuman.Sources;

namespace RecoHuman
{
	public partial class CtrlCamSettings : UserControl
	{

		#region Variables

		/// <summary>
		/// Asociated camera to this control
		/// </summary>
		private CameraAdapter camera;
		/// <summary>
		/// Stores the camera video formats
		/// </summary>
		CameraVideoFormat[] videoFormats;
		/// <summary>
		/// Represents the method UpdateControls
		/// </summary>
		private UpdateControlsEH updateControls;

		#endregion

		#region Constructor

		/// <summary>
		/// Initiates a new instance of CtrlCamSettings
		/// </summary>
		public CtrlCamSettings()
		{
			InitializeComponent();
			updateControls = new UpdateControlsEH(UpdateControls);
		}

		/// <summary>
		/// Initiates a new instance of CtrlCamSettings
		/// </summary>
		/// <param name="camera">Camera to initialize values from</param>
		public CtrlCamSettings(CameraAdapter camera):this()
		{
			this.camera = camera;
			UpdateControls();
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Represents the method that will handle calls to UpdateControls
		/// </summary>
		private delegate void UpdateControlsEH();

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the asociated camera
		/// </summary>
		public CameraAdapter Camera
		{
			get{return camera;}
			set
			{
				camera = value;
				if (camera != null)
				{
					videoFormats = VideoFormat.Cast(camera.AvailableVideoFormats);
				}
				try
				{
					this.BeginInvoke(updateControls);
				}
				catch { }
			}
		}

		#endregion

		#region Methods

		private void UpdateControls()
		{
			cmbVideoFormats.Items.Clear();

			//chkAutomaticSettings.Enabled = (camera != null);
			//chkEnabled.Enabled = (camera != null);
			chkMirrorHorizontal.Enabled = (camera != null);
			chkMirrorVertical.Enabled = (camera != null);
			if (camera == null)
			{
				chkAutomaticSettings.Checked = false;
				chkEnabled.Checked = false;
				chkMirrorHorizontal.Checked = false;
				chkMirrorVertical.Checked = false;
				return;
			}

			int i = 0;
			foreach (CameraVideoFormat format in videoFormats)
			{
				cmbVideoFormats.Items.Add(format.FrameWidth.ToString() + " by " + format.FrameHeight.ToString() + " pixels @" + format.FrameRate.ToString("0") + "fps");
				if (camera.VideoFormat.Equals(format))
					cmbVideoFormats.SelectedIndex = i;
				++i;
			}
			if (camera is VeriLookWebCamAdapter)
				chkAutomaticSettings.Checked = ((VeriLookWebCamAdapter)camera).AutomaticSettings;
			else
				chkAutomaticSettings.Checked = false;
			chkEnabled.Checked = camera.IsCapturing;
			chkMirrorHorizontal.Checked = camera.MirrorHorizontal;
			chkMirrorVertical.Checked = camera.MirrorVertical;
		}

		#endregion

		#region EnventHandlers

		private void cmbVideoFormats_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbVideoFormats.SelectedIndex == -1) return;
			if (cmbVideoFormats.SelectedIndex >= videoFormats.Length) cmbVideoFormats.SelectedIndex = 0;
			if(camera.VideoFormat.Equals(videoFormats[cmbVideoFormats.SelectedIndex]))
				return;
			camera.VideoFormat = videoFormats[cmbVideoFormats.SelectedIndex];

			this.BeginInvoke(updateControls);
		}

		private void chkMirrorVertical_CheckedChanged(object sender, EventArgs e)
		{
			if (camera.MirrorVertical == chkMirrorVertical.Checked)
				return;
			camera.MirrorVertical = chkMirrorVertical.Checked;

			this.BeginInvoke(updateControls);
		}

		private void chkMirrorHorizontal_CheckedChanged(object sender, EventArgs e)
		{
			if (camera.MirrorHorizontal == chkMirrorHorizontal.Checked)
				return;
			camera.MirrorHorizontal = chkMirrorHorizontal.Checked;

			this.BeginInvoke(updateControls);
		}

		private void chkAutomaticSettings_CheckedChanged(object sender, EventArgs e)
		{
			VeriLookWebCamAdapter camera = this.camera as VeriLookWebCamAdapter;
			if (camera == null) return;
			if (camera.AutomaticSettings == chkAutomaticSettings.Checked)
				return;
			camera.AutomaticSettings = chkAutomaticSettings.Checked;

			this.BeginInvoke(updateControls);
		}

		private void chkEnabled_CheckedChanged(object sender, EventArgs e)
		{
			if(camera.IsCapturing == chkEnabled.Checked) return;
			//if (chkEnabled.Checked) camera.StartCapturing();
			//else camera.StopCapturing();
			this.BeginInvoke(updateControls);
		}

		#endregion
	}
}
