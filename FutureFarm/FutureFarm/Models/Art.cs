using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Art
    {
        public virtual int ArtID { get; set; }
        public virtual string Bezeichnung { get; set; }

        public Art()
        {
        }
    }
}
