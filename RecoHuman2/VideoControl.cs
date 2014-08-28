using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Neurotec.Biometrics;

namespace RecoHuman
{
	public partial class VideoControl : System.Windows.Forms.Panel
	{
		
		#region Variables

		/// <summary>
		/// Image to display
		/// </summary>
		private Bitmap image;
		/// <summary>
		/// String to draw
		/// </summary>
		private string drawString;
		/// <summary>
		/// Contains the list of displayed faces
		/// </summary>
		private VleFace[] faces;
		/// <summary>
		/// Stores the face detection details asociated to the image displayed
		/// </summary>
		private VleDetectionDetails[] detectionDetails;
		/// <summary>
		/// Array of pens for enclose faces
		/// </summary>
		private Pen[] pens;
		/// <summary>
		/// Array of pens for enclose faces
		/// </summary>
		private Pen[] translucidPens;
		/// <summary>
		/// Synchronizes the access to the image variable
		/// </summary>
		private ReaderWriterLock rwImageLock;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a nre instance of VideoControl
		/// </summary>
		public VideoControl()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.rwImageLock = new ReaderWriterLock();
			this.image = null;
			this.drawString = null;
			this.faces = null;
			// Array of pens for enclose faces
			pens = new Pen[]
				{
					new Pen(Color.GreenYellow, 3),	// 1
					new Pen(Color.Yellow, 2),		// 2
					new Pen(Color.Aqua, 2),			// 3
					new Pen(Color.Blue, 2),			// 4
					new Pen(Color.Cyan, 2),			// 5
					new Pen(Color.Magenta, 2),		// 6
					new Pen(Color.Orange, 2),		// 7
					new Pen(Color.Red, 2),			// 8
					new Pen(Color.Pink, 2),			// 9
					new Pen(Color.Maroon, 2),		// 10
					new Pen(Color.Silver, 2)		// 11
				};

			translucidPens = new Pen[]
				{
					new Pen(Color.FromArgb(128, Color.GreenYellow), 3),	// 1
					new Pen(Color.FromArgb(128, Color.Yellow), 2),		// 2
					new Pen(Color.FromArgb(128, Color.Aqua), 2),		// 3
					new Pen(Color.FromArgb(128, Color.Blue), 2),		// 4
					new Pen(Color.FromArgb(128, Color.Cyan), 2),		// 5
					new Pen(Color.FromArgb(128, Color.Magenta), 2),		// 6
					new Pen(Color.FromArgb(128, Color.Orange), 2),		// 7
					new Pen(Color.FromArgb(128, Color.Red), 2),			// 8
					new Pen(Color.FromArgb(128, Color.Pink), 2),		// 9
					new Pen(Color.FromArgb(128, Color.Maroon), 2),		// 10
					new Pen(Color.FromArgb(128, Color.Silver), 2)		// 11
				};
		}

		#endregion

		#region Properties

		/// <summary>
		/// Sets the image displayed by the control
		/// </summary>
		public Neurotec.Images.NImage NImage
		{
			set
			{
				if (value == null)
				{
					this.Image = null;
					return;
				}
				this.Image = value.ToBitmap();
			}
		}

