import clr
clr.AddReference('Sonic.Jms')
clr.AddReference('Sonic.Jms.Cf.Impl')
from System import Console
from System import ConsoleColor
from System.Threading import Thread
import Sonic.Jms.Connection                                                                          
import Sonic.Jms.Cf.Impl.ConnectionFactory
import Sonic.Jms.MessageListener
class Engine:
	factory=Sonic.Jms.Cf.Impl.ConnectionFactory()
	connection=Sonic.Jms.Connection
	def __init__(self,broker='sonicintmty.vector.com.mx',port='2759',user='MONITOR_CLIENTE',pwd='MONITOR_CLIENTE',cuenta='266563',id='10100',clase='666'):		
		self.user=user
		self.password=pwd
		self.broker=broker+':'+port 
		self.connections=[]
		self.colorfondo=1
		self.interval=2000
		self.topicoinicio='IMONITOR.INICIO'
		self.topicoacciones='MONITOR.ONLINE.ACCIONES'
		self.topicotickers='IMONITOR.ONLINE.TICKERS'
		self.topicovariables='IMONITOR.VARIABLES'
		self.topicoposicion='IMONITOR.POSICION'
		self.cuenta=cuenta
		self.id=id
		self.clase=clase
		self.properties={
		'CUENTA':cuenta,
		'ID':id,
		'PORIGEN':'1',
		'TIPO':'INICIO',
		'CLASE':clase
		}				
		self.start()
		
	def start(self):
		self.factory=Sonic.Jms.Cf.Impl.ConnectionFactory(self.broker)
		self.connection=self.factory.createConnection(self.user,self.password)
		self.connection.start()
		Console.ForegroundColor=ConsoleColor.Green
		Console.WriteLine('>>> Connection open!!!')
		Console.ResetColor()
		
	def stop(self):
		self.connection.close()
		Console.ForegroundColor=ConsoleColor.Red
		Console.WriteLine('>>> Connection close!!!')
		Console.ResetColor()
		
	def sendMessage(self,topicName,messageBody,props):
		session=self.connection.createSession(False,1004)
		topic=session.createTopic(topicName)
		publisher=session.createProducer(topic)
		message=session.createTextMessage()
		message.setText(messageBody)
		
		for prop in props.iteritems():
			message.setStringProperty(str(prop[0]),str(prop[1]))
		
		publisher.send(message)		
		Console.WriteLine('>>> Message sent to {0}.',topicName)
		Console.ResetColor()
		
	def acciones(self,len):
		for i in range(len):			
			Console.WriteLine('>>> Reading Mensajes\Acciones' + (i+1).ToString() + '.txt')			
			message=open(r'Mensajes\Acciones' + (i+1).ToString() + '.txt').read()
			
			#print '>>> ' + message
			
			self.sendMessage(self.topicoacciones,message,self.properties)
			Thread.Sleep(self.interval)
		
	def Color(self):				
		self.colorfondo=(self.colorfondo%10) +1
		if(self.colorfondo==1): return ConsoleColor.Cyan
		if(self.colorfondo==2): return ConsoleColor.Magenta
		if(self.colorfondo==3): return ConsoleColor.Red
		if(self.colorfondo==4): return ConsoleColor.White
		if(self.colorfondo==5): return ConsoleColor.Yellow
		if(self.colorfondo==6): return ConsoleColor.DarkGreen
		if(self.colorfondo==7): return ConsoleColor.DarkBlue
		if(self.colorfondo==8): return ConsoleColor.DarkCyan
		if(self.colorfondo==9): return ConsoleColor.DarkYellow
		return ConsoleColor.Green		
		
	def inicio(self):
		Console.WriteLine('Reading Mensajes\Inicio.txt')
		message=open(r'Mensajes\Inicio.txt').read()		
		self.sendMessage(self.topicoinicio,message,self.properties)
		
	def tickers(self,len):		
		for i in range(len):			
			Console.WriteLine('>>> Reading Mensajes\Tickers' + (i+1).ToString() + '.txt')			
			message=open(r'Mensajes\Tickers' + (i+1).ToString() + '.txt').read()						
			self.sendMessage(self.topicotickers,message,self.properties)
			Thread.Sleep(self.interval)
			
	def opens(self,Len):
		for i in range(Len):
			tmpconnection=self.factory.createConnection(self.user,self.password)
			tmpconnection.start()
			self.connections.append(tmpconnection)
			
	def closes(self):
		for i in range(len(self.connections)):
			con=self.connections.pop()
			con.close()
			
	def posicion(self,Len):
		for i in range(Len):
			Console.ForegroundColor=self.Color()
			Console.WriteLine('>>> Reading Mensajes\posicion' + (i+1).ToString() + '.txt')			
			message=open(r'Mensajes\Posicion' + (i+1).ToString() + '.txt').read()			
			Console.ResetColor()						
			self.sendMessage(self.topicoposicion,message,self.properties)
			Thread.Sleep(self.interval)
			
	def emisora(self,id):
		Console.WriteLine('>>> Reading Mensajes\posicion' + (id).ToString() + '.txt')
		message=open(r'Mensajes\Posicion' + (id).ToString() + '.txt').read()
		self.sendMessage(self.topicoposicion,message,self.properties)
		Console.WriteLine('>>> Reading Mensajes\Acciones' + (id).ToString() + '.txt')			
		message=open(r'Mensajes\Acciones' + (id).ToString() + '.txt').read()
		self.sendMessage(self.topicoacciones,message,self.properties)
				
	def Reconecta(self,op):
		props={
		'CUENTA':self.cuenta,
		'ID':self.id,
		'PORIGEN':'1',
		'TIPO':'ACTUALIZA',
		'CLASE': self.clase
		}
		message=open(r'Mensajes\Reconecta'+op.ToString()+'.txt').read()
		self.sendMessage(self.topicovariables,message,props)
		
	def valida(self,validado):
		props={
		'CUENTA':self.cuenta,
		'ID':self.id,
		'PORIGEN':'1',
		'TIPO':'VALIDACION',
		'CLASE': self.clase
		}
		self.sendMessage(self.topicoinicio,validado,props)
		
class Listener(Sonic.Jms.MessageListener):	
	def __init__(self):
		self.message=None
		self.count=0
		self.messages=[]
	def onMessage(self,message):		
		self.count+=1
		self.message=message
		self.messages.append(message)
		print '>>>index: ' + self.count +' Origen:' + message.getStringProperty("PORIGEN") +' Cuenta:'+ message.getStringProperty("CUENTA") + ' Id:' + message.getStringProperty("ID") + ' Tipo:' + message.getStringProperty("TIPO")	
		