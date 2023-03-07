open System
open System.IO
open CivsTranslator

try
    Console.OutputEncoding = Text.Encoding.UTF8 |> ignore
finally

printf "Quelle: "
let source = Console.ReadLine()
printf "Ziel: "
let destination = Console.ReadLine()

let content = File.ReadAllLines(source)
let mcCode =
    ItemReader.read(content)
    |> ItemToText.convert

printfn "%s" mcCode

if File.Exists destination then
    printfn "Datei existiert bereits: %s" destination
    printfn "Wollen sie sie ersetzen? (y/n)"
    let mutable run = true
    while run do
        printfn "Datei existiert bereits: %s" destination
        printfn "Wollen sie sie ersetzen? (y/n)"
        let key = Console.ReadLine()
        match key with
        | "y" ->
            File.WriteAllText(destination, mcCode)
            printfn "saved at: %s" destination
            run <- false
        | "n" ->
            printfn "exits..."
            run <- false
        | _ -> ()
else
    File.WriteAllText(destination, mcCode)
    printfn "saved at: %s" destination

printfn "exited"
