﻿using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
	public interface IProductRepository : IRepositoryBase<Product, Guid>
	{
		(IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, 
			int pageSize, DataTablesSearch search, string? order);

		bool IsTitleDuplicate(string title, Guid? id = null);
		Task <Product> GetProductAsync(Guid Id);
	}
}