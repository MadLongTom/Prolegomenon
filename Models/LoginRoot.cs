using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolegomenon.Models
{
    internal class LoginRoot
    {
        public class FlagRoot
        {
            public string Status { get; set; }
        }

        public List<FlagRoot> Flag { get; set; }
    }
}
