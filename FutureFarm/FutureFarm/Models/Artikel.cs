using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Artikel
    {
        public virtual int ArtikelID { get; set; }
        public virtual int ExterneID { get; set; }
        public virtual string Bezeichnung { get; set; }
        public virtual double PreisNetto { get; set; }
        public virtual double Ust { get; set; }
        public virtual Lieferanten Lieferant { get; set; }
        public virtual int Lagerstand { get; set; }
        public virtual int Reserviert { get; set; }
        public virtual string Bild { get; set; }
        public virtual bool Aktiv { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Lieferanten> Lieferanten { get; set; }

        public Artikel()
        {
            this.Lieferanten = new HashSet<Lieferanten>();
        }
    }
}
