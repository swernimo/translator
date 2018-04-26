// Learn more about F# at http://fsharp.org

open System
open Translator
open FileManager
//output to the same directory as source
  (*
    required parameters:
        source file path
        languages
        subscription key
    optional parameters:
        destionation file path (use same directory as source file if not supplied)
  *)

[<EntryPoint>]
let main argv =
    printf "Enter language to translate to: "
    let destinationLanguage = Console.ReadLine()
    //translateJsonDocument "c:\users\swernimont\desktop\messages.en-us.json" destinationLanguage "e8c7cdbe800b4b1faa502ebfd039e977" // |> saveJsonToFile
    
    printfn "Press Enter to exit"
    Console.Read() |> ignore
    0 // return an integer exit code
