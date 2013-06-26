using System;
using System.Linq.Expressions;
using ReactiveUI;

namespace PodcastReader.Phone8
{
    public static class PropertyChangedExtensions
    {
        public static void RaisePropertyChanged<T>(this T This, Expression<Func<T, object>> expression)
            where T : ReactiveObject
        {
            This.RaisePropertyChanged(Reflection.SimpleExpressionToPropertyName(expression));
        }
    }
}
