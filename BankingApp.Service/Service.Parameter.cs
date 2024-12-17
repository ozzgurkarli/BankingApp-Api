using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BankingApp.Service
{
    public partial class Service : IService
    {
        public async Task<MessageContainer> GetParametersByGroupCode(MessageContainer requestMessage)
        {
            EParameter eParameter = new EParameter(requestMessage.UnitOfWork);
            MessageContainer response = new MessageContainer();

            DTOParameter dtoParameter = requestMessage.Get<DTOParameter>();

            dtoParameter.GroupCode = $"'{dtoParameter.GroupCode}'";

            List<DTOParameter> dtoParList = await eParameter.GetByMultipleGroupCode(dtoParameter);

            response.Add("ParameterList", dtoParList);
            return response;
        }

        public async Task<MessageContainer> GetMultipleGroupCode(MessageContainer requestMessage)
        {
            EParameter eParameter = new EParameter(requestMessage.UnitOfWork);
            MessageContainer response = new MessageContainer();
            List<DTOParameter> parList = requestMessage.Get<List<DTOParameter>>();

            DTOParameter dtoParGroupCodes = new DTOParameter();

            foreach (var item in parList)
            {
                if (string.IsNullOrWhiteSpace(dtoParGroupCodes.GroupCode))
                {
                    dtoParGroupCodes.GroupCode = $"'{item.GroupCode}'";
                    continue;
                }

                dtoParGroupCodes.GroupCode += $",'{item.GroupCode}'";
            }

            List<DTOParameter> dtoParList = await eParameter.GetByMultipleGroupCode(dtoParGroupCodes);

            response.Add("ParameterList", dtoParList);
            return response;
        }

        public async Task<MessageContainer> SetCurrencyValuesSchedule(MessageContainer requestMessage)
        {
            EParameter eParameter = new EParameter(requestMessage.UnitOfWork);
            requestMessage.Add(new DTOParameter { GroupCode = "Currency" });
            MessageContainer responseMessage = new MessageContainer();

            MessageContainer responsePar = await GetParametersByGroupCode(requestMessage);
            List<DTOParameter> parList = responsePar.Get<List<DTOParameter>>();

            Dictionary<string, decimal> tempCurrencyDict = new Dictionary<string, decimal>();
            Dictionary<string, decimal> currencyDict = new Dictionary<string, decimal>();

            if (DateTime.Parse(parList.Find(x => x.Code.Equals(1))!.Detail2!).CompareTo(DateTime.Today) < 0)
            {
                // set new values
                HttpClient client = new HttpClient();
                Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        await client.GetStringAsync(
                            $"https://data.fixer.io/api/latest?access_key={Environment.GetEnvironmentVariable("CURRENCY_API_KEY")}"))
                    !;

                foreach (KeyValuePair<string, decimal> item in JsonConvert
                             .DeserializeObject<Dictionary<string, decimal>>(dict["rates"].ToString()!)!)
                {
                    if (item.Key.Equals("TRY") || parList.Select(x => x.Description).ToList().Contains(item.Key))
                    {
                        tempCurrencyDict.Add(item.Key, item.Value);
                    }
                }

                decimal tlVal = tempCurrencyDict["TRY"];

                foreach (var item in tempCurrencyDict) // set base eur to try
                {
                    currencyDict.Add(item.Key, tlVal / item.Value);
                }

                foreach (var item in parList)
                {
                    item.Detail2 = DateTime.Today.ToString();
                    item.Detail3 = item.Detail4;
                    item.Detail4 = item.Description!.Equals("TL")
                        ? "0"
                        : Math.Round(currencyDict[item.Description!], 2, MidpointRounding.AwayFromZero).ToString();
                    responseMessage.Add(item.Description, item);
                }

                await eParameter.UpdateRange(parList);
            }

            return responseMessage;
        }
    }
}