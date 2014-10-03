#!/bin/sh

if [ $# -lt 3 ];
then 
	echo "Faltan arguments"
	echo "Uso: $0 directorio extension1 extension2"
	exit 1
fi

directorio=$1
extencion1=$2
extencion2=$3

if [[ ! -d $directorio || ! -w $directorio ]];
then
	echo "$directorio no existe o no es accesible"
	exit 2
fi

if [ $(ls -l *.$extencion1 2>/dev/null| wc -l) -eq 0 ];
then
	echo "No hay archivos que mover"
	exit 3
fi


cd $directorio


for archivo in $(ls *.$extencion1);
do
	nombre=$(echo $archivo |cut $archivo -d. -f1)
	if [ -w $arhivo ];
	then	
		mv $archivo "$nombre"."$extencion2" 2>/dev/null
	else
		echo "Archivo $archivo no puede modificarse
	fi
done
