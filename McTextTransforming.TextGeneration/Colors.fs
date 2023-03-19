namespace McTextTransforming.TextGeneration
open System
open System.Text.Json
open System.Text.Json.Serialization

type Colors [<JsonConstructor>](white, lightGray, green, red, lightGrayBlue, darkCyan) =
    [<JsonPropertyName("white")>]
    member _.White : string = white
    [<JsonPropertyName("light-gray")>]
    member _.LightGray : string = lightGray
    [<JsonPropertyName("green")>]
    member _.Green : string = green
    [<JsonPropertyName("red")>]
    member _.Red : string = red
    [<JsonPropertyName("light-gray-blue")>]
    member _.LightGrayBlue : string = lightGrayBlue
    [<JsonPropertyName("dark-cyan")>]
    member _.DarkCyan : string = darkCyan

module Colors =
    let fromJson (json : string) : Result<Colors, Exception> =
        try
            Ok(JsonSerializer.Deserialize(json))
        with e ->
            Error(e)