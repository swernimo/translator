module Entities

open CommandLine

 type CommandOptions = {
    [<Option("sourcePath", Required = true, HelpText = "Path to source file")>] filePath: string;
    [<Option("languages", Required = true, HelpText = "Comma seperated list of languages to translate to")>] languages: seq<string>;
    [<Option("key", Required = true, HelpText = "Subscription key for Azure Cognitive Services")>] key: string;
    [<Option("sourceLanguage", Required = false, Default = "en", HelpText="Optional original language for the document. If omitted english (en) will be used.")>] sourceLanguage:string;
}