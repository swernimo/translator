module Translator

open System
open System.Net
open System.IO
open System.Xml
open System.Text.RegularExpressions
open System.Text
open Newtonsoft.Json

let translateJsonDocument filePath subscriptionKey sourceLanguage destinationLanguage (destinationFilePath:string) =
    let translateTextAsync lng subscrptionKey sourceLanguage word =
        async {
            try
               let url = sprintf "https://api.microsofttranslator.com/V2/Http.svc/Translate?to=%s&text=%s&from=%s" lng word sourceLanguage
               let request = WebRequest.Create(url)
               request.Headers.Add("Ocp-Apim-Subscription-Key", subscrptionKey)
               request.Method <- "GET"
               let response = request.GetResponseAsync()
               use reader = new StreamReader(response.Result.GetResponseStream())
               let output = reader.ReadToEnd()
               let xml = new XmlDocument()
               xml.LoadXml(output);
               reader.Close()
               response.Result.Close()
               request.Abort()
               return xml.FirstChild.InnerText
            with
                | ex -> return "" //log the error
        }
    
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
    | false -> 
        ()


let translate (sourceLanguage) (destinationLanguages) (wordsToTranslate:string[]) (subscriptionKey) =
    let buildToLanguageQueryString (destinationLanguages:string[]) : string =
        match destinationLanguages.Length > 1 with
        | true ->
            let queryString = destinationLanguages |> Array.reduce (fun soFar dlang -> sprintf "&to=%s" soFar + sprintf "&to=%s" dlang)
            queryString.Replace("to=&", "")
        | false ->
            sprintf "&to=%s" destinationLanguages.[0]
    let buildRequestBody (wordsToTranslate:string[]) =
        let body = wordsToTranslate |> Array.fold (fun acc (word:string) -> acc + sprintf "{\"text\": \"%s\"}," word) "" |> (fun x -> x.Substring(0, x.Length - 1))
        Encoding.ASCII.GetBytes(body)

    let url = sprintf "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=%s&textType=plain%s" sourceLanguage (destinationLanguages |> Array.fold (fun acc word -> acc + sprintf "&to=%s" word) "")
    let request = WebRequest.Create(url)
    let bodyBytes = buildRequestBody wordsToTranslate
    request.Method <- "POST"
    request.ContentType <- "application/json"
    request.ContentLength <- int64 bodyBytes.Length
    request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey)
    use stream = request.GetRequestStream()
    stream.Write(bodyBytes, 0, bodyBytes.Length)
    stream.Close()

    let response = request.GetResponse()
    use responseStream = response.GetResponseStream()
    use reader = new StreamReader(responseStream)
    let text = reader.ReadToEnd()
    printfn "response body: %s" text
    reader.Close()
    responseStream.Close()
    printfn "url is: %s" url
