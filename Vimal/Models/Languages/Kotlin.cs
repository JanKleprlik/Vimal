using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimal.Models.Languages
{
    internal class Kotlin : ILanguage
    {

        public IEnumerable<Keyword> Keywords { get; } =
            new Keyword[]
            {
                new Keyword { Word = "for" },
                new Keyword { Word = "while" },
                new Keyword { Word = "if" },
                new Keyword { Word = "else" }
            };
    } 
}
