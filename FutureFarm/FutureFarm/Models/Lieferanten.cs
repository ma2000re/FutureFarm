using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Lieferanten
    {
        public virtual int LieferantenID { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
        public virtual string Firma { get; set; }
        public virtual string Telefonnummer { get; set; }
        public virtual string Email { get; set; }
        public virtual string UID { get; set; }
        public virtual string Strasse { get; set; }
        public virtual Postleitzahl Postleitzahl { get; set; }
        public virtual bool Aktiv { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Postleitzahl> PLZ { get; set; }

        public Lieferanten()
        {
            this.PLZ = new HashSet<Postleitzahl>();
        }
    }
}
