open System
open Dotgem.Text
open CivsTranslator

try
    Console.OutputEncoding = Text.Encoding.Unicode |> ignore
finally

let value =
    ItemReader.read(
        [|
            "(coal_mine | de)"
            "    \"Kohlemine\""
            "        \"yes\""
            "            * \"no\""
            "        \"YOOO\""
            "            \"Low\""
            "(redstone_mine | de)"
            "    \"Redstonemine\""
        |]
    )
    |> ItemToText.convert
    |> printfn "%s"

value |> ignore
printfn "exited"
