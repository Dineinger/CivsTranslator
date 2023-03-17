namespace McTextTransforming

[<RequireQualifiedAccess>]
type YesNo =
    | No
    | Yes

module YesNo =
    let asBool x = match x with | YesNo.Yes -> true | YesNo.No -> false
    let toGerman x = match x with | YesNo.Yes -> "Ja" | YesNo.No -> "Nein"
