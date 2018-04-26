module Entities

open System.Collections.Generic
open CommandLine

type Document = {
    Language:string
    Messages: IDictionary<string,string>
}

 type CommandOptions = {
    [<Option("sourcePath", Required = true, HelpText = "Path to source file")>] filePath: string;
    [<Option("languages", Required = true, HelpText = "Comma seperated list of languages to translate to")>] languages: seq<string>;
    [<Option("key", Required = true, HelpText = "Subscription key for Azure Cognitive Services")>] key: string;
    [<Option("destination", Required = false, HelpText = "Optional. Location on disk to save the translated file.")>] destination:string option;
}