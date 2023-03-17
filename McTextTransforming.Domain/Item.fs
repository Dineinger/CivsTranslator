namespace McTextTransforming
open System.Text

type Item =
    {
        Key : string
        Name : string
        Description : Node
    }

module Item =
    let emptyItem() =
        {
            Key = ""
            Name = ""
            Description =
                {
                    NodeType = NodeType.Text
                    Value =
                        {
                            Values =
                                [|
                                    TextValue.Text("")
                                |]
                        }
                    Children = [||]
                }
        }

    let toString item =
        let sb = StringBuilder()
        sb.Append("(").Append(item.Key).Append(" | ").Append("de").AppendLine(")")|> ignore
        Node.toString sb 1 item.Description
        printfn "%s" (sb.ToString())
        ()