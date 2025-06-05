namespace OmPlatform.Repositories
{
    public interface IOrderUnitOfWork
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        Task<int> CompleteAsync();
    }
}
