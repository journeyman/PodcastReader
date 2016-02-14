using System;
using System.Linq.Expressions;
using ReactiveUI;

namespace Pr.Phone8
{
    public static class PropertyChangedExtensions
    {
        public static void RaisePropertyChanged<T, TOut>(this T This, Expression<Func<T, TOut>> expression)
            where T : IReactiveObject
        {
            This.RaisePropertyChanged(Reflection.ExpressionToPropertyNames(expression));
        }
    }
}
