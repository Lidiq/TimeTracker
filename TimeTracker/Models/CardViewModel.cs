using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTracker.DAL;

namespace TimeTracker.Models
{
    public class CardViewModel
    {
        public Guid CardId { get; set; }
        public string LogComment { get; set; }
        public int Duration { get; set; }
        public DateTime start { get; set; }

        public CardViewModel(WorkCards card)
        {
            this.CardId = card.CardId;
           this.Duration = card.Duration;
           this.LogComment = card.LogComment;
           this.start = card.StartTime;
        }
    }
}