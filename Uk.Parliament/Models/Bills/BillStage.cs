using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Bills;

/// <summary>
/// Represents a stage in a bill's parliamentary progress
/// </summary>
public class BillStage
{
	/// <summary>
	/// Stage identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Stage type identifier
	/// </summary>
	[JsonPropertyName("stageId")]
	public int StageId { get; set; }

	/// <summary>
	/// Session identifier
	/// </summary>
	[JsonPropertyName("sessionId")]
	public int SessionId { get; set; }

	/// <summary>
	/// Stage description
	/// </summary>
	[JsonPropertyName("description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Stage abbreviation
	/// </summary>
	[JsonPropertyName("abbreviation")]
	public string? Abbreviation { get; set; }

	/// <summary>
	/// House where the stage is taking place
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// List of sittings for this stage
	/// </summary>
	[JsonPropertyName("stageSittings")]
	public List<StageSitting> StageSittings { get; set; } = new();

	/// <summary>
	/// Sort order for display
	/// </summary>
	[JsonPropertyName("sortOrder")]
	public int SortOrder { get; set; }
}

/// <summary>
/// Represents a sitting during a bill stage
/// </summary>
public class StageSitting
{
	/// <summary>
	/// Sitting identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Stage type identifier
	/// </summary>
	[JsonPropertyName("stageId")]
	public int StageId { get; set; }

	/// <summary>
	/// Bill stage identifier
	/// </summary>
	[JsonPropertyName("billStageId")]
	public int BillStageId { get; set; }

	/// <summary>
	/// Bill identifier
	/// </summary>
	[JsonPropertyName("billId")]
	public int BillId { get; set; }

	/// <summary>
	/// Date of the sitting
	/// </summary>
	[JsonPropertyName("date")]
	public DateTime Date { get; set; }
}
