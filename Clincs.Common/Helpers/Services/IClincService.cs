using Clincs.Common.Models.Database.API;
using Clincs.Common.Models.Request;
using Clincs.Common.Models.Response;
using System.Collections.Generic;

namespace Clincs.Common.Helpers.Services
{
    public interface IClincService
    {
        IEnumerable<User> GetAll();

        IEnumerable<User> GetAllDoctors();

        User GetById(int id);
        AuthenticateResponse Login(LoginRequest user);

        void Create(CreateRequest model);


        void Delete(int id);

        List<Appointment> GetSlotsOfUser(int doctorId);

        void Book(Appointment slot);

        void Cancel(int id);

        Appointment GetAppointment(int id);
    }
}
