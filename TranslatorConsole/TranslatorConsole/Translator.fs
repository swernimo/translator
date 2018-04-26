module Translator

open System.Net
open System.IO
open System.Xml
open FileManager
open Entities

let translateJsonDocument filePath subscriptionKey language=

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
    let doc = loadJsonDocument filePath
    let translateFunc = translateTextAsync language subscriptionKey doc.Language  
    let translatedMessages = doc.Messages |> Seq.map (fun pair -> pair.Key, translateFunc pair.Value |> Async.RunSynchronously) |> dict
    let newDoc:Document = {Language = language; Messages = translatedMessages;}
    newDoc