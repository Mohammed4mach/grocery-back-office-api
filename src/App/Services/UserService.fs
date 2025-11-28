namespace App.Services

open Core.Entities
open Core.Exceptions.Authentication
open App.Interfaces
open App.Repositories
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding
/// user resource
/// </summary>
module UserService =
    let private repo = UserRepository :> IRepository<User | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : User seq =
        repo.get [] filters

    /// <summary>
    /// Get the record of the resource based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    /// <returns>The entity that match for the identifier</returns>
    let show (id : int) : User =
        repo.find (id.ToString()) []

    /// <summary>
    /// Store a record of the resource
    /// </summary>
    /// <param name="user">The user to be stored</param>
    /// <returns>The stored user</returns>
    let store (user : User) : User =
        let storedUser : User = { user with password = Helpers.Hash.hash user.password }

        repo.store storedUser

    /// <summary>
    /// Update user that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedUser">The values to be updated</param>
    /// <returns>The updated record</returns>
    let update (id : int) (updatedUser : User) : User =
        let user = repo.find (id.ToString()) []

        if not(Helpers.Hash.verifyHashed updatedUser.password user.password) then
            raise (AuthenticationException "Wrong Password")

        let values = { updatedUser with password = Helpers.Hash.hash updatedUser.password }

        repo.update (id.ToString()) values

    /// <summary>
    /// Update the password of user that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="password">The current password</param>
    /// <param name="newPassword">The new password</param>
    /// <returns>The updated user</returns>
    let updatePassword (id : int) (password : string) (newPassword : string) : User =
        let user = repo.find (id.ToString()) []

        if not(Helpers.Hash.verifyHashed password user.password) then
            raise (AuthenticationException "Wrong Password")

        let updatedUser : User = { user with password = Helpers.Hash.hash newPassword }

        repo.partialUpdate (id.ToString()) ["password"] updatedUser

    /// <summary>
    /// Delete the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

