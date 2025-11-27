namespace Helpers

open System.Dynamic
open System.Collections.Generic

module DynamicObject =
    let ofMap (_map : Map<string, 'obj>) : ExpandoObject =
        let expando     = new ExpandoObject()
        let expandoDict = expando :> IDictionary<string, obj>

        for KeyValue(key, value) in _map do
            expandoDict.Add (key, value)

        expando

