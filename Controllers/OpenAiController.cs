using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Assistants;
using OpenAiWebApi.Interfaces;

namespace OpenAiWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OpenAiController : ControllerBase
{

  private readonly ILogger<OpenAiController> _logger;
  private readonly IOpenAiService _openAiService;
  public OpenAiController(IOpenAiService openAiService, ILogger<OpenAiController> logger)
  {
    _logger = logger;
    _openAiService = openAiService;
  }

  [HttpGet("get-one-assistant")]
  public async Task<AssistantResponse> GetAssistant(string assistantId)
  {
    var result = await _openAiService.GetOneAssistance(assistantId);
    return result;
  }

  [HttpGet("get-all-assistants")]
  public async Task<ListResponse<AssistantResponse>> GetAssistants()
  {
    var result = await _openAiService.GetManyAssistants();
    return result;
  }

  [HttpPost("create-assistant")]
  public async Task<string> CreateAssistant()
  {
    var result = await _openAiService.CreateAssistance();
    return result;
  }
}
