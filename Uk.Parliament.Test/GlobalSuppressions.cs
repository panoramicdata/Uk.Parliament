// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
	"Naming",
	"CA1707:Identifiers should not contain underscores",
	Justification = "OK for test projects"
)]

// xUnit1004: Skipped tests - intentional for integration tests requiring live APIs
[assembly: SuppressMessage(
	"Usage",
	"xUnit1004:Test methods should not be skipped",
	Justification = "Integration tests require live Parliament APIs which may not be accessible during development"
)]

// CA1852: Seal internal types - not applicable for test helpers
[assembly: SuppressMessage(
	"Performance",
	"CA1852:Seal internal types",
	Justification = "Test helper classes don't require sealing"
)]
