using mixi.Modules.Enums;

namespace mixi.Modules.Generators;

public interface ICharacterNameGenerator
{ 
    Task<string?> GenerateNameAsync(NameType nameType, bool isNoble, string sex);
}