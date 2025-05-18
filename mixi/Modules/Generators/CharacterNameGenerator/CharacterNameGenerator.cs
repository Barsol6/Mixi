using System.Text.Json;
using Microsoft.AspNetCore.Components;
using mixi.Modules.Enums;
using mixi.Modules.UI;
using SecurityDriven.Core;

namespace mixi.Modules.Generators.CharacterNameGenerator;

public class CharacterNameGenerator:ICharacterNameGenerator
{
    private NamesElements? _names;
    private int _firstName;
    private int _lastName;
    private int _middleName;
    private CryptoRandom _cryptoRandom = new CryptoRandom();
    Exception _exception = new();

    public Task<string> GenerateNameAsync(NameType nameType, bool isNoble, string sex)
    {
        var names = File.ReadAllText("Repository/NameRepository/" + sex + "/" + nameType + "Names.json");
        _names = JsonSerializer.Deserialize<NamesElements>(names);

        if (_names != null)
        {
            if (isNoble == false)
            {
                if (_names.FirstNames != null && _names.LastNames != null)
                {
                    _firstName = (int)_cryptoRandom.NextInt64(0, _names.FirstNames.Count - 1);
                    _lastName = (int)_cryptoRandom.NextInt64(0, _names.LastNames.Count - 1);
                    return Task.FromResult(string.Concat(_names.FirstNames[_firstName], " ", _names.LastNames[_lastName]));
                }
            }
            else if(isNoble)
            {
                if (_names.FirstNames != null && _names.LastNames != null)
                {
                    _firstName = (int)_cryptoRandom.NextInt64(0, _names.FirstNames.Count - 1);
                    _lastName = (int)_cryptoRandom.NextInt64(0, _names.LastNames.Count - 1);
                    _middleName = (int)_cryptoRandom.NextInt64(0, _names.FirstNames.Count - 1);
                    return Task.FromResult(string.Concat(_names.FirstNames[_firstName], " ", _names.FirstNames[_middleName], " ",
                        _names.FirstNames[_lastName]));
                }
            }
         
        }

        throw new InvalidOperationException();
    }
}