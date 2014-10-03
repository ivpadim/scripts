#light

#I @"C:\Program Files\Sonic Client\Net Assemblies\bin\release"
#r @"Sonic.Jms.dll"
#r @"Sonic.Jms.Cf.Impl.dll"

#load @"mqlib.fs"

open Scripts.SonicMq
open Scripts.SonicMq.Sql
open Scripts.SonicMq.Sample
open Sonic.Jms
open System

//Inyector de Mensajes
(*let jmsconstrvec =  {Broker="sonicintmty.vector.com.mx:2759";
                User="MONITOR_CLIENTE";
                Password="MONITOR_CLIENTE"}*)
                
let jmsconstrvec =  {Broker="localhost:2506";
                User="Administrator";
                Password=""}

let enginevec = MqSqlEngine(jmsconstrvec)

(*    ---------------------------Procesar Mensaje Inicial-----------------------     *)

let session = MqEngineGenerator.createSession(jmsconstrvec)

let producer = MqProducer(session)

let procInitialMessage(msg:TextMessage) =

    let topic = msg.getJMSDestination() :?> Topic
    let topicname = ref (topic.getTopicName())
    
    //If the message contains Router information remove it.
    let indexroute = (!topicname).IndexOf(':')    
    if indexroute <> -1 then topicname := (!topicname).Remove(0,indexroute + 2)
    let tipo = msg.getStringProperty("TIPO")
    match tipo with
        |"VALIDACION" ->
            producer.sendMessage !topicname "1" [("CUENTA",msg.getStringProperty("CUENTA"),"String");
                                                                                     ("ID",msg.getStringProperty("ID"),"String");
                                                                                     ("PORIGEN","1","String");
                                                                                     ("TIPO","VALIDACION","String");
                                                                                     ("CLASE",msg.getStringProperty("CLASE"),"String")]
        |"INICIO" ->
            producer.sendMessage !topicname getbodyInicio [("CUENTA",msg.getStringProperty("CUENTA"),"String");
                                                                                     ("ID",msg.getStringProperty("ID"),"String");
                                                                                     ("PORIGEN","1","String");
                                                                                     ("TIPO","INICIO","String");
                                                                                     ("CLASE",msg.getStringProperty("CLASE"),"String")]
        | _ ->
            failwith "No se encontro el tipo"


let myListener = MqListener(procInitialMessage)
let topic = session.createTopic("IMONITOR.INICIO")
let consumer = session.createConsumer(topic,"CUENTA = '266563' AND PORIGEN = '0' AND ID IN ('10100','10200') AND TIPO IN ('INICIO','VALIDACION')")
do consumer.setMessageListener(myListener)


(*    ---------------------------Termina Proceso Mensaje Inicial-----------------------     *)

//tel naye:80592283

(*let jmsconstrlocal = {Broker="localhost:2506";
                User="Administrator";
                Password=""}
                
let engine = MqSqlEngine(jmsconstrlocal)

do enginevec.sendsqlmessage "GetAcciones" [] 2000

let body ="10:50#INDICE|0|004488|Hora 10:50:34$FF6600|Indice 26,924.18[-419.39pts,-1.53%]$004488|Volumen 94,214,136$004488|No. de Operaciones 7,693$004488|A la alza 11$004488|A la baja 48$004488|Sin Cambio 3#OPERADAS|1|FF6600|AMX_L Var -1.76%|Operaciones 1,395|Volumen 37,396,981$FF6600|CEMEX_CPO Var -2.71%|Operaciones 567|Volumen 5,740,432$FF6600|WALMEX_3V Var -1.02%|Operaciones 512|Volumen 6,982,671$FF6600|GMEXICO_B Var -11.98%|Operaciones 505|Volumen 3,045,188$FF6600|GEO_B Var -0.67%|Operaciones 352|Volumen 2,037,070$FF6600|TELMEX_L Var -10.65%|Operaciones 335|Volumen 7,149,298$FF6600|TLEVISA_CPO Var -2.86%|Operaciones 330|Volumen 2,494,400$FF6600|GFNORTE_O Var -10.43%|Operaciones 324|Volumen 2,066,300$FF6600|GMODELO_C Var -3.48%|Operaciones 244|Volumen 1,021,776$FF6600|ICA_* Var -3.81%|Operaciones 240|Volumen 528,100#TASAS|0|666666|CETE 28  7.41$666666|CETE 91  7.57$666666|CETE182  7.71$666666|CETE336  7.80$666666|TIIE 28  7.92$666666|TIIE 91  8.00$666666|DOLAR  10.8580-11.0220$666666|UDI  3.943893"

do engine.sendsqlmessage "Ticker" ["@body",body] 0
    
    *)

do enginevec.sendsqlmessage "GetAcciones" [] 3000
