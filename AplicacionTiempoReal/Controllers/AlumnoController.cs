using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AplicacionTiempoReal.HUB;
using AplicacionTiempoReal.DAO;
using AplicacionTiempoReal.Models;

namespace AplicacionTiempoReal.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly DAO_Matricula _repository;
        private readonly IHubContext<AlumnoHUB> _hubContext;


        public AlumnoController(DAO_Matricula repository, IHubContext<AlumnoHUB> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAllAsync();
            return View(products);
        }

        //[HttpPost]
        //public async Task<IActionResult> Add(Alumnos product)
        //{
        //    await _repository.AddAsync(product);
        //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "ProductAdded", product);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Alumnos product)
        //{
        //    await _repository.UpdateAsync(product);
        //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "ProductUpdated", product);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _repository.DeleteAsync(id);
        //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "ProductDeleted", id);
        //    return RedirectToAction(nameof(Index));
        //}



    }
}
