using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Firmendaten
    {
        public virtual int FirmendatenID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Rechtsform { get; set; }
        public virtual string Sitz { get; set; }
        public virtual string Firmenbuchnummer { get; set; }
        public virtual string Anschrift { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefon { get; set; }
        public virtual string MitgliedWKO { get; set; }
        public virtual string Aufsichtsbehörde { get; set; }
        public virtual string Berufsbezeichnung { get; set; }
        public virtual string UIDNummer { get; set; }
        public virtual DateTime Datum { get; set; }



        //[JsonIgnore]
        //public virtual ICollection<Lending> Lendings { get; set; }

        public Firmendaten()
        {
            //this.Lendings = new HashSet<Lending>();
        }
    }
}
