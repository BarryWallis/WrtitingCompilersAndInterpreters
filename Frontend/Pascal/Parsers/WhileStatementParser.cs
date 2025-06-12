using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

public class WhileStatementParser(StatementParser parent) : StatementParser(parent)
{
    private static readonly HashSet<ITokenType.Kind> _doSet = 
        [.. _statementStartSet.Concat(_statementFollowSet).Append(ITokenType.Kind.Do)];

    public override IIntermediateCodeNode? Parse(PascalToken token)
    {
        token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        IIntermediateCodeNode loopNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Loop);
        IIntermediateCodeNode breakNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Test);
        IIntermediateCodeNode notNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Not);

        _ = loopNode.AddChild(breakNode);
        _ = breakNode.AddChild(notNode);

        ExpressionParser expressionParser = new(this);
        _ = notNode.AddChild(expressionParser.Parse(token));

        token = Synchronize(_doSet) as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        if (token.Kind == ITokenType.Kind.Do)
        {
            token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        }
        else
        {
            ErrorHandler.Flag(token, PascalErrorCode.MissingDo, this);
        }

        StatementParser statementParser = new(this);
        _ = loopNode.AddChild(statementParser.Parse(token) 
            ?? throw new UnreachableException("Expected a valid statement node after 'do'"));

        return loopNode;
    }
}
