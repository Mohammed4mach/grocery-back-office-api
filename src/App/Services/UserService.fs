namespace App.Services

open Core.Entities
open Core.Exceptions.Authentication
open Infrastructure.Repositories
open Infrastructure.Core.Types

module UserService =
    let index (filters : Condition seq) : User seq =
        UserRepository.get filters

    let show (id : int) : User =
        UserRepository.find (id.ToString())

    let store (user : User) : User =
        let storedUser : User = { user with password = Helpers.Hash.hash user.password }

        UserRepository.store storedUser

    let update (id : int) (updatedUser : User) : User =
        let user = UserRepository.find (id.ToString())

        if not(Helpers.Hash.verifyHashed updatedUser.password user.password) then
            raise (AuthenticationException "Wrong Password")

        let values = { updatedUser with password = Helpers.Hash.hash updatedUser.password }

        UserRepository.update (id.ToString()) values

    let updatePassword (id : int) (password : string) (newPassword : string) : User =
        let user = UserRepository.find (id.ToString())

        if not(Helpers.Hash.verifyHashed password user.password) then
            raise (AuthenticationException "Wrong Password")

        let updatedUser : User = { user with password = Helpers.Hash.hash newPassword }

        UserRepository.partialUpdate (id.ToString()) ["password"] updatedUser

    let delete (id : int) : unit =
        UserRepository.delete (id.ToString())

