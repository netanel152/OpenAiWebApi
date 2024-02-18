using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Chat;
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

    public async Task<AssistantResponse> CreateAssistance()
    {
      var request = new CreateAssistantRequest(
        "gpt-4-turbo-preview",//model
        "Support Assistant",//name
        "help with troubleshooting in the internet world",//description
        "Each problem with the internet the assistant will help you to solve it"//instructions
        );
      var assistant = await _api.AssistantsEndpoint.CreateAssistantAsync(request);
      return assistant;
    }

    public async Task<AssistantResponse> ModifyAssistance(string assistantId, string? model, string? name, string? description, string? instructions, List<Tool>? tools)
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

    public async Task<string> CreateChatCompletionStream(string prompt, string assistantName)
    {
      List<OpenAI.Chat.Message> listMessage = new()
      {
          new OpenAI.Chat.Message(Role.Assistant, prompt, assistantName)
      };
      var chatRequest = new ChatRequest(listMessage);
      var response = await _api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
      {
        partialResponse.FirstChoice.Delta.ToString();
      });
      var choice = response.FirstChoice;
      return choice.Message;
    }
  }
}
