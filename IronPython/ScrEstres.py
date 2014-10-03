import clr
clr.AddReference('Sonic.Jms')
clr.AddReference('Sonic.Jms.Cf.Impl')
from System import Console
from System import ConsoleColor
from System.Threading import Thread
import Sonic.Jms.Connection
import Sonic.Jms.Cf.Impl.ConnectionFactory
import Sonic.Jms.MessageListener
class Estres:
	factory=Sonic.Jms.Cf.Impl.ConnectionFactory()
	connection=Sonic.Jms.Connection
	def __init__(self,broker='sonicintmty.vector.com.mx',port='2759',user='MONITOR_CLIENTE',pwd='MONITOR_CLIENTE'):
		self.user=user
		self.password=pwd
		self.broker=broker+':'+port 
		self.connections=[]
		self.cuenta='266563'
		self.id='10100'
		self.clase='666'
		self.topicoInicio='IMONITOR.INICIO'
		self.topicoVariables='IMONITOR.VARIABLES'
		self.topicoRenglonColumna='IMONITOR.RENGLONCOLUMNA'
		self.factory=Sonic.Jms.Cf.Impl.ConnectionFactory(self.broker)
		
	def sendMessage(self,connection,topicName,messageBody,props):
		session=connection.createSession(False,1004)
		topic=session.createTopic(topicName)
		publisher=session.createProducer(topic)
		message=session.createTextMessage()
		message.setText(messageBody)		
		for prop in props.iteritems():
			message.setStringProperty(str(prop[0]),str(prop[1]))		
		publisher.send(message)		
		Console.WriteLine('>>> Message sent to {0}.',topicName)
		
	def createConnections(self,numConnections):
		for i in range(numConnections):
			tmpConnection=self.factory.createConnection(self.user,self.password)
			tmpConnection.start()			
			
			props={
			'CUENTA':self.cuenta,
			'ID':self.id,
			'TIPO':'INICIO',
			'ORIGEN':'0',
			'CLASE': self.clase
			}
			
			self.sendMessage(tmpConnection,self.topicoInicio,'',props)
			
			props={
			'CUENTA':self.cuenta,
			'ID':self.id,
			'ORIGEN':'0',
			'TIPO':'INICIO',
			'CLASE': self.clase
			}
			
			message='ESTATUSCLIENTE|salu2'
			
			self.sendMessage(tmpConnection,self.topicoVariables,message,props)
			
			self.connections.append(tmpConnection)
			self.sendKeepAlive()
	
	def closeConnections(self):
		for i in range(len(self.connections)):
			con=self.connections.pop()
			con.close()
			
	def sendKeepAlive(self):
		for i in range(len(self.connections)):
			tmpConnection=self.connections[i];
			props={
			'CUENTA':self.cuenta,
			'ID':self.id,
			'ORIGEN':'0',
			'TIPO':'INICIO',
			'CLASE': self.clase
			}
			message='ESTATUSCLIENTE|salu2'
			self.sendMessage(tmpConnection,self.topicoVariables,message,props)
			
	def sendColumna(self):
		for i in range(len(self.connections)):
			tmpConnection=self.connections[i]
			props={
			'CUENTA':self.cuenta,
			'ID':self.id,
			'ORIGEN':'0',
			'TIPO':'COLUMNA',
			'CLASE': self.clase
			}
			message='129|30|1'
			self.sendMessage(tmpConnection,self.topicoRenglonColumna,message,props)
		
	def sendEmisora(self):
		for i in range(len(self.connections)):
			tmpConnection=self.connections[i]
			props={
			'CUENTA':self.cuenta,
			'ID':self.id,
			'ORIGEN':'0',
			'TIPO':'RENGLON',
			'CLASE': self.clase
			}
			message='129|AXTEL|CPO|1'
			self.sendMessage(tmpConnection,self.topicoRenglonColumna,message,props)