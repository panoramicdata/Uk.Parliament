using System.Collections.Generic;

namespace Uk.Parliament.Petitions;

/// <summary>
///  A query for searching petitions
/// </summary>
public class Query
{
	/// <summary>
	///  The query text
	/// </summary>
	public string Text { get; set; }

	/// <summary>
	/// The maximum number to take
	/// </summary>
	public int? PageSize { get; set; }

	/// <summary>
	/// The page to retrieve
	/// </summary>
	public int? PageNumber { get; set; }

	/// <summary>
	/// Filter by petition state
	/// </summary>
	public PetitionState? State { get; set; }

	/// <inheritdoc />
	public override string ToString()
	{
		var s = new List<string>();

		// Search
		if (!string.IsNullOrWhiteSpace(Text))
		{
			s.Add($"search={Text}");
		}

		// State filter
		if (State.HasValue)
		{
			s.Add($"state={State.Value.ToString().ToLowerInvariant()}");
		}

		// Page size
		if (PageSize != null)
		{
			s.Add($"_pageSize={PageSize}");
		}

		// Page number
		if (PageNumber != null)
		{
			s.Add($"page={PageNumber}");
		}

		return s.Count == 0 ? string.Empty : $"?{string.Join("&", s)}";
	}
}
