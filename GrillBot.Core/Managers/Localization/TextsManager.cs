using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GrillBot.Core.Managers.Localization;

public class TextsManager : ITextsManager
{
    public const string DefaultLocale = "en-US";

    public bool IsInitialized => _data.Count > 0;

    private Dictionary<string, string> _data = new();
    private readonly string _basePath;
    private readonly string _fileMask;

    public TextsManager(string basePath, string fileMask)
    {
        _basePath = basePath;
        _fileMask = fileMask;
    }

    private void Init()
    {
        if (IsInitialized)
            return;

        var files = Directory.GetFiles(_basePath, $"{_fileMask}.*.json", SearchOption.AllDirectories);
        var result = new Dictionary<string, string>();

        foreach (var file in files)
        {
            var locale = LocalizationRegex.ParserRegex().Match(file).Groups["locale"].Value;

            using var reader = new StreamReader(file);
            using var jsonReader = new JsonTextReader(reader);
            var jsonData = JObject.Load(jsonReader);

            ReadJson(result, jsonData, "", locale);
        }

        _data = result;
    }

    private static void ReadJson(IDictionary<string, string> data, JToken token, string prefix, string locale)
    {
        static string Join(string prefix, string name)
            => string.IsNullOrEmpty(prefix) ? name : prefix + "/" + name;

        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (var property in token.Children<JProperty>())
                    ReadJson(data, property.Value, Join(prefix, property.Name), locale);
                break;
            case JTokenType.Array:
                var i = 0;
                foreach (var item in token.Children())
                {
                    ReadJson(data, item, Join(prefix, i.ToString()), locale);
                    i++;
                }
                break;
            default:
                data.Add(GetKey(prefix, locale), (token as JValue)?.Value?.ToString() ?? "");
                break;
        }
    }

    public string? this[string id, string locale] => Get(id, locale);
    private string? Get(string id, string locale) => Get(GetKey(id, locale)) ?? Get(GetKey(id, DefaultLocale));

    private string? Get(string textId)
    {
        Init();
        return _data.TryGetValue(textId, out var value) ? value : null;
    }

    private static string GetKey(string id, string locale) => $"{id}#{locale}";

    public List<string> GetSupportedLocales()
    {
        Init();
        return _data.Select(o => o.Key.Split('#')[1]).Distinct().ToList();
    }

    public bool IsSupportedLocale(string locale)
    {
        Init();
        return _data.Select(o => o.Key.Split('#')[1]).Contains(locale);
    }
}
