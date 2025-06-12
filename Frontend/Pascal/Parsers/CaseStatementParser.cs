
using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

public class CaseStatementParser(StatementParser parent) : StatementParser(parent)
{
    public override IIntermediateCodeNode? Parse(PascalToken token) => throw new NotImplementedException();
}
