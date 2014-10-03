#light

#load @"mqlib.fs"

open Scripts.SonicMq
open Scripts.SonicMq.Sample

//Create the connection to vector
let broker = "www.vector.com.mx:2759"
let up = "MONITOR_CLIENTE"
let  constr = {Broker=broker;User=up;Password=up}
let producer = MqProducer(constr)

//Send MiAsesor Login
let topicmensajes = "IMONITOR.ONLINE.MENSAJES" 
let bodyLogin= "266563|10100|ivan"
let propsLogin = [("CUENTA","266563","String");("ID","10100","String");("PORIGEN","1","String");("TIPO","MIASESOR","String");("ACCION","LOGIN","String")]
do producer.sendMessage topicmensajes bodyLogin propsLogin

//Send Alert
let topicvariables = "IMONITOR.VARIABLES"
let bodyAlert = "ALERTA|ESTO ES UNA ALERTA"
let propsAlert = [("CUENTA","266563","String");("ID","10200","String");("PORIGEN","1","String");("TIPO","ACTUALIZA","String");("CLASE","*","String")]
do producer.sendMessage topicvariables bodyAlert propsAlert