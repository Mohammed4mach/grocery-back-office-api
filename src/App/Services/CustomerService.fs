namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module CustomerService =
    let private repo = CustomerRepository :> IRepository<Customer | null>

    let index (filters : Condition seq) : Customer seq =
        let customers = repo.get [] filters

        customers

    let show (id : int) : Customer =
        let customer = repo.find (id.ToString()) []

        customer

    let store (customer : Customer) : Customer =
        repo.store customer

    let update (id : int) (updatedCustomer : Customer) : Customer =
        repo.update (id.ToString()) updatedCustomer

    let delete (id : int) : unit =
        repo.delete (id.ToString())

