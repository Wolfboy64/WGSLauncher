using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLauncher
{
    public class accounts
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string serial { get; set; }
        public string ip { get; set; }

        public int online { get; set; }
        public string banDatas { get; set; }


        public accounts(int id_, string name_, string password_, string email_, string serial_, string ip_, int online_, string banDatas_)
        {
            this.id = id_;
            this.name = name_;
            this.password = password_;
            this.email = email_;
            this.serial = serial_;
            this.ip = ip_;
            this.online = online_;
            this.banDatas = banDatas_;
        }
        public override string ToString()
        {
            return $"{id} {name} {email} {serial} {ip} {online} {banDatas}";
        }
    }
}
