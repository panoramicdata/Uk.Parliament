using System.Globalization;
using System.Text.Json;

namespace Uk.Parliament.Models.Questions;

/// <summary>
/// JSON converter that handles lists containing any type of value (strings, numbers, dates, objects, etc.)
/// and converts them all to strings
/// </summary>
internal sealed class FlexibleStringListConverter : JsonConverter<List<string>?>
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
				JsonTokenType.Number => reader.TryGetInt64(out var l) ? l.ToString(CultureInfo.InvariantCulture) : reader.GetDouble().ToString(CultureInfo.InvariantCulture),
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
