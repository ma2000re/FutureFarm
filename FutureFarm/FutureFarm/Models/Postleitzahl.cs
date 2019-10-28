using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Postleitzahl
    {
        public virtual int PLZID { get; set; }
        public virtual string PLZ { get; set; }
        public virtual string Ortschaft { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Kunden> Kunden { get; set; }


        public Postleitzahl()
        {
            this.Kunden = new HashSet<Kunden>();
        }
    }
}
