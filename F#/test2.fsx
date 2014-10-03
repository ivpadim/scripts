
open System

type matrix(m:float list list) =
    ///Numero de filas de la matriz
    member this.rows = m.Length
    ///Numero de columnas de la matriz
    member this.cols = m.Head.Length
    //Elementos de la matriz
    member this.items = m        
    //Obtiene el valor de un elemento de la matriz
    member this.item i j = m.[i].[j]        
    override this.ToString() = "\n" + (this.items |> List.fold (fun r s -> r + "[ " + (s |> List.fold (fun x y -> x + (sprintf "%*.02f "4 y)) "") + "]\n") "")
    ///Obtiene la matriz traspuesta   
    member this.t() = 
        matrix ([for i in 0..this.cols - 1 do
                    yield [for j in 0..this.rows - 1 do
                            yield this.item j i]])
    member this.g() =
        let mi = ref (this)
        for i = 0 to mi.Value.rows - 1 do
            //Se hace 1 los elementos que estan en diagonal
            mi := matrix (mi.Value.items 
                          |> List.mapi (fun ix items -> items 
                                                        |> List.map (fun y -> if ix = i then y / mi.Value.item i i else y)))            
            //Hacer 0 las columna i de los otros renglones
            mi := matrix (mi.Value.items 
                          |> List.mapi (fun ix items -> items 
                                                        |> List.mapi (fun j y -> if ix <> i then 
                                                                                    (mi.Value.item i j * - mi.Value.item ix i) + y  
                                                                                 else y )))
        let l = this.cols - 1
        mi.Value.items |> List.map (fun x -> x.[l])
         
   
let a = matrix[[ 3.0; 1.0]
               [ 3.0; 4.0]]

let b = matrix[[2.0;5.0]
               [1.0;5.0]]

2*a
//Ejemplo
//Tenemos el siguiente sistema de ecuaciones lineales
  
//  x +  2y –  4z =   8
// 5x + 11y – 21z = -22               
// 3x –  2y +  3z =  11 

let x = matrix [[ 1.0;  2.0; -4.0]@[  8.0]
                [ 5.0; 11.0;-21.0]@[-22.0]
                [ 3.0; -2.0;  3.0]@[ 11.0]]

x.g()
let a = [1;2;3]@[1;2]
let gauss (x:matrix) = 
    let m = ref(x)
    for i = 0 to x.rows - 1 do 
        m:=  matrix (m.Value.items 
                     |> List.mapi (fun ix items -> items 
                                                   |> List.map (fun y -> if ix = i then y / m.Value.item i i else y)))            
        //Hacer 0 las columna i de los otros renglones
        m:= matrix (m.Value.items 
                    |> List.mapi (fun ix items -> items 
                                                  |> List.mapi (fun j y -> if ix > i then 
                                                                                (m.Value.item i j * - m.Value.item ix i) + y  
                                                                            else y )))
    m.Value

x.g()
gauss x







