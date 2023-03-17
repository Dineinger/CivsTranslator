namespace McTextTransforming

type Language =
    | German
    | Unknown

module Language =
    let parseLanguage (raw : string) =
        match raw.ToLower() with
        | "de" -> Language.German
        | _ -> Language.Unknown