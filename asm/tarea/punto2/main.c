#include <stdio.h> 

int sum(int,int,int,int) ; 

int main() 
{ 
	int eax = 2;
	int ebx = 4;
	int ecx = 6;
	int edx = 8;
    int res = sum(eax,ebx,ecx,edx); 
    printf("%d + %d + %d + %d = %d\n",eax,ebx,ecx,edx,res); 
    return 0; 
} 
