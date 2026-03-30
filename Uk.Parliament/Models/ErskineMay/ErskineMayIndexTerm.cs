namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents an Erskine May index term
/// </summary>
public class ErskineMayIndexTerm
{
	/// <summary>
	/// Index term identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Index term text
	/// </summary>
	[JsonPropertyName("term")]
	public string? Term { get; set; }

	/// <summary>
	/// Display text (may include parent term hierarchy)
	/// </summary>
	[JsonPropertyName("displayAs")]
	public string? DisplayAs { get; set; }

	/// <summary>
	/// "See" cross-reference links
	/// </summary>
	[JsonPropertyName("seeLinks")]
	public List<object> SeeLinks { get; set; } = [];

	/// <summary>
	/// Section references for this index term
	/// </summary>
	[JsonPropertyName("references")]
	public List<object> References { get; set; } = [];

	/// <summary>
	/// Parent term (if this is a sub-term)
	/// </summary>
	[JsonPropertyName("parentTerm")]
	public ErskineMayIndexTerm? ParentTerm { get; set; }

	/// <summary>
	/// Child terms (sub-entries under this term)
	/// </summary>
	[JsonPropertyName("childTerms")]
	public List<ErskineMayIndexTerm> ChildTerms { get; set; } = [];
}
