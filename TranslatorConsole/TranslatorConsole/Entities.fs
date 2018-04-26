module Entities

open System.Collections.Generic

type Document = {
    Language:string
    Messages: IDictionary<string,string>
}