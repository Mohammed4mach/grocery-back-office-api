namespace Helpers

module DynamicObject =
    let ofMap (_map : Map<string, 'obj>) : System.Dynamic.ExpandoObject =
        let expando     = new System.Dynamic.ExpandoObject()
        let expandoDict = expando :> System.Collections.Generic.IDictionary<string, obj>

        for KeyValue(key, value) in _map do
            expandoDict.Add (key, value)

        expando

