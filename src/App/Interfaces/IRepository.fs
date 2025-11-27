namespace App.Interfaces

open System
open Infrastructure.Core.Types

type IRepository<'T when 'T : equality and 'T : null> =
    abstract member get<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : Join<'Z> seq -> Condition<'Y> seq -> 'T list
    abstract member find<'U, 'P, 'Z when 'Z : null> : string -> Join<'Z> seq -> 'T
    abstract member findWhere<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : Join<'Z> seq -> Condition<'Y> seq -> 'T
    abstract member count<'P, 'Y, 'Z when 'Y : null and 'Z : null> : Condition<'Y> seq -> int
    abstract member store<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : 'T -> 'T
    abstract member update<'U, 'P, 'Z when 'Z : null> : string -> 'T -> 'T
    abstract member partialUpdate<'U, 'P, 'Z when 'Z : null> : string -> string seq -> 'T -> 'T
    abstract member delete<'U, 'P, 'Z when 'Z : null> : string -> unit
    abstract member getWithRelation<'U, 'Y, 'Z when 'Y : null and 'Z : null> : Join<'Z> seq -> Condition<'Y> seq -> Func<'T, 'U, 'T> -> 'T list

