using System.Text.Json;
using System.Text.Json.Serialization;
using InterestsAny = Uk.Parliament.Models.Interests.AnyValueToStringConverter;
using InterestsNumberOrString = Uk.Parliament.Models.Interests.NumberOrStringConverter;
using OralAny = Uk.Parliament.Models.OralQuestions.AnyToStringConverter;
using QuestionsNumberOrString = Uk.Parliament.Models.Questions.NumberOrStringConverter;
using TreatyAny = Uk.Parliament.Models.Treaties.AnyValueToStringConverter;
using TreatyStringOrNumber = Uk.Parliament.Models.Treaties.StringOrNumberConverter;

namespace Uk.Parliament.Test;

/// <summary>Offline tests for the lenient scalar converters the Parliament APIs need.</summary>
public class JsonConverterTests
{
	private static readonly Dictionary<string, JsonConverter<string?>> Converters = new()
	{
		["questions-number-or-string"] = new QuestionsNumberOrString(),
		["oral-any"] = new OralAny(),
		["interests-any"] = new InterestsAny(),
		["interests-number-or-string"] = new InterestsNumberOrString(),
		["treaty-any"] = new TreatyAny(),
		["treaty-string-or-number"] = new TreatyStringOrNumber()
	};

	private static readonly Dictionary<string, JsonSerializerOptions> Options = Converters.ToDictionary(
		pair => pair.Key,
		pair => new JsonSerializerOptions { Converters = { pair.Value } });

	private static string? Read(string converter, string json)
		=> JsonSerializer.Deserialize<string?>(json, Options[converter]);

	private static string Write(string converter, string? value)
		=> JsonSerializer.Serialize(value, Options[converter]);

	/// <summary>Strings, integers and null are read by every converter.</summary>
	[Theory]
	[InlineData("questions-number-or-string")]
	[InlineData("oral-any")]
	[InlineData("interests-any")]
	[InlineData("interests-number-or-string")]
	[InlineData("treaty-any")]
	[InlineData("treaty-string-or-number")]
	public void Read_HandlesStringIntegerAndNull(string converter)
	{
		_ = Read(converter, "\"text\"").Should().Be("text");
		_ = Read(converter, "42").Should().Be("42");
		_ = Read(converter, "null").Should().BeNull();
	}

	/// <summary>Every converter writes a string and a null back out.</summary>
	[Theory]
	[InlineData("questions-number-or-string")]
	[InlineData("oral-any")]
	[InlineData("interests-any")]
	[InlineData("interests-number-or-string")]
	[InlineData("treaty-any")]
	[InlineData("treaty-string-or-number")]
	public void Write_HandlesStringAndNull(string converter)
	{
		_ = Write(converter, "text").Should().Be("\"text\"");
		_ = Write(converter, null).Should().Be("null");
	}

	/// <summary>Converters that accept booleans read both values.</summary>
	[Theory]
	[InlineData("oral-any")]
	[InlineData("interests-any")]
	[InlineData("treaty-any")]
	[InlineData("treaty-string-or-number")]
	public void Read_WhereBooleansAreAccepted_ReturnsLowercaseText(string converter)
	{
		_ = Read(converter, "true").Should().Be("true");
		_ = Read(converter, "false").Should().Be("false");
	}

	/// <summary>Converters that read fractional numbers keep the fraction.</summary>
	[Theory]
	[InlineData("questions-number-or-string")]
	[InlineData("oral-any")]
	public void Read_WithFraction_UsesInvariantCulture(string converter)
		=> _ = Read(converter, "1.5").Should().Be("1.5");

	/// <summary>The oral and questions converters collapse containers to null.</summary>
	[Theory]
	[InlineData("oral-any", "{\"a\":[1,2]}")]
	[InlineData("oral-any", "[1,{\"b\":2}]")]
	[InlineData("questions-number-or-string", "true")]
	[InlineData("questions-number-or-string", "{\"a\":[1]}")]
	public void Read_WithUnsupportedToken_ReturnsNull(string converter, string json)
		=> _ = Read(converter, json).Should().BeNull();

	/// <summary>The strict converters reject tokens they do not support.</summary>
	[Theory]
	[InlineData("interests-number-or-string", "true")]
	[InlineData("interests-any", "{}")]
	[InlineData("treaty-string-or-number", "{}")]
	[InlineData("treaty-string-or-number", "[]")]
	public void Read_WithRejectedToken_Throws(string converter, string json)
	{
		var act = () => Read(converter, json);

		_ = act.Should().Throw<JsonException>();
	}

	/// <summary>The treaty converter replaces containers with a placeholder and skips their content.</summary>
	[Theory]
	[InlineData("{\"a\":{\"b\":[1]}}", "{}")]
	[InlineData("[1,[2,3],{\"x\":1}]", "[]")]
	public void Read_TreatyAny_CollapsesContainers(string json, string expected)
	{
		var holder = JsonSerializer.Deserialize<List<string?>>(
			"[" + json + ",\"after\"]",
			Options["treaty-any"]);

		_ = holder.Should().Equal(expected, "after");
	}

	/// <summary>The flexible list converter turns every element into text and skips containers.</summary>
	[Fact]
	public void Read_FlexibleStringList_ConvertsMixedElements()
	{
		var question = JsonSerializer.Deserialize<Models.Questions.WrittenQuestion>(
			"""{"groupedQuestionsDates":["2024-01-01",5,1.5,true,false,null,{"a":[1]},[2]]}""");

		_ = question!.GroupedQuestionsDates.Should().Equal("2024-01-01", "5", "1.5", "true", "false", string.Empty, string.Empty, string.Empty);
	}

	/// <summary>A null list stays null and a non-array is rejected.</summary>
	[Fact]
	public void Read_FlexibleStringList_HandlesNullAndRejectsNonArray()
	{
		var nullList = JsonSerializer.Deserialize<Models.Questions.WrittenQuestion>("""{"groupedQuestionsDates":null}""");
		var act = () => JsonSerializer.Deserialize<Models.Questions.WrittenQuestion>("""{"groupedQuestionsDates":"x"}""");

		_ = nullList!.GroupedQuestionsDates.Should().BeNull();
		_ = act.Should().Throw<JsonException>();
	}

	/// <summary>The list converter writes its strings back as an array, or null.</summary>
	[Fact]
	public void Write_FlexibleStringList_RoundTrips()
	{
		var question = new Models.Questions.WrittenQuestion { GroupedQuestionsDates = ["a", "b"] };
		var empty = new Models.Questions.WrittenQuestion();

		_ = JsonSerializer.Serialize(question).Should().Contain("\"groupedQuestionsDates\":[\"a\",\"b\"]");
		_ = JsonSerializer.Serialize(empty).Should().Contain("\"groupedQuestionsDates\":null");
	}
}
