using System.Runtime.Serialization;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	/// An API response
	/// </summary>
	[DataContract]
	public class ApiResponse<T>
	{
		/// <summary>
		/// Links
		/// </summary>
		[DataMember(Name = "links")]
		public Links Links { get; set; }

		/// <summary>
		/// The data
		/// </summary>
		[DataMember(Name = "data")]
		public T Data { get; set; }
	}
}