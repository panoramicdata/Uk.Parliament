namespace Uk.Parliament.Models;

/// <summary>
/// Petition state
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PetitionState
{
	/// <summary>
	/// Open
	/// </summary>
	Open,

	/// <summary>
	/// Closed
	/// </summary>
	Closed,

	/// <summary>
	/// Rejected
	/// </summary>
	Rejected,

	/// <summary>
	/// Hidden
	/// </summary>
	Hidden,

	/// <summary>
	/// Stopped
	/// </summary>
	Stopped
}