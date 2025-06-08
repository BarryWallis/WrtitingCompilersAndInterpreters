using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Intermediate;

using Messages;

namespace BackendComponents.Interpreter;

public class RuntimeErrorHandler
{
    private const int MaxErrors = 5;

    public static int ErrorCount { get; private set; } = 0;

    public static void Flag(IIntermediateCodeNode errorNode, RuntimeErrorCode errorCode, Backend backend)
    {
        //string? lineNumber = null;

        IIntermediateCodeNode? node = errorNode;
        while ((node is not null) && (node.GetAttribute(IIntermediateCodeKey.Key.Line) is null))
        {
            node = node.Parent;
        }
        Debug.Assert(node is not null);

        Debug.Assert(errorCode.ToString() is not null);
        Debug.Assert(node.GetAttribute(IIntermediateCodeKey.Key.Line) is not null and int);
        backend.SendMessage(new RuntimeErrorMessage(errorCode.ToString()!, (int)node.GetAttribute(IIntermediateCodeKey.Key.Line)!));

        if (++ErrorCount > MaxErrors)
        {
            Console.WriteLine("*** ABORTED AFTER TOO MANY ERRORS.");
            Environment.Exit(-1);
        }
    }
}
