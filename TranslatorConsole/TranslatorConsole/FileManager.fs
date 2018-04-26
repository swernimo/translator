module FileManager

open System.IO;
open Newtonsoft.Json
open System.Collections.Generic

let loadDictionaryFromJsonFile filePath =
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true -> 
        use fileStream = new StreamReader(filePath)
        let jsonReader = new JsonTextReader(fileStream)
        let jsonSerialize = new Newtonsoft.Json.JsonSerializer()
        let dictionary = jsonSerialize.Deserialize<Dictionary<string,string>>(jsonReader)
        jsonReader.Close()
        dictionary
    | false -> new Dictionary<string, string>()