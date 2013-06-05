using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RecoHuman
{
	public partial class CtrlDetectedFace : UserControl
	{

		/// <summary>
		/// Stores the face attached to this control
		/// </summary>
		private Face face;

		/// <summary>
		/// Initializes a new instance of CtrlDetectedFace
		/// </summary>
		public CtrlDetectedFace()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of CtrlDetectedFace
		/// </summary>
		/// <param name="face">Face asociated to this control</param>
		public CtrlDetectedFace(Face face)
			: this()
		{
			if (face == null)
				return;
			this.face = face;

			//if (this.face.VlFace != null)
			//{
			this.txtLocation.Text = "( " + this.face.VlFace.Rectangle.X.ToString() + ", " + this.face.VlFace.Rectangle.Y.ToString() + " )";
			this.txtSize.Text = this.face.VlFace.Rectangle.Width.ToString() + " x " + this.face.VlFace.Rectangle.Height.ToString() + "px";
			//}

			if ((this.face.OriginalBitmap != null) && (this.face.OriginalBitmap.Width > 10) &&(this.face.OriginalBitmap.Height > 10))
			{
				this.pbFace.Image = new  Bitmap(this.face.OriginalBitmap);
				this.pbFace.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			//else return;

			if ((this.face.Features != null) && (this.face.Features.Length > 0))
				txtFeatures.Text = this.face.Features.Length.ToString() + "bytes";
			else txtFeatures.Text = "None";

			try
			{
				txtConfidence.Text = this.face.VlFace.Confidence.ToString();
			}
			catch
			{
				txtConfidence.Text = "None";
			}

		}

		/// <summary>
		/// Gets the Face asociated to this control
		/// </summary>
		public Face Face
		{
			get
			{
				return face;
			}
		}
	}
}
