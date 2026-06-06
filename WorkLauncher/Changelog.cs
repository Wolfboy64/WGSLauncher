using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLauncher
{
    class Changelog
    {
        public int id { get; set; }
        public string script { get; set; }
        public string leiras { get; set; }
        public string developer { get; set; }
        public DateTime datum { get; set; }

        public Changelog(int id_, string script_, string leiras_, string developer_, DateTime datum_)
        {
            this.id = id_;
            this.script = script_;
            this.leiras = leiras_;
            this.developer = developer_;
            this.datum = datum_;
        }
        public override string ToString() { return $"[{id}] {script} - {leiras} ({developer})"; }
    }
}
