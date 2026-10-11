namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Start av programmet ===");

            var path = Path.Combine(AppContext.BaseDirectory, "numbers.txt");
            try
            {
                Console.WriteLine("Försöker läsa fil och räkna...");
                double result = ProcessFile(path);
                Console.WriteLine($"\nResultat: {result}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Filen hittades inte: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Texten kan inte tolkas som tal: {ex.Message}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Kan inte dividera med noll: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Okänt fel: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Cleanup: Logging avslutat anrop.");
            }

            Console.WriteLine("Programmet avslutas normalt.");

            static double ProcessFile(string fileName)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentException("Filnamn får inte vara tomt eller null.", nameof(fileName));
                }

                using StreamReader reader = new StreamReader(fileName); //using: .Dispose() vid out of scope

                string? line = reader.ReadLine();
                    
                if (line == null)
                    throw new InvalidOperationException("Filen är tom.");

                int number = int.Parse(line); // Kan ge FormatException

                if (number == 0)
                {
                    /*
                     * Division med double ger Infinity vid division med noll.
                     * Därför kastar vi DivideByZeroException manuellt.
                     * Vid int / int sker detta automatiskt.
                     */
                    throw new DivideByZeroException("Talet i filen är noll, kan inte dividera.");
                }

                return 100.0 / number;
            }
        }
    }
}

/* 
Input: 5
Resultat: 20
Cleanup: Logging avslutat anrop.
Programmet avslutas normalt.
*/

/*
Input: hello
Texten kan inte tolkas som tal: The input string 'hello' was not in a correct format.
Cleanup: Logging avslutat anrop.
Programmet avslutas normalt.
*/

/*
Input: 0
Kan inte dividera med noll: Talet i filen är noll, kan inte dividera.
Cleanup: Logging avslutat anrop.
Programmet avslutas normalt.
*/

/*
Input: (filen lämnades helt tom)
Okänt fel: Filen är tom.
Cleanup: Logging avslutat anrop.
Programmet avslutas normalt.
*/

/*
Input: (filen har bytt namn)
Filen hittades inte: Could not find file 'OMITTED\numbers.txt'.
Cleanup: Logging avslutat anrop.
Programmet avslutas normalt.
*/