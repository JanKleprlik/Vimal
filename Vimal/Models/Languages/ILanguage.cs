using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimal.Models.Languages
{
    public interface ILanguage
    {
        /// <summary>
        /// IT DEPENS ON THE ORDER!
        /// </summary>
        IEnumerable<Keyword> Keywords { get; }

        LanguageEnum Language { get; }
    }
}
