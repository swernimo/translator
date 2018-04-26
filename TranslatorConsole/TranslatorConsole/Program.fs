// Learn more about F# at http://fsharp.org

open System
open Translator
open FileManager
  
[<EntryPoint>]
let main argv =
    printf "Enter language to translate to: "
    let destinationLanguage = Console.ReadLine()
    translateJsonDocument "c:\users\swernimont\desktop\messages.en-us.json" destinationLanguage "e8c7cdbe800b4b1faa502ebfd039e977" // |> saveJsonToFile
    printfn "Press Enter to exit"
    Console.Read() |> ignore
    0 // return an integer exit code
