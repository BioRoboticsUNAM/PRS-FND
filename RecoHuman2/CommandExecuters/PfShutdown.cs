using System;
using Robotics.API;

namespace RecoHuman.CommandExecuters
{
	/// <summary>
	/// Command executer for the pf_shutdown command
	/// </summary>
	public class PfShutdown : SyncCommandExecuter
	{
		/// <summary>
		/// The human recognizer engine
		/// </summary>
		private HumanRecognizer engine;

		/// <summary>
		/// Initializes a new instance of PfShutdown
		/// </summary>
		/// <param name="engine"></param>
		public PfShutdown(HumanRecognizer engine)
			: base("pf_shutdown")
		{
			if (engine == null)
				throw new ArgumentNullException();
			this.engine = engine;
		}

		/// <summary>
		/// Gets a value indicating if the command requires parameters
		/// </summary>
		public override bool ParametersRequired
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the human recognizer engine
		/// </summary>
		private HumanRecognizer Engine
		{
			get { return this.engine; }
		}

		protected override Response SyncTask(Command command)
		{
			// TODO: Close the application
			return null;
		}
	}
}
