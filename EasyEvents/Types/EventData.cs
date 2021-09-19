using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEvents.Types
{
    public class EventData
    {
        public string EventText;
        public string EventName;

        public EventData(string eventText, string eventName)
        {
            EventText = eventText;
            EventName = eventName;
        }
    }
}
