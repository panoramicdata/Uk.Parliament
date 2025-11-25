using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Interests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Member Interests API client using Refit
/// </summary>
/// <remarks>
/// Provides access to the Register of Members' Financial Interests
/// </remarks>
public interface IInterestsApi
{
	/// <summary>
	/// Get all interest categories
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of interest categories</returns>
	[Get("/api/v1/Categories")]
	Task<List<InterestCategory>> GetCategoriesAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific category by ID
	/// </summary>
	/// <param name="id">Category identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Interest category details</returns>
	[Get("/api/v1/Categories/{id}")]
	Task<InterestCategory> GetCategoryByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search interests across all members
	/// </summary>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="searchTerm">Search term to filter interests</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of interests matching criteria</returns>
	[Get("/api/v1/Interests")]
	Task<List<Interest>> SearchInterestsAsync(
		[Query] int? memberId = null,
		[Query] int? categoryId = null,
		[Query] string? searchTerm = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific interest by ID
	/// </summary>
	/// <param name="id">Interest identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Interest details</returns>
	[Get("/api/v1/Interests/{id}")]
	Task<Interest> GetInterestByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all registers
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of registers</returns>
	[Get("/api/v1/Registers")]
	Task<List<InterestRegister>> GetRegistersAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific register by ID
	/// </summary>
	/// <param name="id">Register identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Register details</returns>
	[Get("/api/v1/Registers/{id}")]
	Task<InterestRegister> GetRegisterByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
