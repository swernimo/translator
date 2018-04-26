module Translator

open System.Net
open System.IO
open System.Xml
open FileManager

let translateTextAsync language subscrptionKey word =
    async {
        try
           let url = "https://api.microsofttranslator.com/V2/Http.svc/Translate?to=" + language + "&text=" + word;
           let key = "e8c7cdbe800b4b1faa502ebfd039e977"
           let request = WebRequest.Create(url)
           request.Headers.Add("Ocp-Apim-Subscription-Key", key)
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
    let dictionary = loadDictionaryFromJsonFile filePath
    for pair in dictionary do
        translateFunc pair.Value |> Async.RunSynchronously |> printfn "Original Word: %s. Translated Word: %s" pair.Value //need to create a json document that has header property for language and an array of key value pairs. pair.key is message lookup and does not get translated.