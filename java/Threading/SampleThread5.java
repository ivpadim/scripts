public class SampleThread5 implements Runnable {
    // En el principio se crea cuenta con un saldo inicial
    private Cuenta cta = new Cuenta();
    // Punto de entrada principal
    public static void main (String [] args) {
        // La clase del programa compite consigo misma, utilizando dos Threads
        SampleThread5 corredor = new SampleThread5();
        // Se instancían dos Threads
        Thread uno = new Thread( corredor );
        Thread dos = new Thread( corredor);
        // Se asignan nombres a dos instancias de igual clase
        uno.setName("Juan");
        dos.setName("Pedro");
        // Se inician dos Threads
        uno.start();
        dos.start();
    }
    // Implementación de 'run' de la interfase 'Runnable'
    public void run() {
        // Intentar hace retiros
        for (int x = 0; x < 5; x++) {
            hacerRetiro(10);
            if (cta.pedirSaldo() < 0) {
                System.out.println("cuenta sobregirada!");
            }
        }
    }
    // Método para hacer retiros
    private void hacerRetiro(int cant) {
        if (cta.pedirSaldo() >= cant) {
            // Avisar de un retiro y suspender un tiempo
            System.out.println(Thread.currentThread().getName() +
            " va a hacer un retiro");
            try {
                Thread.sleep(00);
            } catch(InterruptedException ex) { }
            // Hacer el retiro
            cta.retirar(cant);
            // Avisar que tuvo éxito el retiro
            System.out.println(Thread.currentThread().getName() +
            " completo el retiro");
        } else {
            // Avisar que no hay saldo suficiente
            System.out.println("No hay saldo para que " +
            Thread.currentThread().getName() + " haga retiro, restan: " +
            cta.pedirSaldo());
        }
    }
}
    // La clase cuenta
    class Cuenta {
        // Saldo inicial
        private int saldo = 50;
        // Método para reportar el saldo
        public int pedirSaldo() {
            return saldo;
        }
        // Método para hacer retiros
        public void retirar(int cantidad) {
            saldo = saldo - cantidad;
        }
    }