		/// <summary>
		/// Gets or sets the image displayed by the control
		/// </summary>
		public Bitmap Image
		{
			get
			{
				Bitmap copy;
				rwImageLock.AcquireReaderLock(-1);
				copy = (image != null) ? new Bitmap(image) : null;
				rwImageLock.ReleaseReaderLock();
				return copy;
			}
			set
			{
				//if (value == null) return;
				rwImageLock.AcquireWriterLock(-1);
				Bitmap lastImage = this.image;

				this.image = value;
				this.faces = null;
				this.drawString = null;
				//this.detectionDetails.EyesAvailable = false;
				//this.detectionDetails.FaceAvailable = false;
				this.Invalidate();

				if (lastImage != null)
					lastImage.Dispose();
				rwImageLock.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Gets or sets the array of faces detected in the image displayed by the control
		/// </summary>
		public VleFace[] Faces
		{
			get { return this.faces; }
			set
			{
				this.faces = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the face detection details asociated to the image displayed
		/// </summary>
		public VleDetectionDetails[] DetectionDetails
		{
			get { return detectionDetails; }
			set
			{ 
				detectionDetails = value;
				//if (detectionDetails.EyesAvailable || detectionDetails.FaceAvailable)
				//	faces = null;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the String to draw
		/// </summary>
		public string String
		{
			get { return drawString; }
			set { drawString = value; }
		}

		#endregion

		#region Methods

		private void DrawDetectionCandidates(Graphics g)
		{
			Pen pen;
			for (int i = 0; i < detectionDetails.Length; ++i)
			{
				if (i > 10) pen = new Pen(Color.FromArgb(128, Color.Gray), 2);//if more than 11 faces found
				else pen = translucidPens[i];
				if (detectionDetails[i].FaceAvailable)
				{
					g.DrawRectangle(pen, detectionDetails[i].Face.Rectangle);
					g.DrawString(
						detectionDetails[i].Face.Confidence.ToString("0.00"),
						new Font(this.Font.FontFamily, 10),
						new SolidBrush(Color.GreenYellow),
						detectionDetails[i].Face.Rectangle.X,
						detectionDetails[i].Face.Rectangle.Y + detectionDetails[i].Face.Rectangle.Height + 4);
				}
				if (detectionDetails[i].EyesAvailable)
				{
					g.DrawLine(pen, detectionDetails[i].Eyes.First, detectionDetails[i].Eyes.Second);
				}
			}
		}

		private void DrawImageBase(Graphics g)
		{
			rwImageLock.AcquireReaderLock(-1);
			try
			{
				//g.DrawImage(image, 0, 0, image.Width, image.Height);
				g.DrawImage(image, 0, 0, this.Width, this.Height);
				if ((image.Height > this.Width) || (image.Width > this.Height))
					g.ScaleTransform((float)this.Width / (float)image.Width, (float)this.Height / (float)image.Height);
				g.DrawLine(Pens.Black, 0, image.Height / 2, image.Width, image.Height / 2);
				g.DrawLine(Pens.Black, image.Width / 2, 0, image.Width / 2, image.Height);
			}
			catch { }
			rwImageLock.ReleaseReaderLock();
		}

		private void DrawRectanglesForFaces(Graphics g)
		{
			Pen pen;
			for (int i = 0; i < faces.Length; ++i)
			{

				if (i > 10) pen = new Pen(Color.Yellow, 2);//if more than 11 faces found
				else pen = pens[i];
				g.DrawRectangle(pens[i], faces[i].Rectangle);
				g.DrawString(
					faces[0].Confidence.ToString("0.00"),
					new Font(this.Font.FontFamily, 10),
					new SolidBrush(pen.Color),
					faces[0].Rectangle.X,
					faces[0].Rectangle.Y + faces[0].Rectangle.Height + 4
				);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			try
			{
				// Draw image base
				if (image != null)
					DrawImageBase(g);

				// Enclose all detected faces
				if (this.faces != null)
					DrawRectanglesForFaces(g);
				// if no faces are detected, show the posible detection
				else if (detectionDetails != null)
					DrawDetectionCandidates(g);

				if (drawString != null)
					g.DrawString(drawString, new Font(this.Font.FontFamily, 18), new SolidBrush(Color.Red), 10, 10);
			}
			catch { }
			base.OnPaint(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) 
		{
			Rectangle rec = e.ClipRectangle;
			rwImageLock.AcquireReaderLock(-1);
			try
			{
				if (image != null)
				{
					Rectangle rec1 = new Rectangle(image.Width, 0, rec.Width - image.Width, rec.Height);
					Rectangle rec2 = new Rectangle(0, image.Height, image.Width, rec.Height - image.Height);
					using (Brush br = new SolidBrush(this.BackColor))
					{
						e.Graphics.FillRectangle(br, rec1);
						e.Graphics.FillRectangle(br, rec2);
					}
				}
				else
				{
					base.OnPaintBackground(e);
				}
			}
			catch { }
			rwImageLock.ReleaseReaderLock();
		}

		#endregion
	}
}
