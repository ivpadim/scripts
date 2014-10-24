import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class ProgramCharStream{
	public static void main(String[] args){
		FileReader reader = null;
		FileWriter writer = null;
		try{
			reader = new FileReader("testInput.txt");
			writer = new FileWriter("testWriterOutput.txt");

			int c;
			while((c = reader.read()) != -1){
				writer.write(c);
			}
		}catch(IOException ex){System.out.println(ex);}
		finally{
			try{
			if(reader!=null){
				reader.close();
			}
			if(writer!=null){
				writer.close();
			}
			}catch(IOException ex){System.out.println(ex);}
		}
	}
}
