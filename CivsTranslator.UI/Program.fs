open System
open Dotgem.Text
open CivsTranslator

try
    Console.OutputEncoding = Text.Encoding.Unicode |> ignore
finally

ItemReader.read(
    [|
        "(estate2 | de)"
        "    \"Villa Rang 2\""
        "        \"Stufe\" >"
        "            * \"9\""
        "        \"Erzeugt\" >"
        "            * \"1255.44M pro Tag\""
        "        \"Verbraucht\" >"
        "            * \"Goldene Karroten (21M)\""
        "            * \"Kürbiskuchen (23M)\""
        "            * \"Pilzsuppe (20M)\""
        "            * \"Kuchen (21M)\""
        "            * \"Kanincheneintopf (21M)\""
        "(redstone_mine | de)"
        "    \"Redstonemine\""
        "        \"Stufe\" >"
        "            * \"9\""
        "        \"Erzeugt\" >"
        "            * \"1255.44M pro Tag\""
        "        \"Verbraucht\" >"
        "            * \"Goldene Karroten (21M)\""
        "            * \"Kürbiskuchen (23M)\""
        "            * \"Pilzsuppe (20M)\""
        "            * \"Kuchen (21M)\""
        "            * \"Kanincheneintopf (21M)\""
    |]
)
|> ItemToText.convert
|> printfn "%s"

printfn "exited"
