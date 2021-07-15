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

        [HttpGet("{id:int}",Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.
                    Where(c => c.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
        }

        [HttpGet("{name:string}",Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects.SingleOrDefault(c=> c.Name == name);
            if (celestialObject == null)
                return NotFound();
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
            var celestialObjects = _context.CelestialObjects.ToList();
            if (!celestialObjects.Any())
                return NotFound();
            else
            {
                foreach (var celestialObject in celestialObjects)
                {
                    celestialObject.Satellites = celestialObjects.
                        Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
                }
                return Ok(celestialObjects);
            }
        }
    }
}
