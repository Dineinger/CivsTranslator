﻿module CivsTranslator.ItemReader
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

let parseDefinitionLine (line : Indenting.Line) : DefinitionLine =
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

let isEnclosedWithQuoteMarks (line : string) =
    line.StartsWith("\"") && line.EndsWith("\"") && line.Count(fun s -> s = '"') = 2

let parseNodeLine (line : Indenting.Line) (children : Node array) =
    let item = line.Value
    let trimmed = item.Trim()
    if Patterns.listPoint.IsMatch(item) then
        {
            NodeType = NodeType.Point
            Value =
                if Patterns.listPointText.IsMatch(item) then
                    NodeValue.Text(trimmed[3..trimmed.Length - 2])
                elif Patterns.listPointYes.IsMatch(item) then
                    NodeValue.YesNo true
                elif Patterns.listPointNo.IsMatch(item) then
                    NodeValue.YesNo false
                else
                    raise(NotImplementedException("This type of bullet point is unknown, maybe you miss quotes or curly braces."))
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
        raise(NotImplementedException($"Unknown line syntax at line: {line.LineNumber}"))

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
    let line = item.Line.Value
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

let rec display index (items : Indenting.Container List) : unit =
    for item in items do
        printfn "%s" item.Line.Value
        display (index + 1) item.Children

let read (text : string array) : Item array =
    let items =
        text
        |> Indenting.groupByIndention
        |> Indenting.groupInSubgroups
        |> parseItems

    items
