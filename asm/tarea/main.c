#include <stdio.h> 

int add(int,int) ; 

int main() 
{ 
    int x; 
    x = add(2,2); // should be 4... 
    printf("2+2=%d\n",x); 
    return 0; 
} 
