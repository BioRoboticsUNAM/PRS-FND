using System;
using Robotics.API;

namespace RecoHuman.CommandExecuters
{

	/// <summary>
	/// Command executer for the pf_remember command
	/// </summary>
	public class PfRemember : AsyncCommandExecuter
	{
		/// <summary>
		/// The human recognizer engine
		/// </summary>
		private HumanRecognizer engine;

		/// <summary>
		/// Initializes a new instance of PfRemember
		/// </summary>
		/// <param name="engine"></param>
		public PfRemember(HumanRecognizer engine)
			: base("pf_remember")
		{
			if (engine == null)
				throw new ArgumentNullException();
			this.engine = engine;
		}

		/// <summary>
		/// Gets the human recognizer engine
		/// </summary>
		private HumanRecognizer Engine
		{
			get { return this.engine; }
		}

		/// <summary>
		/// Gets a value indicating if the command requires parameters
		/// </summary>
		public override bool ParametersRequired
		{
			get { return true; }
		}

		protected override Response AsyncTask(Command command)
		{
			bool result;
			string pName;

			if (engine.Busy)
				return Response.CreateFromCommand(command, false);

			engine.SleepCapture = false;
			pName = command.Parameters;
			result = engine.RememberHuman(pName, 3);
			return Response.CreateFromCommand(command, result);
		}

		public override void DefaultParameterParser(string[] parameters)
		{
			return;
		}
	}
}
