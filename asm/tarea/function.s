GLOBAL add 

SECTION .text 
add: 
    push ebp 
    mov ebp,esp 
    mov eax,[ebp+8] 
    mov ecx,[ebp+12]         
    add eax,ecx 
    pop ebp 
    ret 