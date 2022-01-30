using Infrastructure.Runtime.Defintion;

using System.Reflection;

namespace Infrastructure.Runtime.Util
{
    public class Resolver
    {
        public static string GetMethodName<T>(MethodInfo methodInfo, Type methodExecutionAttr) where T : ICustomName
        {
            if (methodExecutionAttr.GetInterfaces().Any(x => x == typeof(ICustomName)))
            {
                object[] attributes = methodInfo.GetCustomAttributes(typeof(T), false);

                foreach (object attibute in attributes)
                {
                    if (attibute.GetType() == typeof(T))
                    {
                        T t = (T)attibute;
                        return t.Name;
                    }
                }
            }

            return methodInfo.Name;
        }
    }
}
