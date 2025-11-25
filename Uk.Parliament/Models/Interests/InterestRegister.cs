using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Interests;

/// <summary>
/// Represents a register of interests
/// </summary>
public class InterestRegister
{
	/// <summary>
	/// Register identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Register name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Publication date
	/// </summary>
	[JsonPropertyName("publicationDate")]
	public DateTime? PublicationDate { get; set; }

	/// <summary>
	/// Document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }

	/// <summary>
	/// List of categories in this register
	/// </summary>
	[JsonPropertyName("categories")]
	public List<InterestCategory>? Categories { get; set; }
}
