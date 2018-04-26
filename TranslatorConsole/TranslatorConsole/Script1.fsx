#r "Newtonsoft.Json"

#load "Entities.fs"
#load "FileManager.fs"
#load "Translator.fs"

open Translator
open FileManager

let filePath = @"c:\users\swernimont\desktop\test.json"

let languages = [|
    "de" 
    "fr"
    "es"|]

let partialFunc = translateJsonDocument filePath "e8c7cdbe800b4b1faa502ebfd039e977"

for lang in languages do
    let translated = partialFunc lang
    let writeToPath = sprintf @"c:\users\swernimont\desktop\messages.%s.json" lang
    writeDocumentToDisk translated writeToPath |> ignore