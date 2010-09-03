using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    public class Command
    {
        public Command()
        {
            //throw new NotImplementedException();
        }
        public virtual void Run(Player ply, string[] cmdparts)
        {
            throw new NotImplementedException();
        }

        private bool m_minlevel_set = false;
        private int m_minlevel = 0;
        public virtual int minlevel { get { if (m_minlevel_set) return m_minlevel; else return reqlevel; } set { m_minlevel = value; m_minlevel_set = true; } }
        
        public virtual int reqlevel { get { throw new NotImplementedException(); } }

        public virtual string Help { get { throw new NotImplementedException(); } }
        public virtual string Usage { get { throw new NotImplementedException(); } }
    }
}
