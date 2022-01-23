using Infrastructure.Runtime.Attributes;

using System.Reflection;

namespace Infrastructure.Runtime.Util
{
    public class MethodExecuter
    {
        public static void ExecuteMethod(Action method, params object[] parameters)
        {
            Type classInfo = method.Method.DeclaringType;

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteMethodAttribute(method.Method);

                    m.Invoke(method.Target, parameters);

                    break;
                }
            }
        }

        public static T ExecuteMethod<T>(Func<T> method, params object[] parameters)
        {
            Type classInfo = method.Method.DeclaringType;

            var methods = classInfo.GetMethods();

            foreach (var m in methods)
            {
                if (m.Name == method.Method.Name && m.GetParameters().Count() == parameters.Count())
                {
                    ExecuteMethodAttribute(method.Method);

                    return (T)m.Invoke(method.Target, parameters);
                }
            }

            throw new Exception("Method bulunamadı");
        }

        public static T ExecuteProperty<T>(object instance, string propertyName)
        {
            T value = (T)instance.GetType().GetProperty(propertyName).GetValue(instance);

            ExecutePropertyAttribute(instance.GetType(), propertyName);

            return value;
        }

        public static T ExecuteField<T>(object instance, string fieldName)
        {
            T value = (T)instance.GetType().GetField(fieldName).GetValue(instance);

            ExecuteFieldAttribute(instance.GetType(), fieldName);

            return value;
        }

        private static void ExecuteMethodAttribute(object method)
        {
            object[] attributes = (method as MethodInfo).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is MethodExecutionAttr)
                {
                    MethodExecutionAttr executionAttr = (MethodExecutionAttr)attribute;

                    break;
                }
            }
        }

        private static void ExecutePropertyAttribute(Type classType, string propertyName)
        {
            object[] attributes = classType.GetProperty(propertyName).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is PropertyExecutionAttr)
                {
                    PropertyExecutionAttr executionAttr = (PropertyExecutionAttr)attribute;

                    break;
                }
            }
        }

        private static void ExecuteFieldAttribute(Type classType, string fieldName)
        {
            object[] attributes = classType.GetField(fieldName).GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is FieldExecutionAttr)
                {
                    FieldExecutionAttr executionAttr = (FieldExecutionAttr)attribute;

                    break;
                }
            }
        }
    }
}
