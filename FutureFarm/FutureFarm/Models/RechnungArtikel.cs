using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class RechnungArtikel
    {
        public virtual int RechnungArtikelID { get; set; }
        public virtual Rechnung Rechnung { get; set; }
        public virtual Artikel Artikel { get; set; }
        public virtual int Menge { get; set; }
        public virtual double NettoPreis { get; set; }
        public virtual double Ust { get; set; }
        public virtual bool Aktiv { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Rechnung> Rechnungen { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Artikel> Arti { get; set; }

        public RechnungArtikel()
        {
            this.Rechnungen = new HashSet<Rechnung>();
            this.Arti = new HashSet<Artikel>();
        }
    }
}
