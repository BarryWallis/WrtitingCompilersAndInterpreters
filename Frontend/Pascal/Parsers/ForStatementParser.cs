
using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

public class ForStatementParser(StatementParser parent) : StatementParser(parent)
{
    public override IIntermediateCodeNode? Parse(PascalToken token) => throw new NotImplementedException();
}
