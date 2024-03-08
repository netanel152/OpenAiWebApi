using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;
using OpenAI.Threads;
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
  public async Task<string> CreateAssistant(AssistantModelDto assistantModel)
  {
    var result = await _openAiService.CreateAssistance(assistantModel);
    return result;
  }

  [HttpPost("delete-assistant")]
  public async Task<bool> DeleteAssistant(string assistantId)
  {
    var result = await _openAiService.DeleteAssistant(assistantId);
    return result;
  }

  [HttpPost("modify-assistant")]
  public async Task<AssistantResponse> ModifyAssistant(string assistantId, string? model, string? name, string? description, string? instructions, List<Tool>? tools)
  {
    var result = await _openAiService.ModifyAssistant(assistantId, model, name, description, instructions, tools);
    return result;
  }

  [HttpPost("create-thread-run")]
  public async Task<RunResponse> CreateThreadRun(string assistantId)
  {
    var result = await _openAiService.CreateThreadRun(assistantId);
    return result;
  }

  [HttpGet("get-thread-run")]
  public async Task<RunResponse> GetThreadRun(string runId, string threadId)
  {
    var result = await _openAiService.GetThreadRun(threadId, runId);
    return result;
  }

  [HttpPost("send-prompt-to-assistant")]
  public async Task<string> SendPromptToAssistant([FromBody] PromptRequest request)
  {
    var result = await _openAiService.CreateChatCompletionStream(request);
    return result;
  }

  [HttpPost("upload-file-for-assistant")]
  public async Task<string> UploadFile(string filePath)
  {
    var result = await _openAiService.UploadFileForAssistant(filePath);
    return result;
  }

  [HttpGet("retrieve-files")]
  public async Task<IReadOnlyList<FileResponse>> RetrieveFilesFromOpenAI()
  {
    var result = await _openAiService.RetrieveFile();
    return result;
  }
}
