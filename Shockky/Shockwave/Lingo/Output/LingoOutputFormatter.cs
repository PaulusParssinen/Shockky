using System.IO;
using Shockky.Shockwave.Lingo.Bytecode.AST;

namespace Shockky.Shockwave.Lingo.Output
{
    class LingoOutputFormatter : IOutputFormatter
    {
        private readonly TextWriter _writer;

        private int _indentCount;
        private bool _needsIndent = true;

        public LingoOutputFormatter(TextWriter writer)
        {
            _writer = writer;
        }

        public void StartNode(AstNode node)
        {
            throw new System.NotImplementedException();
        }

        public void EndNode(AstNode node)
        {
            throw new System.NotImplementedException();
        }

        public void WriteToken(string token)
        {
            throw new System.NotImplementedException();
        }

        public void Space()
        {
            throw new System.NotImplementedException();
        }

        public void Indent()
        {
            throw new System.NotImplementedException();
        }

        public void Unindent()
        {
            throw new System.NotImplementedException();
        }

        public void NewLine()
        {
            throw new System.NotImplementedException();
        }
    }
}
