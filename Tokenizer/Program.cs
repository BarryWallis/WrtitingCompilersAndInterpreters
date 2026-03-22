using System.Reflection;

using Backend.CompositionLib;

using Frontend.CompositionLib;

using FrontendLib;

using IntermediateLib;

namespace Tokenizer;

/// <summary>
/// Compiles or interprets a Pascal source program.
/// </summary>
public partial class Pascal
{
    private const string _flags = "[-ix]";
    private const string _usage = "Usage: Pascal execute|compile " + _flags + " <source file path>";
    private const string _sourceLineFormat = "{0:000} {1}";
    private const string _parserSummaryFormat = "\n{0,20:N0} source lines.\n{1,20:N0} syntax errors.\n{2,20:N2} seconds total parsing time.\n";
    private const string _interpreterSummaryFormat = "\n{0,20:N0} statements executed.\n{1,20:N0} runtime errors.\n{2,20:N2} seconds total execution time.\n";
    private const string _compilerSummaryFormat = "\n{0,20:N0} instructions generated.\n{1,20:N2} seconds total code generation time.\n";

    private readonly Parser _parser;
    private readonly IIntermediateCode _intermediateCode;
    private readonly ISymbolTable _symbolTable;
    private readonly BackendLib.Backend _backend;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pascal"/> class and runs the requested operation.
    /// </summary>
    /// <param name="operation">Either <c>compile</c> or <c>execute</c>.</param>
    /// <param name="filePath">The source file path.</param>
    /// <param name="flags">The command-line flags.</param>
    public Pascal(string operation, string filePath, string flags)
    {
        _parser = null!;
        _intermediateCode = null!;
        _symbolTable = null!;
        _backend = null!;

        try
        {
            bool intermediate = flags.Contains('i');
            bool xref = flags.Contains('x');
            _ = intermediate;
            _ = xref;

            using Source source = new(new StreamReader(filePath));
            source.AddMessageListener(new SourceMessageListener());

            _parser = FrontendFactory.CreateParser("Pascal", "top-down", source);
            _parser.AddMessageListener(new ParserMessageListener());

            _backend = BackendFactory.CreateBackend(operation);
            _backend.AddMessageListener(new BackendMessageListener());

            _parser.Parse();

            _intermediateCode = GetIntermediateCode(_parser)!;
            _symbolTable = GetSymbolTable()!;

            _backend.Process(_intermediateCode, _symbolTable);
        }
        catch (Exception ex)
        {
            Console.WriteLine("***** Internal translator error. *****");
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// Runs the Pascal compiler or interpreter from the command line.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        try
        {
            string operation = args[0];
            if (!string.Equals(operation, "compile", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(operation, "execute", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException();
            }

            int i = 0;
            string flags = string.Empty;

            while (++i < args.Length && args[i].StartsWith('-'))
            {
                flags += args[i][1..];
            }

            if (i < args.Length)
            {
                string path = args[i];
                _ = new Pascal(operation, path, flags);
                return;
            }

            throw new InvalidOperationException();
        }
        catch (Exception)
        {
            Console.WriteLine(_usage);
        }
    }

    /// <summary>
    /// Reads the parser intermediate code using the framework's current non-public storage.
    /// </summary>
    /// <param name="parser">The parser instance.</param>
    /// <returns>The generated intermediate code, if available.</returns>
    private static IIntermediateCode? GetIntermediateCode(Parser parser)
    {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        FieldInfo? fieldInfo = typeof(Parser).GetField("intermediateCode", bindingFlags);
        return fieldInfo?.GetValue(parser) as IIntermediateCode;
    }

    /// <summary>
    /// Reads the parser symbol table using the framework's current non-public storage.
    /// </summary>
    /// <returns>The generated symbol table, if available.</returns>
    private static ISymbolTable? GetSymbolTable()
    {
        const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;
        PropertyInfo? propertyInfo = typeof(Parser).GetProperty("SymbolTable", bindingFlags);
        return propertyInfo?.GetValue(null) as ISymbolTable;
    }
}
