using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Clincs.Common.Models.Database.API;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Microsoft.Bot.Builder.Teams;
using Clincs.Common.Helpers.Bot;
using Clincs.Common.Helpers.Services;
using Clincs.Common.Helpers;
using System.Linq;

namespace Clincs.BotApp
{
    public class Bot : ActivityHandler
    {
        // Messages sent to the user.
        private const string WelcomeMessage = "You can say 'intro' to see the " +
                                                "introduction card. If you are running this bot in the Bot Framework " +
                                                "Emulator";


        private const string PatternMessage = " the bot " +
                                              "handles 'hello', 'hi', 'help' and 'intro'. Try it now, type 'hi'";

        //private readonly string[] _cards = { ".//Resources//DoctorCard.json" };
        private readonly IConversationReferencesHelper _conversationReferenceHelper;


        private readonly BotState _userState;
        private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;
        private readonly IClinicService _clinicService;

        //private readonly IConversationReferencesHelper _conversationReferenceHelper;



        // Initializes a new instance of the "WelcomeUserBot" class.
        public Bot(IConversationReferencesHelper conversationReferencesHelper, 
            UserState userState,
            ConcurrentDictionary<string, ConversationReference> conversationReferences,
            IClinicService clinicService)
        {
            _userState = userState;
            _conversationReferenceHelper = conversationReferencesHelper;
            _conversationReferences = conversationReferences;
            _clinicService = clinicService;

        }

        protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            if (activity.Action.Equals("add"))
                 _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
            //else if (activity.Action.Equals("remove"))
            //     _conversationReferenceHelper.DeleteConversationRefrenceAsync(botConRef, currentMember);


        }

        private void AddConversationReference(Activity activity)
        {
            var conversationReference = activity.GetConversationReference();
            //var conversationReferenceString = conversationReference.Conversation.Id;


            _conversationReferences.AddOrUpdate(conversationReference.User.Id, conversationReference, (key, newValue) => conversationReference);
        }


        protected override Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            AddConversationReference(turnContext.Activity as Activity);
           
