module CivsTranslator.Patterns
open System.Text.RegularExpressions

let listPoint = Regex("[ \t]*\* ((\"[\w\s\d\.\(\)ℳ]*\")|(\{No\})|(\{Yes\}))[ \t]*")
let listHead = Regex("\s*\"[\w\s\d\.\()ℳ]*\" >\s*")