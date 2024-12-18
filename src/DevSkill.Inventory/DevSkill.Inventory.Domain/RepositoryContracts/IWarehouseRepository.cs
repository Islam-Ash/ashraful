﻿using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
	public interface IWarehouseRepository : IRepositoryBase<Warehouse, Guid>
	{
		Task<(IList<Warehouse> data, int total, int totalDisplay)> GetPagedWarehousesAsync(int pageIndex,
		   int pageSize, DataTablesSearch search, string? order);
		Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);
	}
}
