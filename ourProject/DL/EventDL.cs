﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DTO;
//using ourProject.Models;

namespace DL
{
   public class EventDL :IEventDL
    {
        SeatOrganizerContext _myDB;
        ILogger<EventDL> logger;
        public EventDL(SeatOrganizerContext SeatOrganizerContext, ILogger<EventDL> logger)
        {
            _myDB = SeatOrganizerContext;
            this.logger = logger;
        }

        public async Task<List<Event>> getEventByUserIdDL(int id)
        {


            try
            {
                //List<Event> e = await _myDB.EventPerUsers.Include("EventPerUser.EventId").Where(u => u.UserId == id).ToListAsync();
                //List<Event> eventlist = await _myDB.EventPerUsers.Where(u => u.UserId == id).Include(e => e.EventId).ToListAsync();
                List<Event> eventlist = await _myDB.EventPerUsers.Include(e => e.Event).Where(u => u.UserId == id).Select(e => e.Event).ToListAsync();
                return eventlist;
            }


            catch (Exception ex) { logger.LogError(ex.Message); };
            return null;
           
        }

        public async Task<Event> getEventByEventIdDL(int id)
        {

            Event e = await _myDB.Events.Where(e => e.Id.Equals(id)).FirstOrDefaultAsync();
            
                return e;
        }

        public async Task<int> PostDL(Event e)
        {
            await _myDB.Events.AddAsync(e);
            await _myDB.SaveChangesAsync();
            return e.Id;
        }
        
        public async Task PutDL(int id,Event e)
        {
            Event eventToUpdate = await _myDB.Events.FindAsync(id);

            if (eventToUpdate == null)
            {
                return;
            }
            else
            {
                
                _myDB.Entry(eventToUpdate).CurrentValues.SetValues(e);
            }
            
            await _myDB.SaveChangesAsync();
        }

        public async Task DeleteDL(int id)
        {
           Event e= await _myDB.Events.FindAsync(id);
            try
            {
                
                List<EventPerUser> l = await _myDB.EventPerUsers.Where(y => y.EventId == id).ToListAsync();
                _myDB.EventPerUsers.RemoveRange(l);
                _myDB.Events.Remove(e);
                await _myDB.SaveChangesAsync();
            }
            catch(Exception a)
            {
                var w = 1;
            }
            
        }

    }


}