            return base.OnConversationUpdateActivityAsync(turnContext, cancellationToken);
        }

        // Greet when users are added to the conversation.
        // Note that all channels do not send the conversation update activity.
        // If you find that this bot works in the emulator, but does not in
        // another channel the reason is most likely that the channel does not
        // send this activity.
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync($"Hi there - {member.Name}. {WelcomeMessage}", cancellationToken: cancellationToken);
                    //await turnContext.SendActivityAsync(InfoMessage, cancellationToken: cancellationToken);
                    //await turnContext.SendActivityAsync($"{LocaleMessage} Current locale is '{turnContext.Activity.GetLocale()}'.", cancellationToken: cancellationToken);
                    await turnContext.SendActivityAsync(PatternMessage, cancellationToken: cancellationToken);
                }
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeUserStateAccessor = _userState.CreateProperty<WelcomeUserState>(nameof(WelcomeUserState));
            var didBotWelcomeUser = await welcomeUserStateAccessor.GetAsync(turnContext, () => new WelcomeUserState(), cancellationToken);
            AddConversationReference(turnContext.Activity as Activity);


            if (didBotWelcomeUser.DidBotWelcomeUser == false)
            {
                didBotWelcomeUser.DidBotWelcomeUser = true;

                // the channel should sends the user name in the 'From' object
                var userName = turnContext.Activity.From.Name;

                await turnContext.SendActivityAsync("You are seeing this message because this was your first message ever to this bot.", cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync($"It is a good practice to welcome the user and provide personal greeting. For example, welcome {userName}.", cancellationToken: cancellationToken);
            }
            else
            {
                // This example hardcodes specific utterances. You should use LUIS or QnA for more advance language understanding.
                var text = turnContext.Activity.Text.ToLowerInvariant();
                switch (text)
                {
                    case "doctors":

                        // Call the todolist service.
                        var doctors = _clinicService.GetAllDoctors(null);
                        
                        foreach (var item in doctors)
                        {
                            #region
                            //AdaptiveCard card =   new AdaptiveCard(new AdaptiveSchemaVersion("1.1"));
                            //List<AdaptiveColumn> cols = new List<AdaptiveColumn>();

                            //var col2 = new AdaptiveColumn();
                            //col2.Type = "Column";
                            //col2.Spacing = AdaptiveSpacing.Medium;
                            //col2.Width = AdaptiveColumnWidth.Auto;
                            //Random rand = new Random();

                            //col2.Items.Add(new AdaptiveImage()
                            //{
                            //    Url = new Uri("https://picsum.photos/100/100?image="+rand.Next(1,1000)),
                            //    Size = AdaptiveImageSize.Medium,
                            //    Style = AdaptiveImageStyle.Person,
                            //    //Spacing = AdaptiveSpacing.Padding


                            //});
                            //var col = new AdaptiveColumn();
                            //col.Type = "Column";
                            //col.Width = AdaptiveColumnWidth.Auto;
                            //col.Items.Add(new AdaptiveTextBlock()
                            //{
                            //    Weight = AdaptiveTextWeight.Bolder,
                            //    Text = item.Title + " " +item.FirstName + " " + item.LastName,
                            //    //Wrap= true
                            //});
                            //col.Items.Add(new AdaptiveTextBlock()
                            //{
                            //    IsSubtle = true,
                            //    Text = item.Email ,
                            //    Weight = AdaptiveTextWeight.Lighter,
                            //    Spacing = AdaptiveSpacing.None,
                            //    //Wrap = true
                            //}) ;
                            //var DoctorSlots = _clinicService.GetSlotsOfUser(item.Id);
                            //List<object> Result = new List<object>();
                            //Functions func = new Functions();
                            //bool isFull = func.isDoctorFull(DoctorSlots);
                            //if (isFull)
                            //    continue; 

                            //List<Appointment> list = new List<Appointment>();
                            //for (int i = 1; i <= 12; i++)
                            //{
                            //    var slot = DoctorSlots.FirstOrDefault(s => s.Number == i);
                            //    if (slot == null)
                            //    {
                            //        Appointment free = new Appointment();
                            //        free.Number = i;
                            //        free.DoctorId = item.Id;
                            //        list.Add(free);
                            //    }
                            //    else
                            //    {
                            //        slot.Doctor = null;
                            //        list.Add(slot);


                            //    }
                            //}
                            //var DetailsCard = new AdaptiveCard(new AdaptiveSchemaVersion("1.1"));
                            //foreach (var slot in list)
                            //{
                            //    var slots = new List<AdaptiveColumn>();
                            //    var DoctorSlot = new AdaptiveColumn();
                            //    DoctorSlot.Type = "Column";
                            //    DoctorSlot.Width = AdaptiveColumnWidth.Auto;
                            //    if(slot.Duration == 0)
                            //    {
                            //        DoctorSlot.Items.Add(new AdaptiveTextBlock()
                            //        {
                            //            Text = "the slot with number "+ slot.Number + " is free",
                            //            Weight = AdaptiveTextWeight.Lighter,
                            //            Spacing = AdaptiveSpacing.None
                            //        });

                            //    }



                            //    slots.Add(DoctorSlot);
                            //    var container = new AdaptiveContainer()
                            //    {

                            //    }; 

                            //    DetailsCard.Body.Add(new AdaptiveColumnSet()
                            //    {
                            //        Columns = slots,


                            //    });

                            //    string s = "https://clincsapi.azurewebsites.net/doctors/" + item.Id + "/slots";


                            //    //col.Items.Add(container);
                            //}

                            //DetailsCard.Body.Add(new AdaptiveTextInput() 
                            //{ 
                            //    Style = AdaptiveTextInputStyle.Text,
                            //    Id = "RegistrationNumber"
                            //});

                            //DetailsCard.Actions.Add(new AdaptiveSubmitAction()
                            //{
                            //    Title = "Test",


                            //});


                            //cols.Add(col2);
                            //cols.Add(col);

                            //card.Body.Add(new AdaptiveColumnSet()
                            //{
                            //    Spacing = AdaptiveSpacing.Medium,

                            //    Columns = cols
                            //});
                            //card.Actions.Add(new AdaptiveShowCardAction()
                            //{
                            //    Title = "View Available Slots",
                            //    Card = DetailsCard
                            //});
                            //// serialize the card to JSON

                            //string json = card.ToJson();


                            //var adaptiveCardAttachment = CreateAdaptiveCardAttachment(json);
                            #endregion

                            var adaptiveCardAttachment = createCard(item);
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(adaptiveCardAttachment), cancellationToken);

                        }
                     
                        

                        await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);


                        break;
                    case "hello":

                      
                        break;
                    case "intro":
                    case "help":
                        await SendIntroCardAsync(turnContext, cancellationToken);
                        break;
                    default:
                        await turnContext.SendActivityAsync(WelcomeMessage, cancellationToken: cancellationToken);
                        break;
                }
            }

            // Save any state changes.
            await _userState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        private static async Task SendIntroCardAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var card = new HeroCard
            {
                Title = "Welcome to Bot Framework!",
                Text = @"Welcome to Welcome Users bot sample! This Introduction card
                         is a great way to introduce your Bot to the user and suggest
                         some things to get them started. We use this opportunity to
                         recommend a few next steps for learning more creating and deploying bots.",
                Images = new List<CardImage>() { new CardImage("https://aka.ms/bf-welcome-card-image") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(ActionTypes.OpenUrl, "Get an overview", null, "Get an overview", "Get an overview", "https://docs.microsoft.com/en-us/azure/bot-service/?view=azure-bot-service-4.0"),
                    new CardAction(ActionTypes.OpenUrl, "Ask a question", null, "Ask a question", "Ask a question", "https://stackoverflow.com/questions/tagged/botframework"),
                    new CardAction(ActionTypes.OpenUrl, "Learn how to deploy", null, "Learn how to deploy", "Learn how to deploy", "https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-deploy-azure?view=azure-bot-service-4.0"),
                }
            };

            var response = MessageFactory.Attachment(card.ToAttachment());
            await turnContext.SendActivityAsync(response, cancellationToken);
        }

        private static Attachment CreateAdaptiveCardAttachment(string adaptiveCardJson)
        {
            //var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        
        private Attachment createCard(User user)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion("1.1"));
            List<AdaptiveColumn> cols = new List<AdaptiveColumn>();

            
            
            var DoctorSlots = _clinicService.GetSlotsOfUser(user.Id);
            List<object> Result = new List<object>();
            Functions func = new Functions();
            bool isFull = func.isDoctorFull(DoctorSlots);
            if (isFull)
                return null;

            List<Appointment> list = new List<Appointment>();
            for (int i = 1; i <= 12; i++)
            {
                var slot = DoctorSlots.FirstOrDefault(s => s.Number == i);
                if (slot == null)
                {
                    Appointment free = new Appointment();
                    free.Number = i;
                    free.DoctorId = user.Id;
                    list.Add(free);
                }
                else
                {
                    slot.Doctor = null;
                    list.Add(slot);


                }
            }
            var DetailsCard = new AdaptiveCard(new AdaptiveSchemaVersion("1.1"));
            foreach (var slot in list)
            {
                var slots = new List<AdaptiveColumn>();
                var DoctorSlot = new AdaptiveColumn();
                DoctorSlot.Type = "Column";
                DoctorSlot.Width = AdaptiveColumnWidth.Auto;
                if (slot.Duration == 0)
                {
                    DoctorSlot.Items.Add(new AdaptiveTextBlock()
                    {
                        Text = "the slot with number " + slot.Number + " is free",
                        Weight = AdaptiveTextWeight.Lighter,
                        Spacing = AdaptiveSpacing.None
                    });

                }



                slots.Add(DoctorSlot);
                var container = new AdaptiveContainer()
                {

                };

                DetailsCard.Body.Add(new AdaptiveColumnSet()
                {
                    Columns = slots,


                });

                string s = "https://clincsapi.azurewebsites.net/doctors/" + user.Id + "/slots";


                //col.Items.Add(container);
            }

            DetailsCard.Body.Add(new AdaptiveTextInput()
            {
                Style = AdaptiveTextInputStyle.Text,
                Id = "RegistrationNumber"
            });

            DetailsCard.Actions.Add(new AdaptiveSubmitAction()
            {
                Title = "Test",


            });


            cols.Add(GetImageCol());
            cols.Add(GetNameCol(user));

            card.Body.Add(new AdaptiveColumnSet()
            {
                Spacing = AdaptiveSpacing.Medium,

                Columns = cols
            });
            card.Actions.Add(new AdaptiveShowCardAction()
            {
                Title = "View Available Slots",
                Card = DetailsCard
            });
            // serialize the card to JSON

            string json = card.ToJson();


            var adaptiveCardAttachment = CreateAdaptiveCardAttachment(json);
            return adaptiveCardAttachment;
        }

        private AdaptiveColumn GetImageCol()
        {
            var col2 = new AdaptiveColumn();
            col2.Type = "Column";
            col2.Spacing = AdaptiveSpacing.Medium;
            col2.Width = AdaptiveColumnWidth.Auto;
            Random rand = new Random();

            col2.Items.Add(new AdaptiveImage()
            {
                Url = new Uri("https://picsum.photos/100/100?image=" + rand.Next(1, 1000)),
                Size = AdaptiveImageSize.Medium,
                Style = AdaptiveImageStyle.Person,
                //Spacing = AdaptiveSpacing.Padding


            });
            return col2;
        }

        private AdaptiveColumn GetNameCol(User user)
        {
            var col = new AdaptiveColumn();
            col.Type = "Column";
            col.Width = AdaptiveColumnWidth.Auto;
            col.Items.Add(new AdaptiveTextBlock()
            {
                Weight = AdaptiveTextWeight.Bolder,
                Text = user.Title + " " + user.FirstName + " " + user.LastName,
                //Wrap= true
            });
            col.Items.Add(new AdaptiveTextBlock()
            {
                IsSubtle = true,
                Text = user.Email,
                Weight = AdaptiveTextWeight.Lighter,
                Spacing = AdaptiveSpacing.None,
                //Wrap = true
            });
            return col;
        }

    }
}
