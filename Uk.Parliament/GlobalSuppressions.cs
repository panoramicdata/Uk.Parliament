using System.Diagnostics.CodeAnalysis;

// Suppress specific Code Analysis rules globally
// These are suppressed because they're either too noisy or not applicable to this library

// CA1305: StringBuilder locale warnings - not applicable for diagnostic logging
[assembly: SuppressMessage(
	"Globalization",
	"CA1305:Specify IFormatProvider",
	Justification = "Diagnostic logging doesn't require culture-specific formatting"
)]

// CA1716: Reserved keyword parameter names - unavoidable when matching API parameter names
[assembly: SuppressMessage(
	"Naming",
	"CA1716:Identifiers should not match keywords",
	Justification = "Parameter names match Parliament API specification"
)]

// CA1845: Use span-based string methods - performance optimization not critical for diagnostic logging
[assembly: SuppressMessage(
	"Performance",
	"CA1845:Use span-based 'string.Concat'",
	Justification = "Logging code doesn't require micro-optimization"
)]

// CA1873: Logging parameter evaluation - acceptable for diagnostic purposes
[assembly: SuppressMessage(
	"Performance",
	"CA1873:Avoid conditional access for parameters",
	Justification = "Conditional logging is intentional for diagnostics"
	)]

[assembly: SuppressMessage(
	"Style",
	"IDE0072:Add missing cases",
	Justification = "This is good code - the inspection is incorrect"
)]
