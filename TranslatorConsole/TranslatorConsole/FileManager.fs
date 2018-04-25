module FileManager

open System.IO;
open Newtonsoft.Json
open System.Collections.Generic

let loadJsonFile (filePath:string) = 
    use fileStream = new StreamReader(filePath)
    let jsonReader = new JsonTextReader(fileStream)
    let jsonSerialize = new Newtonsoft.Json.JsonSerializer()
    let msgs = jsonSerialize.Deserialize(jsonReader, typeof<Dictionary<string,string>>)    
    jsonReader.Close()
    msgs