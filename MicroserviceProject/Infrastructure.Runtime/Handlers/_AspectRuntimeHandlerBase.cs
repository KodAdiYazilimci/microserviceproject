using Infrastructure.Runtime.Timing;

using System.Reflection;

namespace Infrastructure.Runtime.Handlers
{
    /// <summary>
    /// Çalışma zamanı denetim sağlayıcı sınıfların temeli
    /// </summary>
    public abstract class AspectRuntimeHandlerBase
    {
        /// <summary>
        /// Çözümlenen metotlar
        /// </summary>
        private static Dictionary<Type, MethodInfo[]> resolvedMethods = new Dictionary<Type, MethodInfo[]>();

        /// <summary>
        /// Çözümlenen metot öznitelikleri
        /// </summary>
        private static Dictionary<MethodInfo, object[]> resolvedMethodAttributes = new Dictionary<MethodInfo, object[]>();

        /// <summary>
        /// Bir methodu çalıştırır
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodName">Methodun adı</param>
        /// <param name="parameters">Methoda verilecek parametreler</param>
        public void ExecuteMethod(object instance, string methodName, params object[] parameters)
        {
            MethodInfo[] methods =
                resolvedMethods.ContainsKey(instance.GetType())
                ?
                resolvedMethods[instance.GetType()]
                : instance.GetType().GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == methodName && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke(instance, m, parameters);

                    m.Invoke(instance, parameters);

                    ExecuteAfterInvoke(instance, m, null, parameters);

                    break;
                }
            }

            resolvedMethods[instance.GetType()] = methods;
        }

        /// <summary>
        /// Statik bir methodu çalıştırır
        /// </summary>
        /// <param name="classType">Methodun ait olduğu sınıfın tipi</param>
        /// <param name="methodName">Methodun adı</param>
        /// <param name="parameters">Methoda verilecek parametreler</param>
        public void ExecuteStaticMethod(Type classType, string methodName, params object[] parameters)
        {
            MethodInfo[] methods =
                      resolvedMethods.ContainsKey(classType)
                      ?
                      resolvedMethods[classType]
                      : classType.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == methodName && m.GetParameters().Count() == parameters.Count() && m.IsStatic)
                {
                    ExecuteBeforeInvoke(null, m, parameters);

                    m.Invoke(null, parameters);

                    ExecuteAfterInvoke(null, m, null, parameters);

                    break;
                }
            }

            resolvedMethods[classType] = methods;
        }

        /// <summary>
        /// Sonuç döndüren bir methodu çalıştırır
        /// </summary>
        /// <typeparam name="T">Methodun dönüş tipi</typeparam>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodName">Methodun adı</param>
        /// <param name="parameters">Methoda verilecek parametreler</param>
        /// <returns></returns>
        public T ExecuteResultMethod<T>(object instance, string methodName, params object[] parameters)
        {
            MethodInfo[] methods =
                      resolvedMethods.ContainsKey(instance.GetType())
                      ?
                      resolvedMethods[instance.GetType()]
                      : instance.GetType().GetMethods();

            T result = default(T);

            bool foundResult = false;

            foreach (var m in methods)
            {
                if (m.Name == methodName && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke(instance, m, parameters);

                    result = (T)m.Invoke(instance, parameters);
                    foundResult = true;

                    ExecuteAfterInvoke(instance, m, null, parameters);

                    break;
                }
            }

            resolvedMethods[instance.GetType()] = methods;

            return foundResult ? result : throw new Exception("Method bulunamadı");
        }

        /// <summary>
        /// Sonuç döndüren bir methodu çalıştırır
        /// </summary>
        /// <typeparam name="T">Methodun dönüş tipi</typeparam>
        /// <param name="method">Çalıştırılacak method</param>
        /// <param name="parameters">Methoda verilecek parametreler</param>
        /// <returns></returns>
        public T ExecuteResultMethod<T>(Func<T> method, params object[] parameters)
        {
            MethodInfo[] methods =
                      resolvedMethods.ContainsKey(method.Method.DeclaringType)
                      ?
                      resolvedMethods[method.Method.DeclaringType]
                      : method.Method.DeclaringType.GetMethods();

            T result = default(T);

            bool foundResult = false;

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke(null, m, parameters);

                    result = (T)m.Invoke(method.Target, parameters);

                    foundResult = true;

                    ExecuteAfterInvoke(null, m, null, parameters);

                    return result;
                }
            }

            resolvedMethods[method.Method.DeclaringType] = methods;

            return foundResult ? result : throw new Exception("Method bulunamadı");
        }

        /// <summary>
        /// Çalıştırılmadan önce tetiklenecek method
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="method">Çalıştırılacak hedef methodun bilgisi</param>
        /// <param name="parameters">Çalıştırılan methoda verilen parametreler</param>
        private void ExecuteBeforeInvoke(object instance, MethodInfo method, params object[] parameters)
        {
            object[] attributes =
                resolvedMethodAttributes.ContainsKey(method)
                ?
                resolvedMethodAttributes[method]
                :
                (method as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is IExecutionTime)
                {
                    if ((attribute as IExecutionTime).ExecutionType == ExecutionType.Before)
                    {
                        HandleBeforeInvoke(instance, method, attribute.GetType(), parameters);
                    }
                }
            }

            resolvedMethodAttributes[method] = attributes;
        }

        /// <summary>
        /// Çalıştırıldıktan sonra tetiklenecek method
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodInfo">Çalıştırılacak hedef methodun bilgisi</param>
        /// <param name="executionResult">Çalıştırılma sonrası hedef methodun dönüş değeri</param>
        /// <param name="parameters">Çalıştırılan methoda verilen parametreler</param>
        private void ExecuteAfterInvoke(object instance, MethodInfo methodInfo, object executionResult, params object[] parameters)
        {
            object[] attributes =
                resolvedMethodAttributes.ContainsKey(methodInfo)
                ?
                resolvedMethodAttributes[methodInfo]
                :
                (methodInfo as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is IExecutionTime)
                {
                    if ((attribute as IExecutionTime).ExecutionType == ExecutionType.After)
                    {
                        HandleAfterInvoke(instance, methodInfo, attribute.GetType(), executionResult, parameters);
                    }
                }
            }

            resolvedMethodAttributes[methodInfo] = attributes;
        }

        /// <summary>
        /// Method çalıştırma öncesinde çağrılacak method
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodInfo">Çalıştırılacak hedef methodun bilgisi</param>
        /// <param name="methodExecutionAttr">Çalıştırılacak methoda atanmış öznitelik</param>
        /// <param name="parameters">Çalıştırılacak methoda verilen parametreler</param>
        public abstract void HandleBeforeInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, params object[] passedParameters);

        /// <summary>
        /// Method çalıştırma sonrasında çağrılan method
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodInfo">Çalıştırılacak hedef methodun bilgisi</param>
        /// <param name="methodExecutionAttr">Çalıştırılan methoda atanmış öznitelik</param>
        /// <param name="executionResult">Çalıştırılma sonrası hedef methodun dönüş değeri</param>
        /// <param name="parameters">Çalıştırılan methoda verilen parametreler</param>
        public abstract void HandleAfterInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, object executionResult, params object[] passedParameters);
    }
}
