using Infrastructure.Runtime.Attributes;

using System.Reflection;

namespace Infrastructure.Runtime.Handlers
{
    public abstract class AspectRuntimeHandlerBase<TAttribute> where TAttribute : Attribute
    {
        public void ExecuteMethod(Action method, params object[] parameters)
        {
            Type classInfo = method.Method.DeclaringType;

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke<TAttribute>(method.Method, parameters);

                    m.Invoke(method.Target, parameters);

                    ExecuteAfterInvoke<TAttribute>(method.Method, null, parameters);

                    break;
                }
            }
        }

        public T ExecuteMethod<T>(Func<T> method, params object[] parameters)
        {
            Type classInfo = method.Method.DeclaringType;

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke<TAttribute>(method.Method, parameters);

                    T result = (T)m.Invoke(method.Target, parameters);

                    ExecuteAfterInvoke<TAttribute>(method.Method, result, parameters);

                    return result;
                }
            }

            throw new Exception("Method bulunamadı");
        }

        public TResult ExecuteMethod<TParameter, TParameter2, TResult>(Func<TParameter, TParameter2, TResult> method, params object[] parameters)
        {
            Type classInfo = method.Method.DeclaringType;

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteBeforeInvoke<TAttribute>(method.Method, parameters);

                    TResult result = (TResult)m.Invoke(method.Target, parameters);

                    ExecuteAfterInvoke<TAttribute>(method.Method, result, parameters);

                    return result;
                }
            }

            throw new Exception("Method bulunamadı");
        }

        private void ExecuteBeforeInvoke<T>(object method, params object[] passedParameters) where T : TAttribute
        {
            object[] attributes = (method as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is MethodExecutionAttr)
                {
                    T executionAttr = (T)attribute;

                    HandleBeforeInvoke(executionAttr, passedParameters);

                    break;
                }
            }
        }

        private void ExecuteAfterInvoke<T>(object method, object executionResult, params object[] passedParameters) where T : TAttribute
        {
            object[] attributes = (method as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is MethodExecutionAttr)
                {
                    T executionAttr = (T)attribute;

                    HandleAfterInvoke(executionAttr, executionResult, passedParameters);

                    break;
                }
            }
        }

        public abstract void HandleBeforeInvoke(TAttribute methodExecutionAttr, params object[] passedParameters);
        public abstract void HandleAfterInvoke(TAttribute methodExecutionAttr, object executionResult, params object[] passedParameters);
    }
}
