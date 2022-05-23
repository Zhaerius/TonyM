using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonyM.Core.Interfaces;

namespace TonyM.Core.Services
{
    public class SearchStatutService : ISearchStatutService
    {
        public bool IsSearching { get; set; } = false;
    }
}
