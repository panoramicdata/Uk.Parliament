using System.Globalization;
using System.Text.Json;

namespace Uk.Parliament.Models;

/// <summary>
/// Reads a JSON string, number or (optionally) boolean as text, and reads null for any other token.
/// </summary>
internal abstract class LenientStringConverter : JsonConverter<string?>
{
	/// <summary>Whether true and false are read as text rather than null.</summary>
	protected abstract bool AllowBooleans { get; }

	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
			case JsonTokenType.String:
				return reader.GetString();
			case JsonTokenType.Number:
				return reader.TryGetInt64(out var integer)
					? integer.ToString(CultureInfo.InvariantCulture)
					: reader.GetDouble().ToString(CultureInfo.InvariantCulture);
			case JsonTokenType.True or JsonTokenType.False when AllowBooleans:
				return reader.GetBoolean() ? "true" : "false";
			case JsonTokenType.StartObject or JsonTokenType.StartArray:
				reader.Skip();
				return null;
			default:
				return null;
		}
	}

	public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
	{
		if (value is null)
		{
			writer.WriteNullValue();
		}
		else
		{
			writer.WriteStringValue(value);
		}
	}
}
