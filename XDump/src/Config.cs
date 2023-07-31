using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDump.src
{
    public enum XWormConfiguration
    {  
        HOST                    = 0x04000006,
        PORT                    = 0x04000007,
        ENCRYPTION_KEY          = 0x04000008,
        SPL                     = 0x04000009,
        USB_SPREAD_NAME         = 0x0400000B,
        INSTALLATION_DIRECTORY  = 0x0400000C,
        MUTEX                   = 0x0400000D,
        KEY_LOGGER_PATH         = 0x0400000E,
        BITCOIN_ADDRESS         = 0x0400000F,
        ETHEREUM_ADDRESS        = 0x04000010,
        TRC20_ADDRESS           = 0x04000011,
        TELEGRAM_TOKEN          = 0x04000012,
        TELEGRAM_CHAT_ID        = 0x04000013
    }
}
