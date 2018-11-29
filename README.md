### Release Status

[![Build status](https://dev.azure.com/theblindsquirrel/JSON%20Translator/_apis/build/status/JSON%20Translator%20-%20Master)](https://dev.azure.com/theblindsquirrel/JSON%20Translator/_build/latest?definitionId=1)

[![](https://vsrm.dev.azure.com/theblindsquirrel/_apis/public/Release/badge/de913588-44d8-4c40-8623-b8b76e68431a/4/4)](https://vsrm.dev.azure.com/theblindsquirrel/_apis/public/Release/badge/de913588-44d8-4c40-8623-b8b76e68431a/4/4)

### Develop Build Status

[![Build status](https://dev.azure.com/TheBlindSquirrel/JSON%20Translator/_apis/build/status/JSON%20Translator%20-%20Develop)](https://dev.azure.com/TheBlindSquirrel/JSON%20Translator/_build/latest?definitionId=5)


# JSON Document Translator

This is a .Net Core 2 console application that accepts a JSON file, calls the Microsoft Text Translator API in Azure and writes translated files to disk.

### Installation
To install via nuget package manager 
    
    Install-Package JSONTranslator

To install via .Net Core CLi

    dotnet add package JSONTranslator


### Required Parameters:
* --sourcePath  _this is the path on disk to the JSON file that needs to be translated_
* --key  _this is one of the Ocp-Apim-Subscription-Key from Azure for Translator Text API_
* --languages  _this is a space seperated list of languages that you want to translate the source into. See [Microsoft Text Translator API][1] for a complete list of supported languages_

### Optional Parameters:
* --sourceLanguage _this is the original language of the document. if it is omitted english (en) will be used_

### Runing the Translator
    dotnet run --key "abc123" --languages de fr es --sourcePath "c:\projects\myProject\originalMessage.json" --sourceLanguage en

### Document format
Minified JSON files are not currently supported. The JSON file can be a single object or contain subobjects. New in version 2 you are no longer required to have language specified in your document. You can now set the source language through the command line. Below are examples of different valid schemas that have been translated into Spanish.

Schema 1

    {
        "Language" : "english",
        "Messages": {
            "anniversary.rollupMultipleEmployees": "Employees have Anniversaries on",
            "anniversary.rollupSingleEmployee": "Employee has an Anniversary on",
            "anniversary.singleHas": "has a",
            "anniversary.singleWorkAnniversary": "Year Work Anniversary on"
        }
    }

outputs as

    {
        "Language ":"Inglés",
        "Messages": {
            "anniversary.rollupMultipleEmployees":"Los empleados tienen aniversarios",
            "anniversary.rollupSingleEmployee":"Empleado tiene un aniversario",
            "anniversary.singleHas":"tiene un",
            "anniversary.singleWorkAnniversary":"Aniversario de trabajo en"
        }
    }



Schema 2

    {
        "Language" : "english",
        "anniversary.rollupMultipleEmployees": "Employees have Anniversaries on",
        "anniversary.rollupSingleEmployee": "Employee has an Anniversary on",
        "anniversary.singleHas": "has a",
        "anniversary.singleWorkAnniversary": "Year Work Anniversary on"
    }

outputs as

    {
        "Language ":"Inglés",
        "anniversary.rollupMultipleEmployees":"Los empleados tienen aniversarios",
        "anniversary.rollupSingleEmployee":"Empleado tiene un aniversario",
        "anniversary.singleHas":"tiene un",
        "anniversary.singleWorkAnniversary":"Aniversario de trabajo en"
    }


Schema 3

    {
        "Messages": {
            "anniversary.rollupMultipleEmployees": "Employees have Anniversaries on",
            "anniversary.rollupSingleEmployee": "Employee has an Anniversary on",
            "subObject2" : {
                "lookup" : "message"
            }
        }
    }

outputs as

    {
        "Messages": {
        "anniversary.rollupMultipleEmployees":"Los empleados tienen aniversarios",
        "anniversary.rollupSingleEmployee":"Empleado tiene un aniversario",
            "subObject2" : {
                "lookup ":"Mensaje"
            }
        }
    }



### Output

#### Folder
The translated files will be output to the same folder as the source input file.

#### File Names
There will be one translated file for each new language. If the original file name does not contain the language (ie labels.json) then each new file will be appended with the language. For example if the original file was labels.json and the languages are Spanish and French, the output files will be labels.es.json and labels.fr.json. Additionally, if the original filename was only the language (ie en.json) then the output filenames will be the languages. So if the input file was en.json and the languages were German and Polish then the new files will be de.json and pl.json.

[1]:https://docs.microsoft.com/en-us/azure/cognitive-services/translator/languages/
