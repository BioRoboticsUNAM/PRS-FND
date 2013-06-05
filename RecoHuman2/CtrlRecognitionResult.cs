using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RecoHuman
{
	public partial class CtrlRecognitionResult : UserControl
	{
		/// <summary>
		/// Stores the recognition result attached to this control
		/// </summary>
		private RecognitionResult recognitionResult;

		/// <summary>
		/// Initializes a new instance of CtrlRecognitionResult
		/// </summary>
		private CtrlRecognitionResult()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of CtrlRecognitionResult
		/// </summary>
		/// <param name="recognitionResult">RecognitionResult asociated to this control</param>
		public CtrlRecognitionResult(RecognitionResult recognitionResult) : this()
		{
			if (recognitionResult == null) return;
			this.recognitionResult = recognitionResult;
			if ((this.recognitionResult.BaseFace.Name != null) && (this.recognitionResult.BaseFace.Name.Length > 0))
				this.lblBaseFace.Text = this.recognitionResult.BaseFace.Name;
			else
				this.lblBaseFace.Text = "unknown";
			
			if ((this.recognitionResult.ComparedFace.Name != null) && (this.recognitionResult.ComparedFace.Name.Length > 0))
				this.lblComparedFace.Text = this.recognitionResult.ComparedFace.Name;
			else
				this.lblComparedFace.Text = "unknown";

			if (this.recognitionResult.ComparedFace.OriginalBitmap != null)
			{
				this.pbComparedFace.Image = this.recognitionResult.ComparedFace.OriginalBitmap;
				this.pbComparedFace.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			
			if (this.recognitionResult.BaseFace.OriginalBitmap != null)
			{
				this.pbBaseFace.Image = this.recognitionResult.BaseFace.OriginalBitmap;
				this.pbBaseFace.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			
			if (this.recognitionResult.ComparedFace.OriginalBitmap != null)
			{
				this.pbComparedFace.Image = this.recognitionResult.ComparedFace.OriginalBitmap;
				this.pbComparedFace.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			this.lblAffinity.Text = recognitionResult.Affinity.ToString("0.00");
		}

		/// <summary>
		/// Gets the RecognitionResult asociated to this control
		/// </summary>
		public RecognitionResult RecognitionResult
		{
			get
			{
				return recognitionResult;
			}
		}
	}
}
