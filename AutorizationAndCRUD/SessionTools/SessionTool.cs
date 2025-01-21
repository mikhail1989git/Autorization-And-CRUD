using AutorizationAndCRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutorizationAndCRUD.SessionTools
{
    public class SessionTool
    {
        ApplicationContext db { get; set; }

        public SessionTool(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task CreateSession(User user, HttpContext context)
        {
            user.SessionKey = $"{user.Login}{DateTime.Now.ToString()}";
            context.Session.SetString("SessionName", user.SessionKey);
            db.Users.Update(user);
            await db.SaveChangesAsync();
        }

        public bool IsActiveSession(ISession session)
        {
            if (!session.Keys.Contains("SessionName"))
                return false;
            if (!db.Users.Where(user=>user.SessionKey == session.GetString("SessionName")).Any())
                return false;
            return true;
        }
    }
}
