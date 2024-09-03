using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using BankingApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Service
{
    public partial class Service : IService
    {
        public async Task<MessageContainer> GetParametersByGroupCode(MessageContainer requestMessage)
        {
            EParameter eParameter = new EParameter();
            MessageContainer response = new MessageContainer();

            response.Add("ParameterList", Mapper.Map<List<DTOParameter>>(await eParameter.GetParametersByGroupCode(Mapper.Map<Parameter>(requestMessage.Get<DTOParameter>()))));

            return response;
        }
    }
}
