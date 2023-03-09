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
type NodeValue =
    | None
    | Text of string
    | YesNo of bool

type Node =
    {
        NodeType : NodeType
        Value : NodeValue
        Children : Node array
    }

module private Helpers =
    let addNodeText (sb : StringBuilder) node =
        match node.Value with
        | NodeValue.None -> sb.Append "\"\""
        | NodeValue.Text t ->
            sb.Append('"').Append(t).Append('"')
        | NodeValue.YesNo x -> sb.Append(if x then "{yes}" else "{no}")
        |> ignore

module Node =
    let rec toString (sb : StringBuilder) tabIndex (node: Node) : unit =
        match node.NodeType with
        | NodeType.H1 ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            Helpers.addNodeText sb node |> ignore
            sb.AppendLine() |> ignore
            for n in node.Children do
                toString sb (tabIndex + 1)  n
        | NodeType.Text ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            Helpers.addNodeText sb node
            sb.AppendLine() |> ignore
        | NodeType.ListHeader ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            Helpers.addNodeText sb node
            sb.AppendLine " >" |> ignore
            for n in node.Children do
                toString sb (tabIndex + 1)  n
        | NodeType.Point ->
            sb.AppendSpace(tabIndex * 4) |> ignore
            sb.Append "* " |> ignore
            sb.Append(Helpers.addNodeText sb node) |> ignore
            sb.AppendLine() |> ignore

    let findNextChildWithSameIndentation currentIndentation (node : Node) : Node =
        raise(System.NotImplementedException())
