using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Common.Constants;
using BankingApp.Common.DataTransferObjects;
using BankingApp.Common.Interfaces;
using BankingApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BankingApp.Service
{
    public partial class Service: IService
    {
        public async Task<MessageContainer> RegisterCustomer(MessageContainer requestMessage)
        {
            ELogin eLogin = new ELogin();
            DTOLogin dtoLogin = requestMessage.Get<DTOLogin>("Login");
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            dtoLogin.Password = passwordHasher.HashPassword(dtoLogin.IdentityNo, dtoLogin.Password);
            dtoLogin = await eLogin.Add(requestMessage.Get<DTOLogin>("Login"));

            requestMessage.Clear();
            requestMessage.Add("Login", dtoLogin);
            return requestMessage;
        }

        private string setTemporaryPassword()
        {
            Random random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

        public async Task<MessageContainer> UpdatePassword(MessageContainer requestMessage)
        {
            MessageContainer responseMessage = new MessageContainer();
            MessageContainer responseTask = new MessageContainer();
            ELogin eLogin = new ELogin();

            DTOLogin dtoLogin = requestMessage.Get<DTOLogin>();
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            dtoLogin.Password = passwordHasher.HashPassword(dtoLogin.IdentityNo, dtoLogin.Password);
            dtoLogin.Temporary = false;

            dtoLogin = await eLogin.Update(dtoLogin);

            if(dtoLogin == null)
            {
                throw new Exception("Bilinmeyen hata, şubenize başvurun.");
            }

            Task x = Task.Run(async () =>
            {
                requestMessage.Clear();
                requestMessage.Add(new DTOCustomer { IdentityNo = dtoLogin.IdentityNo });
                responseTask = await GetCustomerByIdentityNo(requestMessage);

                requestMessage.Clear();
                requestMessage.Add(new DTOMailAddresses { CustomerNo = responseTask.Get<DTOCustomer>().CustomerNo });

                responseTask = await GetPrimaryMailAddressByCustomerNo(requestMessage);

                sendMail(new List<string> { responseTask.Get<DTOMailAddresses>().MailAddress }, "ParBank Parola Değişikliği", "Merhaba<br><br>Parolanız isteğiniz doğrultusunda güncellenmiştir. Bu işlemi siz gerçekleştirmediyseniz şubemize başvurun.");
            });

            responseMessage.Add(dtoLogin);

            return responseMessage;
        }

        public async Task<MessageContainer> GetLoginCredentials(MessageContainer requestMessage)
        {
            MessageContainer response = new MessageContainer();
            ELogin eLogin = new ELogin();
            DTOLogin dtoLogin = await eLogin.Select(requestMessage.Get<DTOLogin>());
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            // dtoLogin.Password = passwordHasher.VerifyHashedPassword(dtoLogin.IdentityNo, dtoLogin.Password, "ds");
            
            if(!string.IsNullOrWhiteSpace(requestMessage.Get<DTOLogin>().Password) && passwordHasher.VerifyHashedPassword(dtoLogin.IdentityNo, dtoLogin.Password, requestMessage.Get<DTOLogin>().Password) == PasswordVerificationResult.Failed){
                throw new Exception("Hatalı Şifre");
            }

            if(dtoLogin != null)       // generate token
            {
                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ENV.JwtSecretKey));
                DateTime dtNow = DateTime.UtcNow;
                JwtSecurityToken jwt = new JwtSecurityToken(issuer: ENV.JwtIssuer, audience: ENV.JwtAudience, notBefore: dtNow, expires: dtNow.AddMinutes(300), 
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

                dtoLogin.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
                dtoLogin.TokenExpireDate = dtNow.AddMinutes(300);
                response.Add(dtoLogin);
            }

            return response;
        }

        public async Task<MessageContainer> RegisterCheckDataAlreadyInUse(MessageContainer requestMessage)
        {
            EMailAddresses eMailAddress = new EMailAddresses();
            ELogin eLogin = new ELogin();

            DTOLogin dtoLogin = requestMessage.Get<DTOLogin>();
            DTOMailAddresses dtoMailAddress = requestMessage.Get<DTOMailAddresses>();
            
            dtoMailAddress = await eMailAddress.Get(dtoMailAddress);
            dtoLogin = await eLogin.Select(dtoLogin);


            if (dtoMailAddress != null)
            {
                throw new Exception("Bu mail adresi kullanılıyor.");
            }
            else if(dtoLogin != null)
            {
                throw new Exception("Müşteri zaten kayıtlı.");
            }

            return new MessageContainer();
        }
    }
}
