using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BankingApp.Common.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankingApp.Api.Filters;

public class ValidateCustomerNoAuthorizationFilter: IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        long now = DateTime.Now.Millisecond;
        if (context.HttpContext.User.Identity.IsAuthenticated && context.ActionDescriptor.EndpointMetadata.All(x => x.GetType() != typeof(AllowAnonymousAttribute)))
        {
            MessageContainer message = JsonSerializer.Deserialize<MessageContainer>(await getRequestBody(context.HttpContext.Request), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
            
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
            if (!authorizeFromBody(jwtSecurityToken.Claims.ToList(), message))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

    private static bool authorizeFromBody(List<Claim> claims, MessageContainer message)
    {
        DTOCustomer? dtoCustomer = message.TryToObject<DTOCustomer>(message, "DTOCustomer");
        DTOLogin? dtoLogin = message.TryToObject<DTOLogin>(message, "DTOLogin");
        DTOAccount? dtoAccount = message.TryToObject<DTOAccount>(message, "DTOAccount");
        DTOCreditCard? dtoCreditCard = message.TryToObject<DTOCreditCard>(message, "DTOCreditCard");
        DTOTransactionHistory? dtoTransaction = message.TryToObject<DTOTransactionHistory>(message, "DTOTransactionHistory");
        DTOTransfer? dtoTransfer = message.TryToObject<DTOTransfer>(message, "DTOTransfer");
        string customerNoFromBody = string.Empty;
        string identityNoFromBody = string.Empty;
        
        if (dtoCustomer != null)
        {
            customerNoFromBody = dtoCustomer.CustomerNo ?? string.Empty;
            identityNoFromBody = dtoCustomer.IdentityNo ?? string.Empty;
        }
        else if (dtoLogin != null)
        {
            customerNoFromBody = dtoLogin.CustomerNo ?? string.Empty;
            identityNoFromBody = dtoLogin.IdentityNo ?? string.Empty;
        }
        else if (dtoAccount != null)
        {
            customerNoFromBody = dtoAccount.CustomerNo ?? string.Empty;
        }
        else if (dtoCreditCard != null)
        {
            customerNoFromBody = dtoCreditCard.CustomerNo ?? string.Empty;
        }
        else if (dtoTransaction != null)
        {
            customerNoFromBody = dtoTransaction.CustomerNo ?? string.Empty;
        }
        else if (dtoTransfer != null)
        {
            customerNoFromBody = dtoTransfer.SenderCustomerNo ?? string.Empty;
        }

        return (!string.IsNullOrWhiteSpace(customerNoFromBody) &&
                       claims.FirstOrDefault(x => x.Type == "customerNo" && customerNoFromBody.Equals(x.Value)) != null) || (!string.IsNullOrWhiteSpace(identityNoFromBody) &&
            claims.FirstOrDefault(x => x.Type == "identityNo" && identityNoFromBody.Equals(x.Value)) != null);
    }

    private static async Task<string> getRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen:true))
        {
            string body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return body;
        }
    }
}