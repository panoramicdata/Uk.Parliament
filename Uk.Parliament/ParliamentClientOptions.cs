using System;
using Microsoft.Extensions.Logging;
using Refit;

namespace Uk.Parliament;

/// <summary>
/// Configuration options for the UK Parliament API client
/// </summary>
public class ParliamentClientOptions
{
	/// <summary>
	/// Base URL for the Petitions API
	/// </summary>
	public string PetitionsBaseUrl { get; set; } = "https://petition.parliament.uk/";

	/// <summary>
	/// Base URL for the Members API
	/// </summary>
	public string MembersBaseUrl { get; set; } = "https://members-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Bills API
	/// </summary>
	public string BillsBaseUrl { get; set; } = "https://bills-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Committees API
	/// </summary>
	public string CommitteesBaseUrl { get; set; } = "https://committees-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Commons Divisions API
	/// </summary>
	public string CommonsDivisionsBaseUrl { get; set; } = "https://commonsvotes-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Lords Divisions API
	/// </summary>
	public string LordsDivisionsBaseUrl { get; set; } = "https://lordsvotes-api.parliament.uk/";

	/// <summary>
	/// User agent string for API requests
	/// </summary>
	public string UserAgent { get; set; } = "Uk.Parliament.NET/2.0";

	/// <summary>
	/// Timeout for HTTP requests
	/// </summary>
	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

	/// <summary>
	/// Enable debug validation (unmapped properties throw exceptions)
	/// </summary>
	public bool EnableDebugValidation { get; set; } =
#if DEBUG
		true;
#else
		false;
#endif

	/// <summary>
	/// Logger for HTTP request/response diagnostics
	/// </summary>
	public ILogger? Logger { get; set; }

	/// <summary>
	/// Enable verbose HTTP logging (includes headers and body)
	/// </summary>
	public bool EnableVerboseLogging { get; set; } =
#if DEBUG
		true;
#else
		false;
#endif
}
