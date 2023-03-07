module CivsTranslator.ItemReader
open System
open System.Collections.Generic
open System.Linq

let cleanList list : string array option =
    let result = [|
        for line in list do
            if not(String.IsNullOrWhiteSpace(line)) then
                line
    |]

    if result.Length = 0 then option.None
    else Some result

let parseDefinitionLine (line : string) : DefinitionLine =
    if line.StartsWith('(') && line.EndsWith(')') then
        let parts = line.Trim([| '(' ; ')' |]).Split(" | ")
        if parts.Length = 2 then
            let key = parts[0]
            let language = parts[1]
            { Key = key ; Language = (Language.parseLanguage language)}
        else
            raise(Exception "definition line is wrong")
    else
        raise(Exception "definition line does not start with a ( and ends with a )")

let isEnclosedWithQuoteMarks (line : string) =
    line.StartsWith("\"") && line.EndsWith("\"") && line.Count(fun s -> s = '"') = 2

let parseNodeLine (item : string) (children : Node array) =
    let trimmed = item.Trim()
    if Patterns.listPoint.IsMatch(item) then
        {
            NodeType = NodeType.Point
            Value = NodeValue.Text(trimmed[3..trimmed.Length - 2])
            Children = children
        }
    elif Patterns.listHead.IsMatch(item) then
        {
            NodeType = NodeType.ListHeader
            Value = NodeValue.Text(trimmed[1..trimmed.Length - 4])
            Children = children
        }
    elif isEnclosedWithQuoteMarks trimmed then
        {
            NodeType = NodeType.Text
            Value = NodeValue.Text(trimmed[1..trimmed.Length - 2])
            Children = children
        }
    else
        raise(NotImplementedException())

let rec parseNodes (items : Indenting.Container List) : Node array =
    [|
        for item in items do
            let lineRaw = item.Line
            let childrenRaw = item.Children
            let children = parseNodes childrenRaw
            let line = parseNodeLine lineRaw children
            line
    |]

let parseRootNode (item : Indenting.Container) =
    let line = item.Line
    let name = line.Trim().Trim('"')
    let subNodes = parseNodes item.Children
    let rootNode =
        {
            NodeType = NodeType.H1
            Value = NodeValue.Text name
            Children = subNodes
        }
    (name, rootNode)

let parseItems (items : Indenting.Container List) : Item array =
    [|
        for item in items do
            let line = item.Line
            let definitionLine = parseDefinitionLine line
            let key = definitionLine.Key
            let (name, rootNode) = parseRootNode (item.Children.First())
            {
                Key = key
                Name = name
                Description = rootNode
            }
    |]

let rec display index (items : Indenting.Container List) : unit =
    for item in items do
        printfn "%s" item.Line
        display (index + 1) item.Children

let read (text : string array) : Item array =
    text
    |> Indenting.groupByIndention
    |> Indenting.groupInSubgroups
    |> parseItems
