namespace Shockky.IO.Text
{
    public interface ITextOutput
    {
        void Indent();
        void Unindent();
        void Write(char ch);
        void Write(string text);
        void WriteLine();
    }
}
