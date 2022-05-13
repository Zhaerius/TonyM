using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM.Models.Opts
{
    public class DiscordOptions
    {
        public ulong Token { get; set; }
        public ulong DropChannel { get; set; }
        public ulong DropGroup { get; set; }
    }
}
