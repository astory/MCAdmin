using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class RulesCommand : Command
    {
        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Displays the rules file"; } }
        public override string Usage { get { return ""; } }
    }
}
