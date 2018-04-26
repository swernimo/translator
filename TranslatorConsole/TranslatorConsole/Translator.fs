module Translator

open System.Net
open System.IO
open System.Xml
open FileManager
open Entities

let translateTextAsync language subscrptionKey word =
    async {
        try
           let url = "https://api.microsofttranslator.com/V2/Http.svc/Translate?to=" + language + "&text=" + word;
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
            | ex -> return "" //log the error //return "Error while trying to translate " + word + ". Exception Message: " + ex.Message
    }

let translateJsonDocument filePath lanuage subscriptionKey =
    let translateFunc = translateTextAsync lanuage subscriptionKey
    let doc = loadJsonDocument filePath
    let translatedMessages = doc.Messages |> Seq.map (fun pair -> pair.Key, translateFunc pair.Value |> Async.RunSynchronously) |> dict
    let newDoc:Document = {Language = lanuage; Messages = translatedMessages;}
    newDoc