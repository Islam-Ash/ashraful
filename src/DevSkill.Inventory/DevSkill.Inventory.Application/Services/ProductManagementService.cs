using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
	public class ProductManagementService : IProductManagementService
	{
		private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

		public ProductManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
		{
			_inventoryUnitOfWork = invertoryUnitOfWork;
		}

		public void CreateProduct(Product product)
		{
			if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.ProductName))
			{
				_inventoryUnitOfWork.ProductRepository.Add(product);
				_inventoryUnitOfWork.Save();
			}
		}

		public void DeleteProduct(Guid id)
		{
			_inventoryUnitOfWork.ProductRepository.Remove(id);
			_inventoryUnitOfWork.Save();
		}

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ProductRepository.GetProductAsync(id);
        }

        public (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize, DataTablesSearch search, string? order)
		{
			return _inventoryUnitOfWork.ProductRepository.GetPagedProducts(pageIndex, pageSize, search, order);
		}

		public void UpdateProduct(Product product)
		{
			if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.ProductName, product.Id))
			{
				_inventoryUnitOfWork.ProductRepository.Edit(product);
				_inventoryUnitOfWork.Save();
			}
			else
				throw new InvalidOperationException("Title Should be Unique");
			
		}
	}
}
