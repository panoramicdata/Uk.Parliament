using System.Text.Json;

namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// Base for converters that read a JSON scalar of any primitive type into a string.
/// </summary>
/// <remarks>
/// The Treaties API returns several fields as a string in one response and as a number or
/// boolean in the next, so every one of them needs the same scalar handling. Derived types
/// differ only in what they do with a non-scalar token.
/// </remarks>
internal abstract class ScalarToStringConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
	{
		JsonTokenType.Null => null,
		JsonTokenType.String => reader.GetString(),
		JsonTokenType.Number => reader.GetInt64().ToString(),
		JsonTokenType.True => "true",
		JsonTokenType.False => "false",
		_ => ReadNonScalar(ref reader)
	};

	/// <summary>
	/// Reads a token that is not a JSON scalar. Not valid unless a derived type allows it.
	/// </summary>
	protected virtual string? ReadNonScalar(ref Utf8JsonReader reader)
		=> throw new JsonException($"Unexpected token type: {reader.TokenType}");

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

	/// <summary>
	/// Consumes the container the reader is positioned on and returns <paramref name="representation"/>
	/// in its place, so a structured value does not fail the whole deserialization.
	/// </summary>
	protected static string SkipContainer(ref Utf8JsonReader reader, string representation)
	{
		var depth = reader.CurrentDepth;
		while (reader.Read() && reader.CurrentDepth > depth)
		{
			// Skip through the container
		}

		return representation;
	}
}

/// <summary>
/// JSON converter that handles both string and number values
/// </summary>
internal sealed class StringOrNumberConverter : ScalarToStringConverter;

/// <summary>
/// JSON converter that handles string, number, and boolean values, and collapses
/// arrays and objects to a placeholder rather than failing.
/// </summary>
internal sealed class AnyValueToStringConverter : ScalarToStringConverter
{
	protected override string? ReadNonScalar(ref Utf8JsonReader reader) => reader.TokenType switch
	{
		JsonTokenType.StartArray => SkipContainer(ref reader, "[]"),
		JsonTokenType.StartObject => SkipContainer(ref reader, "{}"),
		_ => base.ReadNonScalar(ref reader)
	};
}
