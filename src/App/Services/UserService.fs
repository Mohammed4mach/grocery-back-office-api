namespace App.Services

open Core.Entities
open Core.Exceptions.Authentication
open App.Interfaces
open App.Repositories
open Infrastructure.Core.Types

module UserService =
    let private repo = UserRepository :> IRepository<User | null>

    let index (filters : Condition seq) : User seq =
        repo.get [] filters

    let show (id : int) : User =
        repo.find (id.ToString()) []

    let store (user : User) : User =
        let storedUser : User = { user with password = Helpers.Hash.hash user.password }

        repo.store storedUser

    let update (id : int) (updatedUser : User) : User =
        let user = repo.find (id.ToString()) []

        if not(Helpers.Hash.verifyHashed updatedUser.password user.password) then
            raise (AuthenticationException "Wrong Password")

        let values = { updatedUser with password = Helpers.Hash.hash updatedUser.password }

        repo.update (id.ToString()) values

    let updatePassword (id : int) (password : string) (newPassword : string) : User =
        let user = repo.find (id.ToString()) []

        if not(Helpers.Hash.verifyHashed password user.password) then
            raise (AuthenticationException "Wrong Password")

        let updatedUser : User = { user with password = Helpers.Hash.hash newPassword }

        repo.partialUpdate (id.ToString()) ["password"] updatedUser

    let delete (id : int) : unit =
        repo.delete (id.ToString())

