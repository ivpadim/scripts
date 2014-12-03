public class SampleThread1 {

    public static void main(String[] args) {
        /* Crear y nombrar cada Thread */
        PrintThread thread1 = new PrintThread("thread1");
        PrintThread thread2 = new PrintThread("thread2");
        PrintThread thread3 = new PrintThread("thread3");
        System.err.println("Iniciando threads");
        thread1.start(); // Iniciar thread1 y ponerlo en estado 'ready'
        thread2.start(); // Iniciar thread1 y ponerlo en estado 'ready'
        thread3.start(); // Iniciar thread1 y ponerlo en estado 'ready'
        System.err.println("Threads iniciados, el Thread principal 'main' termina\n");
    }
}
/* La clase PrintThread controla la ejecución  de cada thread */

class PrintThread extends Thread {

    private int tiempoPausa;
    /* Asignar nombre en el constructor llamando al constructor de Thread */

    public PrintThread(String name) {
        super(name);
        /* Generar un lapso de inactividad a utilizar con esta instancia de Thread */
        tiempoPausa = (int) (Math.random() * 5001); // Entre 0 y 5 segundos
    }
    /* Este método será ejecutado al activar el thread con 'start' */

    public void run() { // JVM decidirá cuando iniciarlo
        try {
            System.err.println(
                    getName() + " yendo a dormir durante " + tiempoPausa + " milisegundos");
            /* Solicitar en su 'stack' que este thread se inactive por un tiempo */
            //Thread.sleep(tiempoPausa);
            Thread.sleep(0);
        } /* Si el thread es interrumpido durante su sueño, mostrar su 'stack' */ catch (InterruptedException exception) {
            exception.printStackTrace();
        }
        /* Mostrar que el thread despertó momentaneamente */
        System.err.println(getName() + " ya despertó");
    }
}
