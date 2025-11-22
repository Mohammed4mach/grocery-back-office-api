namespace App.Services

open Core.Entities
open Core.Exceptions.Authentication
open Infrastructure.Repositories
open Infrastructure.Core.Types

module UserService =
    let index (filters : Condition seq) : User seq =
        UserRepository.get filters

    let show (id : string) : User =
        UserRepository.find id

    let store (user : User) : unit =
        let storedUser : User = { user with password = Helpers.Hash.hash(user.password) }

        UserRepository.store storedUser

    let update (id : string) (updatedUser : User) : unit =
        let user = UserRepository.find id

        if not(Helpers.Hash.verifyHashed updatedUser.password user.password) then
            raise (AuthenticationException("Invalid Password"))

        let values = { updatedUser with password = Helpers.Hash.hash(updatedUser.password) }

        UserRepository.update id values

    let delete (id : string) : unit =
        UserRepository.delete id

