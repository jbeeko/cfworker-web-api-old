module Contacts
open WorkersInterop
open Thoth.Json

type Contact = {
    Name: string
}
let  contactDecoder = Decode.Auto.generateDecoder<Contact>()

// Handle the request returning a ServiceWorker Response promise
let rec routeRequest (verb: Verb) (path: string list) (req: CFWRequest) =
    match (verb, path) with
    | GET, [i] ->   getContact req i
    | GET, [] ->    getContacts req
    | POST, [] ->   postContact req
    | _, _ ->       noHandler req

and private getContact req i =
    newResponse (sprintf "Get contact with id %s" i) "200" |> wrap

and private getContacts req  =
    newResponse "Get contacts" "200" |> wrap

and private postContact req  =
    promise {
         let! body = (req.text())
         let contact = Decode.fromString contactDecoder body
         match contact with
         | Ok c -> return newResponse (sprintf "Post contact %s" c.Name) "200"
         | Error e -> return newResponse (sprintf "Unable to process: %s because: %O" body e) "200"
    }

and private noHandler req  =
    newResponse "Invalid contact request" "200" |> wrap
