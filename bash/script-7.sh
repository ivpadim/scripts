#!/bin/sh

archivo=$1

if [ ! -r "$archivo" ];
then
	echo "El archivo no existe"
	exit 1
fi

lineas=$(wc -l $archivo | awk '{split($1,A,":");print(A[1])}')
lineas=$((lineas-1))


tail -$lineas $archivo