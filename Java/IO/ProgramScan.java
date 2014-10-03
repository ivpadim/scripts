import java.io.*;
import java.util.Scanner;

public class ProgramScan{
	public static void main(String[] args){
		Scanner scanner = null;
		try{
			scanner = new Scanner(new BufferedReader(new FileReader("testInput.txt")));
			while (scanner.hasNext()){
				System.out.println(scanner.next());
			}
		}
		catch(Exception ex){
			System.out.println(ex);
		}
		finally{
			if(scanner!=null){
				scanner.close();
			}			
		}
	}
}
