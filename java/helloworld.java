/*Print hello world*/
import java.lang.*;

class HelloWorld
{
	public static void main(String[] args)
	{
		try{throw new RuntimeException("hola");}
		catch(Exception ex){System.out.println(ex);}
		System.out.println("Hello World!");
	}
}
