using Mixi.Api.Modules.Enums;

namespace Mixi.Shared.Models.Generators;

public class NamesGenerator
{
    public NameType NameType { get; set; }
    
    public bool IsNoble { get; set; }
    
    public string Sex {get; set; } = String.Empty;
}