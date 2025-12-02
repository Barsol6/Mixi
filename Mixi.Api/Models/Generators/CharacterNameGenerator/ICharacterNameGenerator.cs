using Mixi.Api.Modules.Enums;

namespace Mixi.Api.Modules.Generators.CharacterNameGenerator;

public interface ICharacterNameGenerator
{
    Task<string> GenerateNameAsync(NameType nameType, bool isNoble, string sex);
}