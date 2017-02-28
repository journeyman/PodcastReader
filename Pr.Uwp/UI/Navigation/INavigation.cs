using System.Threading.Tasks;

namespace Pr.Uwp.UI.Navigation
{
    public interface INavigation
    {
        Task Completion { get; }
    }
}