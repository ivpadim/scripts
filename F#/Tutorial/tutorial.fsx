//use the F# lightweight syntax, which makes the code white-space sensitive, but simplifies many of the syntactical rules

#light

open System

//Tuples: simple type that groups together two or more values of possibly different types.

let valorMax x y =
   if x > y then x , sprintf "x es el valor maximo: %d" x
   else   y, sprintf "y es el valor maximo: %d" y


let (maxNum,maxStr) = valorMax 3 4

let tuple = (23,"Cadena");;

let (num,str) = tuple;;

String.Format("{0} como estamos", maxNum);;


//Discriminated Union: is used for representing a data type that store one of several 
//possible options (where the options are well known when writing the code)

// Create a value 'v' representing 'x + 10'
  type Expr = 
  | Binary   of string * Expr * Expr
  | Variable of string 
  | Constant of int;;

let v = Binary("+",Variable "4", Constant 10);;

let rec eval x =
    match x with
    | Binary (op, l, r) ->
       let(lv,rv) = (eval l, eval r)
       if (op="+") then lv + rv
       elif (op ="-") then lv -rv
       elif (op = "*") then lv * rv
       elif (op = "/") then lv  / rv
       else failwith "Uknown operator!"
    | Variable (v)->
       Int32.Parse v
    |Constant(n) ->
       n;;

do printfn "%i" (eval v)


//Records-> have to be declared in advance using a type construct

type Product =
  { Name:string
    Price:int};;

// Constructing a value of the 'Product' type    
let p = {Name="Test"; Price = 42;};;

p.Name;;

// Creating a copy with different 'Name'
let p2 = {p with Name="Test2"};;

p2.Name;;

//lists

let nums = [1; 2; 3; 4; 5];;

let rec sum list =
    match list with
    | h::tail -> (sum tail) + h
    | [] -> 0;;
    
sum nums;;

let rec sumAux acc list =
    match list with
    | h::tail -> sumAux (acc+h) tail
    | []-> acc

sumAux 10 nums

let createAdder n =(fun arg -> n + arg);;

let add10 = createAdder 10;;

add10 32;;

let add a b = a + b;;

let addAux10 = add 10;;

//http://tomasp.net/blog/fsharp-ii-functional.aspx

let odds = List.filter(fun n -> n%2<>0 )[1;2;3;4;5];;

let squares = List.map (fun n -> n*n) odds;;

let numsA = [1;2;3;4;5];;

let odds_plus_ten = 
    numsA
    |> List.filter (fun n -> n%2<> 0)
    |> List.map (add 10);;

(fst >> String.uppercase) ("hello world",123);;



let data = [("Jim",1);("John",2);("Jane",3) ];;

data |> List.map (fst >> String.uppercase);;

let n=1;;
let res =
   if n = 1 then
      printfn "..n is one..";
    else
       printfn "something else";;

let n = 21;;
let f =
    if n > 10 then
       let n=n*2
       (fun ()-> print_int n)
    else
       let n = n /2
       (fun () -> print_int n)  
f();;

//Imperative factorial calculation

let x = 10;;
let mutable mA = 1;;

for x = 2 to x do
    mA <- mA * x;;

let arr = [|1..10|];;

for i = 0 to 9 do
    arr.[i]<-11-arr.[i];;
    
let list = new ResizeArray<string>();;
list.Add("hello");;
list.Add("world");;
Seq.to_list list;;

type MyCell(n:int) =
    let mutable data = n +1
    do printfn "Creating MyCell (%d) " n
    
    member x.Data
        with get() = data
        and set(v) = data <- v
     
    member x.Print() =
       printfn "Data: %d" data
        
    override x.ToString() = 
        sprintf "Data: %d" data
    
    static member FromInt(n) =
        MyCell(n)

type AnyCell =
    abstract Value : int with get,set
    abstract Print :  unit -> unit


type ImplementCell(n:int) =
    let mutable  data = n +1
    interface AnyCell with
        member x.Print() = printfn "Data %d" data
        member x.Value
            with get() = data
            and set(v) = data <- v

let newCell n = 
    let data = ref n
    { new AnyCell with
       member x.Print() = printfn "Data: %d" (!data)
       member x.Value 
          with get() = !data
          and set(v) = data:=v};; 

type Variant =
    |Num of int
    |Str of string with
    member x.Print() =
        match x with
        | Num(n) -> printfn"Num %d" n
        | Str(s) -> printfn "Str %s" s;;
        
let xV = Num 42;;
do xV.Print()

let yV = Str "hola wey"
do yV.Print()


type ExprOperacion = 
    |Operacion of string * ExprOperacion * ExprOperacion
    |Operando of int

let sumOperacion =  Operacion("+",Operando 7, Operando 26)

let rec hasOperacion x =
    match x with
    |Operando(numero)->
       numero
    |Operacion(op,lv,rv)->
      let(l,r) = (hasOperacion lv, hasOperacion rv)
      if (op = "+") then l + r
      elif (op = "-") then l - r
      elif (op = "*") then l * r
      elif (op = "/") then l / r
      else failwith "Operador desconocido"
      

let result = hasOperacion  sumOperacion;

(************** Lists Comprehensions **************)

//List
let numericList = [1..10]

//Sequence
let alphaSeq = {'a'..'z'}

let multiplesOfThree = [0..3..30]

let revNumericSeq = {9.. -1 ..0}

let squareList = [ for x in 1..10 -> x*x ]

(************** Pattern Matching **************)

let rec findSequence list = 
    match list with     
    | [x;y;z] ->
        printfn "Last 3 numbers in the list were %i %i %i" x y z
     | 1::2::3::tail ->
        printfn "Find Sequence 1,2,3";
        findSequence tail
     | head::tail -> findSequence tail
     | []->()

let testsequence = [1..9]@[8.. -1 ..1]

findSequence testsequence