#Autor: Ivan Padron Dimas <ivpadim@gmail.com>
#===========================================
#This little script allows to receive commands
#over bluetooth.
#xmodmap -pke
#===========================================

from bluetooth import *
import commands

uuid = '2ae00fa3-8a0a-4d58-b926-8a427eac9de9'
service_name = 'BlueRemote Control'

#Just print the title!! not so much logic inside this
def print_title():
    print '============================================'
    print '             BlueRemote Control             '
    print 'Command Receiver App. Ivan ivpadim@gmail.com'
    print '============================================'
    print ''

def send_key(key):
    print '---> Command received.. sending key %s' % key
    commands.getoutput('xdotool key ' + key)

#Searching and connecting to the service
def wait_command():    
    print 'Creating service'
    
    server = BluetoothSocket(RFCOMM)
    
    server.bind(("",PORT_ANY))
    server.listen(1)
    
    port = server.getsockname()[1]
    
    advertise_service( server, service_name, service_id = uuid, service_classes = [ uuid, SERIAL_PORT_CLASS ], 
    					profiles = [ SERIAL_PORT_PROFILE ] )
    
    print 'Service %s created at port %d' % (service_name, port)
    print 'Waiting for connection...'
    
    try:
		client_sock, client_info = server.accept()
		print 'Accepted connection from ', client_info
		
		while True:
		    received_command = client_sock.recv(1024)
		    data = received_command.split(':')
		    if data[0] == 'key':
		        send_key(data[1])
    except:
        pass
	server.close()

#The check is not really necessary, but is there :)
if __name__ == '__main__':
   	print_title()
   	while True:
		wait_command()		