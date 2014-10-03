(*let form2 = new Form(Visible=true,Text="Manda Mensaje",TopMost=true)

let textTopic = new TextBox(Visible=true)
let textMessage = new TextBox(Visible=true,Multiline=true)
let buttonSend  = new Button(Visible=true,Text="Send")

let sendButtonClick _ =
    monitor.SendMessageToTopic(textMessage.Text,textTopic.Text,new Dictionary <string,string>())
    textMessage.Text <- ""
    MessageBox.Show("Mensaje Enviado") |> ignore

let _ = buttonSend.Click.Add(sendButtonClick)

textTopic.Dock <- DockStyle.Top
textMessage.Dock <- DockStyle.Fill
buttonSend.Dock <- DockStyle.Bottom
form2.Controls.Add(textTopic)
form2.Controls.Add(textMessage)
form2.Controls.Add(buttonSend)
textMessage.BringToFront()*)
#light

#I @"C:\Windows\assembly\GAC_MSIL\Vector.MonitoreoAcciones.Monitor\1.0.0.0__3e215106f14a5480"
#I @"c:\program files\Sonic Client\Net Assemblies\bin\release"
#r "Sonic.Jms.dll"
#r "Sonic.Jms.Cf.Impl.dll"
#r "Vector.MonitoreoAcciones.Monitor.dll"

open System.Windows.Forms
open Vector.MonitoreoAcciones.Monitor
open System.Collections.Generic

let form = new Form()
form.Visible <- true
form.Text <- "Monitor de Acciones Vector"

let monitor = new MonitorContenedor()
monitor.RoutingNode <- ""
monitor.Broker <- "sonicintmty.vector.com.mx"
monitor.Puerto <- "2759"
monitor.Usuario <- "MONITOR_CLIENTE"
monitor.Password <- "MONITOR_CLIENTE"
monitor.TopicoInicio <- "IMONITOR.INICIO"
monitor.Cuenta <- "266563"
monitor.ID <- "10100"
monitor.SesionGeneral <- "20243"
monitor.Sesion <- "40775"
monitor.IsOnline <- true
monitor.IP <- "192.168.8.205"
monitor.Clase <- "666"
monitor.Visible <- true
monitor.Dock <- DockStyle.Fill

form.Controls.Add(monitor)
monitor.CreateConnection()