using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Assistants;
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
  }
}
