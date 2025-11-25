using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Now;

/// <summary>
/// Represents the annunciator message for a chamber
/// </summary>
public class AnnunciatorMessage
{
	/// <summary>
	/// Whether the annunciator is disabled
	/// </summary>
	[JsonPropertyName("annunciatorDisabled")]
	public bool AnnunciatorDisabled { get; set; }

	/// <summary>
	/// Message identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Slides to display
	/// </summary>
	[JsonPropertyName("slides")]
	public List<AnnunciatorSlide> Slides { get; set; } = new();

	/// <summary>
	/// Scrolling messages
	/// </summary>
	[JsonPropertyName("scrollingMessages")]
	public List<string> ScrollingMessages { get; set; } = new();

	/// <summary>
	/// Annunciator type (e.g., CommonsMain, LordsMain)
	/// </summary>
	[JsonPropertyName("annunciatorType")]
	public string? AnnunciatorType { get; set; }

	/// <summary>
	/// When the message was published
	/// </summary>
	[JsonPropertyName("publishTime")]
	public DateTime PublishTime { get; set; }

	/// <summary>
	/// Whether this is a security override message
	/// </summary>
	[JsonPropertyName("isSecurityOverride")]
	public bool IsSecurityOverride { get; set; }

	/// <summary>
	/// Whether to show the Commons bell
	/// </summary>
	[JsonPropertyName("showCommonsBell")]
	public bool ShowCommonsBell { get; set; }

	/// <summary>
	/// Whether to show the Lords bell
	/// </summary>
	[JsonPropertyName("showLordsBell")]
	public bool ShowLordsBell { get; set; }
}

/// <summary>
/// Represents a slide in an annunciator message
/// </summary>
public class AnnunciatorSlide
{
	/// <summary>
	/// Slide identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Lines of text to display
	/// </summary>
	[JsonPropertyName("lines")]
	public string? Lines { get; set; }

	/// <summary>
	/// Type of slide (e.g., Generic, Division)
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }

	/// <summary>
	/// Order in the carousel
	/// </summary>
	[JsonPropertyName("carouselOrder")]
	public int CarouselOrder { get; set; }

	/// <summary>
	/// How long to display the slide in seconds
	/// </summary>
	[JsonPropertyName("carouselDisplaySeconds")]
	public int? CarouselDisplaySeconds { get; set; }

	/// <summary>
	/// Speaker time
	/// </summary>
	[JsonPropertyName("speakerTime")]
	public DateTime? SpeakerTime { get; set; }

	/// <summary>
	/// Slide time
	/// </summary>
	[JsonPropertyName("slideTime")]
	public DateTime? SlideTime { get; set; }

	/// <summary>
	/// Sound to play (e.g., DivisionBell)
	/// </summary>
	[JsonPropertyName("soundToPlay")]
	public string? SoundToPlay { get; set; }
}
