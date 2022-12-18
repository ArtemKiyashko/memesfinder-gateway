using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MemesFinderGateway
{
    // public class that returns content depending on the content in tgUpdate
    public class MassageProcessFactory
    {
        public static string GetMassageProcess(Update tgUpdate)
        {

            if (tgUpdate.Type == UpdateType.Message)
            {
                return tgUpdate.Message.Text;
            }
            else if (tgUpdate.Type == UpdateType.EditedMessage)
            {
                return tgUpdate.EditedMessage.Text;
            }
            // exception handling
            else
            {
                throw new Exception("Unknown message type");
            }
        }
    }
}   
 
