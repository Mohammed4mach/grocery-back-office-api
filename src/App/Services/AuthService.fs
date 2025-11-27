namespace App.Services

open Core.Entities
open Core.Exceptions
open Core.Exceptions.Authentication
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module AuthService =
    let private repo = UserRepository :> IRepository<User | null>

    let authenticateUser (username : string) (password : string) : User =
        let conditions : Condition<string> seq = [ Helpers.Database.where "username" (Some username) ]
        let authException = AuthenticationException "Invalid credentials"

        try
            let user = repo.findWhere [] conditions

            if not (Helpers.Hash.verifyHashed password user.password) then
                raise authException

            user
        with
        | EntityNotFoundError _ -> raise authException

