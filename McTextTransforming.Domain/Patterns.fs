namespace McTextTransforming
open System.Text.RegularExpressions

module Patterns =
    let listPoint = Regex("[ \t]*\* ((\"[\w\s\d\.\(\)ℳ\+\,\|]*\")|(\{No\})|(\{Yes\}))[ \t]*")
    let listPointText = Regex("[ \t]*\* \"[\w\s\d\.\(\)ℳ\+\,\|]*\"[ \t]*")
    let listPointYes = Regex("[ \t]*\* (\{Yes\})[ \t]*")
    let listPointNo = Regex("[ \t]*\* (\{No})[ \t]*")
    let listHead = Regex("\s*\"[\w\s\d\.\()ℳ]*\" >\s*")

type IPatterns =
    abstract member ListPoint : Regex
    abstract member ListPoint_Text : Regex
    abstract member ListPoint_Spacial : Regex
    abstract member ListPoint_Spacial_Yes : Regex
    abstract member ListPoint_Spacial_No : Regex
    abstract member ListHead : Regex

type Patterns =
    interface IPatterns with
        member _.ListPoint = Patterns.listPoint
        member _.ListPoint_Text = Patterns.listPointText
        member _.ListPoint_Spacial = Regex("[ \t]*\* (\{[A-z]+\})[ \t]*")
        member _.ListPoint_Spacial_Yes = Patterns.listPointYes
        member _.ListPoint_Spacial_No = Patterns.listPointNo
        member _.ListHead = Patterns.listHead
