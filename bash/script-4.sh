#!/bin/sh


archivo=$1

linea=$( cat $archivo | grep Linux -n | awk '{split($1,A,":");print(A[1])}')

if [ ! -z "$linea" ];
then
	for numero in $linea
	do
		echo "Palabra Linux encontrada en $archivo en la linea $numero"
	done
fi
	

