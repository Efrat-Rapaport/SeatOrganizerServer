﻿using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface ICategoryBL
    {
        Task<List<Category>> GetCategoryForEventBL(int id);
        Task<List<Category>> GetAllCategoryBL();

        Task PostBL(Category[] c);
    }
}