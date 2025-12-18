using System.Text.Json;

namespace Uk.Parliament.Models.Questions;

/// <summary>
/// Represents a written parliamentary question
/// </summary>
public class WrittenQuestion
{
	/// <summary>
	/// Question identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Unique Identifier Number (UIN) for the question
	/// </summary>
	[JsonPropertyName("uin")]
	public string Uin { get; set; } = string.Empty;

	/// <summary>
	/// Member who asked the question
	/// </summary>
	[JsonPropertyName("askingMemberId")]
	public int AskingMemberId { get; set; }

	/// <summary>
	/// Name of the member who asked the question
	/// </summary>
	[JsonPropertyName("askingMember")]
	public string? AskingMember { get; set; }

	/// <summary>
	/// House where question was asked (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public string House { get; set; } = string.Empty;

	/// <summary>
	/// Member who answered the question
	/// </summary>
	[JsonPropertyName("answeringMemberId")]
	public int? AnsweringMemberId { get; set; }

	/// <summary>
	/// Name of the member who answered
	/// </summary>
	[JsonPropertyName("answeringMember")]
	public string? AnsweringMember { get; set; }

	/// <summary>
	/// Department responsible for answering
	/// </summary>
	[JsonPropertyName("answeringDepartment")]
	public string? AnsweringDepartment { get; set; }

	/// <summary>
	/// ID of the answering body/department
	/// </summary>
	[JsonPropertyName("answeringBodyId")]
	public int? AnsweringBodyId { get; set; }

	/// <summary>
	/// Name of the answering body/department
	/// </summary>
	[JsonPropertyName("answeringBodyName")]
	public string? AnsweringBodyName { get; set; }

	/// <summary>
	/// Date the question was tabled
	/// </summary>
	[JsonPropertyName("dateTabled")]
	public DateTime DateTabled { get; set; }

	/// <summary>
	/// Date for answer
	/// </summary>
	[JsonPropertyName("dateForAnswer")]
	public DateTime? DateForAnswer { get; set; }

	/// <summary>
	/// Date the question was answered
	/// </summary>
	[JsonPropertyName("dateAnswered")]
	public DateTime? DateAnswered { get; set; }

	/// <summary>
	/// Date the answer was corrected
	/// </summary>
	[JsonPropertyName("dateAnswerCorrected")]
	public DateTime? DateAnswerCorrected { get; set; }

	/// <summary>
	/// Date the holding answer was provided
	/// </summary>
	[JsonPropertyName("dateHoldingAnswer")]
	public DateTime? DateHoldingAnswer { get; set; }

	/// <summary>
	/// Question text
	/// </summary>
	[JsonPropertyName("questionText")]
	public string QuestionText { get; set; } = string.Empty;

	/// <summary>
	/// Answer text
	/// </summary>
	[JsonPropertyName("answerText")]
	public string? AnswerText { get; set; }

	/// <summary>
	/// Original answer text (before correction)
	/// </summary>
	[JsonPropertyName("originalAnswerText")]
	public string? OriginalAnswerText { get; set; }

	/// <summary>
	/// Comparable answer text (normalized/formatted for comparison)
	/// </summary>
	[JsonPropertyName("comparableAnswerText")]
	public string? ComparableAnswerText { get; set; }

	/// <summary>
	/// Whether the question has been answered
	/// </summary>
	[JsonPropertyName("isAnswered")]
	public bool IsAnswered { get; set; }

	/// <summary>
	/// Whether the answer is a holding answer
	/// </summary>
	[JsonPropertyName("answerIsHolding")]
	public bool AnswerIsHolding { get; set; }

	/// <summary>
	/// Whether the answer is a correction
	/// </summary>
	[JsonPropertyName("answerIsCorrection")]
	public bool AnswerIsCorrection { get; set; }

	/// <summary>
	/// Member ID who corrected the answer
	/// </summary>
	[JsonPropertyName("correctingMemberId")]
	public int? CorrectingMemberId { get; set; }

