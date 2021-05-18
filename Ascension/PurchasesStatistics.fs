module Ascension.PurchasesStatistics

open System
open Models
open System.Linq
open System.Collections.Generic

let updatePurchasesStatistics (order : Order) =
    let productLines = order.ProductLines
    let productLinePairs = 
                               (*productLines.SelectMany(fun x -> productLines, fun (x, y) -> Tuple.Create(x, y))*)
                               //.Where(fun tuple -> tuple.Item1 < tuple.Item2)
    0
