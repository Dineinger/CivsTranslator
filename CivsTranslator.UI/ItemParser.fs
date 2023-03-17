module CivsTranslator.ItemParser
open System
open System.Collections.Generic
open System.Linq
open McTextTransforming

let private parseDefinitionLine (line : Indenting.Line) : DefinitionLine =
    let (line, number) = line.Value , line.LineNumber
    if line.StartsWith('(') && line.EndsWith(')') then
        let parts = line.Trim([| '(' ; ')' |]).Split(" | ")
        if parts.Length = 2 then
            let key = parts[0]
            let language = parts[1]
            { Key = key ; Language = (Language.parseLanguage language)}
        else
            raise(Exception $"definition line is wrong (Line Number {number})")
    else
        raise(Exception $"definition line does not start with a ( and ends with a ) (Line Number {number})")

let private isEnclosedWithQuoteMarks (line : string) =
    line.StartsWith("\"") && line.EndsWith("\"") && line.Count(fun s -> s = '"') = 2

let private parseNodeLine (line : Indenting.Line) (children : Node array) =
    let item = line.Value
    let trimmed = item.Trim()
    if Patterns.listPoint.IsMatch(item) then
        {
            NodeType = NodeType.Point
            Value =
                if Patterns.listPointText.IsMatch(item) then
                    { Values = [|TextValue.Text(trimmed[3..trimmed.Length - 2])|] }
                elif Patterns.listPointYes.IsMatch(item) then
                    { Values = [| TextValue.YesNo(YesNo.Yes) |] }
                elif Patterns.listPointNo.IsMatch(item) then
                    { Values = [| TextValue.YesNo(YesNo.No) |] }
                else
                    raise(NotImplementedException("This type of bullet point is unknown, maybe you miss quotes or curly braces."))
            Children = children
        }
    elif Patterns.listHead.IsMatch(item) then
        {
            NodeType = NodeType.ListHeader
            Value = { Values = [| TextValue.Text(trimmed[1..trimmed.Length - 4]) |]}
            Children = children
        }
    elif isEnclosedWithQuoteMarks trimmed then
        {
            NodeType = NodeType.Text
            Value = { Values = [| TextValue.Text(trimmed[1..trimmed.Length - 2]) |] }
            Children = children
        }
    else
        raise(NotImplementedException($"Unknown line syntax at line: {line.LineNumber}"))

let rec private parseNodes (items : Indenting.Container List) : Node array =
    [|
        for item in items do
            let lineRaw = item.Line
            let childrenRaw = item.Children
            let children = parseNodes childrenRaw
            let line = parseNodeLine lineRaw children
            line
    |]

let private parseRootNode (item : Indenting.Container) =
    let line = item.Line.Value
    let name = line.Trim().Trim('"')
    let subNodes = parseNodes item.Children
    let rootNode =
        {
            NodeType = NodeType.H1
            Value = { Values = [| TextValue.Text name |] }
            Children = subNodes
        }
    (name, rootNode)

let parseItems (items : Indenting.Container seq) : Item array =
    [|
        for item in items do
            let line = item.Line
            if not(String.IsNullOrWhiteSpace(line.Value)) then
                let definitionLine = parseDefinitionLine line
                let key = definitionLine.Key
                let (name, rootNode) = parseRootNode (item.Children.First())
                {
                    Key = key
                    Name = name
                    Description = rootNode
                }
    |]

let parse (text : string array) : Item array =
    text
    |> Indenting.groupByIndention
    |> Indenting.groupInSubgroups
    |> Indenting.filterOutEmptyLines
    |> parseItems
