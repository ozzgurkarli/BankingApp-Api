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
            Parameter p = Mapper.Map<Parameter>(requestMessage.Get<DTOParameter>());
            List<Parameter> pl = await eParameter.GetParametersByGroupCode(Mapper.Map<Parameter>(requestMessage.Get<DTOParameter>()));
            response.Add("ParameterList", Mapper.Map<List<DTOParameter>>(pl));

            return response;
        }
    }
}
