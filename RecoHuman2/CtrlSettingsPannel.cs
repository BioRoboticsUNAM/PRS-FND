using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RecoHuman
{
	public partial class CtrlSettingsPannel : UserControl
	{
		#region Variables

		/// <summary>
		/// Asociated settings
		/// </summary>
		private RecoHumanSettigs settings;
		/// <summary>
		/// Represents the update method for async calls
		/// </summary>
		private VoidEventHandler dlgUpdateSettings;

		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of CtrlSettingsPannel
		/// </summary>
		public CtrlSettingsPannel()
		{
			InitializeComponent();
			dlgUpdateSettings = new VoidEventHandler(UpdateSettings);
			gbFeaturesExtraction.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the RecoHumanSettigs asociated to this control
		/// </summary>
		public RecoHumanSettigs Settings
		{
			get { return settings; }
			set
			{
				if (value == null) return;//throw new ArgumentNullException();
				if(settings != null)
					settings.RecoHumanSettingsChanged -= new RecoHumanSettingsChangedEH(settings_RecoHumanSettingsChanged);
				settings = value;
				settings.RecoHumanSettingsChanged += new RecoHumanSettingsChangedEH(settings_RecoHumanSettingsChanged);
				UpdateSettings();
			}
		}

		#region Settings

		/// <summary>
		/// Gets or sets the number of attemps to realize while Enrolling
		/// </summary>
		protected int AttemptsWhileEnrolling
		{
			get { return settings.AttemptsWhileEnrolling; }
			set
			{
				if (settings.AttemptsWhileEnrolling == value) return;
				settings.AttemptsWhileEnrolling = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudAttemptsWhileEnrolling.Value = value;

			}
		}

		/// <summary>
		/// Gets or sets the number of attemps to realize while Matching
		/// </summary>
		protected int AttemptsWhileMatching
		{
			get { return settings.AttemptsWhileMatching; }
			set
			{
				if (settings.AttemptsWhileMatching == value) return;
				settings.AttemptsWhileMatching = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudAttemptsWhileMatching.Value = value;

			}
		}

		/// <summary>
		/// Gets or sets the minimal interocular distance
		/// </summary>
		protected int MinimalInterOcularDistance
		{
			get { return settings.MinimalInterOcularDistance; }
			set
			{
				if (settings.MinimalInterOcularDistance == value) return;
				settings.MinimalInterOcularDistance = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudMinIOD.Value = value;

			}
		}

		/// <summary>
		/// Gets or sets the maximum interocular distance
		/// </summary>
		protected int MaximumInterOcularDistance
		{
			get { return settings.MaximumInterOcularDistance; }
			set
			{
				if (settings.MaximumInterOcularDistance == value) return;
				settings.MaximumInterOcularDistance = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudMaxIOD.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the Generalization Threshold
		/// </summary>
		protected double GeneralizationThreshold
		{
			get { return settings.GeneralizationThreshold; }
			set
			{
				if (settings.GeneralizationThreshold == value) return;
				settings.GeneralizationThreshold = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudGeneralizationTreshold.Value = (decimal)value;
			}
		}

		/// <summary>
		/// Gets or sets the Generalization Threshold
		/// </summary>
		protected int ImageCount
		{
			get { return settings.ImageCount; }
			set
			{
				if (settings.ImageCount == value) return;
				settings.ImageCount = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudImageCount.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the Matching Attempts
		/// </summary>
		protected int MatchingAttempts
		{
			get { return settings.MatchingAttempts; }
			set
			{
				if (settings.MatchingAttempts == value) return;
				settings.MatchingAttempts = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudMatchingAttempts.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the Matching Threshold
		/// </summary>
		protected double MatchingThreshold
		{
			get { return settings.MatchingThreshold; }
			set
			{
				if (settings.MatchingThreshold == value) return;
				settings.MatchingThreshold = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudMatchingTreshold.Value = (decimal)value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum matching results
		/// </summary>
		protected int MaximumMatchingResults
		{
			get { return settings.MaximumMatchingResults; }
			set
			{
				if (settings.MaximumMatchingResults == value) return;
				settings.MaximumMatchingResults = value;
				if (InvokeRequired)
				{
					this.Invoke(dlgUpdateSettings);
					return;
				}
				this.nudMaxMatchingResults.Value = value;
			}
		}

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Updates the settings controls
		/// </summary>
		public void UpdateSettings()
		{
			if (settings == null) return;
			if (this.InvokeRequired)
			{
				Invoke(dlgUpdateSettings);
				return;
			}
			nudAttemptsWhileEnrolling.Value = (decimal)settings.AttemptsWhileEnrolling;
			nudAttemptsWhileMatching.Value = (decimal)settings.AttemptsWhileMatching;
			nudGeneralizationTreshold.Value = (decimal)settings.GeneralizationThreshold;
			nudImageCount.Value = (decimal)settings.ImageCount;
			nudMatchingAttempts.Value = (decimal)settings.MatchingAttempts;
			nudMatchingTreshold.Value = (decimal)settings.MatchingThreshold;
			nudMaxIOD.Value = (decimal)settings.MaximumInterOcularDistance;
			nudMaxMatchingResults.Value = (decimal)settings.MaximumMatchingResults;
			nudMinIOD.Value = (decimal)settings.MinimalInterOcularDistance;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Raises when asociated settings has changed
		/// </summary>
		/// <param name="settings"></param>
		private void settings_RecoHumanSettingsChanged(RecoHumanSettigs settings)
		{
			UpdateSettings();
		}

		private void nudAttemptsWhileEnrolling_ValueChanged(object sender, EventArgs e)
		{
			this.AttemptsWhileEnrolling = (int)nudAttemptsWhileEnrolling.Value;
		}

		private void nudAttemptsWhileMatching_ValueChanged(object sender, EventArgs e)
		{
			this.AttemptsWhileMatching = (int)nudAttemptsWhileMatching.Value;
		}

		private void nudMinIOD_ValueChanged(object sender, EventArgs e)
		{
			this.MinimalInterOcularDistance = (int)nudMinIOD.Value;
		}

		private void nudMaxIOD_ValueChanged(object sender, EventArgs e)
		{
			this.MaximumInterOcularDistance = (int)this.nudMaxIOD.Value;
		}

		private void nudGeneralizationTreshold_ValueChanged(object sender, EventArgs e)
		{
			this.GeneralizationThreshold = (double)nudGeneralizationTreshold.Value;
		}

		private void nudImageCount_ValueChanged(object sender, EventArgs e)
		{
			this.ImageCount = (int)nudImageCount.Value;
		}

		private void nudMatchingTreshold_ValueChanged(object sender, EventArgs e)
		{
			this.MatchingThreshold = (double)nudMatchingTreshold.Value;
		}

		private void nudMatchingAttempts_ValueChanged(object sender, EventArgs e)
		{
			this.MatchingAttempts = (int)nudMatchingAttempts.Value;
		}

		private void nudMaxMatchingResults_ValueChanged(object sender, EventArgs e)
		{
			this.MaximumMatchingResults = (int)nudMaxMatchingResults.Value;
		}

		#endregion
	}
}
