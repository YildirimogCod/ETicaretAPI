﻿using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories
{
    public class OrderWriteRepository:WriteRepository<Order>,IOrderWriteRepository
    {
        public OrderWriteRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
