using Infrastructure.Runtime.Defintion;

using System.Reflection;

namespace Infrastructure.Runtime.Util
{
    /// <summary>
    /// Çözümleyici sınıf
    /// </summary>
    public class Resolver
    {
        /// <summary>
        /// Method adını çözümleyen sınıf
        /// </summary>
        /// <typeparam name="T">Özel method ismi tanımlayan öznitelik sınıfının tipi</typeparam>
        /// <param name="methodInfo">Method bilgisi</param>
        /// <param name="methodExecutionAttr">Özel method ismi tanımlayan öznitelik sınıfının tipi</param>
        /// <returns></returns>
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
