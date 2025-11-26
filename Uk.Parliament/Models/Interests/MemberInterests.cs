namespace Uk.Parliament.Models.Interests;

/// <summary>
/// Represents all interests for a specific member, grouped by category
/// </summary>
public class MemberInterests
{
	/// <summary>
	/// Member identifier
	/// </summary>
	[JsonPropertyName("memberId")]
	public int MemberId { get; set; }

	/// <summary>
	/// Member name
	/// </summary>
	[JsonPropertyName("memberName")]
	public string? MemberName { get; set; }

	/// <summary>
	/// List of interest categories with their interests
	/// </summary>
	[JsonPropertyName("categories")]
	public List<InterestCategoryWithInterests> Categories { get; set; } = [];
}

/// <summary>
/// Interest category with associated interests
/// </summary>
public class InterestCategoryWithInterests
{
	/// <summary>
	/// Category details
	/// </summary>
	[JsonPropertyName("category")]
	public required InterestCategory Category { get; set; }

	/// <summary>
	/// List of interests in this category
	/// </summary>
	[JsonPropertyName("interests")]
	public List<Interest> Interests { get; set; } = [];
}
