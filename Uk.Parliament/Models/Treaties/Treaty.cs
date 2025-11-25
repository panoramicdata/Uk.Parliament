using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// Represents an international treaty laid before Parliament
/// </summary>
public class Treaty
{
	/// <summary>
	/// Treaty identifier
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Command paper number (e.g., "CP 123" or numeric value)
	/// </summary>
	[JsonPropertyName("commandPaperNumber")]
	[JsonConverter(typeof(StringOrNumberConverter))]
	public string? CommandPaperNumber { get; set; }

	/// <summary>
	/// Treaty title
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Treaty series reference
	/// </summary>
	[JsonPropertyName("treatySeries")]
	public string? TreatySeries { get; set; }

	/// <summary>
	/// Government organization responsible
	/// </summary>
	[JsonPropertyName("leadGovernmentOrganisationId")]
	public int? LeadGovernmentOrganisationId { get; set; }

	/// <summary>
	/// Name of lead government organization
	/// </summary>
	[JsonPropertyName("leadGovernmentOrganisation")]
	public string? LeadGovernmentOrganisation { get; set; }

	/// <summary>
	/// Date treaty was laid before Parliament
	/// </summary>
	[JsonPropertyName("dateLaid")]
	public DateTime? DateLaid { get; set; }

	/// <summary>
	/// Date treaty came into force
	/// </summary>
	[JsonPropertyName("dateIntoForce")]
	public DateTime? DateIntoForce { get; set; }

	/// <summary>
	/// Date treaty was signed
	/// </summary>
	[JsonPropertyName("dateSigned")]
	public DateTime? DateSigned { get; set; }

	/// <summary>
	/// House where treaty was laid (Commons/Lords/Both)
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// Treaty status (e.g., "In Force", "Not Yet In Force")
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; set; }

	/// <summary>
	/// Web link to treaty document
	/// </summary>
	[JsonPropertyName("webLink")]
	public string? WebLink { get; set; }

	/// <summary>
	/// Whether the treaty is multilateral
	/// </summary>
	[JsonPropertyName("isMultilateral")]
	public bool IsMultilateral { get; set; }

	/// <summary>
	/// Countries/parties involved
	/// </summary>
	[JsonPropertyName("countries")]
	public string? Countries { get; set; }

	/// <summary>
	/// Subject matter of the treaty
	/// </summary>
	[JsonPropertyName("subject")]
	public string? Subject { get; set; }
}

/// <summary>
/// JSON converter that handles both string and number values
/// </summary>
internal class StringOrNumberConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
			return null;

		if (reader.TokenType == JsonTokenType.String)
			return reader.GetString();

		if (reader.TokenType == JsonTokenType.Number)
			return reader.GetInt64().ToString();

		throw new JsonException($"Unexpected token type: {reader.TokenType}");
	}

	public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
	{
		if (value == null)
			writer.WriteNullValue();
		else
			writer.WriteStringValue(value);
	}
}
