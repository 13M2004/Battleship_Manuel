// BATTLESHIP_Manuel
using System;

class Program
{
    static char[,] CrearTablero(int dimensiones, char caracterBase)
    {
        char[,] tablero = new char[dimensiones, dimensiones];

        for (int fila = 0; fila < dimensiones; fila++)
        {
            for (int columna = 0; columna < dimensiones; columna++)
            {
                tablero[fila, columna] = caracterBase;
            }
        }

        return tablero;
    }

    static void ImprimirTablero(char[,] tablero)
    {
        int dimensiones = tablero.GetLength(0);

        Console.Write(" ");
        for (int columna = 0; columna < dimensiones; columna++)
        {
            Console.Write($"{columna + 1,3}");
        }
        Console.WriteLine();

        Console.Write($" {new string('-', dimensiones * 3)}");
        Console.WriteLine();

        for (int fila = 0; fila < dimensiones; fila++)
        {
            char letraFila = Convert.ToChar('A' + fila);
            Console.Write($"{letraFila}|");
            for (int columna = 0; columna < dimensiones; columna++)
            {
                char c = tablero[fila, columna];
                if (c == 'F' || c == 'N' || c == 'P' || c == 'S' || c == 'D')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (c == 'X')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (c == 'O')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write($"{tablero[fila, columna],2} ");
            }
            Console.WriteLine();
        }
        Console.ResetColor(); // Restablecer el color después de imprimir el tablero
    }

    static void ColocarBarcos(char[,] tablero, char barco)
    {
        int dimensiones = tablero.GetLength(0);
        Random random = new Random();

        bool barcoColocado = false;
        while (!barcoColocado)
        {
            int fila = random.Next(dimensiones);
            int columna = random.Next(dimensiones);
            int direccion = random.Next(2); // 0: horizontal, 1: vertical

            if (PodemosColocarBarco(tablero, barco, fila, columna, direccion))
            {
                ColocarBarco(tablero, barco, fila, columna, direccion);
                barcoColocado = true;
            }
        }
    }

    static bool PodemosColocarBarco(char[,] tablero, char barco, int fila, int columna, int direccion)
    {
        int dimensiones = tablero.GetLength(0);
        int longitudBarco = (barco == 'F' || barco == 'N') ? 3 : 2;
        for (int i = 0; i < longitudBarco; i++)
        {
            if (fila >= dimensiones || columna >= dimensiones || tablero[fila, columna] != '*')
            {
                return false;
            }

            if (direccion == 0) // horizontal
            {
                columna++;
            }
            else // vertical
            {
                fila++;
            }
        }

        return true;
    }

    static void ColocarBarco(char[,] tablero, char barco, int fila, int columna, int direccion)
    {
        int longitudBarco = (barco == 'F' || barco == 'N') ? 3 : 2;
        for (int i = 0; i < longitudBarco; i++)
        {
            tablero[fila, columna] = barco;

            if (direccion == 0) // horizontal
            {
                columna++;
            }
            else // vertical
            {
                fila++;
            }
        }
    }

