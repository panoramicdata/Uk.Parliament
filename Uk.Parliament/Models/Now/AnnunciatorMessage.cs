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
	public List<AnnunciatorSlide> Slides { get; set; } = [];

	/// <summary>
	/// Scrolling messages
	/// </summary>
	[JsonPropertyName("scrollingMessages")]
	public List<ScrollingMessage> ScrollingMessages { get; set; } = [];

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
	/// Lines of content to display
	/// </summary>
	[JsonPropertyName("lines")]
	public List<SlideLine> Lines { get; set; } = [];

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

/// <summary>
/// Represents a line of content in a slide
/// </summary>
public class SlideLine
{
	/// <summary>
	/// Display order of this line
	/// </summary>
	[JsonPropertyName("displayOrder")]
	public int DisplayOrder { get; set; }

	/// <summary>
	/// Content type (e.g., Generic, Member)
	/// </summary>
	[JsonPropertyName("contentType")]
	public string? ContentType { get; set; }

	/// <summary>
	/// Content URL if applicable
	/// </summary>
	[JsonPropertyName("contentUrl")]
	public string? ContentUrl { get; set; }

	/// <summary>
	/// Additional JSON content
	/// </summary>
	[JsonPropertyName("contentAdditionalJson")]
	public string? ContentAdditionalJson { get; set; }

	/// <summary>
	/// Style of the line (e.g., Text100, Member)
	/// </summary>
	[JsonPropertyName("style")]
	public string? Style { get; set; }

	/// <summary>
	/// Horizontal alignment
	/// </summary>
	[JsonPropertyName("horizontalAlignment")]
	public string? HorizontalAlignment { get; set; }

	/// <summary>
	/// Vertical alignment
	/// </summary>
	[JsonPropertyName("verticalAlignment")]
	public string? VerticalAlignment { get; set; }

	/// <summary>
	/// Content text
	/// </summary>
	[JsonPropertyName("content")]
	public string? Content { get; set; }

	/// <summary>
	/// Member information if contentType is Member
	/// </summary>
	[JsonPropertyName("member")]
	public SlideLineMember? Member { get; set; }

	/// <summary>
	/// Whether to force capitalisation
	/// </summary>
	[JsonPropertyName("forceCapitalisation")]
	public bool ForceCapitalisation { get; set; }
}

/// <summary>
/// Represents member information in a slide line
/// </summary>
public class SlideLineMember
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
	/// Full title
	/// </summary>
	[JsonPropertyName("nameFullTitle")]
	public string? NameFullTitle { get; set; }

	/// <summary>
	/// Address name
	/// </summary>
	[JsonPropertyName("nameAddressAs")]
	public string? NameAddressAs { get; set; }

	/// <summary>
	/// Thumbnail URL
	/// </summary>
	[JsonPropertyName("thumbnailUrl")]
	public string? ThumbnailUrl { get; set; }

	/// <summary>
	/// Latest party information
	/// </summary>
	[JsonPropertyName("latestParty")]
	public MemberPartyInfo? LatestParty { get; set; }

	/// <summary>
	/// Latest house membership
	/// </summary>
	[JsonPropertyName("latestHouseMembership")]
	public MemberHouseMembershipInfo? LatestHouseMembership { get; set; }
}

/// <summary>
/// Party information for a member
/// </summary>
public class MemberPartyInfo
{
	/// <summary>
	/// Party ID
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Party name
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	/// <summary>
	/// Background colour (hex)
	/// </summary>
	[JsonPropertyName("backgroundColour")]
	public string? BackgroundColour { get; set; }
}

/// <summary>
/// House membership information for a member
/// </summary>
public class MemberHouseMembershipInfo
{
	/// <summary>
	/// Membership from (constituency or title)
	/// </summary>
	[JsonPropertyName("membershipFrom")]
	public string? MembershipFrom { get; set; }
}
