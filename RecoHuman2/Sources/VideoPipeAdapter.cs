using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace RecoHuman.Sources
{
	/// <summary>
	/// Implements an image source that receives video images from a pipe
	/// </summary>
	public class VideoPipeAdapter : IImageSource
	{
		#region Variables

		/// <summary>
		/// Asynchronously capture images and stores it into a Queue
		/// </summary>
		private Thread mainThread;

		/// <summary>
		/// Indicates if main thread is running
		/// </summary>
		protected bool running;

		/// <summary>
		/// flag that indicates if the adapter is ready.
		/// </summary>
		private bool ready;

		/// <summary>
		/// Stores the last captured image
		/// </summary>
		private Bitmap lastCapturedImage;

		/// <summary>
		/// Event to wait until a new image is captured
		/// </summary>
		private AutoResetEvent imageCapturedEvent;

		/// <summary>
		/// The pipe used to acquire data
		/// </summary>
		private NamedPipeClientStream pipeClient;

		/// <summary>
		/// Stores the name of the pipe
		/// </summary>
		private string pipeName;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of VideoPipeAdapter
		/// </summary>
		public VideoPipeAdapter()
		{
			this.ready = false;
			this.running = false;
		}

		/// <summary>
		/// Initializes a new instance of VideoPipeAdapter
		/// </summary>
		/// <param name="pipeName">The name of the pipe to connect</param>
		public VideoPipeAdapter(string pipeName) : this()
		{
			this.PipeName = pipeName;
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when a new image is received and becomes availiable
		/// </summary>
		public event ImageProducedEventHandler ImageProduced;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the name of the pipe
		/// </summary>
		public string PipeName
		{
			get { return pipeName; }
			set
			{
				if (running)
					throw new Exception("Can not change pipe name while running");
				if (String.IsNullOrEmpty(value))
					throw new ArgumentNullException();
				pipeName = value;
			}
		}

		/// <summary>
		/// Gets the type of the Image Source
		/// </summary>
		public ImageSourceType SourceType { get { return ImageSourceType.ImageServer; } }

		#endregion

		#region Methods

		public Bitmap GetImage()
		{
			return lastCapturedImage;
		}

		public Bitmap GetImage(int timeout)
		{
			return lastCapturedImage;
		}


		/// <summary>
		/// Configures the pipe
		/// </summary>
		private void SetupPipe()
		{
			if (this.pipeClient != null)
			{
				if (this.pipeClient.IsConnected)
					this.pipeClient.Close();
				this.pipeClient.Dispose();
			}
			this.pipeClient = new NamedPipeClientStream(
				".",
				pipeName,
				PipeDirection.In,
				PipeOptions.None,
				TokenImpersonationLevel.Impersonation);
			//pipeClient.ReadMode = PipeTransmissionMode.Message;
		}

		/// <summary>
		/// Configures the thread
		/// </summary>
		private void SetupThread()
		{
			if ((mainThread != null) && mainThread.IsAlive)
			{
				mainThread.Abort();
				mainThread.Join(100);
				mainThread = null;
			}
			mainThread = new Thread(new ThreadStart(MainThreadTask));
			mainThread.IsBackground = true;
			mainThread.Start();
		}
		
		/// <summary>
		/// Starts the WebCam adapter
		/// </summary>
		public void Start()
		{
			if (running)
				return;
			running = true;
			SetupPipe();
			SetupThread();
		}

		/// <summary>
		/// Stops the  WebCam adapter
		/// </summary>
		public void Stop()
		{
			if (!running || (mainThread == null) || !mainThread.IsAlive)
				return;
			running = false;
			mainThread.Join(200);
			if ((mainThread != null) && mainThread.IsAlive)
			{
				mainThread.Abort();
				mainThread.Join(100);
				mainThread = null;
			}
		}

		/// <summary>
		/// Rises the ImageCaptured and ImageProduced events, and updates the LastCapturedImage variable
		/// </summary>
		/// <param name="capturedImage">The captured image</param>
		protected void OnImageCaptured(Bitmap capturedImage)
		{
			lastCapturedImage = capturedImage;
			if (this.ImageProduced != null)
				this.ImageProduced(this);

		}

		/// <summary>
		/// Executes the main tasks
		/// </summary>
		protected void MainThreadTask()
		{
			Bitmap bmp;
			//byte[] buffer = new byte[2359326];
			// 10MB buffer must be enough
			byte[] buffer = new byte[1024*10240];

			while (running)
			{
				if (!pipeClient.IsConnected)
				{
					try
					{
						if (!ExistNamedPipe(this.pipeName))
						{
							Thread.Sleep(100);
							continue;
						}
						pipeClient.Connect(0);
					}
					catch (ThreadAbortException) { break; }
					catch (TimeoutException) { Thread.Sleep(100); continue; }
					catch { continue; }
				}

				while (running && pipeClient.IsConnected && (pipeClient.NumberOfServerInstances == 0))
					Thread.Sleep(100);
				if (!running) break;

				try
				{
					if (pipeClient.Read(buffer, 0, buffer.Length) < 30)
					{
						Thread.Sleep(1);
						continue;
					}
					bmp = GetBitmap(buffer);
					if (bmp == null)
						continue;
					OnImageCaptured(bmp);
				}
				catch (ThreadAbortException) { break; }
				catch { continue; }
				
			}
			if(pipeClient.IsConnected)
				pipeClient.Close();
			pipeClient.Dispose();
		}

		/// <summary>
		/// Gets a bitmap from the pipe
		/// </summary>
		/// <returns>Bitmap readed from the pipa</returns>
		private Bitmap GetBitmap(byte[] data)
		{
			// Header 18 bytes: 0x007E  followed by the null-terminated string "kinect.rgb.pipe"
			// Image size 4 bytes: Width and heigh as unsigned 16 bit integers in (little endian)
			// Content
			// Checksum 4 bytes: 1 - sum(content)
			// Footer 4 bytes 0x0004FF02

			int i;
			int imageDataIndex;
			int width;
			int height;
			int size;
			int stride;
			int checksum;
			int sum;

			// Read Header
			i = 0;
			while (running)
			{
				if((i+30)>=data.Length)
					return null;
				if (data[i++] != 0)
					continue;
				if (data[i++] != 0x7E)
					continue;
				//string s = ASCIIEncoding.ASCII.GetString(data, i, 16);
				if (String.Compare(ASCIIEncoding.ASCII.GetString(data, i, 16), "kinect.rgb.pipe\0") == 0)
					break;
			}
			if (!running)
				return null;
			// Read Image size
			if (data.Length <= (i + 4))
				return null;
			width = BitConverter.ToUInt16(data, i);
			i += 2;
			height = BitConverter.ToUInt16(data, i);
			// Width-height relation in a image always is 4-3 so it need to calculate stride as:
			// stride = ((width * bpp + 7) / 8) * 8;
			// Refer to: http://msdn.microsoft.com/en-us/library/windows/desktop/aa473780(v=vs.85).aspx
			stride = ((width * 3 + 7) / 8) * 8;
			// now the size is calculated using the stride
			size = stride * height;

			// Check packet size and get image data index
			if (data.Length <= (i + size + 8))
				return null;
			imageDataIndex = i;

			// Validate checksum
			sum = 0;
			unchecked
			{
				for (int j = 0; j < size; ++i, ++j)
					sum += data[i];
				sum = 1 - sum;
			}
			checksum = BitConverter.ToInt32(data, i);

			if (sum != checksum)
				return null;

			// Read footer
			if (data[i++] != 0x00)
				return null;
			if (data[i++] != 0x04)
				return null;
			if (data[i++] != 0xFF)
				return null;
			if (data[i++] != 0x02)
				return null;

			//Get bitmap
			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			Rectangle rect = new Rectangle(0, 0, width, height);
			BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
			IntPtr scan0 = bitmapData.Scan0;
			Marshal.Copy(data, imageDataIndex, scan0, size);
			bmp.UnlockBits(bitmapData);
			return bmp;
		}

		/// <summary>
		/// Provides an indication if the named pipe exists. 
		/// This has to prove the pipe does not exist.
		/// </summary>
		/// <param name="pipeName">The pipe to connect to</param>
		/// <returns>true if the pipe exist, otherwise false</returns>
		/// <see cref="http://social.msdn.microsoft.com/Forums/pl/netfxnetcom/thread/7bbf5a0b-3c22-4836-b271-999e514c321b"/>
		static public bool ExistNamedPipe(string pipeName)
		{
			try
			{
				string normalizedPath = System.IO.Path.GetFullPath(String.Format(@"\\.\pipe\{0}", pipeName));
				bool exists = WaitNamedPipe(normalizedPath, 0);
				if (!exists)
				{
					int error = Marshal.GetLastWin32Error();
					switch (error)
					{
						// pipe does not exist
						case 0: return false;
						// ERROR_FILE_NOT_FOUND 2L
						case 2: return false;
						// ERROR_BROKEN_PIPE =  109 (0x6d)
						case 109: return true;
						// ERROR_BAD_PATHNAME  161L The specified path is invalid.
						case 161: return false;
						// ERROR_BAD_PIPE =  230  (0xe6) The pipe state is invalid.
						case 230: return false;
						// ERROR_PIPE_BUSY =  231 (0xe7) All pipe instances are busy.
						case 231: return true;
						// ERROR_NO_DATA =   232   (0xe8) the pipe is being closed
						case 232: return false;
						// ERROR_PIPE_NOT_CONNECTED 233L No process is on the other end of the pipe.
						case 233: return true;
						// ERROR_PIPE_CONNECTED        535L There is a process on other end of the pipe.
						case 535: return true;
						// ERROR_PIPE_LISTENING        536L Waiting for a process to open the other end of the pipe.
						case 536: return true;

						default:
							return false;
					}
				}
				return true;
			}
			catch (Exception)
			{
				return false; // assume it NOT exists
			}
		}

		/// <summary>
		/// Provides an indication if the named pipe exists. 
		/// This has to prove the pipe does not exist. If there is any doubt, this
		/// returns that it does exist and it is up to the caller to attempt to connect
		/// to that server. The means that there is a wide variety of errors that can occur that
		/// will be ignored - for example, a pipe name that contains invalid characters will result
		/// in a return value of false.
		/// 
		/// </summary>
		/// <param name="pipeName">The pipe to connect to</param>
		/// <returns>false if it can be proven it does not exist, otherwise true</returns>
		/// <remarks>
		/// Attempts to check if the pipe server exists without incurring the cost
		/// of calling NamedPipeClientStream.Connect. This is because Connect either 
		/// times out and throws an exception or goes into a tight spin loop burning
		/// up cpu cycles if the server does not exist.
		/// 
		/// Common Error codes from WinError.h
		/// ERROR_FILE_NOT_FOUND 2L
		/// ERROR_BROKEN_PIPE =  109 (0x6d)
		/// ERROR_BAD_PATHNAME  161L The specified path is invalid.
		/// ERROR_BAD_PIPE =  230  (0xe6) The pipe state is invalid.
		/// ERROR_PIPE_BUSY =  231 (0xe7) All pipe instances are busy.
		/// ERROR_NO_DATA =   232   (0xe8) the pipe is being closed
		/// ERROR_PIPE_NOT_CONNECTED 233L No process is on the other end of the pipe.
		/// ERROR_PIPE_CONNECTED        535L There is a process on other end of the pipe.
		/// ERROR_PIPE_LISTENING        536L Waiting for a process to open the other end of the pipe.
		/// 
		/// </remarks>
		/// <see cref="http://social.msdn.microsoft.com/Forums/pl/netfxnetcom/thread/7bbf5a0b-3c22-4836-b271-999e514c321b"/>
		static public bool NamedPipeDoesNotExist(string pipeName)
		{
			try
			{
				int timeout = 0;
				string normalizedPath = System.IO.Path.GetFullPath(String.Format(@"\\.\pipe\{0}", pipeName));
				bool exists = WaitNamedPipe(normalizedPath, timeout);
				if (!exists)
				{
					int error = Marshal.GetLastWin32Error();
					if (error == 0) // pipe does not exist
						return true;
					else if (error == 2) // win32 error code for file not found
						return true;
					// all other errors indicate other issues
				}
				return false;
			}
			catch (Exception)
			{
				//Exception.Publish("Failure in WaitNamedPipe()", ex);
				return true; // assume it exists
			}
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool WaitNamedPipe(string name, int timeout);

		#endregion
	}
}
