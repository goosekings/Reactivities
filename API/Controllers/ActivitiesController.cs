using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    // Inherits from BaseApiController, which has [ApiController] and [Route("[controller]")] attributes
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;
        public ActivitiesController(DataContext context)
        {
            _context = context;
        }

        // endpoint to return all activities in database
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        // ADD ERROR HANDLING LATER
        // endpoint to return one activity based on passed in id
        [HttpGet("{id}")] // activities/id
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            // Finds activity with matching id from DbSet
            return await _context.Activities.FindAsync(id);
        }
    }
}