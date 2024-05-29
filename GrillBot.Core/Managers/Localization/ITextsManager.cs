namespace GrillBot.Core.Managers.Localization;

public interface ITextsManager
{
    bool IsInitialized { get; }

    string? this[string id, string locale] { get; }

    List<string> GetSupportedLocales();
    bool IsSupportedLocale(string locale);
}
