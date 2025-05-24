using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

using Intermediate;
using Intermediate.Implementation;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Utilities;

/// <summary>
/// Prints the intermediate code representation as a parse tree to a specified <see cref="TextWriter"/>.
/// </summary>
/// <param name="textWriter">The <see cref="TextWriter"/> used to output the parsed tree.</param>
public class ParseTreePrinter(TextWriter textWriter)
{
    private const int IndentWidth = 4;
    private const int LineWidth = 80;

    private readonly TextWriter _textWriter = textWriter;
    private int _length = 0;
    private readonly string _indent = new(' ', IndentWidth);
    private string _indentation = string.Empty;
    private readonly StringBuilder _line = new();

    /// <summary>
    /// Print the intermediate code representation as a parse tree.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to print.</param>
    public void Print(IIntermediateCode intermediateCode)
    {
        _textWriter.WriteLine();
        _textWriter.WriteLine($"===== INTERMEDIATE CODE =====");
        _textWriter.WriteLine();

        PrintNode((intermediateCode.Root as IntermediateCodeNode) ?? throw new UnreachableException());
        PrintLine();
    }

    /// <summary>
    /// Print a line to the output if the current line has content.
    /// </summary>
    private void PrintLine()
    {
        if (_length > 0)
        {
            _textWriter.WriteLine(_line);
            _ = _line.Clear();
            _length = 0;
        }
    }

    /// <summary>
    /// Print the given node and its children.
    /// </summary>
    /// <param name="node">The node to print.</param>
    private void PrintNode(IntermediateCodeNode node)
    {
        Append(_indentation);
        Append($"<{node}");

        PrintAttributes(node);
        PrintTypeSpecification(node);

        IList<IIntermediateCodeNode> childNodes = node.Children;
        if (childNodes.Count > 0)
        {
            Append(">");
            PrintLine();
            PrintChildNodes(childNodes);
            Append(_indentation);
            Append($"</{node}>");
        }
        else
        {
            Append(" ");
            Append("/>");
        }
        PrintLine();
    }


    /// <summary>
    /// Prints the details of the specified child nodes to the output, using the current text level.
    /// </summary>
    /// <remarks>
    /// The method temporarily increases the text level while printing the child nodes and
    /// restores it afterward.
    /// </remarks>
    /// <param name="childNodes">
    /// A collection of child nodes to be printed. Each node must implement <see cref="IIntermediateCodeNode"/>.
    /// </param>
    private void PrintChildNodes(IList<IIntermediateCodeNode> childNodes)
    {
        string saveIndentation = _indentation;
        _indentation += _indent;
        foreach (IIntermediateCodeNode child in childNodes)
        {
            Debug.Assert(child is IntermediateCodeNode);
            PrintNode((child as IntermediateCodeNode)!);
        }

        _indentation = saveIndentation;
    }


#pragma warning disable IDE0060 // Remove unused parameter
    private static void PrintTypeSpecification(IntermediateCodeNode? node)
#pragma warning restore IDE0060 // Remove unused parameter
    {
    }

    /// <summary>
    /// Print the attributes of the given node.
    /// </summary>
    /// <param name="node">The node with attributes to print.</param>
    private void PrintAttributes(IntermediateCodeNode node)
    {
        string saveIndentation = _indentation;
        _indentation += _indent;

        foreach (KeyValuePair<IIntermediateCodeKey.Key, object?> attribute in node.Attributes)
        {
            Debug.Assert(attribute.Key.ToString() is not null);
            PrintAttribute(attribute.Key.ToString()!, attribute.Value);
        }

        _indentation = saveIndentation;
    }

    /// <summary>
    /// Print the attribute of the given node with the given key and value as <paramref name="keyString"/>=<paramref name="value"/>..
    /// </summary>
    /// <param name="keyString"></param>
    /// <param name="value"></param>
    /// <exception cref="UnreachableException">The attriubute value is <see langword="null"/>.</exception>
    private void PrintAttribute(string keyString, object? value)
    {
        string valueString = (value as ISymbolTableEntry)?.Name ?? value?.ToString()
                                                                   ?? throw new UnreachableException("The attribute value is null.");
        string text = $"{keyString.ToLowerInvariant()}=\"{valueString}\"";
        Append(" ");
        Append(text);

        if (value is ISymbolTableEntry entry)
        {
            int level = entry.SymbolTable.NestingLevel;
            PrintAttribute("LEVEL", level);
        }
    }

    /// <summary>
    /// Append the given text to the current line, and if necessary, print a line break.
    /// </summary>
    /// <param name="text">The text to append.</param>
    private void Append(string text)
    {
        int textLength = text.Length;
        bool lineBreak = false;

        if (_length + textLength > LineWidth)
        {
            PrintLine();
            _ = _line.Append(_indentation);
            _length = _indentation.Length;
            lineBreak = true;
        }

        if (!(lineBreak && text == " "))
        {
            _ = _line.Append(text);
            _length += textLength;
        }
    }

}
