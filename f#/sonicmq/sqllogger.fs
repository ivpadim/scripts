#light

module Scripts.SonicMq.Sql.Logging

//Bring the SonicMQ assemblies to connect with the Message Server.
#I @"C:\Program Files\Sonic Client\Net Assemblies\bin\release"
#r "Sonic.Jms.dll"
#r "Sonic.Jms.Cf.Impl.dll"
#r "System.Configuration.dll"

#load @"mqlib.fs"

open Sonic.Jms
open System
open System.Configuration
open System.Data
open System.Data.SqlClient
open System.Threading
open Scripts.SonicMq


let sqlReadyMsg = "Sql Connection Ready..."
let sonicReadyMsg = "SonicMQ Listener ready..."

//create the sql database connection.
let sqlcon = new SqlConnection()

let connectToSql() =
    try
        let fileMap =new ExeConfigurationFileMap()
        fileMap.ExeConfigFilename <- "app.config"
        let config = ConfigurationManager.OpenMappedExeConfiguration(fileMap,ConfigurationUserLevel.None)
        sqlcon.ConnectionString <- config.ConnectionStrings.ConnectionStrings.Item("SonicMqLogConnectionString").ConnectionString
        sqlcon.Open()    
        Console.Write(sqlReadyMsg)
    with
        | :? NullReferenceException as ex -> printfn "Cannot find the configuration file: %s" ex.Message; exit 1        
        | :? IndexOutOfRangeException as ex-> printfn "An Exception ocurred, while reading the config file: %s" ex.Message; exit 1
        | :? ConfigurationErrorsException as ex -> printfn "An Exception ocurred, while reading the config file: %s" ex.Message; exit 1        
        | :? SqlException as ex -> printfn "An Exception ocurred while trying to connet to the database: %s" ex.Message; exit 1        
        | _ -> printfn "An Error ocurred"; exit 1        
        

(*process the message and insert the context into a sql database.*)
let sqlLoggerProcMsg (msg:Sonic.Jms.TextMessage) =            
    (*Insert the Message*)
    let guid = Guid.NewGuid().ToString()
    let body = msg.getText()
    let destination = msg.getJMSDestination() :?> Sonic.Jms.Topic
    let topicname = ref (destination.getTopicName())
    let indexroute = (!topicname).IndexOf(':')
    if indexroute <> -1 then topicname := (!topicname).Remove(0,indexroute + 2)
    let jmsMsgid = msg.getJMSMessageID()    
    let date = DateTime.Now.ToString()        
    do printfn "message %s from %s at %s" jmsMsgid !topicname date    
    use sqlcmdInsertMessage = new SqlCommand("InsertMessage",sqlcon)
    do sqlcmdInsertMessage.Parameters.AddWithValue("@MessageID",guid) |> ignore
    do sqlcmdInsertMessage.Parameters.AddWithValue("@Destination",!topicname) |> ignore
    do sqlcmdInsertMessage.Parameters.AddWithValue("@JMSMessageID",jmsMsgid) |> ignore
    do sqlcmdInsertMessage.Parameters.AddWithValue("@Body",body) |> ignore
    sqlcmdInsertMessage.CommandType <- CommandType.StoredProcedure
    sqlcmdInsertMessage.ExecuteNonQuery()|>ignore
    (*Insert the Properties*)      
    let props = msg.getPropertyNames()
    props.Reset()
    while props.MoveNext() do 
        let propName = (props.Current :?> string)
        let sqlcmd = new SqlCommand("InsertPropertyMessage",sqlcon)
        do sqlcmd.Parameters.AddWithValue("@MessageID",guid) |> ignore
        do sqlcmd.Parameters.AddWithValue("@Name",propName) |> ignore
        do sqlcmd.Parameters.AddWithValue("@Value",msg.getObjectProperty(propName)) |> ignore
        (*do sqlcmd.Parameters.AddWithValue("@Value",msg.ge*)
        match msg.getObjectProperty(propName) with
            | :? string as x -> sqlcmd.Parameters.AddWithValue("@Type","String") |> ignore
            | :? bool  as x ->  sqlcmd.Parameters.AddWithValue("@Type","Boolean") |> ignore
            | :? float32 as x ->  sqlcmd.Parameters.AddWithValue("@Type","Float") |> ignore
            | :? int as x ->  sqlcmd.Parameters.AddWithValue("@Type","Integer") |> ignore
            | _ ->  sqlcmd.Parameters.AddWithValue("@Type","Object") |> ignore
        sqlcmd.CommandType <- CommandType.StoredProcedure
        sqlcmd.ExecuteNonQuery() |> ignore