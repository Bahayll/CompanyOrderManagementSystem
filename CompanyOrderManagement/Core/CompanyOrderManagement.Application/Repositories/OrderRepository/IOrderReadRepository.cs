﻿using CompanyOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Repositories.OrderRepository
{
    public interface IOrderReadRepository : IReadRepository<Order>
    {
    }
}
