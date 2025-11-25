using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Interests;

/// <summary>
/// Represents a registered interest for a Member of Parliament
/// </summary>
public class Interest
{
	/// <summary>
	/// Interest identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Interest summary
	/// </summary>
	[JsonPropertyName("summary")]
	public string Summary { get; set; } = string.Empty;

	/// <summary>
	/// Parent interest ID if this is a child interest
	/// </summary>
	[JsonPropertyName("parentInterestId")]
	public int? ParentInterestId { get; set; }

	/// <summary>
	/// Date the interest was registered
	/// </summary>
	[JsonPropertyName("registrationDate")]
	public DateTime? RegistrationDate { get; set; }

	/// <summary>
	/// Date the interest was published
	/// </summary>
	[JsonPropertyName("publishedDate")]
	public DateTime? PublishedDate { get; set; }

	/// <summary>
	/// Updated dates
	/// </summary>
	[JsonPropertyName("updatedDates")]
	public string? UpdatedDates { get; set; }

	/// <summary>
	/// Category information
	/// </summary>
	[JsonPropertyName("category")]
	public InterestCategoryInfo Category { get; set; } = new();

	/// <summary>
	/// Member information
	/// </summary>
	[JsonPropertyName("member")]
	public InterestMemberInfo Member { get; set; } = new();

	/// <summary>
	/// Fields/details
	/// </summary>
	[JsonPropertyName("fields")]
	public List<InterestField>? Fields { get; set; }

	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink>? Links { get; set; }

	/// <summary>
	/// Whether rectified
	/// </summary>
	[JsonPropertyName("rectified")]
	public bool Rectified { get; set; }

	/// <summary>
	/// Rectification details
	/// </summary>
	[JsonPropertyName("rectifiedDetails")]
	public string? RectifiedDetails { get; set; }

	/// <summary>
	/// Helper property: Member ID from nested member object
	/// </summary>
	[JsonIgnore]
	public int MemberId => Member?.Id ?? 0;

	/// <summary>
	/// Helper property: Category ID from nested category object
	/// </summary>
	[JsonIgnore]
	public int CategoryId => Category?.Id ?? 0;

	/// <summary>
	/// Helper property: Interest details (alias for Summary)
	/// </summary>
	[JsonIgnore]
	public string InterestDetails => Summary;
}

/// <summary>
/// Category information in an interest
/// </summary>
public class InterestCategoryInfo
{
	/// <summary>
	/// Category ID
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Category number
	/// </summary>
	[JsonPropertyName("number")]
	public int Number { get; set; }

	/// <summary>
	/// Category name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Parent category IDs
	/// </summary>
	[JsonPropertyName("parentCategoryIds")]
	public List<int>? ParentCategoryIds { get; set; }

	/// <summary>
	/// Type (Commons/Lords)
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }

	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink>? Links { get; set; }
}

/// <summary>
/// Member information in an interest
/// </summary>
public class InterestMemberInfo
{
	/// <summary>
	/// Member ID
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Display name
	/// </summary>
	[JsonPropertyName("nameDisplayAs")]
	public string? NameDisplayAs { get; set; }

	/// <summary>
	/// List name
	/// </summary>
	[JsonPropertyName("nameListAs")]
	public string? NameListAs { get; set; }

	/// <summary>
	/// House
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// Member from (constituency)
	/// </summary>
	[JsonPropertyName("memberFrom")]
	public string? MemberFrom { get; set; }

	/// <summary>
	/// Party
	/// </summary>
	[JsonPropertyName("party")]
	public string? Party { get; set; }

	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink>? Links { get; set; }
}

/// <summary>
/// Interest field
/// </summary>
public class InterestField
{
	/// <summary>
	/// Field name
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	/// <summary>
	/// Field value
	/// </summary>
	[JsonPropertyName("value")]
	public string? Value { get; set; }
}
