
using OpenAI;

public class AssistantModelDto
{
  public string? Model { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public string? Instructions { get; set; }
  public IEnumerable<Tool>? Tools { get; set; }
  public IEnumerable<string>? FilesIds { get; set; }
  public IReadOnlyDictionary<string, string>? Metadata { get; set; }

  public AssistantModelDto(string? model, string? name, string? description, string? instructions, IEnumerable<Tool>? tools, IEnumerable<string>? filesIds, IReadOnlyDictionary<string, string>? metadata)
  {
    Model = model;
    Name = name;
    Description = description;
    Instructions = instructions;
    Tools = tools;
    FilesIds = filesIds;
    Metadata = metadata;
  }
}