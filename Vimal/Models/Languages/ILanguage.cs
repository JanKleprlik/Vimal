﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimal.Models.Languages
{
    internal interface ILanguage
    {
        IEnumerable<Keyword> Keywords { get; }
    }
}
