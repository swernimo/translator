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
            let lines = getInputFileText options.filePath
            let outputPaths = getOutputFiles sourceLanguage options.languages options.filePath
            let translationFunc = getTranslations sourceLanguage options.languages options.key
            lines |> Array.iter (fun line ->
                match lineNeedsToBeTranslated line with 
                | true ->
                    let split = line.Split(':')
                    let firstPart = sprintf "\"%s\":" (split.[0].Replace("\"", ""))
                    let wordToTranslate = split.[1].Replace("\"", "")
                    match wordToTranslate.EndsWith(",") with
                    | true ->
                        let newWord = wordToTranslate.Substring(0, wordToTranslate.Length - 1)
                        let translations = translationFunc [|newWord|]
                        writeTranslationsToDisk outputPaths translations firstPart true
                    | false ->
                        let translations = translationFunc [|wordToTranslate|]
                        writeTranslationsToDisk outputPaths translations firstPart false
                | false ->
                    outputPaths |> Seq.iter (fun path -> writeToFile path line)
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