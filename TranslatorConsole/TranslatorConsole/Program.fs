open System
open FileManager
open Entities
open CommandLine
open Translator
open System.Collections.Generic

[<EntryPoint>]
let main argv =
    let result = Parser.Default.ParseArguments<CommandOptions>(argv)
    match result with
    | :? Parsed<CommandOptions> as parsed ->
        try
            let options = parsed.Value
            let translationFunc = translateJsonDocument options.filePath options.key
            options.languages |> Seq.iter (fun lang ->
                let writeDirectory = getDestinationFolderPath options.filePath
                let writePath = sprintf "%s\messages.%s.json" writeDirectory lang
                translationFunc lang |> writeDocumentToDisk writePath
                //match options.destination |> String.IsNullOrWhiteSpace with
                //| false ->
                //    let writeDirectory = getDestinationFolderPath options.destination
                //    let writePath = sprintf "%s\messages.%s.json" writeDirectory lang
                //    translationFunc lang |> writeDocumentToDisk writePath
                //| true ->
                //    let writeDirectory = getDestinationFolderPath options.filePath
                //    let writePath = sprintf "%s\messages.%s.json" writeDirectory lang
                //    translationFunc lang |> writeDocumentToDisk writePath
                )        
            0
        with
        | ex -> 
            printfn "An error occurred. Error message: %s" ex.Message
            -1
    | :? NotParsed<CommandOptions> -> 
        printfn "Invalid arguments."
        -1
    | _ ->
        Console.Read() |> ignore
        1