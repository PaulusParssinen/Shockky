namespace Shockky.Shockwave.Lingo.Output
{
    public interface IOutputFormatter
    {   
		int Indentation { get; set; }

        void WriteToken(string token);
        void Space();

        void NewLine();
    }
}
