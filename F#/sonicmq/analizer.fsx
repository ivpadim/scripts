#light

#load @"mqlib.fs"

open Scripts.SonicMq
open Scripts.SonicMq.Sql
open System
open System.Data
open System.Data.SqlClient
open System.Windows.Forms

//Create the form
let form = new Form(Text="Analizer")
let dgv = new DataGridView(Dock=DockStyle.Fill)
dgv.AllowUserToAddRows <- false
dgv.AllowUserToDeleteRows <- false
form.TopMost<-true
do form.Controls.Add(dgv)
do form.Show()

let mutable sqldap = new SqlDataAdapter()
let mutable data = new DataSet()

let getMessages(query) = 
    let sqlconmsg = new SqlConnection(sqlconstr)
    let sqlcmdmsg = new SqlCommand(query,sqlconmsg)
    sqlcmdmsg.CommandTimeout<-10000
    sqldap <- new SqlDataAdapter(sqlcmdmsg)
    let ds = new DataSet()
    sqldap.Fill(ds) |> ignore
    ds

let populateGrid(query) =    
    data <- getMessages(query)
    dgv.DataSource <- data.Tables.Item(0)
    

let acceptChanges() =
    let builder = new SqlCommandBuilder(sqldap)
    builder.GetUpdateCommand() |> ignore
    sqldap.Update(data.Tables.Item(0)) |> ignore
    data.AcceptChanges()
    
populateGrid "select top 20 * from message where destination = 'IMONITOR.VARIABLES' order by datemessage asc"
populateGrid "select distinct(destination) from message"
populateGrid "select top 1 * from message where destination = 'MONITOR.ONLINE.ACCIONES' order by datemessage asc"

acceptChanges()
