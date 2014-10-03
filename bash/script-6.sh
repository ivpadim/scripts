#!/bin/sh

if [ $# -eq 0 ];
then 
	echo "Error se requiere parametro"
	echo "Uso: $0 <usuario>"
	exit 1
fi

if [ -z "$(grep ^$1: /etc/passwd)" ];
then
	echo "Error el usuario no existe"
	exit 2
fi

ps -u $1
