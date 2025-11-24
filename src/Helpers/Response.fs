namespace Helpers

module Response =
    [<CLIMutable>]
    type ErrorResponseBody = {
        code : int
        message : string
    }

    let getErrorBody (message : string) (status : int) : ErrorResponseBody =
        let body : ErrorResponseBody = {
            code = status
            message = message
        }

        body

