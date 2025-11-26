namespace App.Interfaces

open Infrastructure.Core.Types

type IRepository<'T when 'T : equality and 'T : null> =
    abstract member get : Join seq -> Condition seq -> 'T list
    abstract member find : string -> Join seq -> 'T
    abstract member findWhere : Join seq -> Condition seq -> 'T
    abstract member count : Condition seq -> int
    abstract member store : 'T -> 'T
    abstract member update : string -> 'T -> 'T
    abstract member partialUpdate : string -> string seq -> 'T -> 'T
    abstract member delete : string -> unit

