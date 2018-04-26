open System
open FileManager
open Entities
open CommandLine

[<EntryPoint>]
let main argv =
    let result = Parser.Default.ParseArguments<CommandOptions>(argv)
    match result with
    | :? Parsed<CommandOptions> as parsed -> 
        let options = parsed.Value
        
        0        
    | :? NotParsed<CommandOptions> as error -> -1    
        //translateJsonDocument "c:\users\swernimont\desktop\messages.en-us.json" destinationLanguage "e8c7cdbe800b4b1faa502ebfd039e977" // |> saveJsonToFile