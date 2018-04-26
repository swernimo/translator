module FileManager

open System.IO;
open Newtonsoft.Json
open System.Collections.Generic

let loadDictionaryFromJsonFile (filePath:string) =
    use fileStream = new StreamReader(filePath)
    let jsonReader = new JsonTextReader(fileStream)
    let jsonSerialize = new Newtonsoft.Json.JsonSerializer()
    let dictionary = jsonSerialize.Deserialize<Dictionary<string,string>>(jsonReader)
    jsonReader.Close()
    dictionary