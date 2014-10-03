import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

public class ProgramByteStream{
	public static void main(String[] args){
		FileInputStream in = null;
		FileOutputStream out = null;
		try{
			in = new FileInputStream("testInput.txt");
			out = new FileOutputStream("testOutput.txt");
			int c;

			while((c = in.read()) != -1){
				out.write(c);			
			}
		}
		catch(IOException ex)
		{
			System.out.println(ex);
		}
		finally{
			try{
			if(in != null){
				in.close();
			}
			if(out != null){
				out.close();
			}
			}catch(IOException ex){System.out.println(ex);}

		}
	}
}
