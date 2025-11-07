namespace Infrastructure.Core

module Exceptions =
    exception DatabaseConnectionError of string
    exception DatabaseChoosingError of string

