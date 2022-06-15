using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Vimal.Models.Languages
{
    internal class OutputLanguage : ILanguage
    {
        public IEnumerable<Keyword> Keywords { get; } =
            new Keyword[]
            {
                        new Keyword { Word = "error", Color = Colors.Red },
                        new Keyword { Word = "Kotlin", Color = Colors.MediumPurple },
            };
    }
}
