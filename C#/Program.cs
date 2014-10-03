using System;


public interface ITransporte
{
	string Nombre{get;set;}
	void Avanza();
}

public abstract class Transporte : ITransporte
{
	protected string nombre;

	public string Nombre
	{
		get{return this.nombre;}
		set{this.nombre=value;}
	}
	public abstract void Avanza();
}

public class Automovil : Transporte
{
	private string modelo;
	public string Modelo
	{
		get{return this.modelo;}
		set{this.modelo = value;}
	}
	public override void Avanza()
	{
		Console.WriteLine("El auto avanza {0}",nombre);
	}
}

public class Avion : Transporte
{
	private int altura;
	private string clase;

	public int Altura
	{
		get{return this.altura;}
		set{this.altura = value;}
	}
	public string Clase
	{
		get{return this.clase;}
		set{this.clase = value;}
	}
	public override void Avanza()
	{
		Console.WriteLine("El avion avanza en el aire a {0} mts de altura", altura);
	}

}

public class Program
{
	public static void Main(string[] args)
	{
		Automovil auto = new Automovil();
		auto.Modelo = "2004";
		auto.Nombre = "Chevy";
		auto.Avanza();

		Avion avion = new Avion();
		avion.Clase = "777";
		avion.Altura = 9000;
		avion.Avanza();
	}
}


