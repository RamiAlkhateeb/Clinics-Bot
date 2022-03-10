using Microsoft.AspNetCore.Mvc;

namespace Clincs.BotApp
{
    public class WelcomeUserState
    {
        // Gets or sets whether the user has been welcomed in the conversation.
        public bool DidBotWelcomeUser { get; set; } = false;
    }
}
