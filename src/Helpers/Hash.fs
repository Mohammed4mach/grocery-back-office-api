namespace Helpers

open BCrypt.Net

/// <summary>
/// Module that holds string hashing related helpers
/// </summary>
module Hash =

    /// <summary>
    /// Hash a given string. The used algorimth is `bcrypt`
    /// </summary>
    /// <param name="str">The string to be hashed</param>
    /// <returns>The hashed string</returns>
    let hash (str : string) : string =
        BCrypt.HashPassword (str, workFactor = 12)

    /// <summary>
    /// Verify a string against a hashed value using `bcrypt` algorithm
    /// </summary>
    /// <param name="str">The string to be validated</param>
    /// <returns>
    /// True if the hash is generated from an identical string value
    /// </returns>
    let verifyHashed (str : string) (hash : string) : bool =
        BCrypt.Verify (str, hash)

