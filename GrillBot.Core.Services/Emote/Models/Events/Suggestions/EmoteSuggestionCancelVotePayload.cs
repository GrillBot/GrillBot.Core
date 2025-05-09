﻿using GrillBot.Core.RabbitMQ.V2.Messages;

namespace GrillBot.Core.Services.Emote.Models.Events.Suggestions;

public class EmoteSuggestionCancelVotePayload : IRabbitMessage
{
    public string Topic => "Emote";
    public string Queue => "EmoteSuggestionCancelVote";

    public Guid SuggestionId { get; set; }

    public EmoteSuggestionCancelVotePayload()
    {
    }

    public EmoteSuggestionCancelVotePayload(Guid suggestionId)
    {
        SuggestionId = suggestionId;
    }
}
