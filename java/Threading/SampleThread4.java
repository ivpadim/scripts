public class SampleThread4 {

    public static void main(String[] args) {
        /* Objeto compartido por los threads productor y consumidor */
        EspacioSincronizado objCompartido = new EspacioSincronizado();
        /* Mostrar títulos de las columnas */
        StringBuffer titulosCols = new StringBuffer(40);
        titulosCols.append("Operacion\t\tVar. Compartida\t\tHay Dato");
        System.out.println(titulosCols);
        System.out.println();
        objCompartido.muestraEstado("Estado Inicial\t");
        /* Crear el Productor y el Consumidor */
        Productor elProductor = new Productor(objCompartido);
        Consumidor elConsumidor = new Consumidor(objCompartido);
        elProductor.start();    // Iniciar thread productor
        elConsumidor.start();   // Iniciar thread consumidor
    }
}
/* Definición de lo que debe incluir un Espacio */

interface Espacio {

    public void setVal(int valor);  // Guardar un valor en Espacio

    public int getVal();              // Obtener un valor de Espacio
}
/* Clase implementadora de Espacio */

class EspacioSincronizado implements Espacio {

    private int varCompartida = -1;         // Compartida por los threads productor y consumidor
    private boolean valEnEspera = false;    // indica que hay un nuevo valor en espera
    /* Asignar un nuevo valor a varCompartida en forma sincronizada */

    public synchronized void setVal(int valor) {
        /* Obtener el nombre del thread que invocó al método */
        String nombre = Thread.currentThread().getName();
        while (valEnEspera) { // Mientras haya otro valor en espera de ser leido
            try { // Mostrar estado y esperar
                System.out.println(nombre + " intentando escribir. Aún no han leido");
                muestraEstado(nombre + " espera.");
                wait(); // Poner en espera al thread que mandó el nuevo valor
            } catch (InterruptedException excepcion) {
                excepcion.printStackTrace();
            }
        }
        varCompartida = valor; // asignar un nuevo valor
        valEnEspera = !valEnEspera; // Señalar que hay un nuevo valor en espera
        muestraEstado(nombre + " escribiendo ");
        notify(); // Avisar a otros threads que hay un nuevo valor en espera
    }
    /* Leer un nuevo valor de varCompartida en forma sincronizada */

    public synchronized int getVal() {
        /* Obtener el nombre del thread que invocó al método */
        String nombre = Thread.currentThread().getName();
        while (!valEnEspera) { // Mientras no haya un valor esperando ser leido
            try { // Mostrar estado y esperar
                System.out.println(nombre + " intentando leer. Aún no escriben");
                muestraEstado(nombre + " espera.");
                wait(); // Poner en espera al thread que solicita un nuevo valor
            } catch (InterruptedException excepcion) {
                excepcion.printStackTrace();
            }
        }
        valEnEspera = !valEnEspera; // Indicar que ya fué leido el valor en espera
        muestraEstado(nombre + " leyendo ");
        notify(); // Avisar a otros threads que el valor ya fué leido
        return varCompartida;
    }
    /* Mostrar el estado actual de la variable compartida */

    public void muestraEstado(String operacion) {
        StringBuffer lineaSalida = new StringBuffer(40);
        lineaSalida.append(operacion + "\t\t" + varCompartida + "\t\t" + valEnEspera);
        System.out.println(lineaSalida);
        System.out.println();
    }
}
/* Clase para objetos productores */

class Productor extends Thread {

    private Espacio objCompartido; // Referencia a un objeto compartido
    /* constructor (overload) */

    public Productor(Espacio compartido) { // Recibe objeto a compartir
        super("Productor");
        objCompartido = compartido;
    }
    /* Método 'run' (override) */

    public void run() {
        for (int cuenta = 1; cuenta <= 4; cuenta++) {
            /* Pausar de 0 a 3 segundos y modificar valor en Espacio */
            try {
                //Thread.sleep( ( int ) ( Math.random() * 3001 ) );
                Thread.sleep(0);
                objCompartido.setVal(cuenta);
            } /* Si el thread es interrumpido */ catch (InterruptedException excepcion) {
                excepcion.printStackTrace();
            }
        }
        System.out.println(getName() + " concluyó producción."
                + "\nTerminando " + getName() + ".");
    }
}
/* Clase para objetos consumidores */

class Consumidor extends Thread {

    private Espacio objCompartido; // Referencia a un objeto compartido
    /* constructor (overload) */

    public Consumidor(Espacio compartido) { // Recibe objeto a compartir
        super("Consumidor");
        objCompartido = compartido;
    }
    /* Método 'run' (override) */

    public void run() {
        int suma = 0;
        for (int cuenta = 1; cuenta <= 4; cuenta++) {
            /* Pausar de 0 a 3 segundos y acumular el valor de Espacio */
            try {
                //Thread.sleep( ( int ) ( Math.random() * 3001 ) );
                Thread.sleep(0);
                suma += objCompartido.getVal();
            } /* Si el thread es interrumpido */ catch (InterruptedException excepcion) {
                excepcion.printStackTrace();
            }
        }
        System.out.println(getName() + " Suma de valores leídos: " + suma
                + "\nTerminando " + getName() + ".");
    }
}