    static void Atacar(char[,] tablero, char[,] tableroEnemigo)
    {
        int dimensiones = tablero.GetLength(0);

        Console.WriteLine("Ingrese la fila (Letra):");
        char letraFila = char.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese la columna (Número):");
        int numeroColumna = int.Parse(Console.ReadLine()) - 1;

        int fila = letraFila - 'A';
        if (fila < 0 || fila >= dimensiones || numeroColumna < 0 || numeroColumna >= dimensiones)
        {
            Console.WriteLine("Posición fuera de rango.");
            return;
        }

        if (tableroEnemigo[fila, numeroColumna] != '*')
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("¡Has impactado un barco!");
            tableroEnemigo[fila, numeroColumna] = 'X'; // Marcamos el impacto en el tablero enemigo
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("¡Has fallado, ponte las pilas!");
            tableroEnemigo[fila, numeroColumna] = 'O'; // Marcamos el fallo en el tablero enemigo
        }
        Console.ResetColor(); // Restablecer el color después de imprimir el mensaje
    }

    static string ObtenerNombreJugador(int numeroJugador)
    {
        Console.WriteLine($"Ingrese el nombre del jugador {numeroJugador}:");
        return Console.ReadLine();
    }

    static void Main(string[] args)
    {
        Console.WriteLine("==========================");
        Console.WriteLine("¡BIENVENIDOS A BATTLESHIP!");
        Console.WriteLine("==========================");
        Console.WriteLine("");
        Console.WriteLine("1. Iniciar juego");
        Console.WriteLine("2. Salir");

        string opcion = Console.ReadLine();

        if (opcion == "1")
        {
            // Configuración del juego
            char caracterBase = '*';
            char[] barcos = { 'F', 'N', 'P', 'S', 'D' }; // Barcos de 2 o 3 posiciones
            int dimensionesTablero = 20;

            // Crear tableros para jugador y enemigo
            char[,] tableroJugador = CrearTablero(dimensionesTablero, caracterBase);
            char[,] tableroEnemigo = CrearTablero(dimensionesTablero, caracterBase);

            // Colocar barcos en el tablero del jugador
            foreach (char barco in barcos)
            {
                ColocarBarcos(tableroJugador, barco);
                ColocarBarcos(tableroEnemigo, barco);
            }

            // Obtener nombres de jugadores
            string nombreJugador1 = ObtenerNombreJugador(1);
            string nombreJugador2 = ObtenerNombreJugador(2);

            // Comienza el juego
            Console.WriteLine("\n¡Comienza el juego!");

            while (true)
            {
                // Mostrar el tablero del jugador y el tablero del enemigo
                Console.WriteLine($"\nTablero de {nombreJugador1}:");
                ImprimirTablero(tableroJugador);
                Console.WriteLine($"\nTablero de {nombreJugador2}:");
                ImprimirTablero(tableroEnemigo);

                // Turno del jugador 1
                Console.WriteLine($"\nTurno de {nombreJugador1}:");
                Atacar(tableroJugador, tableroEnemigo);

                // Verificar si el jugador 2 ha perdido
                if (TableroSinBarcos(tableroEnemigo))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n¡Felicidades, {nombreJugador1}! Has destruido todos los barcos de {nombreJugador2}.");
                    Console.ResetColor();
                    Console.WriteLine("");
                    
                    Console.WriteLine("                                                  |  |[][][][]");
                    Console.WriteLine("                                      °           |  |");
                    Console.WriteLine("                  ===|||===          >|<          |  |");
                    Console.WriteLine("                  =========___________|___________|  |");
                    Console.WriteLine("                 /------------------------------------\\");
                    Console.WriteLine("                /--------------------------------------\\");
                    Console.WriteLine("               /----------°--------°---------°----------\\");
                    Console.WriteLine("              /------------------------------------------\\");
                    Console.WriteLine("             /--------------------------------------------\\");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.ResetColor();

                    break;
                }

                // Mostrar el tablero del jugador y el tablero del enemigo después del ataque del jugador 1
                Console.WriteLine($"\nTablero de {nombreJugador1}:");
                ImprimirTablero(tableroJugador);
                Console.WriteLine($"\nTablero de {nombreJugador2}:");
                ImprimirTablero(tableroEnemigo);

                // Turno del jugador 2
                Console.WriteLine($"\nTurno de {nombreJugador2}:");
                Atacar(tableroEnemigo, tableroJugador);

                // Verificar si el jugador 1 ha perdido
                if (TableroSinBarcos(tableroJugador))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n¡Felicidades, {nombreJugador1}! Has destruido todos los barcos de {nombreJugador2}.");
                    Console.ResetColor();
                    Console.WriteLine("");

                    Console.WriteLine("                                                  |  |[][][][]");
                    Console.WriteLine("                                      °           |  |");
                    Console.WriteLine("                  ===|||===          >|<          |  |");
                    Console.WriteLine("                  =========___________|___________|  |");
                    Console.WriteLine("                 /------------------------------------\\");
                    Console.WriteLine("                /--------------------------------------\\");
                    Console.WriteLine("               /----------°--------°---------°----------\\");
                    Console.WriteLine("              /------------------------------------------\\");
                    Console.WriteLine("             /--------------------------------------------\\");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.ResetColor();
                    break;
                }
            }
        }
        else if (opcion == "2")
        {
            Console.WriteLine("¡Hasta luego amigo programador!");
        }
        else
        {
            Console.WriteLine("Opción no válida.");
        }
    }

    static bool TableroSinBarcos(char[,] tablero)
    {
        foreach (char c in tablero)
        {
            if (c == 'F' || c == 'N' || c == 'P' || c == 'S' || c == 'D')
            {
                return false;
            }
        }
        return true;
    }
}
