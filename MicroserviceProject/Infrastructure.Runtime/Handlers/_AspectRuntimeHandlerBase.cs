using Infrastructure.Runtime.Timing;

using System.Reflection;

namespace Infrastructure.Runtime.Handlers
{
    public abstract class AspectRuntimeHandlerBase
    {
        private static Dictionary<Type, MethodInfo[]> resolvedMethods = new Dictionary<Type, MethodInfo[]>();
        private static Dictionary<MethodInfo, object[]> resolvedMethodAttributes = new Dictionary<MethodInfo, object[]>();

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

        private void ExecuteAfterInvoke(object instance, MethodInfo method, object executionResult, params object[] parameters)
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
                    if ((attribute as IExecutionTime).ExecutionType == ExecutionType.After)
                    {
                        HandleAfterInvoke(instance, method, attribute.GetType(), executionResult, parameters);
                    }
                }
            }

            resolvedMethodAttributes[method] = attributes;
        }

        public abstract void HandleBeforeInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, params object[] passedParameters);
        public abstract void HandleAfterInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, object executionResult, params object[] passedParameters);
    }
}
