open System
open FileManager
open Entities
open CommandLine
open Translator

[<EntryPoint>]
let main argv =
    let result = Parser.Default.ParseArguments<CommandOptions>(argv)
    match result with
    | :? Parsed<CommandOptions> as parsed -> 
        let options = parsed.Value
        let translationFunc = translateJsonDocument options.filePath options.key
        options.languages |> Seq.iter (fun lang ->
            match options.destination |> String.IsNullOrWhiteSpace with
            | false ->
                let writeDirectory = getDestinationFolderPath options.destination
                let writePath = sprintf "%s\messages.%s.json" writeDirectory lang
                translationFunc lang |> writeDocumentToDisk writePath
            | true ->
                let writeDirectory = getDestinationFolderPath options.filePath
                let writePath = sprintf "%s\messages.%s.json" writeDirectory lang
                translationFunc lang |> writeDocumentToDisk writePath
            )        
        0        
    | :? NotParsed<CommandOptions> as error -> 
        Console.Read() |> ignore
        //write errors to console
        -1
    | _ ->
        Console.Read() |> ignore
        1