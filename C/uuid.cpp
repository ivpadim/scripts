#include <CoreFoundation/CoreFoundation.h>

int main()
{
    CFUUIDRef   uuid;
    CFStringRef string;
 
    uuid = CFUUIDCreate( NULL );
    string = CFUUIDCreateString( NULL, uuid );
 
    CFShow( string );
}
