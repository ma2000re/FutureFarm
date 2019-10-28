using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Bestellung
    {
        public virtual int BestellungID { get; set; }
        public virtual Kunden Kunde { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime Lieferdatum { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Kunden> Kunden { get; set; }

        public Bestellung()
        {
            this.Kunden = new HashSet<Kunden>();
        }
    }
}
