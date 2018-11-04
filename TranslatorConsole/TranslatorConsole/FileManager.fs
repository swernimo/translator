module FileManager

open System.IO;
open Newtonsoft.Json
open Newtonsoft.Json.Linq

//let loadJsonDocument filePath =
//    let fileInfo = new FileInfo(filePath)
//    match fileInfo.Exists with
//    | true -> 
//        use fileStream = new StreamReader(filePath)
//        let line = fileStream.ReadLine().Trim()
//        match line.EndsWith('{') with
//        | true ->
//            //write this line back to the file, do not translate it
//            ()
//        | false ->
//            //check if it ends with ,
//            ()
//        //let jsonReader = new JsonTextReader(fileStream)
//        //let jsonSerialize = new Newtonsoft.Json.JsonSerializer()
//        //let jobj = jsonSerialize.Deserialize<JObject>(jsonReader)
//        //jsonReader.Close()
//        //jobj
//        ()
//    | false -> 
//        ()
        //invalidArg "file not found" |> ignore
        //let nil = new JObject();
        //nil
  
let writeDocumentToDisk (writeToPath:string) (line) = 
    let writeToDisk (line:string) = 
        use fileStream = new StreamWriter(writeToPath, true)
        fileStream.WriteLine(line)
        ()
        //let jsonText = JsonConvert.SerializeObject(doc)
        //let jsonWriter = new JsonTextWriter(fileStream)
        //jsonWriter.WriteRaw(jsonText)
        //jsonWriter.Close()
    
    let info = new FileInfo(writeToPath)
    match info.Directory.Exists with
    | true -> 
        writeToDisk line
    | false ->
        try
            Directory.CreateDirectory(writeToPath) |> ignore
            writeToDisk line
        with
        | ex -> ()//log the error & stop the program

let getDestinationFolderPath sourcepath = 
    let fileInfo = new FileInfo (sourcepath)
    match fileInfo.Exists with
    | true ->
        fileInfo.DirectoryName
    | false ->
        invalidArg "file not found" |> ignore
        ""

let getInputFileName filePath (language:string) =
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true ->
        let fileName = fileInfo.Name.Replace(fileInfo.Extension, "")
        let lang = sprintf ".%s" language
        match fileName.Contains(lang) with 
        | true ->
            fileName.Replace(lang, "")
        | false ->
            fileName
    | false ->
        invalidArg "file not found" |> ignore
        ""