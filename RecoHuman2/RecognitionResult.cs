using System;
using System.Collections.Generic;
using System.Text;

namespace RecoHuman
{
	public class RecognitionResult : IComparable<RecognitionResult>
	{
		/// <summary>
		/// Stores the Face in which comparation was based
		/// </summary>
		private Face baseFace;
		/// <summary>
		/// Stores the Face compared
		/// </summary>
		private Face comparedFace;
		/// <summary>
		/// Stores the result of the match between faces
		/// </summary>
		private double affinity;

		/// <summary>
		/// Initializrs a new instance of RecognitionResult
		/// </summary>
		/// <param name="baseFace">Face in which comparation was based</param>
		/// <param name="comparedFace">Face compared</param>
		/// <param name="match">Affinity between faces</param>
		public RecognitionResult(Face baseFace, Face comparedFace, double affinity)
		{
			this.baseFace = baseFace;
			this.comparedFace = comparedFace;
			this.affinity = affinity;
		}

		/// <summary>
		/// Gets the Face in which comparation was based
		/// </summary>
		public Face BaseFace
		{
			get { return baseFace; }
		}
		/// <summary>
		/// Gets the Face compared
		/// </summary>
		public Face ComparedFace
		{
			get { return comparedFace; }
		}
		/// <summary>
		/// Gets the affinity between faces obtained as result of match
		/// </summary>
		public double Affinity
		{
			get { return affinity; }
		}

		#region IComparable<Face> Members

		public int CompareTo(RecognitionResult other)
		{
			//return this.affinity.CompareTo(other.affinity);
			return other.affinity.CompareTo(this.affinity);
		}

		#endregion
	}
}
