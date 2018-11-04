module Translator

open System
open System.Net
open System.IO
open System.Xml
open System.Text.RegularExpressions

let translateJsonDocument filePath subscriptionKey sourceLanguage destinationLanguage =
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
    
    let translateFunc = translateTextAsync destinationLanguage subscriptionKey sourceLanguage
    let fileInfo = new FileInfo(filePath)
    match fileInfo.Exists with
    | true -> 
        use writer = new StreamWriter(@"C:\Users\swernimont\Desktop\languages\es.json");
        let lines = File.ReadAllLines(filePath)
        lines |> Seq.iter(fun (l:string) -> 
            let line = l.Replace("\\", String.Empty).Replace("\"", String.Empty).Trim()
            match line.EndsWith('{') with
            | true ->
                writer.WriteLine(line)
            | false ->
                match line.EndsWith("},") with
                | true ->
                    writer.WriteLine(line)
                | false ->
                    match line.EndsWith(',') with
                    |true ->                    
                        let split = line.Split(':')
                        let word = split.[1].TrimEnd(',')
                        let translated = translateFunc word |> Async.RunSynchronously
                        let newLine = String.Format("{0}:{1},", split.[0], translated)
                        writer.WriteLine(newLine)
                    |false ->
                        match line.EndsWith('}') with
                        |true ->
                            writer.WriteLine(line)
                        |false ->
                            let lastChar = line.Chars(line.Length - 1).ToString().ToLower()
                            match Regex.Match(lastChar, "[A-Za-z]").Success with
                            | true ->
                                let split = line.Split(':')
                                let word = split.[1].TrimEnd(',')
                                let translated = translateFunc word |> Async.RunSynchronously
                                let newLine = String.Format("{0}:{1},", split.[0], translated)
                                writer.WriteLine(newLine)
                            | false ->
                                //no idea how it would get to here
                                () 
        )
        ()
    | false -> 
        ()
    ()
    (*
    let jObj = loadJsonDocument filePath
    let translateFunc = translateTextAsync destinationLanguage subscriptionKey sourceLanguage
    let newObj = jObj.DeepClone()
    let writer = new JTokenWriter()
    let rec parseJToken (token:JToken) =   
        match token.Type with
        | JTokenType.String ->
            //let tokenValue = token.Value<string>()
            //let translated = translateFunc tokenValue |> Async.RunSynchronously
            let translated = "translated text"
            let newToken = token
            let reader = newToken.CreateReader()
            writer.WriteValue(translated)
            writer.WriteToken(reader)
            let t = writer.Token
            ()
        | JTokenType.Object ->
            token.Children() |> Seq.iter (fun t -> parseJToken t)
        | JTokenType.Property ->
            token.ToObject<JProperty>().Value |> parseJToken            
        | _ ->
            ()
    
    for p in jObj do
        parseJToken p.Value

    newObj
    *)