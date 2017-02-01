using System.Runtime.Serialization;
using JetBrains.Annotations;
using Uk.Parliament.Interfaces;

namespace Uk.Parliament.Petitions
{
	/// <summary>
	/// A petition resource
	/// </summary>
	[DataContract(Name = "petition")]
	[UsedImplicitly]
	public class Petition : Resource
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Petition() : base("petitions")
		{
		}

		/// <summary>
		///  The petition attributes
		/// </summary>
		[DataMember(Name = "attributes")]
		public PetitionAttributes Attributes { get; set; }

		/// <inheritdoc />
		public override string ToString() => $"{Id}: {Attributes}";
	}
}
