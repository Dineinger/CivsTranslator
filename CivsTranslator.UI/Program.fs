module Program
open System
open System.IO
open CivsTranslator

module FileSaver =
    let save (path : string) doOverride (content : string) =
        if File.Exists path then
            if doOverride then
                File.WriteAllText(path, content)
            else
                raise(UnauthorizedAccessException($"File {path} already exists but was not meant to be overriden."))
        else
            File.WriteAllText(path, content)


    let saver destination mcCode =
        printfn "%s" mcCode

        if File.Exists destination then
            let mutable run = true
            while run do
                printfn "Datei existiert bereits: %s" destination
                printfn "Wollen sie sie ersetzen? (y/n)"
                let key = Console.ReadLine().ToLower()
                match key with
                | "y" ->
                    save destination true mcCode 
                    printfn "Speicherort: %s" destination
                    run <- false
                | "n" ->
                    printfn "Verlässt Programm..."
                    run <- false
                | _ -> ()
        else
            save destination false mcCode
            printfn "Speicherort: %s" destination

let runFileConverterWithPaths source destination saver =
    let content = File.ReadAllLines(source)
    let mcCode =
        ItemParser.parse(content)
        |> ItemToText.convert
    saver destination mcCode

let runFileConverter() =
    printf "Quelle: "
    let source = Console.ReadLine()
    printf "Ziel: "
    let destination = Console.ReadLine()
    let mcCode = runFileConverterWithPaths source destination FileSaver.saver

    printfn "exited"

let mainMenu() =
    printfn "(1) exit"
    printfn "(2) open file converter"
    let decision = Console.ReadLine()
    match decision with
    | "1" -> ()
    | "2" -> runFileConverter()
    | _ -> ()


let processArguments (args : string array) =
    match args with
    | null -> runFileConverter()
    | x when x.Length = 0 -> runFileConverter()
    | x when x.Length = 3 ->
        match x with
        | x when x[0] = "convert" ->
            runFileConverterWithPaths x[1] x[2] FileSaver.saver
        | x -> printfn $"Unknown command {x}"
    | _ ->
        printfn "Unknown input arguments, what do you want to do?"
        mainMenu()


[<EntryPoint>]
let main args =

    try
        Console.OutputEncoding = Text.Encoding.UTF8 |> ignore
    finally
        ()

    try
        processArguments args
        0
    with e ->
        printfn "%s" e.Message
        -1

