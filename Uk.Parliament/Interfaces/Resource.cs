using System.Runtime.Serialization;
using Uk.Parliament.Petitions;

namespace Uk.Parliament.Interfaces
{
	/// <summary>
	/// An API resource
	/// </summary>
	[DataContract]
	[KnownType(typeof(Petition))]
	public abstract class Resource
	{
		private readonly string _baseEndpoint;

		/// <summary>
		///  Constructor
		/// </summary>
		/// <param name="baseEndpoint"></param>
		protected Resource(string baseEndpoint)
		{
			_baseEndpoint = baseEndpoint;
		}

		/// <summary>
		///  The Id
		/// </summary>
		[DataMember(Name = "id")]
		public int Id { get; set; }

		/// <summary>
		///  The links
		/// </summary>
		[DataMember(Name = "links")]
		public Links Links { get; set; }

		/// <summary>
		///  The API endpoint
		/// </summary>
		public string Endpoint => $"{_baseEndpoint}{(Id == 0 ? string.Empty : $"/{Id}")}.json";
	}
}