namespace App.Services

open Core.Entities
open Core.Exceptions
open Core.Exceptions.Authentication
open Infrastructure.Repositories
open Infrastructure.Core.Types

module AuthService =
    let authenticateUser (username : string) (password : string) : User =
        let conditions : Condition seq = [ Helpers.Database.where "username" (Some username) ]
        let authException = AuthenticationException "Invalid credentials"

        try
            let user = UserRepository.findWhere [] conditions

            if not (Helpers.Hash.verifyHashed password user.password) then
                raise authException

            user
        with
        | EntityNotFoundError _ -> raise authException

