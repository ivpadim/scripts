#!/bin/sh

if [ $# -lt 3 ];
then 
	echo "Faltan arguments"
	echo "Uso: $0 directorio1 directorio2 cadena"
	exit 1
fi

directorio1=$1
directorio2=$2
cadena=$3

if [[ ! -d $directorio1 || ! -w $directorio1 ]];
then
	echo "$directorio1 no existe o no es accesible"
	exit 2
fi

if [ ! -d $directorio2 ];
then
	echo "$directorio2 no existe se creara"	
	mkdir $directorio2	2>/dev/null
	if [ $? -ne 0 ];
	then
		echo "Error: $directorio2 no pudo ser creado"
		exit 6
	fi	
else
	if [ ! -w $directorio2 ];
	then
		echo "Error no se tiene premisos de escritura en $directorio2"
		exit 4
	fi
fi

for archivo in $(ls $directorio1/*$cadena*)
do
	mv $archivo $directorio2
done