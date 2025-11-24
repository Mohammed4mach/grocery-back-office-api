namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module CustomerService =
    let index (filters : Condition seq) : Customer seq =
        let customers = CustomerRepository.get filters

        customers

    let show (id : int) : Customer =
        let customer = CustomerRepository.find (id.ToString())

        customer

    let store (customer : Customer) : Customer =
        CustomerRepository.store customer

    let update (id : int) (updatedCustomer : Customer) : Customer =
        CustomerRepository.update (id.ToString()) updatedCustomer

    let delete (id : int) : unit =
        CustomerRepository.delete (id.ToString())