	/// <summary>
	/// Name of member who corrected the answer
	/// </summary>
	[JsonPropertyName("correctingMember")]
	public string? CorrectingMember { get; set; }

	/// <summary>
	/// Whether the question is named day
	/// </summary>
	[JsonPropertyName("isNamedDay")]
	public bool IsNamedDay { get; set; }

	/// <summary>
	/// Whether the question is withdrawn
	/// </summary>
	[JsonPropertyName("isWithdrawn")]
	public bool IsWithdrawn { get; set; }

	/// <summary>
	/// Whether the asking member has a registered interest
	/// </summary>
	[JsonPropertyName("memberHasInterest")]
	public bool MemberHasInterest { get; set; }

	/// <summary>
	/// Heading/subject of the question
	/// </summary>
	[JsonPropertyName("heading")]
	public string? Heading { get; set; }

	/// <summary>
	/// Related document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }

	/// <summary>
	/// Grouped questions (if this question is grouped with others)
	/// </summary>
	[JsonPropertyName("groupedQuestions")]
	[JsonConverter(typeof(FlexibleStringListConverter))]
	public List<string>? GroupedQuestions { get; set; }

	/// <summary>
	/// Dates for grouped questions (stored as strings due to API inconsistency)
	/// </summary>
	[JsonPropertyName("groupedQuestionsDates")]
	[JsonConverter(typeof(FlexibleStringListConverter))]
	public List<string>? GroupedQuestionsDates { get; set; }

	/// <summary>
	/// Number of attachments
	/// </summary>
	[JsonPropertyName("attachmentCount")]
	public int AttachmentCount { get; set; }

	/// <summary>
	/// List of attachments
	/// </summary>
	[JsonPropertyName("attachments")]
	public List<object>? Attachments { get; set; }

	/// <summary>
	/// Links related to this question
	/// </summary>
	[JsonPropertyName("links")]
	public List<object>? Links { get; set; }
}

/// <summary>
/// JSON converter that handles lists containing any type of value (strings, numbers, dates, objects, etc.)
/// and converts them all to strings
/// </summary>
internal class FlexibleStringListConverter : JsonConverter<List<string>?>
{
	public override List<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return null;
		}

		if (reader.TokenType != JsonTokenType.StartArray)
		{
			throw new JsonException($"Expected StartArray but got {reader.TokenType}");
		}

		var list = new List<string>();
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndArray)
			{
				break;
			}

			var value = reader.TokenType switch
			{
				JsonTokenType.String => reader.GetString() ?? string.Empty,
				JsonTokenType.Number => reader.TryGetInt64(out var l) ? l.ToString() : reader.GetDouble().ToString(),
				JsonTokenType.True => "true",
				JsonTokenType.False => "false",
				JsonTokenType.Null => string.Empty,
				JsonTokenType.StartObject => SkipAndReturnEmpty(ref reader, JsonTokenType.EndObject),
				JsonTokenType.StartArray => SkipAndReturnEmpty(ref reader, JsonTokenType.EndArray),
				_ => string.Empty
			};
			list.Add(value);
		}

		return list;
	}

	private static string SkipAndReturnEmpty(ref Utf8JsonReader reader, JsonTokenType endToken)
	{
		var depth = 1;
		while (depth > 0 && reader.Read())
		{
			if (reader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray)
			{
				depth++;
			}
			else if (reader.TokenType == endToken || reader.TokenType == JsonTokenType.EndObject || reader.TokenType == JsonTokenType.EndArray)
			{
				depth--;
			}
		}

		return string.Empty;
	}

	public override void Write(Utf8JsonWriter writer, List<string>? value, JsonSerializerOptions options)
	{
		if (value == null)
		{
			writer.WriteNullValue();
			return;
		}

		writer.WriteStartArray();
		foreach (var item in value)
		{
			writer.WriteStringValue(item);
		}

		writer.WriteEndArray();
	}
}
