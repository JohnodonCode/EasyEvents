using Exiled.Events.Extensions;
using EasyEvents.API.EventArgs;
using static Exiled.Events.Events;
using EasyEvents.API;
namespace EasyEvents.Handlers
{
    /// <summary>
    /// Handler for this plugin's events.
    /// </summary>
    public static class Events
    {
        /// <inheritdoc cref="StartingEventEventArgs"/>
        public static event CustomEventHandler<StartingEventEventArgs> StartingEvent;

        /// <summary>
        /// Safely invokes <see cref="StartingEvent"/> event.
        /// </summary>
        /// <param name="ev"></param>
        public static void OnStartingEvent(StartingEventEventArgs ev) => StartingEvent?.InvokeSafely(ev);
    }
}
