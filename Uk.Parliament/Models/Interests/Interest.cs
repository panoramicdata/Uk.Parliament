using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Interests;

/// <summary>
/// Represents a registered interest for a Member of Parliament
/// </summary>
public class Interest
{
	/// <summary>
	/// Interest identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Interest summary
	/// </summary>
	[JsonPropertyName("summary")]
	public string Summary { get; set; } = string.Empty;

	/// <summary>
	/// Parent interest ID if this is a child interest
	/// </summary>
	[JsonPropertyName("parentInterestId")]
	public int? ParentInterestId { get; set; }

	/// <summary>
	/// Date the interest was registered
	/// </summary>
	[JsonPropertyName("registrationDate")]
	public DateTime? RegistrationDate { get; set; }

	/// <summary>
	/// Date the interest was published
	/// </summary>
	[JsonPropertyName("publishedDate")]
	public DateTime? PublishedDate { get; set; }

	/// <summary>
	/// Updated dates
	/// </summary>
	[JsonPropertyName("updatedDates")]
	public List<DateTime>? UpdatedDates { get; set; }

	/// <summary>
	/// Category information
	/// </summary>
	[JsonPropertyName("category")]
	public InterestCategoryInfo Category { get; set; } = new();

	/// <summary>
	/// Member information
	/// </summary>
	[JsonPropertyName("member")]
	public InterestMemberInfo Member { get; set; } = new();

	/// <summary>
	/// Fields/details
	/// </summary>
	[JsonPropertyName("fields")]
	public List<InterestField>? Fields { get; set; }

	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink>? Links { get; set; }

	/// <summary>
	/// Whether rectified
	/// </summary>
	[JsonPropertyName("rectified")]
	public bool Rectified { get; set; }

	/// <summary>
	/// Rectification details
	/// </summary>
	[JsonPropertyName("rectifiedDetails")]
	public string? RectifiedDetails { get; set; }

	/// <summary>
	/// Helper property: Member ID from nested member object
	/// </summary>
	[JsonIgnore]
	public int MemberId => Member?.Id ?? 0;

	/// <summary>
	/// Helper property: Category ID from nested category object
	/// </summary>
	[JsonIgnore]
	public int CategoryId => Category?.Id ?? 0;

	/// <summary>
	/// Helper property: Interest details (alias for Summary)
	/// </summary>
	[JsonIgnore]
	public string InterestDetails => Summary;
}

/// <summary>
/// Category information in an interest
/// </summary>
public class InterestCategoryInfo
{
	/// <summary>
	/// Category ID
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Category number (can be string like "1a" or numeric)
	/// </summary>
	[JsonPropertyName("number")]
	[JsonConverter(typeof(NumberOrStringConverter))]
	public string? Number { get; set; }

	/// <summary>
	/// Category name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Parent category IDs
	/// </summary>
	[JsonPropertyName("parentCategoryIds")]
	public List<int>? ParentCategoryIds { get; set; }

	/// <summary>
	/// Type (Commons/Lords)
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }

	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink>? Links { get; set; }
}

/// <summary>
/// Member information in an interest
/// </summary>
public class InterestMemberInfo
{
	/// <summary>
	/// Member ID
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Display name
	/// </summary>
	[JsonPropertyName("nameDisplayAs")]
	public string? NameDisplayAs { get; set; }

	/// <summary>
	/// List name
	/// </summary>
	[JsonPropertyName("nameListAs")]
	public string? NameListAs { get; set; }

	/// <summary>
	/// House
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// Member from (constituency)
	/// </summary>
	[JsonPropertyName("memberFrom")]
	public string? MemberFrom { get; set; }

	/// <summary>
	/// Party
	/// </summary>
	[JsonPropertyName("party")]
	public string? Party { get; set; }

	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink>? Links { get; set; }
}

/// <summary>
/// Interest field
/// </summary>
public class InterestField
{
	/// <summary>
	/// Field name
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	/// <summary>
	/// Field description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }

	/// <summary>
	/// Field type
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }

	/// <summary>
	/// Field type information (detailed type info)
	/// </summary>
	[JsonPropertyName("typeInfo")]
	public object? TypeInfo { get; set; }

	/// <summary>
	/// Field values (for multi-value fields)
	/// </summary>
	[JsonPropertyName("values")]
	public List<string>? Values { get; set; }

	/// <summary>
	/// Field value (can be string, number, or boolean)
	/// </summary>
	[JsonPropertyName("value")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Value { get; set; }
}

/// <summary>
/// JSON converter that handles string, number, and boolean values
/// </summary>
internal class AnyValueToStringConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
			return null;

		if (reader.TokenType == JsonTokenType.String)
			return reader.GetString();

		if (reader.TokenType == JsonTokenType.Number)
			return reader.GetInt64().ToString();

		if (reader.TokenType == JsonTokenType.True)
			return "true";

		if (reader.TokenType == JsonTokenType.False)
			return "false";

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

/// <summary>
/// JSON converter that handles both number and string values
/// </summary>
internal class NumberOrStringConverter : JsonConverter<string?>
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
