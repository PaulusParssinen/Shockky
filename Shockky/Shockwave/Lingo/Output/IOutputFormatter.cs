using Shockky.Shockwave.Lingo.Bytecode.AST;

namespace Shockky.Shockwave.Lingo.Output
{
    public interface IOutputFormatter
    {
        void StartNode(AstNode node);
        void EndNode(AstNode node);
        
        void WriteToken(string token);
        void Space();

        void Indent();
        void Unindent();

        void NewLine();
    }
}
