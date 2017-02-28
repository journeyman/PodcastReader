using System.Threading.Tasks;

namespace Pr.Uwp.UI.Navigation
{
    public class Navigation : INavigation
    {
        public Navigation(Task completion)
        {
            Completion = completion;
        }

        public Task Completion { get; }
    }
}