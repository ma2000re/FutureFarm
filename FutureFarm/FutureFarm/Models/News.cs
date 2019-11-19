using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class News
    {
        public virtual int NewsID { get; set; }
        public virtual string Titel { get; set; }
        public virtual string Beitrag { get; set; }
        public virtual DateTime Datum { get; set; }
        public virtual Login Login { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Login> Logins { get; set; }

        public News()
        {
            this.Logins = new HashSet<Login>();
        }
    }
}
