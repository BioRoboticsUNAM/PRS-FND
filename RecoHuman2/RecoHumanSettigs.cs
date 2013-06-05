using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace RecoHuman
{
	/// <summary>
	/// Represents the method that will handle the RecoHumanSettingsChanged event
	/// </summary>
	/// <param name="settings">Settings which value has changed</param>
	public delegate void RecoHumanSettingsChangedEH(RecoHumanSettigs settings);

	/// <summary>
	/// Represents s group of settings for RecoHuman application
	/// </summary>
	[XmlRootAttribute(ElementName = "RecoHumanSettigs", IsNullable = false)]
	public class RecoHumanSettigs
	{
		#region Variables

		/// <summary>
		/// Gets or sets the number of attemps to realize while Enrolling
		/// </summary>
		private int attemptsWhileEnrolling;
		/// <summary>
		/// Gets or sets the number of attemps to realize while Matching
		/// </summary>
		private int attemptsWhileMatching;
		/// <summary>
		/// Gets or sets the Generalization Threshold
		/// </summary>
		private double generalizationThreshold;
		/// <summary>
		/// Gets or sets the Image Count
		/// </summary>
		private int imageCount;
		/// <summary>
		/// Gets or sets the minimal interocular distance
		/// </summary>
		private int minimalInterOcularDistance;
		/// <summary>
		/// Gets or sets the maximum interocular distance
		/// </summary>
		private int maximumInterOcularDistance;
		/// <summary>
		/// Gets or sets the Matching Attempts
		/// </summary>
		private int matchingAttempts;
		/// <summary>
		/// Gets or sets the Matching Threshold
		/// </summary>
		private double matchingThreshold;
		/// <summary>
		/// Gets or sets the maximum matching results
		/// </summary>
		private int maximumMatchingResults;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the number of attemps to realize while Enrolling
		/// </summary>
		public int AttemptsWhileEnrolling
		{
			get { return attemptsWhileEnrolling; }
			set
			{
				if (attemptsWhileEnrolling == value) return;
				if (value > 100) attemptsWhileEnrolling = 100;
				else if (value < 1) attemptsWhileEnrolling = 1;
				else attemptsWhileEnrolling = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the number of attemps to realize while Matching
		/// </summary>
		public int AttemptsWhileMatching
		{
			get { return attemptsWhileMatching; }
			set
			{
				if (attemptsWhileMatching == value) return;
				if (value > 100) attemptsWhileMatching = 100;
				else if (value < 1) attemptsWhileMatching = 1;
				else attemptsWhileMatching = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the Generalization Threshold
		/// </summary>
		public double GeneralizationThreshold
		{
			get { return generalizationThreshold; }
			set
			{
				if (generalizationThreshold == value) return;
				if (value > 1) generalizationThreshold = 1;
				else if (value < 0) generalizationThreshold = 0;
				else generalizationThreshold = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the Generalization Threshold
		/// </summary>
		public int ImageCount
		{
			get { return imageCount; }
			set
			{
				if (imageCount == value) return;
				if (value > 4) imageCount = 4;
				else if (value < 2) imageCount = 2;
				else imageCount = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the minimal interocular distance
		/// </summary>
		public int MinimalInterOcularDistance
		{
			get { return minimalInterOcularDistance; }
			set
			{
				if (minimalInterOcularDistance == value) return;
				if (value > 3000) minimalInterOcularDistance = 3000;
				else if (value < 40) minimalInterOcularDistance = 40;
				else minimalInterOcularDistance = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the maximum interocular distance
		/// </summary>
		public int MaximumInterOcularDistance
		{
			get { return maximumInterOcularDistance; }
			set
			{
				if (maximumInterOcularDistance == value) return;
				if (value > 3000) maximumInterOcularDistance = 3000;
				else if (value < 40) maximumInterOcularDistance = 40;
				else maximumInterOcularDistance = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the maximum matching results
		/// </summary>
		public int MaximumMatchingResults
		{
			get { return maximumMatchingResults; }
			set
			{
				if (maximumMatchingResults == value) return;
				if (value < 1) matchingAttempts = 1;
				else maximumMatchingResults = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the Matching Attempts
		/// </summary>
		public int MatchingAttempts
		{
			get { return matchingAttempts; }
			set
			{
				if (matchingAttempts == value) return;
				if (value > 30) matchingAttempts = 30;
				else if (value < 1) matchingAttempts = 1;
				else matchingAttempts = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the Matching Threshold
		/// </summary>
		public double MatchingThreshold
		{
			get { return matchingThreshold; }
			set
			{
				if (matchingThreshold == value) return;
				if (value > 1) matchingThreshold = 1;
				else if (value < 0) matchingThreshold = 0;
				else matchingThreshold = value;
				if (RecoHumanSettingsChanged != null)
					RecoHumanSettingsChanged(this);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Raises when a value in the settings has changed
		/// </summary>
		public event RecoHumanSettingsChangedEH RecoHumanSettingsChanged;

		#endregion

		/// <summary>
		/// Gets default settings for application
		/// </summary>
		public static RecoHumanSettigs Default
		{
			get
			{
				RecoHumanSettigs settings = new RecoHumanSettigs();
				settings.attemptsWhileEnrolling = 10;
				settings.attemptsWhileMatching = 3;
				settings.generalizationThreshold = 0.625f;
				settings.imageCount = 3;
				settings.maximumInterOcularDistance = 3000;
				settings.maximumMatchingResults = 10;
				settings.matchingThreshold = 0.650f;
				settings.matchingAttempts = 10;
				settings.minimalInterOcularDistance = 40;
				return settings;
			}
		}

		public static bool Save(string path, RecoHumanSettigs settings)
		{
			XmlSerializer xs = new XmlSerializer(typeof(RecoHumanSettigs));
			FileStream fs;

			try
			{
				fs = File.OpenWrite(path);
				xs.Serialize(fs, settings);
				fs.Close();
				return true;
			}
			catch{return false;}
		}

		public static RecoHumanSettigs Load(string path)
		{
			XmlSerializer xs = new XmlSerializer(typeof(RecoHumanSettigs));
			FileStream fs;
			RecoHumanSettigs settings;

			try
			{
				fs = File.OpenRead(path);
				settings = (RecoHumanSettigs)xs.Deserialize(fs);
				fs.Close();
				return settings;
			}
			catch{return null;}
		}
	}
}
