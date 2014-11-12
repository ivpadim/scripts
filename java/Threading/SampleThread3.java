public class SampleThread3 {

    public static void main(String[] args) {
        /* Crear el objeto compartido por los threads */
        Espacio objCompartido = new EspacioNoSinc();
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

    public int getVal();            // Obtener un valor de Espacio
}
/* Clase implementadora de Espacio */

class EspacioNoSinc implements Espacio {

    private int varCompartida = -1;   // Compartida por los threads productor y consumidor

    public void setVal(int valor) { // Asignar un valor
        System.out.println(Thread.currentThread().getName()
                + " escribiendo " + valor);
        varCompartida = valor;
    }

    public int getVal() { // Leer un valor
        System.out.println(Thread.currentThread().getName()
                + " leyendo " + varCompartida);
        return varCompartida;
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
                //Thread.sleep((int) (Math.random() * 3001));
                Thread.sleep(1);
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
                //Thread.sleep((int) (Math.random() * 3001));
                Thread.sleep(1);
                suma += objCompartido.getVal();
            } /* Si el thread es interrumpido */ catch (InterruptedException excepcion) {
                excepcion.printStackTrace();
            }
        }
        System.out.println(getName() + " Suma de valores leídos: " + suma
                + "\nTerminando " + getName() + ".");
    }
}
