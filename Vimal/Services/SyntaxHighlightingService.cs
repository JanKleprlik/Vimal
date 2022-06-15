using System;
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
                int prevIdx = 0;
                int index = line.IndexOf(keyword.Word);
                while (index != -1)
                {
                    //code before keyword
                    //indexes.Add(new CodePart{
                    //    StartIndex = prevIdx,
                    //    Length = index - prevIdx,
                    //    Text = line.Substring(prevIdx, index-prevIdx),
                    //    KeyWord = null,
                    //    IsKeyword = false
                    //});

                    //KeyWord itself
                    indexes.Add(new CodePart
                    {
                        StartIndex = index,
                        Length = keyword.Word.Length,
                        Text = line.Substring(index, keyword.Word.Length),
                        KeyWord = keyword,
                        IsKeyword = true
                    });

                    //advance the search
                    prevIdx = index;
                    index = line.IndexOf(keyword.Word, index + keyword.Word.Length);
                }
                //add the rest
                //indexes.Add(new CodePart
                //{
                //    StartIndex = prevIdx,
                //    Length = line.Length - prevIdx,
                //    Text = line.Substring(prevIdx, line.Length - prevIdx),
                //    KeyWord = null,
                //    IsKeyword = false
                //});
            }
            return indexes;
        }

    }

    internal class CodePart
    {
        public bool IsKeyword { get; set; }
        public string Text { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public Keyword KeyWord { get; set; }
    }
}
