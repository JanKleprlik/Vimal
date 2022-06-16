using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Vimal.Models.Languages
{
    internal class Swift : ILanguage
    {
        public IEnumerable<Keyword> Keywords { get; } =
            new Keyword[]
            {
                new Keyword { Word = "as", Color = Colors.MediumPurple},
                new Keyword { Word = "break", Color = Colors.MediumPurple},
                new Keyword { Word = "Class", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "Elass", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "continue", Color = Colors.MediumPurple},
                new Keyword { Word = "do", Color = Colors.MediumPurple},
                new Keyword { Word = "else", Color = Colors.MediumPurple},
                new Keyword { Word = "if", Color = Colors.MediumPurple},
                new Keyword { Word = "false", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "true", Color = Colors.MediumPurple},
                new Keyword { Word = "Func", Color = Colors.Orange},
                new Keyword { Word = "for", Color = Colors.MediumPurple},
                new Keyword { Word = "switch", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "in" , Color = Colors.MediumPurple},
                new Keyword { Word = "public", Color = Colors.DeepSkyBlue },
                new Keyword { Word = "private", Color = Colors.DeepSkyBlue },
                new Keyword { Word = "{", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "}", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "(", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = ")", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "[", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "]", Color = Colors.Yellow, CanNeighboutLetters = true},
            };

        public LanguageEnum Language => LanguageEnum.Swift;
    }
}
