using System;

namespace OperationSystem.FastReflection
{
    public interface IAccessor
    {
        object GetValue(object target);

        void SetValue(object target, object value);
    }

    internal sealed class PropertyAccessor<TTarget, TProperty> : IAccessor
    {
        private readonly Func<TTarget, TProperty> _getter;
        private readonly Action<TTarget, TProperty> _setter;

        public PropertyAccessor(Func<TTarget, TProperty> getter, Action<TTarget, TProperty> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        public object GetValue(object target)
        {
            return _getter((TTarget)target);
        }

        public void SetValue(object target, object value)
        {
            _setter((TTarget)target, (TProperty)value);
        }
    }
}
