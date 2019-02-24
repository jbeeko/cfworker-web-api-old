module Recipes
open WorkersInterop


// Handle the request returning a ServiceWorker Response promise
let rec routeRequest (verb: Verb) (path: string list) (req: CFWRequest) =
    match (verb, path) with
    | _, ["aggregating"] ->    Aggregating.fetchAndApply req
    | _, _ ->       noHandler req

and private noHandler req  =
    newResponse "Invalid recipes request" "200" |> wrap
