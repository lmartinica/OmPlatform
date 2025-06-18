namespace OmPlatform.Interfaces
{
    public interface IOrderUnitOfWork
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        Task<int> SaveAsync();
    }
}
