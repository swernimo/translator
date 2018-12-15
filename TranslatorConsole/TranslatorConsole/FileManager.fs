module FileManager

open System.IO;
open System.Text.RegularExpressions
open Entities

let getInputFileText filePath  =
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true ->
        File.ReadAllLines(filePath)
    | false ->
        invalidArg "file not found" |> ignore
        [|""|]
 
let createFileIfDoesNotExist filePath =
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true ->
        ()
    | false ->
        use stream = File.Create(filePath)
        stream.Close()

let writeToFile filePath (textToWrite:string) = 
    createFileIfDoesNotExist filePath
    use writer = new StreamWriter(filePath, true)
    writer.WriteLine(textToWrite)
    writer.Close()

let getOutputFiles sourceLanguage destinationLanguages inputFilePath =
    let fileInfo = new FileInfo(inputFilePath)
    let fileName = fileInfo.Name.Replace(fileInfo.Extension, "")
    match fileName.Contains(sprintf ".%s" sourceLanguage) with
    | true ->
        let regexPattern = sprintf ".%s[A-Za-z]" sourceLanguage
        match Regex.Match(fileName, regexPattern).Success with
        | true ->
            destinationLanguages |> Seq.map (fun dl -> sprintf "%s/%s.%s.json" fileInfo.DirectoryName fileName dl)
        | false ->
            destinationLanguages |> Seq.map (fun dl ->
                let outputFileName = fileName.Replace(sprintf ".%s" sourceLanguage, sprintf ".%s" dl)
                sprintf "%s/%s.json" fileInfo.DirectoryName outputFileName
                )
    | false ->
        match fileName.ToLower().Equals(sourceLanguage.ToLower()) with
        | true ->
            destinationLanguages |> Seq.map (fun dl -> sprintf "%s/%s.json" fileInfo.DirectoryName dl)
        | false ->
            destinationLanguages |> Seq.map (fun dl -> sprintf "%s/%s.%s.json" fileInfo.DirectoryName fileName dl)

let writeTranslationsToDisk (outputPaths: seq<string>) (translations: Translation[]) (untranslated:string) (appendComma: bool) =
    translations |> Array.iter (fun t -> 
        let newLine = sprintf "%s \"%s\"" untranslated t.text
        let path = outputPaths |> Seq.find (fun p -> Regex.Match(p, sprintf ".?%s.json" t.ToLanguage).Success)
        let writeFunc = writeToFile path
        match appendComma with
        | true ->
           writeFunc (sprintf "%s," newLine)
        | false ->
           writeFunc newLine
    )

let deleteExistingFiles (files:seq<string>) =
    files |> Seq.iter (fun file -> 
        let fileInfo = new FileInfo(file)
        match fileInfo.Exists with
        | true ->
            File.Delete(file)
        | false ->
            ())