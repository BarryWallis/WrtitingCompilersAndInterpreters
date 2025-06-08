using System.Diagnostics;

using Intermediate;

using Messages;

namespace BackendComponents.Compiler;

/// <summary>
/// Generates code by processing intermediate code and symbol tables. Measures processing time and sends a 
/// summary message.
/// </summary>
public class CodeGenerator : Backend
{
    /// <summary>
    /// Processes intermediate code and symbol table, measuring execution time and counting instructions.
    /// </summary>
    /// <param name="intermediateCode">
    /// Represents the intermediate representation of the code to be processed.
    /// </param>
    /// <param name="symbolTable"
    /// >Holds the symbols and their associated information for the code being processed.
    /// </param>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTableStack symbolTableStack)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        stopwatch.Stop();
        double elapsedTime = stopwatch.Elapsed.TotalSeconds;
        int instructionCount = 0;
        SendMessage(new CompilerSummaryMessage(instructionCount, elapsedTime));
    }
}
