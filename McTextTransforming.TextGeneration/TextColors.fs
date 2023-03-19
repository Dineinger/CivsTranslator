namespace McTextTransforming.TextGeneration
open System
open System.Text.Json
open System.Text.Json.Serialization

type TextColors [<JsonConstructor>](h1, text, listHeader, point, yes, no) =
    [<JsonPropertyName("h1")>]
    member _.H1 = h1
    [<JsonPropertyName("text")>]
    member _.Text = text
    [<JsonPropertyName("list-header")>]
    member _.ListHeader = listHeader
    [<JsonPropertyName("point")>]
    member _.Point = point
    [<JsonPropertyName("yes")>]
    member _.Yes = yes
    [<JsonPropertyName("no")>]
    member _.No = no

module TextColors =
    let fromJson (json : string) : Result<TextColors, Exception> =
        try
            Ok(JsonSerializer.Deserialize(json))
        with e ->
            Error(e)