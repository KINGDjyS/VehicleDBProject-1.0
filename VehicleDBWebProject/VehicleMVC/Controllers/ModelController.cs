using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleMVC.Models;
using VehicleService;

namespace VehicleMVC.Controllers
{
    public class ModelController : Controller
    {
        private readonly IVehicleModelService ModelService;
        private readonly IMapper Mapper;

        public ModelController(IVehicleModelService modelService, IMapper mapper)
        {
            ModelService = modelService;
            Mapper = mapper;
        }
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["MakerIDParam"] = sortOrder == "Make" ? "make_desc" : "Make";
            ViewData["AbrvSortParam"] = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var model = ModelService.GetVehicleModels();

            if (!String.IsNullOrEmpty(searchString))
            {
                model = ModelService.VehicleModelFindByName(searchString);
                var modelDTOs = model.ToList().Select(Mapper.Map<VehicleModel, VehicleModelDTO>);
                int pageSearchSize = 10;
                return View(PagedList<VehicleModelDTO>.Create(modelDTOs, pageNumber ?? 1, pageSearchSize));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(descn => descn.Name);
                    break;
                case "Abrv":
                    model = model.OrderBy(abr => abr.Abrv);
                    break;
                case "abrv_desc":
                    model = model.OrderByDescending(abd => abd.Abrv);
                    break;
                case "Make":
                    model = model.OrderBy(mak => mak.VehicleMakeId);
                    break;
                case "make_desc":
                    model = model.OrderByDescending(mad => mad.VehicleMakeId);
                    break;
                default:
                    model = model.OrderBy(nam => nam.Name);
                    break;
            }

            int pageSize = 10;
            var modelDTO = model.ToList().Select(Mapper.Map<VehicleModel, VehicleModelDTO>);
            return View(PagedList<VehicleModelDTO>.Create(modelDTO, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Makers = ModelService.GetMakes();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleModelDTO vehicleModelDTO)
        {
            if (ModelState.IsValid)
            {
                var vehicleModel = Mapper.Map<VehicleModel>(vehicleModelDTO);
                await ModelService.AddVehicleModelAsync(vehicleModel);
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
            VehicleModelDTO modelDTO = Mapper.Map<VehicleModelDTO>(await ModelService.GetOneVehicleModelAsync(Id));
            return View(modelDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleModelDTO vehicleModelDTO)
        {
            VehicleModel model = Mapper.Map<VehicleModel>(vehicleModelDTO);
            await ModelService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id.Equals(null))
            {
                return NotFound();
            }

            await ModelService.DeleteVehicleModelAsync(Id);

            return RedirectToAction(nameof(Index));
        }
    }
}