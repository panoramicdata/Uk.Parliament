using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	/// Petition attributes
	/// </summary>
	[DataContract]
	public class PetitionAttributes
	{
		/// <summary>
		/// The action
		/// </summary>
		[DataMember(Name = "action")]
		public string Action { get; set; }

		/// <summary>
		/// The background
		/// </summary>
		[DataMember(Name = "background")]
		public string Background { get; set; }

		/// <summary>
		/// Additional details
		/// </summary>
		[DataMember(Name = "additional_details")]
		public string AdditionalDetails { get; set; }

		/// <summary>
		/// The current state
		/// </summary>
		[DataMember(Name = "state")]
		public PetitionState State { get; set; }

		/// <summary>
		/// The number of signatures
		/// </summary>
		[DataMember(Name = "signature_count")]
		public int SignatureCount { get; set; }

		/// <summary>
		/// created_at
		/// </summary>
		[DataMember(Name = "created_at")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// updated_at
		/// </summary>
		[DataMember(Name = "updated_at")]
		public DateTime? UpdatedAtUtc { get; set; }

		/// <summary>
		/// open_at
		/// </summary>
		[DataMember(Name = "open_at")]
		public DateTime? OpenAtUtc { get; set; }

		/// <summary>
		/// closed_at
		/// </summary>
		[DataMember(Name = "closed_at")]
		public DateTime? ClosedAtUtc { get; set; }

		/// <summary>
		/// government_response_at
		/// </summary>
		[DataMember(Name = "government_response_at")]
		public DateTime? GovernmentResponseAtUtc { get; set; }

		/// <summary>
		/// The scheduled_debate_date
		/// </summary>
		[DataMember(Name = "scheduled_debate_date")]
		public DateTime? ScheduledDebateDate { get; set; }

		/// <summary>
		/// debate_threshold_reached_at
		/// </summary>
		[DataMember(Name = "debate_threshold_reached_at")]
		public DateTime? DebateThresholdReachedAtUtc { get; set; }

		/// <summary>
		/// rejected_at
		/// </summary>
		[DataMember(Name = "rejected_at")]
		public DateTime? RejectedAt { get; set; }

		/// <summary>
		/// debate_outcome_at
		/// </summary>
		[DataMember(Name = "debate_outcome_at")]
		public DateTime? DebateOutcomeAt { get; set; }

		/// <summary>
		/// moderation_threshold_reached_at
		/// </summary>
		[DataMember(Name = "moderation_threshold_reached_at")]
		public DateTime? ModerationThresholdReachedAt { get; set; }

		/// <summary>
		/// The creator_name
		/// </summary>
		[DataMember(Name = "creator_name")]
		public string CreatorName { get; set; }

		/// <summary>
		/// The rejection
		/// </summary>
		[DataMember(Name = "rejection")]
		public string Rejection { get; set; }

		/// <summary>
		/// The government response
		/// </summary>
		[DataMember(Name = "government_response")]
		public PetitionGovernmentResponse GovernmentResponse { get; set; }

		/// <summary>
		/// The debate
		/// </summary>
		[DataMember(Name = "debate")]
		public PetitionDebate Debate { get; set; }

		/// <summary>
		///  The petition signatures by country
		/// </summary>
		[DataMember(Name = "signatures_by_country")]
		public List<CountrySignatures> SignaturesByCountry { get; set; }

		/// <inheritdoc />
		public override string ToString() => $"{Action ?? "Invalid petition"}";
	}
}