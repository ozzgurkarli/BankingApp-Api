using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using Xunit;

namespace BankingApp.Test
{
    public partial class Test
    {
        private readonly IService _proxy;

        public Test()
        {
            _proxy = new Service.Service();
        }

        [Fact]
        public async void GetParametersByGroupCodeTest()
        {
            MessageContainer requestMessage = new MessageContainer();
            requestMessage.Add(new DTOParameter { GroupCode = "City" });

            MessageContainer responseMessage = await _proxy.GetParametersByGroupCode(requestMessage);
            List<DTOParameter> parList = responseMessage.Get<List<DTOParameter>>();

            Assert.Equal(4, parList.Count);

            for (int i = 0; i < parList.Count; i++)
            {
                Assert.Equal(i + 1, parList[i].Code);
            }
        }

        [Theory]
        [InlineData(new string[] { "CardType" }, new int[] { 2 })]
        [InlineData(new string[] { "CardType", "Gender" }, new int[] { 2, 2 })]
        public async void GetMultipleGroupCodeTest(string[] groupCodes, int[] counts)
        {
            MessageContainer requestMessage = new MessageContainer();
            List<DTOParameter> requestParList = new List<DTOParameter>();
            foreach (var item in groupCodes)
            {
                requestParList.Add(new DTOParameter { GroupCode = item });
            }

            requestMessage.Add(requestParList);

            MessageContainer responseMessage = await _proxy.GetMultipleGroupCode(requestMessage);
            List<DTOParameter> parList = responseMessage.Get<List<DTOParameter>>();

            for (int i = 0; i < groupCodes.Length; i++)
            {
                List<DTOParameter> itemList = parList.Where(x => x.GroupCode.Equals(groupCodes[i])).ToList();

                Assert.Equal(counts[i], itemList.Count);
                for (int j = 0; j < itemList.Count; j++)
                {
                    Assert.Equal(j + 1, itemList[j].Code);
                }
            }
        }
    }
}