using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Committees;

/// <summary>
/// Represents a parliamentary committee
/// </summary>
public class Committee
{
	/// <summary>
	/// Committee identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Committee name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Parent committee (if this is a sub-committee)
	/// </summary>
	[JsonPropertyName("parentCommittee")]
	public Committee? ParentCommittee { get; set; }

	/// <summary>
	/// List of sub-committees (simplified - contains dynamic data)
	/// </summary>
	[JsonPropertyName("subCommittees")]
	public List<Committee>? SubCommittees { get; set; }

	/// <summary>
	/// House (Commons or Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// Lead house (for joint committees)
	/// </summary>
	[JsonPropertyName("leadHouse")]
	public LeadHouse? LeadHouse { get; set; }

	/// <summary>
	/// Committee category
	/// </summary>
	[JsonPropertyName("category")]
	public CommitteeCategory? Category { get; set; }

	/// <summary>
	/// Committee types (simplified - contains dynamic type data)
	/// </summary>
	[JsonPropertyName("committeeTypes")]
	public List<CommitteeType>? CommitteeTypes { get; set; }

	/// <summary>
	/// Whether to show on website
	/// </summary>
	[JsonPropertyName("showOnWebsite")]
	public bool ShowOnWebsite { get; set; }

	/// <summary>
	/// Legacy website URL
	/// </summary>
	[JsonPropertyName("websiteLegacyUrl")]
	public string? WebsiteLegacyUrl { get; set; }

	/// <summary>
	/// Whether legacy redirect is enabled
	/// </summary>
	[JsonPropertyName("websiteLegacyRedirectEnabled")]
	public bool WebsiteLegacyRedirectEnabled { get; set; }

	/// <summary>
	/// Committee start date
	/// </summary>
	[JsonPropertyName("startDate")]
	public DateTime? StartDate { get; set; }

	/// <summary>
	/// Committee end date (null if still active)
	/// </summary>
	[JsonPropertyName("endDate")]
	public DateTime? EndDate { get; set; }

	/// <summary>
	/// Date appointed in Commons
	/// </summary>
	[JsonPropertyName("dateCommonsAppointed")]
	public DateTime? DateCommonsAppointed { get; set; }

	/// <summary>
	/// Date appointed in Lords
	/// </summary>
	[JsonPropertyName("dateLordsAppointed")]
	public DateTime? DateLordsAppointed { get; set; }

	/// <summary>
	/// Departments being scrutinised
	/// </summary>
	[JsonPropertyName("scrutinisingDepartments")]
	public List<ScrutinisingDepartment>? ScrutinisingDepartments { get; set; }

	/// <summary>
	/// Whether this is the lead committee
	/// </summary>
	[JsonPropertyName("isLeadCommittee")]
	public bool? IsLeadCommittee { get; set; }

	/// <summary>
	/// Name history (simplified - contains dynamic date/name data)
	/// </summary>
	[JsonPropertyName("nameHistory")]
	public List<NameHistory>? NameHistory { get; set; }

	/// <summary>
	/// Contact information
	/// </summary>
	[JsonPropertyName("contact")]
	public CommitteeContact? Contact { get; set; }
}

/// <summary>
/// Lead house information for joint committees
/// </summary>
public class LeadHouse
{
	/// <summary>
	/// Whether Commons is the lead house
	/// </summary>
	[JsonPropertyName("isCommons")]
	public bool IsCommons { get; set; }

	/// <summary>
	/// Whether Lords is the lead house
	/// </summary>
	[JsonPropertyName("isLords")]
	public bool IsLords { get; set; }
}

/// <summary>
/// Department being scrutinised by a committee
/// </summary>
public class ScrutinisingDepartment
{
	/// <summary>
	/// Department identifier
	/// </summary>
	[JsonPropertyName("departmentId")]
	public int DepartmentId { get; set; }

	/// <summary>
	/// Department name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Committee category information
/// </summary>
public class CommitteeCategory
{
	/// <summary>
	/// Category identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Category name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Committee type information
/// </summary>
public class CommitteeType
{
	/// <summary>
	/// Type identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Type name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Committee category
	/// </summary>
	[JsonPropertyName("committeeCategory")]
	public CommitteeCategory? CommitteeCategory { get; set; }
}

/// <summary>
/// Committee name history
/// </summary>
public class NameHistory
{
	/// <summary>
	/// History record identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Committee identifier
	/// </summary>
	[JsonPropertyName("committeeId")]
	public int CommitteeId { get; set; }

	/// <summary>
	/// Name at this point in history
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Start date for this name
	/// </summary>
	[JsonPropertyName("startDate")]
	public DateTime StartDate { get; set; }

	/// <summary>
	/// End date for this name (null if current)
	/// </summary>
	[JsonPropertyName("endDate")]
	public DateTime? EndDate { get; set; }
}

/// <summary>
/// Committee contact information
/// </summary>
public class CommitteeContact
{
	/// <summary>
	/// Email address
	/// </summary>
	[JsonPropertyName("email")]
	public string? Email { get; set; }

	/// <summary>
	/// Phone number
	/// </summary>
	[JsonPropertyName("phone")]
	public string? Phone { get; set; }

	/// <summary>
	/// Postal address
	/// </summary>
	[JsonPropertyName("address")]
	public string? Address { get; set; }

	/// <summary>
	/// Contact disclaimer
	/// </summary>
	[JsonPropertyName("contactDisclaimer")]
	public string? ContactDisclaimer { get; set; }
}
