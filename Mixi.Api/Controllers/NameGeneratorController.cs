using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Enums;
using Mixi.Api.Modules.Generators.CharacterNameGenerator;
using Mixi.Shared.Models.Generators;

namespace Mixi.Api.Controllers;

[ApiController]
[Route("api/generator/[controller]")]

public class NameGeneratorController: ControllerBase
{
    private readonly ICharacterNameGenerator _characterNameGenerator;

    public NameGeneratorController(ICharacterNameGenerator characterNameGenerator)
    {
        _characterNameGenerator = characterNameGenerator;
    }

    [HttpPost("name_generator")]
    public async Task<IActionResult> GenerateName([FromBody] NamesGenerator namesGenerator)
    {
        Console.WriteLine(namesGenerator.NameType);
        var name = await _characterNameGenerator.GenerateNameAsync(namesGenerator.NameType, namesGenerator.IsNoble, namesGenerator.Sex);
        return Ok(new {generatedName =  name});
    }

}