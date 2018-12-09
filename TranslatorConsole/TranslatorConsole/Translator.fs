module Translator

open System
open System.Net
open System.IO
open System.Text.RegularExpressions
open System.Text
open Newtonsoft.Json
open Entities
(*
let translateJsonDocument filePath subscriptionKey sourceLanguage destinationLanguage (destinationFilePath:string) =
    
    let getEndingString (line:string) =
        let trimmed = line.Trim()
        match trimmed.Length with
        | 0 ->
            String.Empty
        | 1 | 2 ->
            trimmed
        | _ ->
            trimmed.Chars(trimmed.Length - 1).ToString()            

    let translateFunc = translateTextAsync destinationLanguage subscriptionKey sourceLanguage
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true -> 
        use writer = new StreamWriter(destinationFilePath);
        File.ReadAllLines(filePath) |> Seq.iter(fun (l:string) -> 
            let line = l.Replace("\\", String.Empty).Replace("\"", String.Empty).Trim()
            let ending = getEndingString line
            match ending with
            | "{" | "}" | "}," ->
                writer.WriteLine(l)
            | "," -> 
                let split = line.Split(':')
                let word = split.[1].TrimEnd(',')
                let translated = translateFunc word |> Async.RunSynchronously
                let newLine = String.Format("\"{0}\":\"{1}\",", split.[0], translated.Trim())
                writer.WriteLine(newLine)
            | _ ->
                match Regex.Match(ending, "[A-Za-z]").Success with
                | true ->
                    let split = line.Split(':')
                    let word = split.[1].TrimEnd(',')
                    let translated = translateFunc word |> Async.RunSynchronously
                    let newLine = String.Format("\"{0}\":\"{1}\"", split.[0], translated.Trim())
                    writer.WriteLine(newLine)
                | false ->
                    //ends in a number or special character don't translate
                    writer.WriteLine(l)
        )
        ()
        *)

let getTranslations sourceLanguage destinationLanguages subscriptionKey wordsToTranslate =
    let buildRequestBody (wordsToTranslate:string[]) =
        let body = wordsToTranslate |> Array.fold (fun acc word -> acc + sprintf "{\"text\": \"%s\"}," word) "[" |> (fun x -> x.Substring(0, x.Length - 1) + "]")
        Encoding.ASCII.GetBytes(body)

    let createWebRequest subscriptionKey (url:string) = 
        let request = WebRequest.Create(url)
        let bodyBytes = buildRequestBody wordsToTranslate
        request.Method <- "POST"
        request.ContentType <- "application/json"
        request.ContentLength <- int64 bodyBytes.Length
        request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey)
        use stream = request.GetRequestStream()
        stream.Write(bodyBytes, 0, bodyBytes.Length)
        stream.Close()
        request

    let getResponseAsString (request:WebRequest) = 
        let response = request.GetResponse()
        use responseStream = response.GetResponseStream()
        use reader = new StreamReader(responseStream)
        let text = reader.ReadToEnd()
        reader.Close()
        responseStream.Close()
        text

    let url = sprintf "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=%s&textType=plain%s" sourceLanguage (destinationLanguages |> Seq.fold (fun acc word -> acc + sprintf "&to=%s" word) "")
    let request = createWebRequest subscriptionKey url
    let response = getResponseAsString request
    let x = response.Replace("[{\"translations\":", "").Substring(0, response.Length - 19)
    let json = JsonConvert.DeserializeObject<Translation[]>(x)
    json

let lineNeedsToBeTranslated line =
    let getEndingString (line:string) =
        let trimmed = line.Trim()
        match trimmed.Length with
        | 0 ->
            String.Empty
        | 1 | 2 ->
            trimmed
        | _ ->
            trimmed.Chars(trimmed.Length - 1).ToString()  
            
    match String.IsNullOrWhiteSpace(line) with
    | true ->
        false
    | false ->
        let ln = line.Replace("\\", String.Empty).Replace("\"", String.Empty).Trim()
        let ending = getEndingString ln
        match ending with
            | "{" | "}" | "}," ->
                false
            | "," -> 
                true
            | _ ->
                match Regex.Match(ending, "[A-Za-z]").Success with
                | true ->
                    true
                | false ->
                    false