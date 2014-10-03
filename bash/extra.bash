 #!/bin/bash
 
 ps -fe -o "ppid,pid" | awk '{if ( $1 != "PPID" )print("Padre \t#",$1,"\t-> Hijo #",$2)}'