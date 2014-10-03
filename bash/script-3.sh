#!/bin/sh

if [ $# -ne 1 ];
then 
	echo "Error se requiere parametro"
	echo "Uso: $0 <nombre de archivo>"
	exit 1
fi

archivo=$1

if [-f $archivo ];
then
	encontrado=$(cat $archivo| grep Linux)

	if [ ! -z "$encontrado" ];
	then
		echo "Palabra Linux encontrada en $archivo"
	fi
else
	echo "Archivo $archivo no se puede leer"
	exit 2
fi
	
