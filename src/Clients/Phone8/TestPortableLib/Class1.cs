using ReactiveUI;

namespace TestPortableLib
{
    public interface Interface1 : IReactiveNotifyPropertyChanged<IReactiveObject>, IHandleObservableErrors { }

    public class Class1 : ReactiveObject, Interface1
    {
    }
}
