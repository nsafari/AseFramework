using System;
using NHibernate.Mapping.Attributes;

namespace Ase.Messaging.CommandHandling
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandHandlerAttribute: Attribute
    {
        
    }
}