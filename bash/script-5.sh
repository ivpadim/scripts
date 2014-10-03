#!/bin/sh

archivos=$(ls ~)

if [ $(echo $archivos | wc -l) -eq 0 ];
then
	echo "no hay archivos en el directorio."
	exit 1
fi
for archivo in $archivos
do
	if [ -f $archivo ];
	then
		lineas=$(cat $archivo | grep Linux -c)
		if [ ! $lineas -eq 0 ] ;
		then
			echo "Palabra linux encontrada en $archivo $lineas veces"	
		fi
	fi
done

