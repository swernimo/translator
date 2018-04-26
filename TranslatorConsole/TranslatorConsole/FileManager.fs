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
        let doc : Document = { Language = ""; Messages = new Dictionary<string,string>();}
        doc
  
let writeDocumentToDisk (doc:Document) (writeToPath:string) = 
    let writeToDisk doc = 
        use fileStream = new StreamWriter(writeToPath)
        let jsonText = JsonConvert.SerializeObject(doc)
        let jsonWriter = new JsonTextWriter(fileStream)
        jsonWriter.WriteRaw(jsonText)
        jsonWriter.Close()
    
    let info = new FileInfo(writeToPath)
    match info.Directory.Exists with
    | true -> 
        writeToDisk doc
    | false ->
        try
            Directory.CreateDirectory(writeToPath) |> ignore
            writeToDisk doc
        with
        | ex -> ()//log the error & stop the program