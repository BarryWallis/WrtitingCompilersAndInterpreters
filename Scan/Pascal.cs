using BackendComponents;

using FrontendComponents;

using Intermediate;

namespace Scan;

/// <summary>
/// Initializes a Pascal compiler or executor, parsing a source file and processing it with a backend. 
/// </summary>
public class Pascal
{
    private readonly Parser? _parser;
    private readonly Source? _source;
    private readonly IIntermediateCode? _intermediateCode;
    private readonly ISymbolTable? _symbolTable;
    private readonly Backend? _backend;

    /// <summary>
    /// Initializes a Pascal compiler with specified operation, file path, and flags for processing source 
    /// code.
    /// </summary>
    /// <param name="operation">
    /// Defines the specific operation to be performed by the backend during compilation.
    /// </param>
    /// <param name="filePath">
    /// Specifies the location of the source code file to be read and processed.
    /// </param>
    /// <param name="flags">
    /// Indicates additional options for the compilation process, such as generating intermediate code or
    /// cross-references.
    /// </param>
    public Pascal(string operation, string filePath, string flags)
    {
        try
        {
            bool intermediate = flags.Contains('i');
            bool xref = flags.Contains('x');

            _source = new Source(new StreamReader(filePath));
            _source.AddMessageListener(new SourceMessageListener());
            _parser = FrontendFactory.CreateParser("pascal", "top-down", _source);
            _parser.AddMessageListener(new ParserMessageListener());
            _backend = BackendFactory.CreateBackend(operation);
            _backend.AddMessageListener(new BackendMessageListener());

            _parser.Parse();
            _source.Close();

            // TODO: Uncomment the following lines when the properties are implemented in the Parser class.
            //_intermediateCode 
            //    = _parser.IntermediateCode 
            //      ?? throw new InvalidOperationException($"{nameof(IIntermediateCode)} is null.");
            //_symbolTable 
            //    = Parser.SymbolTable 
            //      ?? throw new InvalidOperationException($"{nameof(Parser.SymbolTable)} is null.");
            _backend.Process(_intermediateCode!, _symbolTable!);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Internal translator error: {ex.Message}");
        }
    }
}
