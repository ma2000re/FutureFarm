using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class BestellungArtikel
    {
        public virtual int BestellungArtikelID { get; set; }
        public virtual Bestellung Bestellung { get; set; }
        public virtual Artikel Artikel { get; set; }
        public virtual int Menge { get; set; }
        public virtual double Nettopreis { get; set; }
        public virtual double Ust { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Bestellung> Bestellungen { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Artikel> Arti { get; set; }

        public BestellungArtikel()
        {
            this.Bestellungen = new HashSet<Bestellung>();
            this.Arti = new HashSet<Artikel>();
        }
    }
}
