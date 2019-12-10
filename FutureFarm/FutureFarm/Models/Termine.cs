using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Termine
    {
        public virtual int TerminID { get; set; }
        public virtual string Titel { get; set; }
        public virtual string Beschreibung { get; set; }
        public virtual DateTime DatumVon { get; set; }
        public virtual DateTime DatumBis { get; set; }
        public virtual DateTime UhrzeitVon { get; set; }
        public virtual DateTime UhrzeitBis { get; set; }
        public virtual Login Login { get; set; }
        public virtual bool Aktiv { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Login> Logins { get; set; }

        public Termine()
        {
            this.Logins = new HashSet<Login>();
        }
    }
}
