SECTION		.text 
GLOBAL		sum

sum:
	push 	ebp
	mov 	ebp,esp
	mov 	eax,[ebp+8]
	mov 	ebx,[ebp+12]
	mov 	ecx,[ebp+16]
	mov 	edx,[ebp+20]
	add 	eax,ebx
	add 	eax,ecx
	add 	eax,edx
	pop 	ebp
    ret
    int 	0x80