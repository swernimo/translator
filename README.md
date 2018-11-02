[![Build status](https://dev.azure.com/TheBlindSquirrel/JSON%20Translator/_apis/build/status/JSON%20Translator%20-%20Develop)](https://dev.azure.com/TheBlindSquirrel/JSON%20Translator/_build/latest?definitionId=5)

# JSON Document Translator

This is a .Net Core 2 console application that accepts a JSON file, calls the Microsoft Text Translator API in Azure and writes translated files to disk.


### Required Parameters:
* --sourcePath  _this is the path on disk to the JSON file that needs to be translated_
* --key  _this is one of the Ocp-Apim-Subscription-Key from Azure for Translator Text API_
* --languages  _this is a space seperated list of languages that you want to translate the source into. See [Microsoft Text Translator API][1] for a complete list of supported languages_

### Optional Parameters:
* --sourceLanguage _this is the original language of the document. if it is omitted english (en) will be used_

### Runing the Translator
    dotnet run --key "abc123" --languages de fr es --sourcePath "c:\projects\myProject\originalMessage.json" --sourceLanguage en

### Document format
Your JSON file needs to have two (and only 2) elements. The first is a string that represents the country code for the original language. The second element is a sub element of key value pairs.
   
    {
        "language" : "en",
        "Messages": {
            "welcomeMessage" : "Welcome",
            "myAccount" : "My Account",
            "loginButton" : "Login"
        }
    }


### Output

#### Folder
The translated files will be output to the same folder as the source input file.

#### File Names
Their will be one translated file for each new language with the same name as the input file, but with the language code appended to the end. For example, if the source file is called labels.json and the languages are german, spanish, and french then there will be 3 output files called labels.de.json, labels.es.json, and labels.fr.json.

[1]:https://docs.microsoft.com/en-us/azure/cognitive-services/translator/languages/
