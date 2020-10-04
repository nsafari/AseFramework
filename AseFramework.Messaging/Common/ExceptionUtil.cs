using System;
using System.Reflection;

namespace Ase.Messaging.Common
{
    public static class ExceptionUtil
    {
        public static Exception  AddInnerException(this Exception originalException, Exception? innerException)
        {
            var parentException = originalException; 
            while (parentException?.InnerException != null)
            {
                parentException = parentException?.InnerException;
            }

            typeof(Exception)
                .GetField("_innerException", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(parentException , innerException);
            
            return originalException;
        }
    }
}