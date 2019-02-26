using System.Collections.Generic;

namespace Shockky.Lingo
{
    public class LingoMachine
    {
        public Stack<object> Values { get; set; }

        public LingoMachine(LingoHandler handler)
        {
            Values = new Stack<object>();
        }
    }
}