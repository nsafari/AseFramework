using System.Reflection;

namespace Ase.Messaging.Messaging.Annotation
{
    public interface IParameterResolverFactory
    {
        IParameterResolver<T>? CreateInstance<T>(MethodBase executable, ParameterInfo[] parameters, int parameterIndex);

    }
}