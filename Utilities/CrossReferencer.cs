using System.Text;

using Intermediate;

namespace Utilities;

public class CrossReferencer
{
    private const int NameWidth = 16;
    private const string NummbersLabel = " Line numbers    ";
    private const string NumbersUnderline = " ------------    ";
    private static readonly int _labelWidth = NummbersLabel.Length;
    private static readonly int _indentWidth = NameWidth + _labelWidth;
#pragma warning disable IDE0052 // Remove unread private members
    private static readonly StringBuilder _indent = new(new string(' ', _indentWidth));
#pragma warning restore IDE0052 // Remove unread private members

    /// <summary>
    /// Print the cross-reference table for the given symbol table stack.
    /// </summary>
    /// <param name="symbolTableStack">The symbol table stack to print the cross-reference for.</param>
    public static void Print(ISymbolTableStack symbolTableStack)
    {
        Console.WriteLine();
        Console.WriteLine("===== CROSS-REFERENCE TABLE =====");
        PrintColumnHeadings();
        PrintSymbolTable(symbolTableStack.LocalSymbolTable);
    }

    private static void PrintSymbolTable(ISymbolTable symbolTable)
    {
        IList<ISymbolTableEntry> sortedSymbolTableEntries = symbolTable.SortedEntries;
        foreach (ISymbolTableEntry symbolTableEntry in sortedSymbolTableEntries)
        {
            IList<int> lineNumbers = symbolTableEntry.LineNumbers;
            Console.Write($"{symbolTableEntry.Name,-20}");
            if (lineNumbers.Count > 0)
            {
                foreach (int lineNumber in lineNumbers)
                {
                    Console.Write($"{lineNumber:D3} ");
                }
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Print the column headings for the cross-reference table.
    /// </summary>
    private static void PrintColumnHeadings()
    {
        Console.WriteLine();
        Console.WriteLine($"{"Identifier",-20}{NummbersLabel}");
        Console.WriteLine($"{"----------",-20}{NumbersUnderline}");
    }
}
