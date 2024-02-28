using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SA;

public class Message
{
    private readonly ILogger<Message> _logger;
    
    public Message(ILogger<Message> logger)
    {
        _logger = logger;
    }

    [Function("message")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        try {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string? name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonSerializer.Deserialize<dynamic>(requestBody!);
            
            name ??= data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "Esta função acionada por HTTP foi executada com sucesso. Passe um nome na string de consulta ou no corpo da solicitação para uma resposta personalizada."
                : $"Olá, {name}. Espero que esse evento seja útil para você.";

            var dataToResponse = new 
            {
                Message = responseMessage,
                Date = DateTime.Now,
                Application = "FunctionAppParaApresentar",
                Version = "1.0.0",
                Program = "Student Ambassadors - Microsoft - 2024"
            };

            return new OkObjectResult(dataToResponse);
        }
        catch (Exception ex) {
            var dataToResponse = new 
            {
                Message = ex.Message,
                Date = DateTime.Now,
                Application = "FunctionAppParaApresentar",
                Version = "1.0.0",
                Program = "Student Ambassadors - Microsoft - 2024"
            };

            return new OkObjectResult(dataToResponse);
        }
    }
}