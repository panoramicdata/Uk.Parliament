using Microsoft.Extensions.Logging;

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
	/// Base URL for the Member Interests API
	/// </summary>
	public string InterestsBaseUrl { get; set; } = "https://interests-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Written Questions and Statements API
	/// </summary>
	public string QuestionsStatementsBaseUrl { get; set; } = "https://questions-statements-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Oral Questions and Motions API
	/// </summary>
	public string OralQuestionsMotionsBaseUrl { get; set; } = "https://oralquestionsandmotions-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Treaties API
	/// </summary>
	public string TreatiesBaseUrl { get; set; } = "https://treaties-api.parliament.uk/";

	/// <summary>
	/// Base URL for the Erskine May API
	/// </summary>
	public string ErskineMayBaseUrl { get; set; } = "https://erskinemay-api.parliament.uk/";

	/// <summary>
	/// Base URL for the NOW (Annunciator) API
	/// </summary>
	public string NowBaseUrl { get; set; } = "https://now-api.parliament.uk/";

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
	public bool EnableDebugValidation { get; set; }
#if DEBUG
		= true;
#endif

	/// <summary>
	/// Logger for HTTP request/response diagnostics
	/// </summary>
	public ILogger? Logger { get; set; }

	/// <summary>
	/// Enable verbose HTTP logging (includes headers and body)
	/// </summary>
	public bool EnableVerboseLogging { get; set; }
#if DEBUG
		 = true;
#endif
}
