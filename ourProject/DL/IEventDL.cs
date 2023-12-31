﻿using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
   public interface IEventDL
    {

         Task<List<Event>> getEventByUserIdDL(int id);
        Task<int> PostDL(Event e);
        Task<Event> getEventByEventIdDL(int id);
        Task PutDL(int id, Event e);
        Task DeleteDL(int id);
    }
}