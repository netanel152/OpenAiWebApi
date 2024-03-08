using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Chat;
using OpenAI.Files;
using OpenAI.Threads;
using OpenAiWebApi.Configurations;
using OpenAiWebApi.Interfaces;

namespace OpenAiWebApi.Services
{
  public class OpenAiService : IOpenAiService
  {
    private readonly OpenAiConfig _config;
    private readonly OpenAIClient _api;
    public OpenAiService(IOptionsMonitor<OpenAiConfig> optionsMonitor)
    {
      _config = optionsMonitor.CurrentValue;
      _api = new OpenAIClient(_config.Key);
    }


    public async Task<ListResponse<AssistantResponse>> GetManyAssistants()
    {
      var assistantsList = await _api.AssistantsEndpoint.ListAssistantsAsync();
      return assistantsList;
    }

    public async Task<AssistantResponse> GetOneAssistance(string assistantId)
    {
      var assistant = await _api.AssistantsEndpoint.RetrieveAssistantAsync(assistantId);
      return assistant;
    }

    public async Task<AssistantResponse> CreateAssistance(AssistantModelDto assistantModel)
    {
      var data = new AssistantModelDto(
        assistantModel.Model,
        assistantModel.Name,
        assistantModel.Description,
        assistantModel.Instructions,
        assistantModel.Tools = new List<Tool> { Tool.Retrieval, Tool.CodeInterpreter },
        assistantModel.FilesIds,
        assistantModel.Metadata
      );
      var request = new CreateAssistantRequest(
        data.Model,
        data.Name,
        data.Description,
        data.Instructions,
        data.Tools,
        data.FilesIds,
        data.Metadata
      );
      var assistant = await _api.AssistantsEndpoint.CreateAssistantAsync(request);
      return assistant;
    }

    public async Task<AssistantResponse> ModifyAssistant(string assistantId, string? model, string? name, string? description, string? instructions, List<Tool>? tools)
    {
      var assistant = await GetOneAssistance(assistantId);
      var request = new CreateAssistantRequest(
        model ?? assistant.Model,
        name ?? assistant.Name,
        description ?? assistant.Description,
        instructions ?? assistant.Instructions,
        tools ?? assistant.Tools
      );
      return await _api.AssistantsEndpoint.ModifyAssistantAsync(assistantId, request);
    }

    public async Task<RunResponse> CreateThreadRun(string assistantId)
    {
      var assistant = await GetOneAssistance(assistantId);
      var thread = await _api.ThreadsEndpoint.CreateThreadAsync();
      return await thread.CreateRunAsync(assistant);
    }

    public async Task<RunResponse> GetThreadRun(string threadId, string runId)
    {
      var run = await _api.ThreadsEndpoint.RetrieveRunAsync(threadId, runId);
      await run.UpdateAsync();
      return run;
    }

    public async Task<string> CreateChatCompletionStream(PromptRequest request)
    {
      List<OpenAI.Chat.Message> listMessage = new()
      {
          new OpenAI.Chat.Message(Role.System, request.Prompt)
      };
      var chatRequest = new ChatRequest(listMessage);
      var response = await _api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
      {
        partialResponse.FirstChoice.Delta.ToString();
      });
      var choice = response.FirstChoice;
      return choice.Message;
    }

    public async Task<string> UploadFileForAssistant(string path)
    {
      var file = await _api.FilesEndpoint.UploadFileAsync(path, "assistants");
      return file.Id;
    }

    public async Task<bool> DeleteAssistant(string assistantId)
    {
      var isDeleted = await _api.AssistantsEndpoint.DeleteAssistantAsync(assistantId);
      return isDeleted;
    }

    public async Task<IReadOnlyList<FileResponse>> RetrieveFile()
    {
      var fileList = await _api.FilesEndpoint.ListFilesAsync();
      return fileList;
    }
  }
}
