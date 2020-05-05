#include <stdio.h>
#include "add.h"

int main()
{
    // print 1~10
    int i = 0, n = 10;
    for (i = 0; i < n; i++)  
    {
        printf("%d\n", adder(i, 1));
    }
    
    return 0;
}