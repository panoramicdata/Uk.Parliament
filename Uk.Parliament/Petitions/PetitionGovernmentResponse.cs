using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	/// A government response to a petition
	/// </summary>
	[DataContract]
	[UsedImplicitly]
	public class PetitionGovernmentResponse
	{
		/// <summary>
		/// The summary
		/// </summary>
		[DataMember(Name = "summary")]
		public string Summary { get; set; }

		/// <summary>
		/// The details
		/// </summary>
		[DataMember(Name = "details")]
		public string Details { get; set; }

		/// <summary>
		/// When the response was created
		/// </summary>
		[DataMember(Name = "created_at")]
		public string CreatedAtUtc { get; set; }

		/// <summary>
		/// When the response was updated
		/// </summary>
		[DataMember(Name = "updated_at")]
		public string UpdatedAtUtc { get; set; }
	}
}