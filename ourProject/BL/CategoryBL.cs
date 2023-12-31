﻿using DL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL//Hello for try git!//
{
   public class CategoryBL:ICategoryBL
    {
        ICategoryDL icategorydl;
        public CategoryBL(ICategoryDL icategorydl)
        {
            this.icategorydl = icategorydl;
        }
        public async Task<List<Category>> GetCategoryForEventBL(int id)
        {
            return await icategorydl.GetCategoryForEventDL(id);
        }
        public async Task<List<Category>> GetAllCategoryBL()
        {
            return await icategorydl.GetAllCategoryDL();
        }

        public async Task PostBL(Category[] c)
        {
            await icategorydl.PostDL(c);
        }
    }
}
