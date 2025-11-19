using System.Diagnostics;

using BackendLib;

using FrontendLib;

using IntermediateLib;

namespace Chapter2;

/// <summary>
/// Coordinates the end-to-end translation workflow: constructing the source, parser,
/// and backend, executing parsing, and then processing (compiling or executing) the
/// intermediate code.
/// </summary>
/// <remarks>
/// This class is a thin facade over the underlying components and is intentionally
/// minimal for early chapters. Errors are caught broadly and reported as internal translator errors.
/// </remarks>
public class Pascal
{
    private readonly Parser? _parser;
    private readonly Source? _source;
    private readonly IIntermediateCode? _intermediateCode;
    private readonly ISymbolTable? _symbolTable;
    private readonly Backend? _backend;

    /// <summary>
    /// Initializes and runs the translation pipeline.
    /// </summary>
    /// <param name="operation">The backend operation: "execute" or "compile".</param>
    /// <param name="filePath">Path to the Pascal source file.</param>
    /// <param name="flags">Optional concatenated flag characters impacting output (e.g. i, x).</param>
    public Pascal(string operation, string filePath, string flags)
    {
        try
        {
            bool showIntermediate = flags.Contains('i');
            bool createCrossReference = flags.Contains('x');
            // Flags are placeholders for future feature enabling.

            _source = new(new StreamReader(filePath));
            _source.AddMessageListener(new SourceMessageListener());
            _parser = FrontendFactory.CreateParser("Pascal", "top-down", _source);
            _parser.AddMessageListener(new ParserMessageListener());

            _backend = BackendFactory.CreateBackend(operation);
            _backend.AddMessageListener(new BackendMessageListener());

            _parser.Parse();
            _source.Close();

            _intermediateCode = _parser.IntermediateCode;
            _symbolTable = _parser.SymbolTable;

            _backend.Process(_intermediateCode, _symbolTable);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("***** Internal translator error! *****");
            Console.Error.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}
