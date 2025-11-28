namespace Helpers

/// <summary>
/// Module that contains HTTP responses helpers
/// </summary>
module Response =

    /// <summary>
    /// The error body response used in the application
    /// </summary>
    [<CLIMutable>]
    type ErrorResponseBody = {
        code : int
        message : string
    }

    /// <summary>
    /// Get error body in form that is used in the application
    /// </summary>
    /// <param name="message">The message indicating the error</param>
    /// <param name="status">HTTP status code</param>
    /// <returns>App's error response body</returns>
    let getErrorBody (message : string) (status : int) : ErrorResponseBody =
        let body : ErrorResponseBody = {
            code = status
            message = message
        }

        body

