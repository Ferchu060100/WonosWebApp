using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WonosWebApp.Data;
using WonosWebApp.Helpers;
using WonosWebApp.Models;
using WonosWebApp.Models.Resultados;
using WonosWebApp.Models.ViewModels;

namespace WonosWebApp.Controllers
{
    public class BonoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BonoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Calcular()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Calcular(Bono bono)
        {
            int ID;
                //bono.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                _context.Bonos.Add(bono);
                _context.SaveChanges();
                ID = bono.Id;
            return RedirectToAction("Resultados", new { bonoId = ID });
        }

        [HttpGet]
        public ActionResult Resultados(int? bonoId)
        {
            Bono bono;

            
            bono = _context.Bonos.FirstOrDefault(x => x.Id == bonoId);
            Estructuracion estructuracion = new Estructuracion();
            estructuracion = Finanzas.ResultadosEstructuracion(bono);
            List<Periodo> periodos = new List<Periodo>();
            if (bono.TipoMetodo == "Americano")
            {
                periodos = Finanzas.ResultadosPeriodosAmericano(bono, estructuracion);
            }
            else if (bono.TipoMetodo == "Frances")
            {
                periodos = Finanzas.ResultadosPeriodosFrances(bono, estructuracion);
            }
            else
            {
                periodos = Finanzas.ResultadosPeriodosAleman(bono, estructuracion);
            }
            Rentabilidad rentabilidad = new Rentabilidad();
            rentabilidad = Finanzas.ResultadosRentabilidad(estructuracion, periodos, bono);
            Utilidad utilidad = new Utilidad();
            utilidad = Finanzas.ResultadosUtilidad(estructuracion, periodos);
            ResultadosViewModel resultados = new ResultadosViewModel();
            resultados.estructura = estructuracion;
            resultados.periodos = periodos;
            resultados.rentabilidad = rentabilidad;
            resultados.utilidad = utilidad;
            ViewBag.nombre = bono.Nombre;
            ViewBag.tipometodo = bono.TipoMetodo;
            return View(resultados);
        }
        [HttpGet]
        public ActionResult Listar()
        {
            ListarBonosViewModel vm = new ListarBonosViewModel();
            
            vm.Bonos = _context.Bonos.Where(x => x.ApplicationUserId == SessionHelper.User.Id).ToList();
            return View(vm.Bonos);
        }




    }
}
