using System.Text.Json;
using Mixi.Api.Modules.Enums;
using SecurityDriven.Core;

namespace Mixi.Api.Modules.Generators.CharacterNameGenerator;
public class CharacterNameGenerator : ICharacterNameGenerator
{
        private readonly CryptoRandom _cryptoRandom = new();
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        public async Task<string> GenerateNameAsync(NameType nameType, bool isNoble, string sex)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sex))
                    throw new ArgumentException("Sex must be specified", nameof(sex));
                
                var filePath = Path.Combine("Repository", "NameRepository", sex, $"{nameType}Names.json");
                
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Name data file not found at: {filePath}");
                
                await using var fileStream = File.OpenRead(filePath);
                var names = await JsonSerializer.DeserializeAsync<NamesElements>(fileStream, _jsonOptions) 
                    ?? throw new InvalidDataException("Deserialized name data is null");
                
                if (names.FirstNames == null || names.FirstNames.Count == 0)
                    throw new InvalidDataException("First names collection is empty or null");
                
                if (names.LastNames == null || names.LastNames.Count == 0)
                    throw new InvalidDataException("Last names collection is empty or null");
                
                var firstNameIndex = GetRandomIndex(names.FirstNames.Count);
                var lastNameIndex = GetRandomIndex(names.LastNames.Count);

                if (isNoble)
                {
                    var middleNameIndex = GetRandomIndex(names.FirstNames.Count);
                    return $"{names.FirstNames[firstNameIndex]} {names.FirstNames[middleNameIndex]} {names.LastNames[lastNameIndex]}";
                }

                return $"{names.FirstNames[firstNameIndex]} {names.LastNames[lastNameIndex]}";
            }
            catch (Exception ex) when (ex is not ArgumentException and not FileNotFoundException)
            {
                throw new InvalidOperationException($"Name generation failed for {nameType} {sex} (Noble: {isNoble})", ex);
            }
        }

        private int GetRandomIndex(int maxValue) => (int)_cryptoRandom.NextInt64(0, maxValue - 1);
}