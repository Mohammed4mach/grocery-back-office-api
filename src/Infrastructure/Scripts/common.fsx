#r @"../../../bin/Debug/net8.0/grocery-back-office-api.dll"
#r "nuget: DotNetEnv"

open System.IO
open DotNetEnv
open Infrastructure.Core
open Infrastructure.Core.Types

module Common =
    let configure () =
        Env.Load() |> ignore
        Database.configure()

    let getDirContents (path : string) : string array =
        Array.concat (Seq.ofArray [| (Directory.GetDirectories path); (Directory.GetFiles path) |])

    let getFileContents (path : string) : string =
        File.ReadAllText path

    let getFileName (path : string) : string =
        Path.GetFileName path

    let __ = Path.DirectorySeparatorChar.ToString()
    let migrationsPath : string = __SOURCE_DIRECTORY__ + ($"{__}..{__}Migrations{__}")

    let files : string array = getDirContents migrationsPath

module Migrate =
    let exec () : unit =
        Common.configure()

        for file in Common.files do
            let __ = Common.__
            let up : string = Common.getFileContents $"{file}{__}up.sql"
            let migration : string = Common.getFileName file

            printfn $"Migrating: {migration}"

            Database.operations.execute (QueryOnly(up)) |> ignore

            printfn $"DONE     : {migration}\n"

module Rollback =
    let exec () : unit =
        Common.configure()

        for file in Common.files do
            let __ = Common.__
            let down : string = Common.getFileContents $"{file}{__}down.sql"
            let migration : string = Common.getFileName file

            printfn $"Rollback: {migration}"

            Database.operations.execute (QueryOnly(down)) |> ignore

            printfn $"DONE    : {migration}\n"

module MigrateFresh =
    let exec () : unit =
        Common.configure()
        Rollback.exec()
        Migrate.exec()

