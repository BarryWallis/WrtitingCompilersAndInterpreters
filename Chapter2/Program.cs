using Chapter2;

/// <summary>
/// Entry point for the Chapter2 Pascal translator demonstration. Accepts an operation (execute|compile)
/// optionally followed by dash-prefixed flags and a source file path.
/// </summary>
/// <remarks>
/// Flags:
/// i - request intermediate code be shown (placeholder)
/// x - request a cross reference listing (placeholder)
/// </remarks>
try
{
    string operation = args[0];

    if (operation is not ("execute" or "compile"))
    {
        // CA2208 fix: Use nameof(args) instead of nameof(operation)
        throw new ArgumentException("Invalid operation specified. Must be 'execute' or 'compile'.", nameof(args));
    }

    int i = 0;
    string flags = string.Empty;
    while ((++i < args.Length) && (args[i][0] == '-'))
    {
        flags += args[i][1..];
    }

    if (i < args.Length)
    {
        string path = args[i];
        _ = new Pascal(operation, path, flags);
    }
    else
    {
        throw new ArgumentException("Source file path argument is missing.", nameof(args));
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Console.Error.WriteLine("Usage: Pascal execute|compile [-ix] <source file path>");
    Environment.Exit(1);
}
