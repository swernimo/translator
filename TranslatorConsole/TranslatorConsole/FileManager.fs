module FileManager

open System.IO;
open Newtonsoft.Json
open System.Collections.Generic
open Entities

let loadJsonDocument filePath =
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true -> 
        use fileStream = new StreamReader(filePath)
        let jsonReader = new JsonTextReader(fileStream)
        let jsonSerialize = new Newtonsoft.Json.JsonSerializer()
        let doc = jsonSerialize.Deserialize<Document>(jsonReader)
        jsonReader.Close()
        doc
    | false -> 
        let doc : Document = { Language = "error"; Messages = new Dictionary<string,string>();}
        doc