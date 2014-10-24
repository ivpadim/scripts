import java.io.FileReader;
import java.io.FileWriter;
import java.io.BufferedReader;
import java.io.PrintWriter;
import java.io.IOException;

public class CopyLines{
	public static void main(String[] args){
		BufferedReader reader = null;
		PrintWriter writer = null;
		try{
			reader = new BufferedReader(new FileReader("testInput.txt"));
			writer = new PrintWriter(new FileWriter("testPrintOutput"));
			String line;
			while((line=reader.readLine())!=null){
				writer.println(line);
			}
		}
		catch(IOException ex){
			System.out.println(ex);
		}
		finally{
			try{
				if(reader!=null){
					reader.close();
				}
				if(writer!=null){
					writer.close();
				}
			}catch(IOException ex){
				System.out.println(ex);
			}

		}
	}
}
