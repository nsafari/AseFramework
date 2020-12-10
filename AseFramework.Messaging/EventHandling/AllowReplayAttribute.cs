using System;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Annotation marking a Handler (or class) as being capable of handling replays, or not, depending on the value
    /// passed.
    /// When placed on the type (class) level, the setting applies to all handlers that don't explicitly override it
    /// on the method level.
    /// Marking methods as not allowing replay will not change the routing of a message (i.e. will not invoke another
    /// handler method). Messages that would otherwise be handled by such handler are simply ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowReplayAttribute: Attribute
    {

        /// <summary>
        /// Whether to allow replays on this handler, or not. Defaults to {@code true}
        /// <returns>Whether to allow replays on this handler, or not</returns>
        /// </summary>
        private bool Value { get; set; } = true;
    }
}