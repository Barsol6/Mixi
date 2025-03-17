using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using mixi.Modules.Enums;
using SecurityDriven.Core;

namespace mixi.Modules.Generators;

public class CharacterNameGenerator:ICharacterNameGenerator
{
    private string _characterName = string.Empty;

    private NamesElements? _names;
    private int _firstName = 0;
    private int _lastName = 0;
    private int _middleName = 0;
    private bool _isMiddleName = false;
    private CryptoRandom CryptoRandom = new CryptoRandom();
    Exception _exception = new();

    public async Task<string?> GenerateNameAsync(NameType nameType, bool isNoble, string sex)
    {
        var names = File.ReadAllText("Repository/NameRepository/"+sex+"/"+ nameType + "Names.json");
        _names = JsonSerializer.Deserialize<NamesElements>(names);

        if (_names != null)
        {
            if (isNoble == false)
            {
                _firstName = (int)CryptoRandom.NextInt64(0, _names.first_names.Count-1);
                _lastName = (int)CryptoRandom.NextInt64(0, _names.last_names.Count-1);
                Console.Out.Write("dupa");
                return string.Concat(_names.first_names[_firstName]," ",_names.last_names[_lastName]);
            }
            else if(isNoble==true)
            {
                _firstName = (int)CryptoRandom.NextInt64(0, _names.first_names.Count-1);
                _lastName = (int)CryptoRandom.NextInt64(0, _names.last_names.Count-1);
                _middleName = (int)CryptoRandom.NextInt64(0, _names.first_names.Count-1);
                Console.Out.Write("dupa");
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