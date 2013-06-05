using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RecoHuman
{
	public delegate void FaceEventHandler(Face face);

	public partial class CtrlKnownFace : UserControl
	{
		#region Variables

		/// <summary>
		/// Stores asociated face
		/// </summary>
		private Face face;

		#endregion

		#region Constructors

		private CtrlKnownFace()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of CtrlKnownFace
		/// </summary>
		/// <param name="face">Face used to create control</param>
		public CtrlKnownFace(Face face)
			: this()
		{
			this.face = face;
			this.pbImage.Image = face.OriginalBitmap;
			this.pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
			this.txtName.Text = face.Name;
		}

		#endregion

		#region Events
		/// <summary>
		/// Raises when the Delete button is pressed
		/// </summary>
		public event FaceEventHandler DeleteClick;
		#endregion

		#region Event Handler Methods

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if ((face != null) && (DeleteClick != null))
				DeleteClick(face);
		}

		private void txtName_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Space:
					e.SuppressKeyPress = true;
					break;

				case Keys.Enter:
					face.Name = txtName.Text;
					e.SuppressKeyPress = true;
					break;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			string ext;
			int extPos;
			System.Drawing.Imaging.ImageFormat format;

			dlgSaveThumbnail.FileName = face.Name;

			if (dlgSaveThumbnail.ShowDialog() != DialogResult.OK)
				return;

			extPos = dlgSaveThumbnail.FileName.LastIndexOf('.') + 1;
			ext = ((extPos != 0) && ((dlgSaveThumbnail.FileName.Length - extPos) > 0)) ?
				dlgSaveThumbnail.FileName.Substring(extPos, dlgSaveThumbnail.FileName.Length - extPos) : "bmp";
			switch (ext.ToLower())
			{
				case "bmp":
					format = System.Drawing.Imaging.ImageFormat.Bmp;
					break;

				case "gif":
					format = System.Drawing.Imaging.ImageFormat.Gif;
					break;

				case "jpg":
					format = System.Drawing.Imaging.ImageFormat.Jpeg;
					break;

				case "png":
					format = System.Drawing.Imaging.ImageFormat.Png;
					break;

				default:
					format = System.Drawing.Imaging.ImageFormat.Jpeg;
					break;
			}
			face.OriginalBitmap.Save(dlgSaveThumbnail.FileName, format);
		}

		#endregion
		
	}
}
