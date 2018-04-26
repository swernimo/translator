#r "Newtonsoft.Json"
open System.IO;
open Newtonsoft.Json
open System.Collections.Generic

let loadJsonFile (filePath:string) = 
    use fileStream = new StreamReader(filePath)
    let jsonReader = new JsonTextReader(fileStream)
    let jsonSerialize = new Newtonsoft.Json.JsonSerializer()
    let dictionary = jsonSerialize.Deserialize<Dictionary<string,string>>(jsonReader)
    jsonReader.Close()
    dictionary

let x = loadJsonFile "c:\users\swernimont\desktop\messages.en-us.json"
for y in x do
    printfn "%s %s" y.Key y.Value