using System.Runtime.Serialization;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	/// Country signatures
	/// </summary>
	[DataContract]
	public class CountrySignatures
	{
		/// <summary>
		/// Name
		/// </summary>
		[DataMember(Name="name")]
		public string Name { get; set; }

		/// <summary>
		/// Code
		/// </summary>
		[DataMember(Name = "code")]
		public string Code { get; set; }

		/// <summary>
		/// Signature count
		/// </summary>
		[DataMember(Name = "signature_count")]
		public string SignatureCount { get; set; }
	}
}