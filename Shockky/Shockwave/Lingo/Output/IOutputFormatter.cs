using Shockky.Shockwave.Lingo.Bytecode.AST;

namespace Shockky.Shockwave.Lingo.Output
{
    public interface IOutputFormatter
    {   
        void WriteToken(string token);
        void Space();

        void Indent();
        void Unindent();

        void NewLine();
    }
}
