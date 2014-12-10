import java.util.Random;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Program {
  public static void main(String args[]) {
      ExecutorService executor = Executors.newFixedThreadPool(3);
      for(int i = 1; i<= 15; i++){
        Task task = new Task("Task-" + i);
        executor.execute(task);
      }
      executor.shutdown();
  }
} 

class Task implements Runnable {
   private Thread t;
   private String name;
   
   Task( String name){
       this.name = name;
   }

   public void run() {
      try {
        System.out.println("Thread: " + name + " started ");
        Thread.sleep((new Random()).nextInt(5)*1000);
        System.out.println("Thread: " + name + " finished ");
     } catch (InterruptedException e) {
         System.out.println("Thread " +  name + " interrupted.");
     }
   }
}