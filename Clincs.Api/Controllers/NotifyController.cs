using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using Clincs.Common.Helpers.Bot;
using Clincs.Common.Helpers.Services;
using Clincs.Common.Models.Database.API;
//using Clincs.Bot;


namespace Clincs.Api.Controllers
{
    //[Route("api/notify")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string _appId;
        private ConcurrentDictionary<string, ConversationReference> _conversationReferences;
        private readonly IConversationReferencesHelper _conversationReferenceHelper;
        private readonly IClinicService _clinicService;


        public NotifyController(IConversationReferencesHelper conversationReferencesHelper,
            IBotFrameworkHttpAdapter adapter, 
            IConfiguration configuration, ConcurrentDictionary<string, ConversationReference> conversationReferences,
            IClinicService clinicService
            )
        {
            _adapter = adapter;
            _conversationReferenceHelper = conversationReferencesHelper;
            _conversationReferences = conversationReferences;
            _appId = configuration["MicrosoftAppId"] ?? string.Empty;
            _clinicService = clinicService;
        }
        [HttpGet("api/notify")]
        public async Task<IActionResult> Get()
        {

            if(_conversationReferences.Count == 0)
            {
                _conversationReferences = new ConcurrentDictionary<string, ConversationReference>();
                _conversationReferences = getConversationDectionary();
            }

            var notifications = getNotifications();

            foreach (var conversationReference in _conversationReferences.Values)
            {
                var conversationNotification = notifications.Where(n => n.UserId == conversationReference.User.Id && n.IsSent == false).ToList();
                foreach (var notification in conversationNotification)
                {
                    IMessageActivity message = MessageFactory.Text(notification.Body);
                    await ((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationReference,
                       async (context, token) => await BotCallback(message, context, token),
                       default(CancellationToken));
                    NotificationIsSent(notification.Id);
                }
               
                    
            }

            // Let the caller know proactive messages have been sent
            return new ContentResult()
            {
                Content = "<html><body><h1>Proactive messages have been sent.</h1></body></html>",
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        private async Task BotCallback(IMessageActivity message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("proactive hello "+message.Text);
        }

        [NonAction]
        public ConcurrentDictionary<string, ConversationReference> getConversationDectionary()
        {
            var list = _conversationReferenceHelper.GetConversationRefrenceAsync();
            var dict = new ConcurrentDictionary<string, ConversationReference>();
            foreach (var item in list)
            {
                ConversationReference cr = new ConversationReference();
                cr.ActivityId = item.ActivityId;
                cr.ChannelId = item.ChannelId;
                ChannelAccount bot = new ChannelAccount();
                ChannelAccount user = new ChannelAccount();
                ConversationAccount ca = new ConversationAccount();
                user.Id = item.UserId;
                bot.Id = item.BotId;
                cr.Bot = bot;
                cr.User = user;
                ca.Id = item.ConversationId;
                cr.Conversation = ca;
                cr.ServiceUrl = item.ServiceUrl;
                dict[item.UserId] = cr;
            }
            return dict;
        }

        [NonAction]
        public IEnumerable<NotificationEntity> getNotifications()
        {
            var list = _clinicService.GetNotifications();
            
            return list;
        }

        [NonAction]
        public void NotificationIsSent(int id)
        {
            _clinicService.NotificationIsSent(id);
        }
    }
}
