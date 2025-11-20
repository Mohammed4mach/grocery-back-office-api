namespace Helpers

open Microsoft.FSharp.Reflection

module Map =
    let ofRecord<'T> (record : 'T) : Map<string, string> =
        let fields   = FSharpType.GetRecordFields (typeof<'T>)
        let mutable keyValue = Map.empty<string, string>

        for field in fields do
            let key   = field.Name
            let value = field.GetValue(record).ToString()

            keyValue <- keyValue.Add (key, value)

        keyValue

