open System
open System.Net
open System.IO
open System.Xml
open System.Text.RegularExpressions
open System.Web.UI.WebControls
open System.Text
//open Newtonsoft.Json

(*let translate (sourceLanguage) (destinationLanguages) (wordsToTranslate:string[]) (subscriptionKey) =
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

let wordsToTranslate = [|"Paylocity"; "Changes will take effect when you make your selection. This will only affect specific parts of the application at this time. Menu items that appear translated will also have their contents translated."; "Punch"; "Done"|]
let subscriptionKey = "eb223efd50524eddb48dd51e804ff6d5"
*)
let destLanguages = [|"es"; "pl";"de";"fr"|]
let langs = destLanguages |> Array.fold (fun acc word -> acc + sprintf "&to=%s" word) ""

printf "query string: %s" langs
