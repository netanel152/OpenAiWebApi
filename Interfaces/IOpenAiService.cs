using OpenAI;
using OpenAI.Assistants;
using OpenAI.Threads;

namespace OpenAiWebApi.Interfaces;
public interface IOpenAiService
{
  Task<AssistantResponse> CreateAssistance();
  Task<ListResponse<AssistantResponse>> GetManyAssistants();
  Task<AssistantResponse> GetOneAssistance(string assistantId);
  Task<RunResponse> CreateThreadRun(string assistantId);
  Task<RunResponse> GetThreadRun(string threadId, string runId);
  Task<string> CreateChatCompletionStream(string prompt, string assistantName);
  Task<AssistantResponse> ModifyAssistance(string assistantId, string? model, string? name, string? description, string? instructions, List<Tool>? tools);
}
