using System.IO;
using System.Text;

namespace Shockky.IO.Text
{
    class TextOutputWriter : TextWriter
    {
        private readonly ITextOutput _output;

        public TextOutputWriter(ITextOutput output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            _output.Write(value);
        }

        public override void Write(string value)
        {
            _output.Write(value);
        }

        public override void WriteLine()
        {
            _output.WriteLine();
        }

        public override Encoding Encoding
            => Encoding.UTF8;
    }
}
