// Learn more about F# at http://fsharp.org

open System
open Translator
open FileManager
  
[<EntryPoint>]
let main argv =
    printfn "Enter word to translate: "
    let word = Console.ReadLine()
    printfn "Enter language to translate to: "
    let destinationLanguage = Console.ReadLine()
    translateTextAsync word destinationLanguage |> Async.RunSynchronously |>  printfn "Translated Word:  %s"
    printfn "Press Enter to exit"
    Console.Read() |> ignore
    0 // return an integer exit code
