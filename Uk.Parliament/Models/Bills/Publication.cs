using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Bills;

/// <summary>
/// Represents a publication related to a bill
/// </summary>
public class Publication
{
	/// <summary>
	/// Publication identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Publication title
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Publication type
	/// </summary>
	[JsonPropertyName("publicationType")]
	public string? PublicationType { get; set; }

	/// <summary>
	/// Date of publication
	/// </summary>
	[JsonPropertyName("publicationDate")]
	public DateTime? PublicationDate { get; set; }

	/// <summary>
	/// Links to the publication documents
	/// </summary>
	[JsonPropertyName("links")]
	public List<PublicationLink>? Links { get; set; }
}

/// <summary>
/// Represents a link to a publication document
/// </summary>
public class PublicationLink
{
	/// <summary>
	/// Title of the link
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// URL of the document
	/// </summary>
	[JsonPropertyName("url")]
	public string? Url { get; set; }

	/// <summary>
	/// Content type (e.g., application/pdf)
	/// </summary>
	[JsonPropertyName("contentType")]
	public string? ContentType { get; set; }
}
