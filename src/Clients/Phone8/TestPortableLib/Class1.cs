using ReactiveUI;

namespace TestPortableLib
{
    public interface Interface1 : IReactiveObject { }

    public class Class1 : ReactiveObject, Interface1
    {
    }
}
