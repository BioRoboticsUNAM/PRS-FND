using System;
using System.Net;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;

namespace RecoHuman
{
	static class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
#if !DEBUG
			if (!UniqueProcess())
				return;
#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (!StartService())
				return;
			//VideoPipeAdapter vpa = new VideoPipeAdapter("kinectRGB");
			//vpa.Start();
			FrmRecoHuman form = new FrmRecoHuman();
			if (args.Length > 0)
				parseArgs(form, args);
			Application.Run(form);
		}

		private static bool UniqueProcess()
		{
			Process[] copies;
			Process current;
			int i;

			current = Process.GetCurrentProcess();
			copies = Process.GetProcessesByName(current.ProcessName);
			// Unique instance, just quit
			if (copies.Length == 1)
			{
				current.Dispose();
				copies[0].Dispose();
				return true;
			}

			// Kill dead processess
			for (i = 0; i < copies.Length; ++i)
			{
				try
				{
					if (copies[i].Handle == current.Handle)
						continue;
					if (!copies[i].Responding)
						copies[i].Kill();
				}
				catch { }
				copies[i].Dispose();
			}

			// Wait untill processes die, get the processes count again,
			// if there is more than one, exit application
			copies = Process.GetProcessesByName(current.ProcessName);
			for (i = 0; i < copies.Length; ++i)
				copies[i].Dispose();
			current.Dispose();
			return copies.Length == 1;
		}

		private static bool StartService()
		{
			ServiceController sc = new ServiceController("Neurotechnologija");
			try
			{
				sc.Status.ToString();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			bool checkComplete = false;
			int i;

			for (i = 0; i < 10 && !checkComplete; ++i)
			{
				switch (sc.Status)
				{
					case ServiceControllerStatus.StartPending:
					case ServiceControllerStatus.Running:
						return true;

					case ServiceControllerStatus.Stopped:
						sc.Start();
						checkComplete = true;
						break;

					case ServiceControllerStatus.Paused:
						sc.Continue();
						checkComplete = true;
						return true;
				}
				System.Threading.Thread.Sleep(100);
			}

			for (i = 0; i < 10; ++i)
			{
				if(sc.Status != ServiceControllerStatus.Running)
					System.Threading.Thread.Sleep(200);
			}
			sc.Dispose();
			return true;
		}

		private static void parseArgs(FrmRecoHuman form, string[] args)
		{
			int resultInt;
			IPAddress resultIP;
			for (int i = 0; i < args.Length; ++i)
			{

				switch (args[i].ToLower())
				{
					case "-a":
						if (++i > args.Length) return;
						if (IPAddress.TryParse(args[i], out resultIP))
							form.TcpServerAddress = resultIP;
						break;

					case "-c":
						if (++i > args.Length) return;
						if (Int32.TryParse(args[i], out resultInt))
							form.CameraNumber = resultInt;
						break;

					case "-h":
					case "--h":
					case "-help":
					case "--help":
					case "/h":
						showHelp();
						break;

					case "-r":
						if (++i > args.Length) return;
						if (Int32.TryParse(args[i], out resultInt) && (resultInt >= 0))
							form.TcpPortIn = resultInt;
						break;

					/*
					case "-vca":
						if (++i > args.Length) return;
						if (IPAddress.TryParse(args[i], out resultIP))
							form.VideoClientAddress = resultIP;
						break;

					case "-vcp":
						if (++i > args.Length) return;
						if (Int32.TryParse(args[i], out resultInt) && (resultInt >= 0))
							form.VideoClientPort = resultInt;
						break;
					*/

					case "-w":
						if (++i > args.Length) return;
						if (Int32.TryParse(args[i], out resultInt) && (resultInt >= 0))
							form.TcpPortOut = resultInt;
						break;
				}
			}
		}

		private static void showHelp()
		{
			Console.WriteLine("Motion Planner Help");
			Console.WriteLine("-a\t\tTcp server Address");
			Console.WriteLine("-r\t\tTcp input port (server)");
			Console.WriteLine("-vca\t\tVideoClient Addres");
			Console.WriteLine("-vcp\t\ttVideoClient port");
			Console.WriteLine("-w\t\tTcp output port (client)");
			Application.Exit();
		}
	}
}