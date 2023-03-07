module CivsTranslator.Indenting
open System
open System.Collections.Generic
open Dotgem.Text

module Helpers =
    let getTabCount line =
        let spaceCount = CharCounter.GetLeadingSpaceCount line
        if spaceCount % 4 <> 0 then
            ReadResult.Errors([|{Message = "Wrong indentation of lines. Lines must be indented by 4 spaces OR 1 tab or multiblied values of those."}|])
        else
            ReadResult.Some(spaceCount / 4)


[<RequireQualifiedAccess>]
type IndentedCode =
    | Line of string
    | Group of IndentedCode List

type Container =
    {
        Line : string
        Children : Container List
    }
module Container =
    let create x = { Line = x ; Children = List<Container>() }

let rec groupInSubgroups (groupedText : IndentedCode List) : Container List =
    let result = List<Container>()
    let mutable lastLineAsContainer = ""
    for item in groupedText do
        match item with
        | IndentedCode.Line line ->
            lastLineAsContainer <- line
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
        let line = text[index]
        let tabIndex =
            match Helpers.getTabCount (String.op_Implicit(line)) with
            | ReadResult.Errors x -> raise(NotImplementedException())
            | ReadResult.Some x -> x
        if tabIndex >= indentationBefore || indentationBefore = -1 then
            if not(buffer.ContainsKey tabIndex) then
                buffer.Add(tabIndex, List<IndentedCode>())
            let currentBuffer = buffer[tabIndex] 
            currentBuffer.Add(IndentedCode.Line line)
        elif tabIndex = (indentationBefore - 1) then
            if not(buffer.ContainsKey tabIndex) then
                buffer.Add(tabIndex, List<IndentedCode>())
            if not(buffer.ContainsKey (tabIndex + 1)) then
                buffer.Add(tabIndex + 1, List<IndentedCode>())
            let currentBuffer = buffer[tabIndex]
            let indentedBuffer = buffer[tabIndex + 1]
            currentBuffer.Insert(0, IndentedCode.Group(indentedBuffer))
            currentBuffer.Insert(0, IndentedCode.Line(line))
            buffer[tabIndex + 1] <- List<IndentedCode>()
        indentationBefore <- tabIndex

        index <- index - 1
    let root = buffer[0]
    root

