using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vimal.Models.Languages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Vimal.Services
{
    internal static class SyntaxHighlightingService
    {

        public static void Highlight(string text, TextBlock TB, ILanguage language)
        {
            var indexes = GetKeywordIndexes(text, language.Keywords);
            
            indexes = indexes.Distinct(new CodePartEqualityComparer()).ToList();
            indexes.Sort((x, y) => x.StartIndex.CompareTo(y.StartIndex));

            int lastIndex = 0;
            foreach (var codePart in indexes)
            {
                if (codePart.StartIndex > lastIndex)
                {
                    TB.Inlines.Add(new Run() { Text = text.Substring(lastIndex, codePart.StartIndex - lastIndex) });
                }

                TB.Inlines.Add(new Run() { Text = text.Substring(codePart.StartIndex, codePart.Length), Foreground = new SolidColorBrush(codePart.KeyWord.Color)});
                lastIndex = codePart.StartIndex + codePart.Length;
            }
            
            if (lastIndex < text.Length)
            {
                TB.Inlines.Add(new Run() { Text = text.Substring(lastIndex) });
            }

        }

        //Find all starting idnexes of keywords in given string
        private static List<CodePart> GetKeywordIndexes(string line, IEnumerable<Keyword> keywords)
        {
            List<CodePart> indexes = new List<CodePart>();
            foreach (var keyword in keywords)
            {
                int index = line.IndexOf(keyword.Word);
                while (index != -1)
                {
                        //can neighbout letters
                    if (keyword.CanNeighboutLetters ||
                        //check front
                        ((index == 0 || //is at the start
                        ('A' > line[index - 1] || 'z' < line[index - 1])) //is not part of a word
                        &&
                        //check back
                        (index + keyword.Word.Length == line.Length || //is at the end
                        ('A' > line[index + keyword.Word.Length] || 'z' < line[index + keyword.Word.Length])) //is not par of a word
                       ))
                    {
                        //KeyWord itself
                        indexes.Add(new CodePart
                        {
                            StartIndex = index,
                            Length = keyword.Word.Length,
                            Text = line.Substring(index, keyword.Word.Length),
                            KeyWord = keyword,
                            IsKeyword = true
                        });
                    }

                    //advance the search
                    index = line.IndexOf(keyword.Word, index + keyword.Word.Length);
                }
            }
            return indexes;
        }

    }

    internal class CodePart : IEqualityComparer<CodePart>
    {
        public bool IsKeyword { get; set; }
        public string Text { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public Keyword KeyWord { get; set; }

        public bool Equals(CodePart x, CodePart y)
        {
            return (x as CodePart).StartIndex == (y as CodePart).StartIndex;
        }

        public int GetHashCode(CodePart obj)
        {
            return obj.StartIndex.GetHashCode();
        }
    }

    internal class CodePartEqualityComparer : IEqualityComparer<CodePart>
    {
        public bool Equals(CodePart x, CodePart y)
        {
            return (x as CodePart).StartIndex == (y as CodePart).StartIndex;
        }

        public int GetHashCode(CodePart obj)
        {
            return obj.StartIndex.GetHashCode();
        }
    }
}
