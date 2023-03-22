namespace McTextTransforming.TextGeneration
open System
open System.Text.Json
open System.Text.Json.Serialization
open McTextTransforming

type TextColors [<JsonConstructor>](h1, text, listHeader, point, yes, no) =
    [<JsonPropertyName("h1")>]
    member self.H1 = h1
    [<JsonPropertyName("text")>]
    member self.Text = text
    [<JsonPropertyName("list-header")>]
    member self.ListHeader = listHeader
    [<JsonPropertyName("point")>]
    member self.Point = point
    [<JsonPropertyName("yes")>]
    member self.Yes = yes
    [<JsonPropertyName("no")>]
    member self.No = no

    member self.GetColorFor nodeType textValue =
        match nodeType with
        | NodeType.H1 -> self.H1
        | NodeType.Text ->
            match textValue with
            | TextValue.Text _ -> self.Text
            | TextValue.YesNo x ->
                match x with
                | YesNo.Yes -> self.Yes
                | YesNo.No -> self.No
            | TextValue.Extensive _ -> String.Empty
        | NodeType.ListHeader ->
            match textValue with
            | TextValue.Text _ -> self.ListHeader
            | TextValue.YesNo x ->
                match x with
                | YesNo.Yes -> self.Yes
                | YesNo.No -> self.No
            | TextValue.Extensive _ -> String.Empty
        | NodeType.Point ->
            match textValue with
            | TextValue.Text _ -> self.Point
            | TextValue.YesNo x ->
                match x with
                | YesNo.Yes -> self.Yes
                | YesNo.No -> self.No
            | TextValue.Extensive _ -> String.Empty


module TextColors =
    let fromJson (json : string) : Result<TextColors, Exception> =
        try
            Ok(JsonSerializer.Deserialize(json))
        with e ->
            Error(e)