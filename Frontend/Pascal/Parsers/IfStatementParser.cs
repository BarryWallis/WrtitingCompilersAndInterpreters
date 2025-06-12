
using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

public class IfStatementParser(StatementParser parent) : StatementParser(parent)
{
    public override IIntermediateCodeNode? Parse(PascalToken token) => throw new NotImplementedException();
}
