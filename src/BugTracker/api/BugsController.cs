﻿using BugTracker.Hubs;
using BugTracker.Model;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace BugTracker.api
{
    //Test Change
    [Route("api/bugs/[action]")]
    public class BugsController : Controller
    {
        IBugsRepository _bugsRepository = new BugsRepository();
        private IHubContext _hub;

        public BugsController(IConnectionManager connectionManager)
        {
            _hub = connectionManager.GetHubContext<BugHub>();
        }
	
	[HttpGet("~/api/bugs")]
        public IEnumerable<Bug> Get()
        {
            return _bugsRepository.GetBugs();
        }

        // Web API expects primitives coming from the request body to have no key value (e.g. '') - they should be encoded, then as '=value'
        [HttpPost]
        public Bug BackLog(int id)
        {
            var bug = _bugsRepository.GetBugs().First(b => b.id == id);
            bug.state = "backlog";

            _hub.Clients.All.moved(bug);

            return bug;
        }

        [HttpPost]
        public Bug Working(int id)
        {
            var bug = _bugsRepository.GetBugs().First(b => b.id == id);
            bug.state = "working";

            _hub.Clients.All.moved(bug);

            return bug;
        }

        [HttpPost]
        public Bug Done(int id)
        {
            var bug = _bugsRepository.GetBugs().First(b => b.id == id);
            bug.state = "done";

            _hub.Clients.All.moved(bug);

            return bug;
        }
    }
}