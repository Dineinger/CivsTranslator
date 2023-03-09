namespace CivsTranslator

open System.Text
open Dotgem.Text

[<RequireQualifiedAccess>]
type NodeType =
    | H1
    | Text
    | ListHeader
    | Point

[<RequireQualifiedAccess>]
type YesNo =
    | No
    | Yes
module YesNo =
    let asBool x = match x with | YesNo.Yes -> true | YesNo.No -> false
    let toGerman x = match x with | YesNo.Yes -> "Ja" | YesNo.No -> "Nein"

[<RequireQualifiedAccess>]
type TextValue =
    | Text of string
    | YesNo of YesNo
    | Extensive of ExtensiveNodeValue
and
    ExtensiveNodeValue =
    {
        Values : TextValue array
    }

type Node =
    {
        NodeType : NodeType
        Value : ExtensiveNodeValue
        Children : Node array
    }

module private Helpers =        
    let rec addNodeText (sb : StringBuilder) (values : TextValue array) =
        for value in values do
            match value with
            | TextValue.Text t ->
                sb.Append('"').Append(t).Append('"') |> ignore
            | TextValue.YesNo x -> sb.Append(if YesNo.asBool(x) then "{yes}" else "{no}") |> ignore
            | TextValue.Extensive x -> addNodeText sb x.Values

module Node =
    let rec toString (sb : StringBuilder) tabIndex (node: Node) : unit =
        match node.NodeType with
        | NodeType.H1 ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            Helpers.addNodeText sb node.Value.Values |> ignore
            sb.AppendLine() |> ignore
            for n in node.Children do
                toString sb (tabIndex + 1)  n
        | NodeType.Text ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            Helpers.addNodeText sb node.Value.Values
            sb.AppendLine() |> ignore
        | NodeType.ListHeader ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            Helpers.addNodeText sb node.Value.Values
            sb.AppendLine " >" |> ignore
            for n in node.Children do
                toString sb (tabIndex + 1)  n
        | NodeType.Point ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            sb.Append "* " |> ignore
            sb.Append(Helpers.addNodeText sb node.Value.Values) |> ignore
            sb.AppendLine() |> ignore

    let findNextChildWithSameIndentation currentIndentation (node : Node) : Node =
        raise(System.NotImplementedException())
