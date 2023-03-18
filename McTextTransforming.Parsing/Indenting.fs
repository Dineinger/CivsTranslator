module McTextTransforming.Parsing.Indenting
open System
open System.Collections.Generic
open Dotgem.Text

module private Helpers =
    /// <summary>
    /// Gets the space before the content of the line and returns the value devided by 4 (4 = tab).
    /// Throws if the space is not devidable by 4
    /// </summary>
    let getTabCount line lineNumber =
        let spaceCount = CharCounter.GetLeadingSpaceCount line
        if spaceCount % 4 <> 0 then
            raise(Exception($"Wrong indentation, indentation must be devidable by 4. Line Number: {lineNumber}"))
        else
            spaceCount / 4

type Line =
    {
        Value : string
        LineNumber : int
    }

[<RequireQualifiedAccess>]
type IndentedCode =
    | Line of Line
    | Group of IndentedCode List

type Container =
    {
        Line : Line
        Children : Container List
    }
module Container =
    let create x = { Line = x ; Children = List<Container>() }

let rec groupInSubgroups (groupedText : IndentedCode List) : Container List =
    let result = List<Container>()
    for item in groupedText do
        match item with
        | IndentedCode.Line line ->
            result.Add(Container.create line)
        | IndentedCode.Group group ->
            let group = groupInSubgroups group
            result[result.Count - 1].Children.AddRange group
    result

let groupByIndention (text : string array) =
    let buffer = Dictionary<int, List<IndentedCode>>()
    let mutable indentationBefore = -1
    let mutable index = text.Length - 1
    while index >= 0 do
        let line = if String.IsNullOrWhiteSpace(text[index]) then String.Empty else text[index]
        let tabIndex = Helpers.getTabCount (String.op_Implicit(line)) index
        if tabIndex >= indentationBefore || indentationBefore = -1 then
            if not(buffer.ContainsKey tabIndex) then
                buffer.Add(tabIndex, List<IndentedCode>())
            let currentBuffer = buffer[tabIndex] 
            currentBuffer.Insert(0, IndentedCode.Line { Value = line; LineNumber = index + 1})
        elif tabIndex = (indentationBefore - 1) then
            if not(buffer.ContainsKey tabIndex) then
                buffer.Add(tabIndex, List<IndentedCode>())
            if not(buffer.ContainsKey (tabIndex + 1)) then
                buffer.Add(tabIndex + 1, List<IndentedCode>())
            let currentBuffer = buffer[tabIndex]
            let indentedBuffer = buffer[tabIndex + 1]
            currentBuffer.Insert(0, IndentedCode.Group(indentedBuffer))
            currentBuffer.Insert(0, IndentedCode.Line({ Value = line; LineNumber = index + 1}))
            buffer[tabIndex + 1] <- List<IndentedCode>()
        indentationBefore <- tabIndex

        index <- index - 1
    let root = buffer[0]
    root

let groupToContainer input =
    input
    |> groupByIndention
    |> groupInSubgroups

let filterOutEmptyLines (list : Container seq) =
    [|
        for item in list do
            if not(String.IsNullOrWhiteSpace(item.Line.Value)) then
                item
    |]
        
