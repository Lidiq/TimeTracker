using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.DAL
{
    public class CardsDAL
    {
        /**
         * Create work card for a given task by the logged in user
         */ 
        public static void CreateCard(Guid taskId, Guid userId, DateTime start, int duration, string logComment)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            entity.AddToWorkCards(new WorkCards()
            {
                CardId = Guid.NewGuid(),
                UserId = userId,
                TaskId = taskId,
                StartTime = DateTime.Now,
                Duration = duration,
                LogComment = logComment
            });
            entity.SaveChanges();
        }


        /**
         * Edit work card
         */ 
        public static void EditCard(Guid cardId, DateTime start, int duration, string comment)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            WorkCards card = entity.WorkCards.First(el => el.CardId == cardId);
            card.StartTime = start;
            card.Duration = duration;
            card.LogComment = comment;
            entity.SaveChanges();
        }


        /**
         * Delete work card
         */
        public static void DeleteCard(Guid cardId)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            WorkCards card = entity.WorkCards.First(el => el.CardId == cardId);
            entity.WorkCards.DeleteObject(card);
            entity.SaveChanges();
        }


        public static List<WorkCards> GetCardsByUser(string username)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Guid userId = entity.Users.First(el => el.Username == username).UserID;
            return entity.WorkCards.Where(el => el.UserId == userId).ToList();
        }

        public static WorkCards GetCardById(Guid cardId)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.WorkCards.First(el => el.CardId == cardId);
        }
    }

}
