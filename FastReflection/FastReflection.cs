using System;
using System.Collections.Concurrent;

namespace OperationSystem.FastReflection
{
    public class FastReflection
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, IAccessor>> Cache 
            = new ConcurrentDictionary<Type, ConcurrentDictionary<string, IAccessor>>();

        private static IAccessor MakeAccessor(object _object, string propertyName)
        {
            var propertyInfo = _object.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                return null;
            
            var getterDelegateType = typeof(Func<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            var getter = Delegate.CreateDelegate(getterDelegateType, propertyInfo.GetGetMethod());

            var setterDelegateType = typeof(Action<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            var setter = Delegate.CreateDelegate(setterDelegateType, propertyInfo.GetSetMethod());

            var accessorType = typeof(PropertyAccessor<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (IAccessor)Activator.CreateInstance(accessorType, getter, setter);
        }

        private static IAccessor GetAccesser(object _object, string propertyName)
        {
            var accessers = Cache.GetOrAdd(_object.GetType(), x => new ConcurrentDictionary<string, IAccessor>());
            return accessers.GetOrAdd(propertyName, x => MakeAccessor(_object, propertyName));
        }

        public static void SetProperty(object _object, string property, object value)
        {
            GetAccesser(_object,property).SetValue(_object,value);
        }

        public static object GetProperty(object _object , string property)
        {
            return GetAccesser(_object, property).GetValue( _object);
        }
    }
}
