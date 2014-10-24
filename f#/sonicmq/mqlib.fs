#light

module Scripts.SonicMq

(*Bring the SonicMQ assemblies to connect with the Message Server*)
#I @"C:\Program Files\Sonic Client\Net Assemblies\bin\release"
#r @"Sonic.Jms.dll"
#r @"Sonic.Jms.Cf.Impl.dll"


open Sonic.Jms
open System
open System.Threading
        
(*record of sonic mq connection data.*)
type MqConnectionString  = {Broker:string;User:string;Password:string}

let read_password() =
    let mutable password=""
    let mutable info = Console.ReadKey(true)
    while info.Key <> ConsoleKey.Enter do 
        if info.Key  <> ConsoleKey.Backspace then            
            password<- password + info.KeyChar.ToString()            
            info <- Console.ReadKey(true)
        elif info.Key = ConsoleKey.Backspace then
            if not (string.IsNullOrEmpty(password)) then
                password<- password.Substring(0,password.Length-1)                
            info <- Console.ReadKey(true)
    [1..password.Length] |> List.iter(fun _ -> printf "*")
    print_newline()
    password

type MqEngineGenerator() =
    with 
        static member createConnectionString() =
            (*{Broker="localhost:2506";
            User="Administrator";
            Password=""}*)
            printf "Broker: "    
            let broker = ref (read_line())
            printf "User: "
            let user = ref (read_line())
            printf "Password: "
            let password = ref (read_password())

            if !broker = "" || !user ="" || !password="" then
                let jmsconstrdefault = {Broker="www.vector.com.mx:2759";User="MONITOR_CLIENTE";Password="MONITOR_CLIENTE"}
                if !broker = "" then
                    broker := jmsconstrdefault.Broker
                if !user = "" then
                    user:= jmsconstrdefault.User
                    password:= jmsconstrdefault.Password

            {Broker=(!broker);User=(!user);Password=(!password)}
            
        static member createSession(currentJMSconstr) =
            try
                let factory = Sonic.Jms.Cf.Impl.ConnectionFactory(currentJMSconstr.Broker)    
                let connection = factory.createConnection(currentJMSconstr.User,currentJMSconstr.Password)
                do connection.start()
                connection.createSession(false,1004)
            with
                | :? JMSException as ex -> 
                    failwith (string.Format("Connection Error  {0}",ex.Message))
                | :? JMSSecurityException as ex ->
                    failwith (string.Format("Security Error {0}", ex.Message))
                | _ -> failwith "An unknown error was ocurred."

///JMS Listener get the message and call the process method.
type MqListener(ptrProcessMsg:TextMessage->unit) =
    let processMsg = ptrProcessMsg
    interface Sonic.Jms.MessageListener with
        ///Here is where the messages comes in.
        member this.onMessage(incomingMsg:Message) =
            //Only support for text message
            if (incomingMsg :? TextMessage) then
                let msg = incomingMsg :?> TextMessage                
                processMsg msg

//Recursive function that add all the properties on the list to the text message.
let rec addProps (properties:(string*string*string) list) (message:TextMessage) =
    match properties with
        |head::tail ->
            let (name,value,valtype) = head
            match valtype with
                | "String" -> message.setStringProperty(name,value)
                | "Integer" -> message.setIntProperty(name,(Int32.Parse(value)))
                | "Float" ->  message.setFloatProperty(name,(Float32.of_string(value)))                    
                | "Boolean" -> message.setBooleanProperty(name,bool.Parse(value))
                | _ -> message.setObjectProperty(name,value)
            ignore(addProps tail message)
        |[] -> ()

///JMS Producer, sends messages to a specific topic.
type MqProducer(arg:obj)=        
    let findsession = 
        match arg with
            | :? Session -> arg :?> Session
            | :? MqConnectionString -> MqEngineGenerator.createSession((arg:?>MqConnectionString))
            | _ -> null
    let session = findsession    
    with 
        ///Function that process and send the message.
        member this.sendMessage topicname body props =
            try
                let topic = session.createTopic(topicname)
                let producer = session.createProducer(topic)
                let message = session.createTextMessage(body)
                addProps props message
                producer.send(message)
            with
                | :? JMSException as ex -> 
                    failwith (string.Format("Connection Error {0}", ex.Message))
                | :? JMSSecurityException as ex ->
                    failwith (string.Format("Security Error {0}", ex.Message))
                | _ -> failwith "An unknown error was ocurred."

