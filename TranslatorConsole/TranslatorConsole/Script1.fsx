#r "Newtonsoft.Json"

#load "Entities.fs"
#load "FileManager.fs"
#load "Translator.fs"

open Translator
open FileManager

//let filePath = "c:\users\swernimont\desktop\messages.en-us.json"

let filePath = @"c:\users\swernimont\desktop\test.json"

let translatedDocument = translateJsonDocument filePath "de" "e8c7cdbe800b4b1faa502ebfd039e977"

writeDocumentToDisk @"c:\users\swernimont\desktop\messages.de.json" translatedDocument |> ignore