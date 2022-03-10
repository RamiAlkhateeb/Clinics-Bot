using Clincs.Common.Models.Database;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clincs.Common.Helpers.Bot
{
    public interface IConversationReferencesHelper
    {
        void AddorUpdateConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
        //aTask DeleteConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
        IEnumerable<ConversationReferenceEntity> GetConversationRefrenceAsync();
    }
}
