module CivsTranslator.Patterns
open System.Text.RegularExpressions

let listPoint = Regex("[ \t]*\* ((\"[\w\s\d\.\(\)ℳ\+\,\|]*\")|(\{No\})|(\{Yes\}))[ \t]*")
let listPointText = Regex("[ \t]*\* \"[\w\s\d\.\(\)ℳ\+\,\|]*\"[ \t]*")
let listPointYes = Regex("[ \t]*\* (\{Yes\})[ \t]*")
let listPointNo = Regex("[ \t]*\* (\{No})[ \t]*")
let listHead = Regex("\s*\"[\w\s\d\.\()ℳ]*\" >\s*")