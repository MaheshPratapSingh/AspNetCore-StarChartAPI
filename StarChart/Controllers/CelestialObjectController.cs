using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetById")]
        public IActionResult GetById(int Id)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);
            if (celestialObject == null)
                return NotFound(Id);
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.
                    Where(c => c.OrbitedObjectId == Id).ToList();
                return Ok(celestialObject);
            }
        }

        [HttpGet(Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects.Find(name);
            if (celestialObject == null)
                return NotFound(name);
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.
                    Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
                return Ok(celestialObject);
            }
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = celestialObjects.
                    Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
        }
    }
}
