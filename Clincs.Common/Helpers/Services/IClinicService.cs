using Clincs.Common.Models;
using Clincs.Common.Models.Database;
using Clincs.Common.Models.Database.API;
using Clincs.Common.Models.Request;
using Clincs.Common.Models.Response;
using System.Collections.Generic;

namespace Clincs.Common.Helpers.Services
{
    public interface IClinicService
    {
        IEnumerable<User> GetAll();

        IEnumerable<User> GetAllDoctors(FilteringParams filteringParams);

        User GetById(int id);
        AuthenticateResponse Login(LoginRequest user);

        void Create(CreateRequest model);


        void Delete(int id);

        List<Appointment> GetSlotsOfUser(int doctorId);

        void Book(BookRequest slot);

        void Cancel(int id);

        Appointment GetAppointment(int id);

        ConversationReferenceEntity SaveConversationReference(ConversationReferenceEntity cr);

        IEnumerable<ConversationReferenceEntity> GetConversationReferences();

        IEnumerable<NotificationEntity> GetNotifications();

        void NotificationIsSent(int id);

    }
}
