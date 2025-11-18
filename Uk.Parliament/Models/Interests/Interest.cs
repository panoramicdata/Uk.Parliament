using System;
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
	/// Member identifier
	/// </summary>
	[JsonPropertyName("memberId")]
	public int MemberId { get; set; }

	/// <summary>
	/// Category identifier
	/// </summary>
	[JsonPropertyName("categoryId")]
	public int CategoryId { get; set; }

	/// <summary>
	/// Interest description/details
	/// </summary>
	[JsonPropertyName("interest")]
	public required string InterestDetails { get; set; }

	/// <summary>
	/// Date the interest was registered
	/// </summary>
	[JsonPropertyName("registeredDate")]
	public DateTime? RegisteredDate { get; set; }

	/// <summary>
	/// Date the interest was last amended
	/// </summary>
	[JsonPropertyName("amendedDate")]
	public DateTime? AmendedDate { get; set; }

	/// <summary>
	/// Date the interest was deleted/removed
	/// </summary>
	[JsonPropertyName("deletedDate")]
	public DateTime? DeletedDate { get; set; }

	/// <summary>
	/// Whether the interest is currently active
	/// </summary>
	[JsonPropertyName("isActive")]
	public bool IsActive { get; set; }

	/// <summary>
	/// Sort order within category
	/// </summary>
	[JsonPropertyName("sortOrder")]
	public int? SortOrder { get; set; }
}
