all:
	nasm -f elf32 function.s 
	gcc -c main.c -m32
	gcc function.o main.o -o main.exe -m32