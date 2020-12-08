using System;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command
{
    [AttributeUsage(AttributeTargets.Method)]
    [MessageHandler(MessageType = typeof(ICommandMessage<>))]
    public class CommandHandlerInterceptorAttribute : Attribute
    {
    }
}