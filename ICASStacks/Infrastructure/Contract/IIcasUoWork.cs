namespace ICASStacks.Infrastructure.Contract
{
    internal interface IIcasUoWork
    {
        void SaveChanges();
        IcasContext Context { get; }
    }
}
