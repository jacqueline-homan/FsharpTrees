open System
open System.IO

type FoodStuff(name : string, allergenic : bool, FoodStuffs : FoodStuff list) =
    member this.Name = name
    member this.FoodStuffs = FoodStuffs
    member this.Allergenic = allergenic

let cake =
    FoodStuff("cake", false, 
        [
            FoodStuff("flour", false, [])
            FoodStuff("eggs", false, [])
            FoodStuff("mixed fruit", false, 
                [
                    FoodStuff("raisins", false, [])
                    FoodStuff("saltanas", false, [])
                ])
            FoodStuff("mixed nuts", false, 
                [
                    FoodStuff("walnuts", false, [])
                    FoodStuff("almonds", false, [])
                    FoodStuff("peanuts", true, [])
                    ])
        ]
    )

let rec allergenic (foodStuff : FoodStuff) =
    foodStuff.Allergenic || foodStuff.FoodStuffs |> List.exists allergenic 
allergenic cake |> printfn "%A"
[<EntryPoint>]
let main argv = 
    //printfn "%A" argv
    0 // return an integer exit code

