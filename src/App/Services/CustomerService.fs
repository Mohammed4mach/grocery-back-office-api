namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding customer resource
/// </summary>
module CustomerService =
    let private repo = CustomerRepository :> IRepository<Customer | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : Customer seq =
        let customers = repo.get [] filters

        customers

    /// <summary>
    /// Get the record of the resource based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    /// <returns>The entity that match for the identifier</returns>
    let show (id : int) : Customer =
        let customer = repo.find (id.ToString()) []

        customer

    /// <summary>
    /// Store a record of the resource
    /// </summary>
    /// <param name="customer">The customer to be stored</param>
    /// <returns>The stored customer</returns>
    let store (customer : Customer) : Customer =
        repo.store customer

    /// <summary>
    /// Update the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedCustomer">The values to be updated</param>
    /// <returns>The updated record</returns>
    let update (id : int) (updatedCustomer : Customer) : Customer =
        repo.update (id.ToString()) updatedCustomer

    /// <summary>
    /// Delete the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

