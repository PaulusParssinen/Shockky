using System.IO;

namespace Shockky.Shockwave.Lingo.Output
{
    public class LingoOutputFormatter : IOutputFormatter
    {
        private readonly TextWriter _writer;
	    private bool _needsIndent = true;

		public int Indentation { get; set; }

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

        private void WriteIndentation()
        {
	        if (!_needsIndent) return;

	        _needsIndent = false;
			_writer.Write(new string('\t', Indentation));
        }

        public void NewLine()
        {
            _writer.WriteLine();
            _needsIndent = true;
        }
    }
}
