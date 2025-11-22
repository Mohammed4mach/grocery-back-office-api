namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module CustomerService =
    let index (filters : Condition seq) : Customer seq =
        let customers = CustomerRepository.get filters

        customers

    let show (id : string) : Customer =
        let customer = CustomerRepository.find id

        customer

    let store (customer : Customer) : unit =
        CustomerRepository.store customer

    let update (id : string) (updatedCustomer : Customer) : unit =
        let customer = CustomerRepository.find id

        CustomerRepository.update id updatedCustomer

    let delete (id : string) : unit =
        CustomerRepository.delete id

