﻿module FileManager

open System.IO;

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
            fileName.Replace(lang, "") + "."
        | false ->
            match fileName.ToLower().Equals(language.ToLower()) with
            | true ->
                ""
            | false ->
                fileName + "."      
    | false ->
        invalidArg "file not found" |> ignore
        ""