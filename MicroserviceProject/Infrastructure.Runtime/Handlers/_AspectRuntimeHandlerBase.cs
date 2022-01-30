using Infrastructure.Runtime.Timing;

using System.Reflection;

namespace Infrastructure.Runtime.Handlers
{
    public abstract class AspectRuntimeHandlerBase
    {
        public void ExecuteMethod(object instance, string methodName, params object[] parameters)
        {
            Type classInfo = instance.GetType();

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == methodName && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke(m, parameters);

                    m.Invoke(instance, parameters);

                    ExecuteAfterInvoke(m, null, parameters);

                    break;
                }
            }
        }

        public void ExecuteStaticMethod(Type classType, string methodName, params object[] parameters)
        {
            var methods = classType.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == methodName && m.GetParameters().Count() == parameters.Count() && m.IsStatic)
                {
                    ExecuteBeforeInvoke(m, parameters);

                    m.Invoke(null, parameters);

                    ExecuteAfterInvoke(m, null, parameters);

                    break;
                }
            }
        }

        public T ExecuteResultMethod<T>(object instance, string methodName, params object[] parameters)
        {
            Type classInfo = instance.GetType();

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == methodName && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke(m, parameters);

                    T result = (T)m.Invoke(instance, parameters);

                    ExecuteAfterInvoke(m, null, parameters);

                    return result;
                }
            }

            throw new Exception("Method bulunamadı");
        }

        public T ExecuteResultMethod<T>(Func<T> method, params object[] parameters)
        {
            Type classInfo = method.Method.DeclaringType;

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke(m, parameters);

                    T result = (T)m.Invoke(method.Target, parameters);

                    ExecuteAfterInvoke(m, null, parameters);

                    return result;
                }
            }

            throw new Exception("Method bulunamadı");
        }

        private void ExecuteBeforeInvoke(MethodInfo method, params object[] parameters)
        {
            object[] attributes = (method as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is IExecutionTime)
                {
                    if ((attribute as IExecutionTime).ExecutionType == ExecutionType.Before)
                    {
                        HandleBeforeInvoke(method, attribute.GetType(), parameters);
                    }
                }
            }
        }

        private void ExecuteAfterInvoke(MethodInfo method, object executionResult, params object[] parameters)
        {
            object[] attributes = (method as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is IExecutionTime)
                {
                    if ((attribute as IExecutionTime).ExecutionType == ExecutionType.After)
                    {
                        HandleAfterInvoke(method, attribute.GetType(), parameters);
                    }
                }
            }
        }

        public abstract void HandleBeforeInvoke(MethodInfo methodInfo, Type methodExecutionAttr, params object[] passedParameters);
        public abstract void HandleAfterInvoke(MethodInfo methodInfo, Type methodExecutionAttr, object executionResult, params object[] passedParameters);
    }
}
