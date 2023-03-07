namespace CivsTranslator

type ReadError =
    {
        Message : string
    }

[<RequireQualifiedAccess>]
type ReadResult<'a> =
    | Some of 'a
    | Errors of ReadError array

module ReadResult =
    let some value = ReadResult.Some value


type ItemRaw = string array

[<RequireQualifiedAccess>]
type LinesOrdered =
        | DefinitionLine of string
        | Children of LinesOrdered option * LinesOrdered List    

type Language =
| German
| Unknown

module Language =
    let parseLanguage (raw : string) =
        match raw.ToLower() with
        | "de" -> Language.German
        | _ -> Language.Unknown

type DefinitionLine =
    {
        Key : string
        Language : Language
    }
