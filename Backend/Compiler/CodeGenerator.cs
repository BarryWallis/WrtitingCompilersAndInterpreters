using IntermediateLib;

using MessageLib;

namespace BackendLib.Compiler;

/// <summary>
/// Backend implementation that would generate target code from the populated
/// <see cref="IIntermediateCode"/> and <see cref="ISymbolTable"/>. Currently acts as
/// a stub that reports a summary message with zero instructions.
/// </summary>
public class CodeGenerator : Backend
{
    /// <summary>
    /// Performs code generation. This stub only records timing and emits a
    /// <see cref="CompilerSummaryMessage"/> with an instruction count of zero.
    /// </summary>
    /// <param name="intermediateCode">The intermediate representation produced by the parser.</param>
    /// <param name="symbolTable">The symbol table containing declarations and metadata.</param>
    public override void Process(IIntermediateCode? intermediateCode, ISymbolTable? symbolTable)
    {
        DateTime startTime = DateTime.Now;
        // Real implementation would walk intermediateCode and symbolTable to emit target instructions.
        TimeSpan elapsedTime = DateTime.Now - startTime;
        int instructionCount = 0;

        SendMessage(new CompilerSummaryMessage(instructionCount, elapsedTime));
    }
}
