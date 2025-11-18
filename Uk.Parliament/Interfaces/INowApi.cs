using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Now;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament NOW (Annunciator) API client using Refit
/// </summary>
/// <remarks>
/// Provides real-time information about chamber activities
/// </remarks>
public interface INowApi
{
	/// <summary>
	/// Get current status of the House of Commons
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Commons chamber status</returns>
	[Get("/api/Now/Commons")]
	Task<ChamberStatus> GetCommonsStatusAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get current status of the House of Lords
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Lords chamber status</returns>
	[Get("/api/Now/Lords")]
	Task<ChamberStatus> GetLordsStatusAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get upcoming business items for a house
	/// </summary>
	/// <param name="house">House identifier (Commons/Lords)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of upcoming business items</returns>
	[Get("/api/Now/{house}/Business")]
	Task<List<BusinessItem>> GetUpcomingBusinessAsync(
		string house,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get current business item for a house
	/// </summary>
	/// <param name="house">House identifier (Commons/Lords)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Current business item</returns>
	[Get("/api/Now/{house}/Current")]
	Task<BusinessItem?> GetCurrentBusinessAsync(
		string house,
		CancellationToken cancellationToken = default);
}
