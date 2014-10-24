#light

//Bring the SonicMQ assemblies to connect with the Message Server.
#I @"C:\Program Files\Sonic Client\Net Assemblies\bin\release"
#r "Sonic.Jms.dll"
#r "Sonic.Jms.Cf.Impl.dll"
#r "System.Configuration.dll"

#load @"mqlib.fs"
#load @"sqlloger.fs"

open Sonic.Jms
open System
open System.Data
open System.Data.SqlClient
open System.Threading
open Scripts.SonicMq
open Scripts.SonicMq.Sql.Logging


[<STAThread>]
let main() =
    
    try
        let session = 
            MqEngineGenerator.createSession(MqEngineGenerator.createConnectionString())
        
        do connectToSql()
        let itopic = session.createTopic("IMONITOR.#")
        let topic = session.createTopic("MONITOR.#")
        let iconsumer = session.createConsumer(itopic)
        let consumer = session.createConsumer(topic)
        let listener = new MqListener(sqlLoggerProcMsg)
        do iconsumer.setMessageListener(listener)
        do consumer.setMessageListener(listener)
        Console.CursorLeft<-0
        Console.WriteLine(sonicReadyMsg)
        Thread.Sleep(1000)
        Console.WriteLine("Go!")
        read_line() |> ignore
        do iconsumer.close()
        do consumer.close()
        session.close()
    with
        | Failure msg -> printfn "An Exception was Caught: %s" msg
        
    exit 1

do main()
