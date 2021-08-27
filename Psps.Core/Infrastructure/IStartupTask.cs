namespace Psps.Core.Infrastructure
{
    public interface IStartupTask
    {
        int Order { get; }

        void Execute();
    }
}