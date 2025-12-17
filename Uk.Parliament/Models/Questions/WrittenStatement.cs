using System.Text.Json;

namespace Uk.Parliament.Models.Questions;

/// <summary>
/// Represents a written ministerial statement
/// </summary>
public class WrittenStatement
{
	/// <summary>
	/// Statement identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Unique Identifier Number (UIN) for the statement
	/// </summary>
	[JsonPropertyName("uin")]
	public string Uin { get; set; } = string.Empty;

	/// <summary>
	/// Notice number for the statement
	/// </summary>
	[JsonPropertyName("noticeNumber")]
	[JsonConverter(typeof(NumberOrStringConverter))]
	public string? NoticeNumber { get; set; }

	/// <summary>
	/// Member who made the statement
	/// </summary>
	[JsonPropertyName("makingMemberId")]
	public int MakingMemberId { get; set; }

	/// <summary>
	/// Alternative member ID property
	/// </summary>
	[JsonPropertyName("memberId")]
	public int? MemberId { get; set; }

	/// <summary>
	/// Member object (alternative to makingMember string)
	/// </summary>
	[JsonPropertyName("member")]
	public object? Member { get; set; }

	/// <summary>
	/// Member role (e.g., "Minister", "Secretary of State")
	/// </summary>
	[JsonPropertyName("memberRole")]
	public string? MemberRole { get; set; }

	/// <summary>
	/// Name of the member who made the statement
	/// </summary>
	[JsonPropertyName("makingMember")]
	public string? MakingMember { get; set; }

	/// <summary>
	/// House where statement was made (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public string House { get; set; } = string.Empty;

	/// <summary>
	/// Answering body ID
	/// </summary>
	[JsonPropertyName("answeringBodyId")]
	public int? AnsweringBodyId { get; set; }

	/// <summary>
	/// Answering body name
	/// </summary>
	[JsonPropertyName("answeringBodyName")]
	public string? AnsweringBodyName { get; set; }

	/// <summary>
	/// Department that issued the statement
	/// </summary>
	[JsonPropertyName("answeringBody")]
	public string? Department { get; set; }

	/// <summary>
	/// Date the statement was made
	/// </summary>
	[JsonPropertyName("dateMade")]
	public DateTime DateMade { get; set; }

	/// <summary>
	/// Statement title/heading
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Statement text/content
	/// </summary>
	[JsonPropertyName("statementText")]
	public string StatementText { get; set; } = string.Empty;

	/// <summary>
	/// Text (alternative to statementText)
	/// </summary>
	[JsonPropertyName("text")]
	public string? Text { get; set; }

	/// <summary>
	/// Related document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }

	/// <summary>
	/// Whether this is a correction statement
	/// </summary>
	[JsonPropertyName("isCorrection")]
	public bool IsCorrection { get; set; }

	/// <summary>
	/// Whether this statement has been withdrawn
	/// </summary>
	[JsonPropertyName("isWithdrawn")]
	public bool IsWithdrawn { get; set; }

	/// <summary>
	/// Whether this statement has attachments
	/// </summary>
	[JsonPropertyName("hasAttachments")]
	public bool HasAttachments { get; set; }

	/// <summary>
	/// Whether this statement has linked statements
	/// </summary>
	[JsonPropertyName("hasLinkedStatements")]
	public bool HasLinkedStatements { get; set; }

	/// <summary>
	/// Linked statements
	/// </summary>
	[JsonPropertyName("linkedStatements")]
	public List<object>? LinkedStatements { get; set; }
}

/// <summary>
/// JSON converter that handles both number and string values
/// </summary>
internal class NumberOrStringConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
	{
		JsonTokenType.Null => null,
		JsonTokenType.String => reader.GetString(),
		JsonTokenType.Number => reader.TryGetInt64(out var l) ? l.ToString() : reader.GetDouble().ToString(),
		_ => null
	};

	public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
	{
		if (value == null)
		{
			writer.WriteNullValue();
		}
		else
		{
			writer.WriteStringValue(value);
		}
	}
}
