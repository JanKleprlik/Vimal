using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Vimal.Models.Languages
{
    internal class Kotlin : ILanguage
    {

        public IEnumerable<Keyword> Keywords { get; } =
            new Keyword[]
            {
                new Keyword { Word = "forcycle", Color = Colors.Orange},
                new Keyword { Word = "for", Color = Colors.MediumPurple},
                new Keyword { Word = "package", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "while" },
                new Keyword { Word = "if" },
                new Keyword { Word = "else" },
                new Keyword { Word = "println", Color = Colors.YellowGreen},
                new Keyword { Word = "{", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "}", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "(", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = ")", Color = Colors.Yellow, CanNeighboutLetters = true},
            };
    } 
}
