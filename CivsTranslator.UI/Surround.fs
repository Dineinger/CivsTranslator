module CivsTranslator.Surround
open System.Text

let withQuotes (sb : StringBuilder) (line : string) =
    sb.Append('"').Append(line).Append('"') |> ignore