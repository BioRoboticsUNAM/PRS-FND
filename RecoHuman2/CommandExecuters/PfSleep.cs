using System;
using Robotics.API;

namespace RecoHuman.CommandExecuters
{
	/// <summary>
	/// Command executer for the pf_sleep command
	/// </summary>
	public class PfSleep : SyncCommandExecuter
	{
		/// <summary>
		/// The human recognizer engine
		/// </summary>
		private HumanRecognizer engine;

		/// <summary>
		/// Initializes a new instance of PfSleep
		/// </summary>
		/// <param name="engine"></param>
		public PfSleep(HumanRecognizer engine)
			: base("pf_sleep")
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
			string p = command.HasParams ? command.Parameters : String.Empty;

			if(String.IsNullOrEmpty(p) || (String.Compare(p, "get", true) == 0))
			{
				command.Parameters = engine.SleepCapture ? "enable" : "disable";
				return Response.CreateFromCommand(command, true);
			}
			else if(command.Parameters.StartsWith("enable", StringComparison.InvariantCultureIgnoreCase))
			{
				engine.SleepCapture = true;
				command.Parameters = "enable";
				return Response.CreateFromCommand(command, true);
			}
			else if (command.Parameters.StartsWith("disable", StringComparison.InvariantCultureIgnoreCase))
			{
				engine.SleepCapture = false;
				command.Parameters = "disable";
				return Response.CreateFromCommand(command, true);
			}
			return Response.CreateFromCommand(command, false);
		}
	}
}
