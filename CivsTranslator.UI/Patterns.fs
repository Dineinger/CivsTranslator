module CivsTranslator.Patterns
open System.Text.RegularExpressions

let listPoint = Regex("\s*\* \"[\w\s\d\.\()]*\"\s*")
let listHead = Regex("\s*\"[\w\s\d\.\()]*\" >\s*")