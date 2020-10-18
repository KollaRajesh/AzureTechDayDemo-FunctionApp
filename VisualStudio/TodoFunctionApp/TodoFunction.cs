using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TodoFunctionApp
{
    public static class TodoApi
    {
        static List<Todo> items = new List<Todo>();
        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new todo list item");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription };
            
            items.Add(todo);
            return new OkObjectResult(todo);
        }

        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get" , Route = "todo")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting Todo items");

            
            return new OkObjectResult(items);
        }

        [FunctionName("GetTodoById")]
        public static async Task<IActionResult> GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "todo/{id}")] HttpRequest req,
            ILogger log,string id)
        {
            log.LogInformation("Getting Todo items");
            var todo = items.FirstOrDefault(t => t.Id == id);

            if (todo==null)
            {
                return new NotFoundResult();
            }
          return    new OkObjectResult(todo);

        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "put", Route = "todo")] HttpRequest req,
            ILogger log,string id)
        {
            log.LogInformation("Creating a new todo list item");


            var todo = items.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var updated = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);
            todo.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrWhiteSpace(updated.TaskDescription))
                todo.TaskDescription = updated.TaskDescription;
            return new OkObjectResult(todo);
        }
        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> DeleteToDO(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("deleting a new todo list item");


            var todo = items.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }

            items.Remove(todo);
            return new OkResult();
        }
    }
}
