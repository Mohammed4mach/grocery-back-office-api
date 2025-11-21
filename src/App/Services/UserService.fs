namespace App.Services

open Core.Entities
open Core.Exceptions.Authentication
open Infrastructure.Repositories
open Infrastructure.Core.Types

module UserService =
    let index (filters : Condition seq) : User seq =
        let users = UserRepository.get filters

        users

    let show (id : string) : User =
        let user = UserRepository.find id

        user

    let store (user : User) : unit =
        UserRepository.store user

    let update (id : string) (updatedUser : User) : unit =
        let user = UserRepository.find id

        if not(Helpers.Hash.verifyHashed updatedUser.password user.password) then
            raise (AuthenticationException("Invalid Password"))

        let values = { updatedUser with password = Helpers.Hash.hash(updatedUser.password) }

        UserRepository.update id values

    let delete (id : string) : unit =
        UserRepository.delete id

