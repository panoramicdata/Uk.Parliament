namespace Uk.Parliament.Requests;

/// <summary>
/// Marker interface for request types that support automatic pagination.
/// Implement this interface on a request record to enable
/// <see cref="ParliamentClient.GetAllAsync{TItem}"/> and
/// <see cref="ParliamentClient.GetAllListAsync{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">The item type returned by pagination.</typeparam>
public interface IPaginatedRequest<TItem>;
