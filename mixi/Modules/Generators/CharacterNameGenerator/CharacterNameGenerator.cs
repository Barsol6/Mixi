using System.Text.Json;
using mixi.Modules.Enums;
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

    public async Task<string?> GenerateNameAsync(NameType nameType, bool isNoble, string sex)
    {
        var names = File.ReadAllText("Repository/NameRepository/" + sex + "/" + nameType + "Names.json");
        _names = JsonSerializer.Deserialize<NamesElements>(names);

        if (_names != null)
        {
            if (isNoble == false)
            {
                _firstName = (int)_cryptoRandom.NextInt64(0, _names.first_names.Count-1);
                _lastName = (int)_cryptoRandom.NextInt64(0, _names.last_names.Count-1);
                return string.Concat(_names.first_names[_firstName]," ",_names.last_names[_lastName]);
            }
            else if(isNoble)
            {
                _firstName = (int)_cryptoRandom.NextInt64(0, _names.first_names.Count-1);
                _lastName = (int)_cryptoRandom.NextInt64(0, _names.last_names.Count-1);
                _middleName = (int)_cryptoRandom.NextInt64(0, _names.first_names.Count-1);
                return string.Concat(_names.first_names[_firstName]," ", _names.first_names[_middleName]," ",_names.last_names[_lastName]);
            }
         
        }
        else
        {
            Console.Out.Write("dupa");
            return _exception.StackTrace;
        }
        Console.Out.Write("dupa");
        return _exception.StackTrace;
    }
}