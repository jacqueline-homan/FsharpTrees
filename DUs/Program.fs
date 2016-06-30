open System
open System.IO

type Description =
    { Name : string;
      PartNo : string;
      Price : decimal option }

type Part =
    | Item of Description
    | Repeat of Part * int
    | Compound of Description * Part list

let pad = Item { Name = "Brake Pad"; PartNo = "F150XLT"; Price = Some 15.90M }
let calliperBody = Item { Name = "Calliper Body"; PartNo = "CF150XLT"; Price = Some 10.00M }
let brakePiston = Item { Name = "Piston"; PartNo = "PF150XLT"; Price = Some 29.50M }
let disc = Item { Name = "Disc"; PartNo = "DF150XLT"; Price = Some 45.95M }
let clip = Item { Name = "Clip"; PartNo = "CLF150XLT"; Price = Some 15.00M }
let pin = Item { Name = "Pin"; PartNo = "PINF150XLT"; Price = Some 5.75M }

let calliper =
    Compound(
        {Name = "Calliper"; PartNo = "RF150XLT"; Price = None},
        [calliperBody; Repeat(brakePiston, 2)] )

let brake =
    Compound(
        {Name = "Brake"; PartNo = "BRF150XLT"; Price = None},
        [disc; calliper; Repeat(pin, 2); Repeat(pad, 2); clip])


let flat (part:Part)=
    let rec flatten p =
        seq {
            match p with
            | Item d ->
                yield d
            | Repeat(rp, count) ->
                for _ in 0..count-1 do
                    yield! flatten rp
            | Compound(d, children) ->
                yield d
                for child in children do
                    yield! flatten child}
    flatten part
brake 
|> flat
|> Seq.iter(fun desc -> printfn "%A" desc)

let totalCost part =
    part
    |> flat
    |> Seq.sumBy(fun d ->
        match d.Price with
        | Some c -> c
        | None -> 0.0M)

totalCost brake |> printfn "Total brake price: %A"
printfn "\n\n"
let indented (part : Part) =
    let rec indent level count p =
        seq {
            match p with 
            | Item d -> 
                yield level, count, d
            | Repeat(pr, count) -> 
                yield! indent (level+1) count pr
            | Compound(d, children) ->
                yield level, count, d
                for child in children do
                    yield! indent (level+1) 1 child }
    indent 0 1 part

let printTree (part : Part) =
    let printItem (indent: int, count : int, desc : Description) =
        let costStr = 
            match desc.Price with
            | Some c -> sprintf "%0.2f" c
            | None -> sprintf "N/A"
        printfn "%s %s %s %s x %i" (String(' ', indent*3)) desc.Name desc.PartNo costStr count
    part
    |> indented
    |> Seq.iter printItem 

printTree brake

(*
printfn "\n\n"
let list1 = [ 3 ; 2 ; 1 ]
let list2 = 4 :: list1
match list2 with
| head::tail -> printfn "%b" (obj.ReferenceEquals(tail,list1))
| _ -> ()
*)

    (*
type Tree<'T> =
    | Leaf of 'T
    | Node of Tree<'T> * Tree<'T>

let myTree : Tree<string> =
    Node(Leaf "one", Node(Leaf "two", Leaf "three"))
*)




[<EntryPoint>]
let main argv = 
    //printfn "%A" argv
    0 // return an integer exit code

