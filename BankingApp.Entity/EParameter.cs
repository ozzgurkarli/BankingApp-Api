﻿using BankingApp.Entity.Config;
using BankingApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Entity
{
    public class EParameter
    {
        public readonly BankingDbContext database = new BankingDbContext();

        public async Task<List<Parameter>> GetParametersByGroupCode(Parameter par)
        {
            using (var context = new BankingDbContext())
            {
                return await context.Parameter.Where(x => x.GroupCode.Equals(par.GroupCode)).ToListAsync();
            }
        }
        
        public async Task<List<Parameter>> GetParametersByMultipleGroupCode(List<Parameter> parList)
        {
            using (var context = new BankingDbContext())
            {
                return await context.Parameter.Where(x => parList.Select(y=> y.GroupCode).ToList().Contains(x.GroupCode)).ToListAsync();
            }
        }

        public async Task<Parameter> GetParameter(Parameter par)
        {
            return await database.Parameter.FirstOrDefaultAsync(x => x.GroupCode.Equals(par.GroupCode) && x.Code.Equals(par.Code));
        }

        public async Task<Parameter> UpdateParameter(Parameter item){
            using (var context = new BankingDbContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                item = (context.Parameter.Update(item)).Entity;

                await context.SaveChangesAsync();
            }

            return item;
        }
    }
}
