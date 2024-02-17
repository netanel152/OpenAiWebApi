using OpenAI;
using OpenAI.Assistants;

namespace OpenAiWebApi.Interfaces;
public interface IOpenAiService
{
  Task<AssistantResponse> CreateAssistance();
  Task<ListResponse<AssistantResponse>> GetManyAssistants();
  Task<AssistantResponse> GetOneAssistance(string assistantId);
}
