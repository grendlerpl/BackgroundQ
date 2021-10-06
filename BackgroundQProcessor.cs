using System.Threading.Tasks;

namespace CodeServiceJD.BackgroundQ
{
    public interface IBackgroundQProcessor<T>
    {
        Task ProcessQElementAsync(T element);
    }

    public abstract class BackgroundQProcessor<T> : IBackgroundQProcessor<T>
    {
        public abstract Task ProcessQElementAsync(T element);
    }
}
