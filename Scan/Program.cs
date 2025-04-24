namespace Scan;

public class Program
{
    /// <summary>
    /// Entry point for the application that processes command-line arguments to determine the operation 
    /// and source file path.
    /// </summary>
    /// <param name="args">
    /// Contains command-line arguments passed to the application, used to specify the operation and source 
    /// file.
    /// </param>
    private static void Main(string[] args)
    {
        try
        {
            string operation = args[0];
            if (!(operation.Equals("compile", StringComparison.OrdinalIgnoreCase)
                || operation.Equals("execute", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(
                    $"Invalid operation: {operation}.");
            }

            int i = 0;
            string flags = string.Empty;
            while (++i < args.Length && args[i].StartsWith('-'))
            {
                flags += args[i][1..];
            }

            if (i >= args.Length)
            {
                throw new ArgumentException($"Missing source file path.");
            }

            string path = args[i];
            _ = new Pascal(operation, path, flags);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine();
        }
    }
}
