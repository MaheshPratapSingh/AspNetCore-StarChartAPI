using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{id:int}", Name = "GetById")]
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

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name);
            if (!celestialObjects.Any())
                return NotFound();
            else
            {
                foreach (var item in celestialObjects)
                {
                    item.Satellites = _context.CelestialObjects.
                    Where(c => c.OrbitedObjectId == item.Id).ToList();
                }
                return Ok(celestialObjects);
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
        
        [HttpPost]
        public IActionResult Create(CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { celestialObject.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existingCelestialObject = _context.CelestialObjects.Find(id);
            if (existingCelestialObject == null)
                return NotFound();
            else
            {
                existingCelestialObject.Name = celestialObject.Name;
                existingCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
                existingCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
                _context.CelestialObjects.Update(existingCelestialObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existingCelestialObject = _context.CelestialObjects.Find(id);
            if (existingCelestialObject == null)
                return NotFound();
            else
            {
                existingCelestialObject.Name = name;
                _context.CelestialObjects.Update(existingCelestialObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCelestialObjects = _context.CelestialObjects.Where(c=> c.Id==id || c.OrbitedObjectId == id).ToList();
            if (!existingCelestialObjects.Any())
                return NotFound();
            else
            {                
                _context.CelestialObjects.RemoveRange(existingCelestialObjects);
                _context.SaveChanges();
                return NoContent();
            }
        }
    }
}
