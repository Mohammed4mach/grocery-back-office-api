namespace Infrastructure.Core

/// <summary>
/// Database exceptions
/// </summary>
module Exceptions =

    /// <summary>
    /// Indicates error connecting with the database
    /// </summary>
    exception DatabaseConnectionError of string

    /// <summary>
    /// Indicates misconfigured database connection
    /// </summary>
    exception DatabaseChoosingError of string

