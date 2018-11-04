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
        try
            let options = parsed.Value
            let sourceLanguage = options.sourceLanguage
            let translationFunc = translateJsonDocument options.filePath options.key sourceLanguage
            options.languages |> Seq.iter (fun lang ->
                let writeDirectory = getDestinationFolderPath options.filePath
                let fileName = getInputFileName options.filePath sourceLanguage
                let writePath = sprintf "%s\%s.%s.json" writeDirectory fileName lang
                //translationFunc lang |> writeDocumentToDisk writePath
                translationFunc lang
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