using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Server.config
{
    public class StartupMasterSlaveConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("MasterSlave")]
        public MasterSlaveCollection MasterSlaveItems
        {
            get { return ((MasterSlaveCollection)(base["MasterSlave"])); }
        }
    }
}
