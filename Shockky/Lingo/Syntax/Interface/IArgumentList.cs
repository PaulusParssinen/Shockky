using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public interface IArgumentList
    {
        IList<Expression> Items { get; set; }
    }
}
