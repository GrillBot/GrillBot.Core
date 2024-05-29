using GrillBot.Core.Services.GrillBot.Models.Events.Messages.Components;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages;

public class DiscordMessageComponent
{
    public List<ButtonComponent> Buttons { get; set; } = new();
    public List<string> ComponentOrder { get; set; } = new();

    public void AddButton(ButtonComponent component)
        => AddComponent(() => Buttons, component, "Button");

    private void AddComponent(Func<System.Collections.IList> listSelector, object item, string componentType)
    {
        if (ComponentOrder.Count > Discord.ComponentBuilder.MaxActionRowCount * Discord.ActionRowBuilder.MaxChildCount)
            throw new ArgumentException("Unable to add next component. List is full.");

        var list = listSelector();
        var index = list.Count;

        list.Add(item);
        ComponentOrder.Add($"{componentType}/{index}");
    }

    public IEnumerable<Discord.IMessageComponent> BuildComponents()
    {
        return ComponentOrder
            .Select(o => o.Split('/', StringSplitOptions.TrimEntries))
            .Select(o => o[0] switch
            {
                "Button" => Buttons[Convert.ToInt32(o[1])].ToDiscordComponent(),
                _ => null
            })
            .Where(o => o is not null)!;
    }
}
