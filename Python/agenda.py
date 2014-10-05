#!/usr/bin/env python

#Autor: Ivan Padron Dimas <ivpadim@gmail.com>
#Matricula: 678419
#Examen 2do Parcial
#===================================================================
#Diseniar y ejecutar un programa tipo agenda, que contenga el nombre,
#telefono y direccion de un grupo de 7 personas.
#La llave principal es la del telefono y sus valores deberan ser el
#nombre completo de la persona y su direccion.
#Este programa se crea como un "diccionario".
#===================================================================

agenda = {
			'4180000001':('Anna Pavlovna','San Petersburgo, Rusia'),
			'4180000002':('Andrey Bolkonski','Moscu, Rusia'),
			'4180000003':('Elena Kuragina','Kazan, Rusia'),
			'4180000004':('Ivan Padron','Omsk, Rusia'),
			'4180000005':('Maria Volkonskaya','Saratov, Rusia'),
			'4180000006':('Natasha Rostova','Krasnodar, Rusia'),
			'4180000007':('Pierre Bezukov','Yaroslavl, Rusia'),
		 }


def menu():
	opcion = 0
	while opcion != '3':
		print '======================================'
		print 'Opciones:'
		print '1.- Busqueda'
		print '2.- Imprimir Agenda'
		print '3.- Salir'
		opcion = raw_input('Selecciona la opcion deseada (1,2,3): ')
		if opcion == '1':
			busqueda()
		if opcion == '2':
			imprime_agenda()
		else:
			pass

def busqueda():
	print ''
	print ''
	print ''
	telefono = '4180000001'
	while telefono != '':
		telefono = raw_input('Ingresa el telefono de la persona a buscar: ')
		if agenda.has_key(telefono):
			nombre, direccion = agenda[telefono]
			print 'El telefono %s corresponde a %s con direccion en %s' % ( telefono, nombre, direccion)
		elif telefono != '':
			print 'No se encontro ninguna persona con ese telefono.'
			print 'Intentente nuevamente.'
	raw_input('Presiona {Enter} para continuar')

def imprime_agenda():
	print ''
	print ''
	print ''
 	for telefono in agenda.keys():
 		nombre, direccion = agenda[telefono]
		print 'Telefono: %s, Nombre: %s, Direccion: %s' % (telefono, nombre, direccion)
	raw_input('Presiona {Enter} para continuar')


print 'Bienvenido (Examen 2do Parcial)'
menu()
print 'Hasta la proxima!!!'
