// Learn more about F# at http://fsharp.org

open System
open Translator
  
[<EntryPoint>]
let main argv =
    printfn "Enter word to translate: "
    let word = Console.ReadLine()
    translateTextAsync word |> Async.RunSynchronously |>  printfn "Translated Word:  %s"    
    Console.Read() |> ignore
    0 // return an integer exit code
