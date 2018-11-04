module Translator

open System
open System.Net
open System.IO
open System.Xml
open System.Text.RegularExpressions

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
                writer.WriteLine(line)
            | "," -> 
                let split = line.Split(':')
                let word = split.[1].TrimEnd(',')
                let translated = translateFunc word |> Async.RunSynchronously
                let newLine = String.Format("{0}:{1},", split.[0], translated)
                writer.WriteLine(newLine)
            | _ ->
                match Regex.Match(ending, "[A-Za-z]").Success with
                | true ->
                    let split = line.Split(':')
                    let word = split.[1].TrimEnd(',')
                    let translated = translateFunc word |> Async.RunSynchronously
                    let newLine = String.Format("{0}:{1}", split.[0], translated)
                    writer.WriteLine(newLine)
                | false ->
                    //ends in a number or special character don't translate
                    writer.WriteLine(line)
        )
        ()
    | false -> 
        ()
    ()