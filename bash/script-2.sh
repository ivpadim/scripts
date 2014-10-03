#!/bin/sh

for archivo in $(ls)
do
	if [ -f $archivo ];
	then
                palabras=$(wc -w $archivo | awk '{print($1)}')
		echo "$archivo: $palabras palabras"
	fi
done
