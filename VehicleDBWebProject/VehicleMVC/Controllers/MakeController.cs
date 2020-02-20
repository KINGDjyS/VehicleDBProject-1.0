using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleMVC.Models;
using VehicleService;

namespace VehicleMVC.Controllers
{
    public class MakeController : Controller
    {
        private readonly IVehicleMakeService MakeService;
        private readonly IMapper Mapper;

        public MakeController(IVehicleMakeService makeService, IMapper mapper)
        {
            MakeService = makeService;
            Mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AbrvSortParam"] = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";
            
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var make = MakeService.GetVehicleMakes();

            if (!String.IsNullOrEmpty(searchString))
            {
                make = MakeService.VehicleMakeFindByName(searchString);
                var makeDtos = make.ToList().Select(Mapper.Map <VehicleMake, VehicleMakeDTO>);
                int pageSearchSize = 10;
                return View(PagedList<VehicleMakeDTO>.Create(makeDtos, pageNumber ?? 1, pageSearchSize));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    make = make.OrderByDescending(descn => descn.Name);
                    break;
                case "Abrv":
                    make = make.OrderBy(abr => abr.Abrv);
                    break;
                case "abrv_desc":
                    make = make.OrderByDescending(abd => abd.Abrv);
                    break;
                default:
                    make = make.OrderBy(nam => nam.Name);
                    break;
            }

            int pageSize = 10;
            var makeDto = make.ToList().Select(Mapper.Map<VehicleMake, VehicleMakeDTO>);
            return View(PagedList<VehicleMakeDTO>.Create(makeDto, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleMakeDTO makeDTO)
        {
            if (ModelState.IsValid)
            {
                var vehicleMake = Mapper.Map<VehicleMake>(makeDTO);
                await MakeService.AddVehicleMakerAsync(vehicleMake);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            VehicleMakeDTO makeDTO = Mapper.Map<VehicleMakeDTO>(await MakeService.GetOneVehicleMakerAsync(Id));
            return View(makeDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMakeDTO vehicleMakeDTO)
        {
            VehicleMake make = Mapper.Map<VehicleMake>(vehicleMakeDTO);
            await MakeService.UpdateAsync(make);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id.Equals(null))
            {
                return NotFound();
            }

            await MakeService.DeleteVehicleMakerAsync(Id);
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
