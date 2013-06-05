using System;
using System.Text.RegularExpressions;
using Robotics.API;

namespace RecoHuman.CommandExecuters
{
	/// <summary>
	/// Command executer for the pf_auto command
	/// </summary>
	public class PfAuto : SyncCommandExecuter
	{
		/// <summary>
		/// The human recognizer engine
		/// </summary>
		private HumanRecognizer engine;

		/// <summary>
		/// Regular expression used to extract pf_auto params
		/// </summary>
		private Regex rxAutoFind = new Regex(@"(?<mode>(1|0|enable|disable))(\s+(?<pName>\w+))?", RegexOptions.Compiled);

		/// <summary>
		/// Initializes a new instance of PfAuto
		/// </summary>
		/// <param name="engine"></param>
		public PfAuto(HumanRecognizer engine)
			: base("pf_auto")
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
			// TODO: Enable automatic human recognition

			bool enable;
			string mode;
			string name;
			Match m;

			if (!command.HasParams)
			{
				command.Parameters = engine.AutoFindHuman ? "enabled" : "disabled";
				return Response.CreateFromCommand(command, true);
			}

			m = rxAutoFind.Match(command.Parameters);
			if (!m.Success)
				return Response.CreateFromCommand(command, false);
			name = m.Result("${pName}");
			mode = m.Result("${mode}");
			enable = mode.StartsWith("enable") || mode == "1";
			engine.SetupAutoFindHuman(enable, name);
			return Response.CreateFromCommand(command, true);
		}
	}
}
