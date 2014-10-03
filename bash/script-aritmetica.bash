#!/bin/sh

echo "Ingresa el primer numero"
read numeroA
echo "Ingresa el segundo numero"
read numeroB

if [[ -z "$numeroA" ]] || [[ -z "$numeroB" ]];
then
	echo "Debes insertar numeros"
	exit 1
fi

if [[ $(echo $numeroA | grep -c [a-z]) -gt 0 ]] || [[ $(echo $numeroB | grep -c [a-z]) -gt 0 ]];
then
	echo "Debes de ingresar solo numeros"
	exit 2
fi


echo "Menu de operaciones"
echo "1) Suma"
echo "2) Resta"
echo "3) Multiplicacion"
echo "4) Division"
echo "5) Modulo"
echo "6) Salir"
echo "-------------------"
echo "Ingresa la opcion deseada"
read opcion

case "$opcion" in
	"1")
		echo "$numeroA + $numeroB"
		echo "--------------------"
		resultado=$((numeroA+numeroB));;
	"2")
		echo "$numeroA - $numeroB"
		echo "--------------------"
		resultado=$((numeroA-numeroB));;
	"3")
		echo "$numeroA * $numeroB"
		echo "--------------------"
		resultado=$((numeroA*numeroB));;
	"4")
		if [ $numeroB -eq 0 ];
		then
			echo "Division entre 0"
			exit 3
		else
			if [ $((numeroA%numeroB)) -eq 0 ];
			then	
				echo "$numeroA / $numeroB"
				echo "--------------------"			
				resultado=$((numeroA/numeroB))
			else
				echo "Division no entera"
				exit 4
			fi
		fi
		;;
	"5")
		echo "$numeroA % $numeroB"
		echo "--------------------"
		resultado=$((numeroA%numeroB));;
	"6")
		echo "Bye..."
		exit 5;;
	*)
		echo "opcion no deseada"
esac

echo "el resultado es $resultado"

