using System.Text.Json;

namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// JSON converter that handles string, number, and boolean values
/// </summary>
internal sealed class AnyValueToStringConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
	{
		JsonTokenType.Null => null,
		JsonTokenType.String => reader.GetString(),
		JsonTokenType.Number => reader.GetInt64().ToString(),
		JsonTokenType.True => "true",
		JsonTokenType.False => "false",
		JsonTokenType.StartArray => ReadArrayAsString(ref reader),
		JsonTokenType.StartObject => ReadObjectAsString(ref reader),
		JsonTokenType.None => throw new NotImplementedException(),
		JsonTokenType.EndObject => throw new NotImplementedException(),
		JsonTokenType.EndArray => throw new NotImplementedException(),
		JsonTokenType.PropertyName => throw new NotImplementedException(),
		JsonTokenType.Comment => throw new NotImplementedException(),
		_ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
	};

	private static string ReadArrayAsString(ref Utf8JsonReader reader)
	{
		var depth = reader.CurrentDepth;
		while (reader.Read() && reader.CurrentDepth > depth)
		{
			// Skip through the array
		}

		return "[]";
	}

	private static string ReadObjectAsString(ref Utf8JsonReader reader)
	{
		var depth = reader.CurrentDepth;
		while (reader.Read() && reader.CurrentDepth > depth)
		{
			// Skip through the object
		}

		return "{}";
	}

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

/// <summary>
/// JSON converter that handles both string and number values
/// </summary>
internal class StringOrNumberConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
	{
		JsonTokenType.Null => null,
		JsonTokenType.String => reader.GetString(),
		JsonTokenType.Number => reader.GetInt64().ToString(),
		JsonTokenType.True => "true",
		JsonTokenType.False => "false",
		JsonTokenType.None => throw new NotImplementedException(),
		JsonTokenType.StartObject => throw new NotImplementedException(),
		JsonTokenType.EndObject => throw new NotImplementedException(),
		JsonTokenType.StartArray => throw new NotImplementedException(),
		JsonTokenType.EndArray => throw new NotImplementedException(),
		JsonTokenType.PropertyName => throw new NotImplementedException(),
		JsonTokenType.Comment => throw new NotImplementedException(),
		_ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
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
