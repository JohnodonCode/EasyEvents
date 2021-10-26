using System;
using System.Linq;
using CommandSystem;
using EasyEvents.Types;
using Exiled.API.Features;

namespace EasyEvents.API.EventArgs
{
    /// <summary>
    /// Contains all the information before an EasyEvents event starts
    /// </summary>
    public class StartingEventEventArgs : System.EventArgs
    {
        /// <inheritdoc/>
        public StartingEventEventArgs(ICommandSender sender, EventData eventdata, bool isAllowed = true)
        {
            Sender = sender;
            IsAllowed = isAllowed;
            eventData = eventdata;
        }

        /// <summary>
        /// Gets the <see cref="CommandSystem.ICommandSender"/> who ran the command, if any. Null if another plugin ran the event.
        /// </summary>
        public ICommandSender Sender { get; }

        /// <summary>
        /// Gets the <see cref="EventData"/> of the event.
        /// </summary>
        public EventData eventData { get; }

        /// <summary>
        /// Gets or sets a value indicating if the event will be run.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}