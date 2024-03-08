using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;
using OpenAI.Threads;

namespace OpenAiWebApi.Interfaces;
public interface IOpenAiService
{
  public Task<AssistantResponse> CreateAssistance(AssistantModelDto assistantModel);
  public Task<ListResponse<AssistantResponse>> GetManyAssistants();
  public Task<AssistantResponse> GetOneAssistance(string assistantId);
  public Task<RunResponse> CreateThreadRun(string assistantId);
  public Task<RunResponse> GetThreadRun(string threadId, string runId);
  public Task<string> CreateChatCompletionStream(PromptRequest request);
  public Task<AssistantResponse> ModifyAssistant(string assistantId, string? model, string? name, string? description, string? instructions, List<Tool>? tools);
  public Task<string> UploadFileForAssistant(string file);
  public Task<bool> DeleteAssistant(string assistantId);
  public Task<IReadOnlyList<FileResponse>> RetrieveFile();
}
