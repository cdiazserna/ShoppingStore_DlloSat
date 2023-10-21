using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShoppingStore_DlloSat.DAL;
using ShoppingStore_DlloSat.DAL.Entities;

namespace ShoppingStore_DlloSat.Controllers
{
    public class CountriesController : Controller
    {
        //Creo el parámetro ReadOnly que es el que se va a manejar para el resto de la clase: _context
        private readonly DataBaseContext _context;

        //Patrón de diseño llamado INYECCIÓN DE DEPENDENCIAS
        public CountriesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index() //MÉTODOS = ACTIONS DEL CONTROLADOR
        {
            return _context.Countries != null ? View(await _context.Countries.ToListAsync()) : Problem("Entity set 'DataBaseContext.Countries'  is null.");
            //IF TERNARIO, el famoso IF pero simplificado..... el signo ? significa ENTONCES...... el signo : significa SINO

            //El Método ToListAsync me sirve para consultar UNA LISTA
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(Guid? id) //Cédula = 123
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            //El Método FirstOrDefaultAsync me sirve para consultar UN OBJETO

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create //GET = SELECT
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create //POST = CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    country.Id = Guid.NewGuid();
                    country.CreatedDate = DateTime.Now; //Aquí automatizo el CreatedDate de un objeto
                    _context.Add(country); //Método Add() es para crear en BD
                    await _context.SaveChangesAsync(); //aquí va a la capa MODEL y GUARDA el país en la tabla Countries
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre");
                    }
                }
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id); //Aquí voy a la BD y me traigo ese país con ese ID    

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    country.ModifiedDate = DateTime.Now; //Aquí automáticamente le pongo fecha de modificación a la propiedad o campo ModifiedDate de la tabla Countries.

                    _context.Update(country); //Método Update() es para actualizar ese objeto en BD
                    await _context.SaveChangesAsync(); //Aquí ya hago el update en BD
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre");
                    }
                }
            }

            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DataBaseContext.Countries'  is null.");
            }

            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country); //El método Remove() es para eliminar el país
            }

            await _context.SaveChangesAsync(); //Elimino directamente el país en la BD
            return RedirectToAction(nameof(Index)); // Redireciono al index de País
        }

        private bool CountryExists(Guid id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
