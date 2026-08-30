namespace Uk.Parliament.Requests;

/// <summary>
/// Marker interface for request types that support automatic pagination.
/// Implement this interface on a request record to enable
/// <see cref="ParliamentClient.GetAllAsync{TItem}"/> and
/// <see cref="ParliamentClient.GetAllListAsync{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">The item type returned by pagination.</typeparam>
// S2326: TItem is a deliberate phantom type parameter. It carries the element type of the
// paginated response so that GetAllAsync/GetAllListAsync can infer it from the request alone.
#pragma warning disable S2326
public interface IPaginatedRequest<TItem>;
#pragma warning restore S2326
