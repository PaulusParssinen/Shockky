using System.IO;

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

        public void WriteToken(string token)
        {
            WriteIndentation();
            _writer.Write(token);
        }

        public void Space()
        {
            //WriteIndentation();
            _writer.Write(' ');
        }

        public void Indent()
        {
            _indentCount++;
        }

        public void Unindent()
        {
            _indentCount--;
        }

        void WriteIndentation()
        {
            if (_needsIndent)
            {
                _needsIndent = false;
                for (int i = 0; i < _indentCount; i++)
                {
                    _writer.Write('\t');
                }
            }
        }

        public void NewLine()
        {
            _writer.WriteLine();
            _needsIndent = true;
        }
    }
}
