using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

/// <summary>
/// Petition attributes
/// </summary>
[DataContract]
public class PetitionAttributes
{
	/// <summary>
	/// The action
	/// </summary>
	[JsonPropertyName("action")]
	public string Action { get; set; }

	/// <summary>
	/// The background
	/// </summary>
	[JsonPropertyName("background")]
	public string Background { get; set; }

	/// <summary>
	/// Additional details
	/// </summary>
	[JsonPropertyName("additional_details")]
	public string AdditionalDetails { get; set; }

	/// <summary>
	/// Committee note
	/// </summary>
	[JsonPropertyName("committee_note")]
	public string CommitteeNote { get; set; }

	/// <summary>
	/// The current state
	/// </summary>
	[JsonPropertyName("state")]
	public PetitionState State { get; set; }

	/// <summary>
	/// The number of signatures
	/// </summary>
	[JsonPropertyName("signature_count")]
	public int SignatureCount { get; set; }

	/// <summary>
	/// created_at
	/// </summary>
	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// updated_at
	/// </summary>
	[JsonPropertyName("updated_at")]
	public DateTime? UpdatedAt { get; set; }

	/// <summary>
	/// opened_at
	/// </summary>
	[JsonPropertyName("opened_at")]
	public DateTime? OpenedAt { get; set; }

	/// <summary>
	/// closed_at
	/// </summary>
	[JsonPropertyName("closed_at")]
	public DateTime? ClosedAt { get; set; }

	/// <summary>
	/// government_response_at
	/// </summary>
	[JsonPropertyName("government_response_at")]
	public DateTime? GovernmentResponseAt { get; set; }

	/// <summary>
	/// response_threshold_reached_at
	/// </summary>
	[JsonPropertyName("response_threshold_reached_at")]
	public DateTime? ResponseThresholdReachedAt { get; set; }

	/// <summary>
	/// The scheduled_debate_date
	/// </summary>
	[JsonPropertyName("scheduled_debate_date")]
	public DateTime? ScheduledDebateDate { get; set; }

	/// <summary>
	/// debate_threshold_reached_at
	/// </summary>
	[JsonPropertyName("debate_threshold_reached_at")]
	public DateTime? DebateThresholdReachedAt { get; set; }

	/// <summary>
	/// rejected_at
	/// </summary>
	[JsonPropertyName("rejected_at")]
	public DateTime? RejectedAt { get; set; }

	/// <summary>
	/// debate_outcome_at
	/// </summary>
	[JsonPropertyName("debate_outcome_at")]
	public DateTime? DebateOutcomeAt { get; set; }

	/// <summary>
	/// moderation_threshold_reached_at
	/// </summary>
	[JsonPropertyName("moderation_threshold_reached_at")]
	public DateTime? ModerationThresholdReachedAt { get; set; }

	/// <summary>
	/// The creator_name
	/// </summary>
	[JsonPropertyName("creator_name")]
	public string CreatorName { get; set; }

	/// <summary>
	/// The rejection
	/// </summary>
	[JsonPropertyName("rejection")]
	public PetitionRejection Rejection { get; set; }

	/// <summary>
	/// The government response
	/// </summary>
	[JsonPropertyName("government_response")]
	public PetitionGovernmentResponse GovernmentResponse { get; set; }

	/// <summary>
	/// The debate
	/// </summary>
	[JsonPropertyName("debate")]
	public PetitionDebate Debate { get; set; }

	/// <summary>
	/// Government departments associated with this petition
	/// </summary>
	[JsonPropertyName("departments")]
	public List<Department> Departments { get; set; }

	/// <summary>
	/// Topics associated with this petition
	/// </summary>
	[JsonPropertyName("topics")]
	public List<Topic> Topics { get; set; }

	/// <summary>
	///  The petition signatures by country
	/// </summary>
	[JsonPropertyName("signatures_by_country")]
	public List<CountrySignatures> SignaturesByCountry { get; set; }

	/// <summary>
	///  The petition signatures by constituency
	/// </summary>
	[JsonPropertyName("signatures_by_constituency")]
	public List<ConstituencySignatures> SignaturesByConstituency { get; set; }

	/// <summary>
	///  The petition signatures by region
	/// </summary>
	[JsonPropertyName("signatures_by_region")]
	public List<RegionSignatures> SignaturesByRegion { get; set; }

	/// <summary>
	/// Other parliamentary business related to this petition
	/// </summary>
	[JsonPropertyName("other_parliamentary_business")]
	public List<object> OtherParliamentaryBusiness { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Action ?? "Invalid petition"}";
}