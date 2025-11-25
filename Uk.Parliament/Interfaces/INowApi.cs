using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Now;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament NOW (Annunciator) API client using Refit
/// </summary>
/// <remarks>
/// Provides real-time information about chamber annunciator displays
/// </remarks>
public interface INowApi
{
	/// <summary>
	/// Get current annunciator message for a specific annunciator
	/// </summary>
	/// <param name="annunciator">Annunciator identifier (e.g., "commons", "lords")</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Current annunciator message</returns>
	[Get("/api/Message/message/{annunciator}/current")]
	Task<AnnunciatorMessage> GetCurrentMessageAsync(
		string annunciator,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get annunciator message for a specific annunciator and date
	/// </summary>
	/// <param name="annunciator">Annunciator identifier (e.g., "commons", "lords")</param>
	/// <param name="date">Date in format YYYY-MM-DD</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Annunciator message for the specified date</returns>
	[Get("/api/Message/message/{annunciator}/{date}")]
	Task<AnnunciatorMessage> GetMessageByDateAsync(
		string annunciator,
		string date,
		CancellationToken cancellationToken = default);
}
