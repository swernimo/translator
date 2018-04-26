#r "Newtonsoft.Json"

#load "Entities.fs"
#load "FileManager.fs"
#load "Translator.fs"

open Translator

//let filePath = "c:\users\swernimont\desktop\messages.en-us.json"

let filePath = "c:\users\swernimont\desktop\\test.json"

let x = translateJsonDocument filePath "de" "e8c7cdbe800b4b1faa502ebfd039e977"

x