namespace Helpers

open Microsoft.FSharp.Reflection

/// <summary>
/// Module that hold maps manipulation helpers
/// </summary>
module Map =

    /// <summary>
    /// Get map from record fields
    /// </summary>
    /// <typeparam name="'T">Type of the record</typeparam>
    /// <param name="record">Record to extract fields from</param>
    /// <returns>Map that have the record fields</returns>
    let ofRecord<'T> (record : 'T) : Map<string, string> =
        let fields   = FSharpType.GetRecordFields (typeof<'T>)
        let mutable keyValue = Map.empty<string, string>

        for field in fields do
            let key   = field.Name
            let value = field.GetValue(record).ToString()

            keyValue <- keyValue.Add (key, value)

        keyValue

