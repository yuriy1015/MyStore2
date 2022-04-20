using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class LogTimeAttribute : ActionFilterAttribute
    {
        //Запись в лог каждого обращения к методам действия
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var employee = context.HttpContext.Session.Get<Employee>("authorizedEmployee");
            var client = context.HttpContext.Session.Get<Client>("authorizedClient");
            string message = 
                $"Выполнен метод {context.ActionDescriptor.DisplayName}, " +
                $"время обращения {DateTime.Now.ToString(/*"T"*/)}, " +
                $"пользователь { (employee != null ? employee.Email : "") } { (client != null ? client.Email : "") }";
            // до вызова делегата ActionExecutionDelegate, код который запустится перед методом действия
            await next();
            // после вызова делегата ActionExecutionDelegate, код который запустится по завершению метода действия


            using (StreamWriter streamWriter = new StreamWriter("log.txt", true))
            {
                await streamWriter.WriteLineAsync(message);
                streamWriter.Close();
            }
                
        }

    }
}
