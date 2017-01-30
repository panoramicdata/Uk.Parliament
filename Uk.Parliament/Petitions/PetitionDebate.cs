using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	/// Petition debate
	/// </summary>
	[UsedImplicitly]
	[DataContract]
	public class PetitionDebate
	{
		/// <summary>
		/// The date on which the petition was debated
		/// </summary>
		[DataMember(Name = "summary")]
		public DateTime DebatedOnDate { get; set; }

		/// <summary>
		/// The transcript url
		/// </summary>
		[DataMember(Name = "transcript_url")]
		public string TranscriptUrl { get; set; }

		/// <summary>
		/// The video_url
		/// </summary>
		[DataMember(Name = "video_url")]
		public string VideoUrl { get; set; }

		/// <summary>
		/// The overview
		/// </summary>
		[DataMember(Name = "overview")]
		public string Overview { get; set; }
	}
}