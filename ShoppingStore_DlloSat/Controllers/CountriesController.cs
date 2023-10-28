using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingStore_DlloSat.DAL;
using ShoppingStore_DlloSat.DAL.Entities;
using ShoppingStore_DlloSat.Models;

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

        #region Acciones de Country
        // GET: Countries
        public async Task<IActionResult> Index() //MÉTODOS = ACTIONS DEL CONTROLADOR
        {
            var xsdfaf = await _context.Countries
                .Include(c => c.States) //El Include me hace las veces del INNER JOIN
                .ToListAsync();
            return View(xsdfaf);

        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(Guid? id) //Cédula = 123
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.Include(c => c.States)
                .ThenInclude(s => s.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);
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
            if (id == null || _context.Countries == null) return NotFound();

            var country = await _context.Countries.FindAsync(id); //Aquí voy a la BD y me traigo ese país con ese ID    

            if (country == null) return NotFound();

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
            if (id == null || _context.Countries == null) return NotFound();

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

            if (country == null) return NotFound();

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Countries == null)
                return Problem("Entity set 'DataBaseContext.Countries'  is null.");

            var country = await _context.Countries.FindAsync(id);
            if (country != null)
                _context.Countries.Remove(country); //El método Remove() es para eliminar el país

            await _context.SaveChangesAsync(); //Elimino directamente el país en la BD
            return RedirectToAction(nameof(Index)); // Redireciono al index de País
        }

        private bool CountryExists(Guid id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion

        #region Acciones de State

        //AddState
        [HttpGet]
        public async Task<IActionResult> AddState(Guid? countryId)
        {
            if (countryId == null) return NotFound();

            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryId);

            if (country == null) return NotFound();

            StateViewModel stateViewModel = new StateViewModel
            {
                CountryId = country.Id
            };

            return View(stateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel stateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new() //Este ya es la tabla State donde guardaré el estado/dpto en BD
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == stateViewModel.CountryId),
                        Name = stateViewModel.Name,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = null,
                    };

                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = stateViewModel.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un Dpto/Estado con el mismo nombre en este país.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(stateViewModel);
        }

        #endregion

        #region Acciones de City
        #endregion
    }
}
