# JSON Document Translator

This is a .Net Core 2 console application that accepts a JSON file, calls the Microsoft Text Translator API in Azure and writes translated files to disk.


### Required parameters:
* --sourcePath this is the path on disk to the JSON file that needs to be translated
* --key this is one of the keys from Azure for Translator Text API
* --languages this is a space seperated list of languages that you want to translate the source into. See [Microsoft Text Translator API] (https://docs.microsoft.com/en-us/azure/cognitive-services/translator/languages) for a complete list of supported languages

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


### Output Folder
The app will output one new file for each language in the same folder as the source. For example if the source path is c:\temp\labels.json and the languages of de, es, and fr are supplied. The app will output 3 new files to c:\temp named messages.de.json, messages.es.json, and messages.fr.json, respectively.