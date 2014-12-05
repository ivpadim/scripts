all:
	nasm -f aout function.s
	gcc -c main.c
	gcc function.o main.o -o main.exe