///Sample function that process a text message, this function can be used in a MqListener instance.
///Only print relevant information about the message.
let procMsg (msg:TextMessage) =

    //Get the basic data of the text message.
    let msgId = msg.getJMSMessageID()
    let topic = msg.getJMSDestination() :?> Topic
    let topicname = ref (topic.getTopicName())
    
    //If the message contains Router information remove it.
    let indexroute = (!topicname).IndexOf(':')    
    if indexroute <> -1 then topicname := (!topicname).Remove(0,indexroute + 2)
    
    //Get the content of the message and give the corresponsive format to display on console.
    let body = ref (msg.getText())    
    if (!body).Length>80 then
        body :=  (!body).Substring(0,80) + "..."                
     
     //Print a content resume of the message received.
    do printfn "from %s at %s" !topicname (DateTime.Now.ToString())   
    do printfn "%s" !body
    
    //Get the properties of the message.
    let props = msg.getPropertyNames()
    do props.Reset()
    //Print all the properties of the message.
    while props.MoveNext() do 
        let propName = (props.Current :?> string)
        let value = msg.getObjectProperty(propName)                        
        print_string (string.Format("  -{0,-16}->  ",propName))        
        print_any value        
        printf "\n"
        
    printfn "---------------------------------------------------------------------------------"
    
    
module Sql =
    
    open System.Data
    open System.Data.SqlClient
    
    let sqlconstr = "Data Source=.\SQL2005;Initial Catalog=SonicMQLog;Integrated Security=SSPI;Persist Security Info=true;"
    
    let getbodyInicio =
        use sqlconn = new SqlConnection(sqlconstr)
        sqlconn.Open()
        use sqlcmd = new SqlCommand("select body from message where MessageID = '4BB6C16C-8262-4E56-9431-DAF5E6CB4AB2'",sqlconn)
        sqlcmd.ExecuteScalar() :?> string
        
    let getProps msgid =         
        use sqlconprops = new SqlConnection(sqlconstr)            
        sqlconprops.Open()            
        let mutable props:(string*string*string) list = []            
        use sqlcmdprops = new SqlCommand("GetPropertiesByMessageID",sqlconprops)
        sqlcmdprops.CommandTimeout<-10000
        sqlcmdprops.Parameters.AddWithValue("@MessageID",msgid) |> ignore
        sqlcmdprops.CommandType <- CommandType.StoredProcedure            
        use drprops = sqlcmdprops.ExecuteReader()            
        while drprops.Read() do
            let name = drprops.Item "Name" :?> string     
            let value = drprops.Item "Value" :?> string
            let valtype = drprops.Item "Type" :?> string
            props <- (name,value,valtype)::props
        props

    let rec addSqlParams (sqlparams) (sqlcmd:SqlCommand)=
            match sqlparams with
                | head::tail -> 
                    let (name,value) = head
                    sqlcmd.Parameters.AddWithValue(name,value) |> ignore
                    addSqlParams tail sqlcmd
                |[] -> ()

    type MqSqlEngine (connectionstring)=
        let constring = connectionstring
        let session = MqEngineGenerator.createSession(constring)
        let producer = MqProducer(session)

        let getAndSendMessages sqlproc sqlparameters interval=
            use sqlconmsg = new SqlConnection(sqlconstr)
            sqlconmsg.Open()
            let mutable time = new DateTime()   
            use sqlcmd = new SqlCommand(sqlproc,sqlconmsg)
            sqlcmd.CommandTimeout<-10000
            addSqlParams sqlparameters |> ignore
            sqlcmd.CommandType <- CommandType.StoredProcedure
            use dr = sqlcmd.ExecuteReader()
            while dr.Read() do            
                let destination = dr.Item "Destination" :?> string
                let body = dr.Item "Body" :?> string
                let msgid = (dr.Item "MessageID").ToString()
                if interval <> 0 then Thread.Sleep(interval)                
                elif time <> new DateTime() then
                    let timespan = (dr.Item "DateMessage" :?> DateTime).Subtract(time)
                    if timespan.Seconds < 10 then
                        Thread.Sleep(timespan)
                    else
                        Thread.Sleep(10000)
                time <- (dr.Item "DateMessage" :?> DateTime)
                producer.sendMessage destination body (("Fecha",time.ToString(),"String")::(getProps msgid))
                printfn "message received on %s sent to %s at %s" (time.ToString()) destination (DateTime.Now.ToString())
               
        with
            member this.sendacciones interval = 
                getAndSendMessages "GetAcciones" [] interval
            
            member this.sendmessages (count:int) interval =
                if count <> 0 then
                    getAndSendMessages "GetMessages" [("@count",count)] interval
                else
                    getAndSendMessages "GetMessages" [] interval

            member this.sendreconecta()  =
                getAndSendMessages "GetReconecta" [] 0
            
            member this.sendsqlmessage sqlproc props interval =
                getAndSendMessages sqlproc props interval
                                
module Sample =

    let runSampleListener() =
        let session = MqEngineGenerator.createSession(MqEngineGenerator.createConnectionString())
        let topic = session.createTopic("IMONITOR.#")
        let topic2 = session.createTopic("MONITOR.#")    
        let consumer = session.createConsumer(topic)
        let consumer2 = session.createConsumer(topic2)
        let listener = new MqListener(procMsg)
        do consumer.setMessageListener(listener)
        do consumer2.setMessageListener(listener)    
        do printfn "Listener Ready!!!"
        ignore(Console.ReadLine())
        do consumer.close()
        do consumer2.close()
        session.close()