using System.Text.Json;
using BankingApp.Common.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankingApp.Api.Filters;

public class DTOCreatorFilter: IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("requestMessage", out var requestObj) &&
            requestObj is MessageContainer requestMessage)
        {
            int count = requestMessage.Contents.Count;
            for (int i = 0; i < count; i++)
            {
                KeyValuePair<string, object> value = requestMessage.Contents.ElementAt(i);
                if(value.Key.Contains("List"))
                    continue;
                Type dtoType = Type.GetType($"BankingApp.Common.DataTransferObjects.{value.Key.Split('.').First()}, BankingApp.Common")!;
                var dto = Activator.CreateInstance(dtoType)!;
                dto = JsonSerializer.Deserialize(value.Value.ToString()!, dtoType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                requestMessage.Add(dto);
                requestMessage.Contents.Remove(value.Key);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}