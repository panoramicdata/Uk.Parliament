namespace Uk.Parliament.Models.Members;

/// <summary>
/// Represents a parliamentary constituency
/// </summary>
public class Constituency
{
	/// <summary>
	/// The constituency's unique identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// The constituency name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Start date of the constituency
	/// </summary>
	[JsonPropertyName("startDate")]
	public DateTime? StartDate { get; set; }

	/// <summary>
	/// End date of the constituency (null if current)
	/// </summary>
	[JsonPropertyName("endDate")]
	public DateTime? EndDate { get; set; }

	/// <summary>
	/// Current representation details
	/// </summary>
	[JsonPropertyName("currentRepresentation")]
	public CurrentRepresentation? CurrentRepresentation { get; set; }
}

/// <summary>
/// Represents current constituency representation with member and representation details
/// </summary>
public class CurrentRepresentation
{
	/// <summary>
	/// The representing member (wrapped in ValueWrapper)
	/// </summary>
	[JsonPropertyName("member")]
	public ValueWrapper<Member>? Member { get; set; }

	/// <summary>
	/// Representation details (membership information)
	/// </summary>
	[JsonPropertyName("representation")]
	public RepresentationDetails? Representation { get; set; }
}

/// <summary>
/// Represents the representation details for a constituency
/// </summary>
public class RepresentationDetails : MembershipBase;
