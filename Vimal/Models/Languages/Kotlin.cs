using System.Collections.Generic;
using Windows.UI;

namespace Vimal.Models.Languages
{
    internal class Kotlin : ILanguage
    {
        public IEnumerable<Keyword> Keywords { get; } =
            new Keyword[]
            {
                new Keyword { Word = "as?", Color = Colors.MediumPurple},
                new Keyword { Word = "break", Color = Colors.MediumPurple},
                new Keyword { Word = "class", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "continue", Color = Colors.MediumPurple},
                new Keyword { Word = "do", Color = Colors.MediumPurple},
                new Keyword { Word = "else", Color = Colors.MediumPurple},
                new Keyword { Word = "if", Color = Colors.MediumPurple},
                new Keyword { Word = "false", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "true", Color = Colors.MediumPurple},
                new Keyword { Word = "fun", Color = Colors.Orange},
                new Keyword { Word = "for", Color = Colors.MediumPurple},
                new Keyword { Word = "package", Color = Colors.DeepSkyBlue},
                new Keyword { Word = "!in" , Color = Colors.MediumPurple},
                new Keyword { Word = "in" , Color = Colors.MediumPurple},
                new Keyword { Word = "null", Color = Colors.DeepSkyBlue },
                new Keyword { Word = "this", Color = Colors.DeepSkyBlue },
                new Keyword { Word = "try" , Color = Colors.MediumPurple},
                new Keyword { Word = "typealias" , Color = Colors.DeepSkyBlue},
                new Keyword { Word = "typeof" , Color = Colors.DeepSkyBlue},
                new Keyword { Word = "var" , Color = Colors.DeepSkyBlue},
                new Keyword { Word = "val" , Color = Colors.DeepSkyBlue},
                new Keyword { Word = "object" , Color = Colors.DeepSkyBlue},
                new Keyword { Word = "when" , Color = Colors.MediumPurple},
                new Keyword { Word = "while" , Color = Colors.MediumPurple},
                new Keyword { Word = "{", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "}", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "(", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = ")", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "[", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "]", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "++", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "--", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "..", Color = Colors.Yellow, CanNeighboutLetters = true},
                new Keyword { Word = "println", Color = Colors.YellowGreen}, //Not a keyword ... i know
            };

        public LanguageEnum Language => LanguageEnum.Kotlin;
    } 
}
