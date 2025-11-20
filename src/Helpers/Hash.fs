namespace Helpers

open BCrypt.Net

module Hash =
    let hash (str : string) : string =
        BCrypt.HashPassword (str, workFactor = 12)

    let verifyHashed (str : string) (hash : string) : bool =
        BCrypt.Verify (str, hash)

