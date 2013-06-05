using System;
using Robotics.API;

namespace RecoHuman.CommandExecuters
{
	/// <summary>
	/// Command executer for the pf_find command
	/// </summary>
	public class PfFind : AsyncCommandExecuter
	{
		/// <summary>
		/// The human recognizer engine
		/// </summary>
		private HumanRecognizer engine;

		/// <summary>
		/// Initializes a new instance of PfFind
		/// </summary>
		/// <param name="engine"></param>
		public PfFind(HumanRecognizer engine)
			: base("pf_find")
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
			double hFoV;
			double vFoV;

			// TODO: Parse command parameters

			if (engine.Busy)
				return Response.CreateFromCommand(command, false);
			engine.SleepCapture = false;
			pName = command.Parameters;
			result = engine.FindHuman(ref pName, out hFoV, out vFoV);
			if (Double.IsNaN(hFoV) || Double.IsNaN(vFoV))
				command.Parameters = pName;
			else
				command.Parameters = pName + " " + hFoV.ToString("0.0000") + " " + vFoV.ToString("0.0000");
			return Response.CreateFromCommand(command, result);
		}

		public override void DefaultParameterParser(string[] parameters)
		{
			return;
		}
	}
}
