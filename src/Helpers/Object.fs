namespace Helpers

open System.Dynamic
open System.Collections.Generic

/// <summary>
/// Module that hold dynamic objects manipulation helpers
/// </summary>
module DynamicObject =

    /// <summary>
    /// Get dynamic object from map fields
    /// </summary>
    /// <param name="_map">Map to extract fields from</param>
    /// <returns>Dynamic object that have the map fields</returns>
    let ofMap (_map : Map<string, 'obj>) : ExpandoObject =
        let expando     = new ExpandoObject()
        let expandoDict = expando :> IDictionary<string, obj>

        for KeyValue(key, value) in _map do
            expandoDict.Add (key, value)

        expando

