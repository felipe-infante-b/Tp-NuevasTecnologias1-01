using Biblioteca07.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca07.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Libros.ToListAsync());
        }

        public async Task<IActionResult> Libros()
        {
            return View(await _context.Libros.ToListAsync());
        }

        public async Task<IActionResult> Clientes()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        public async Task<IActionResult> Alquileres()
        {
            return View(await _context.Alquileres.ToListAsync());
        }

        public IActionResult CrearLibro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearLibro([Bind("Id,Titulo,Autor,CantidadCopias")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Libros));
            }
            return View(libro);
        }

        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCliente([Bind("Id,Nombre")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Clientes));
            }
            return View(cliente);
        }

        public IActionResult CrearAlquiler()
        {
            ViewData["Libros"] = new SelectList(_context.Libros, "Id", "Titulo");
            ViewData["Clientes"] = new SelectList(_context.Clientes, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAlquiler([Bind("Id,LibroId,ClienteId")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                var libro = await _context.Libros.FindAsync(alquiler.LibroId);
                if (libro != null && libro.CantidadCopias > 0)
                {
                    libro.CantidadCopias--; // Decrementar la cantidad de copias
                    alquiler.Devuelto = "Alquiler";
                    _context.Update(libro); // Actualizar el libro en el contexto
                    _context.Add(alquiler); // Agregar el alquiler
                    await _context.SaveChangesAsync(); // Guardar los cambios
                    return RedirectToAction(nameof(Alquileres));
                }
                else
                {
                    // Manejar el caso cuando no hay copias disponibles
                    ModelState.AddModelError("", "No hay copias disponibles para alquilar.");
                }
            }
            ViewData["Libros"] = new SelectList(_context.Libros, "Id", "Titulo", alquiler.LibroId);
            ViewData["Clientes"] = new SelectList(_context.Clientes, "Id", "Nombre", alquiler.ClienteId);
            return View(alquiler);
        }

        public IActionResult CrearDevolucion()
        {
            ViewData["Libros"] = new SelectList(_context.Libros, "Id", "Titulo");
            ViewData["Clientes"] = new SelectList(_context.Clientes, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDevolucion([Bind("Id,LibroId,ClienteId")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                var libro = await _context.Libros.FindAsync(alquiler.LibroId);
                if (libro != null)
                {
                    libro.CantidadCopias++; // Decrementar la cantidad de copias
                    alquiler.Devuelto = "Devolucion";
                    _context.Update(libro); // Actualizar el libro en el contexto
                    _context.Update(alquiler); // Agregar el alquiler
                    await _context.SaveChangesAsync(); // Guardar los cambios
                    return RedirectToAction(nameof(Alquileres));
                }
                else
                {
                    // Manejar el caso cuando no hay copias disponibles
                    ModelState.AddModelError("", "No hay copias disponibles para alquilar.");
                }
            }
            ViewData["Libros"] = new SelectList(_context.Libros, "Id", "Titulo", alquiler.LibroId);
            ViewData["Clientes"] = new SelectList(_context.Clientes, "Id", "Nombre", alquiler.ClienteId);
            return View(alquiler);
        }
    }
}