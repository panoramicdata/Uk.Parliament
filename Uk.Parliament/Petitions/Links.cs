using System.Runtime.Serialization;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	///  Response links
	/// </summary>
	[DataContract]
	public class Links
	{
		/// <summary>
		/// Self link
		/// </summary>
		[DataMember(Name = "self")]
		public string Self { get; set; }

		/// <summary>
		/// First link
		/// </summary>
		[DataMember(Name = "first")]
		public string First { get; set; }

		/// <summary>
		/// Last link
		/// </summary>
		[DataMember(Name = "last")]
		public string Last { get; set; }

		/// <summary>
		/// Next link
		/// </summary>
		[DataMember(Name = "next")]
		public string Next { get; set; }

		/// <summary>
		/// Previous link
		/// </summary>
		[DataMember(Name = "prev")]
		public string Previous { get; set; }
	}
}