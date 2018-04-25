module Translator

open System.Net
open System.IO
open System.Xml

let translateTextAsync (word:string) =
    async {
        try
           let url = "https://api.microsofttranslator.com/V2/Http.svc/Translate?to=fr-fr&text=" + word;
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
            | ex -> return "Error while trying to translate " + word + ". Exception Message: " + ex.Message
    }