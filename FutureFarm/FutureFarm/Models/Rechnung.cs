using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Rechnung
    {
        public virtual int RechnungID { get; set; }
        public virtual DateTime Datum { get; set; }
        public virtual bool Bezahlt { get; set; }
        public virtual DateTime BezahltAm { get; set; }
        public virtual Kunden Kunde { get; set; }
        public virtual Bestellung Bestellung { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Kunden> Kunden { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Bestellung> Bestellungen { get; set; }

        public Rechnung()
        {
            this.Kunden = new HashSet<Kunden>();
            this.Bestellungen = new HashSet<Bestellung>();
        }
    }
}
