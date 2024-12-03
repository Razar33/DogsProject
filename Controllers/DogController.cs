using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Domain;
using DogsApp.Models.Dog;
using Microsoft.AspNetCore.Mvc;

namespace DogsApp.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        private readonly ApplicationDbContext _context;

        public DogController(IDogService dogService, ApplicationDbContext context)
        {
            this._dogService = dogService;
            this._context = context;
        }

        // GET: DogController
        public IActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _dogService.GetDogs(searchStringBreed, searchStringName)
                .Select(dogFromDb => new DogAllViewModel
                {
                    Id = dogFromDb.Id,
                    Name = dogFromDb.Name,
                    Age = dogFromDb.Age,
                    Breed = dogFromDb.Breed,
                    Picture = dogFromDb.Picture
                }).ToList();

            return View(dogs);
        }

        // Updated GET: DogController/Details/5
        public IActionResult Details(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item == null)
            {
                return NotFound();
            }

            DogDetailsViewModel dog = new DogDetailsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };

            return View(dog);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogCreateViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = _dogService.Create(
                    bindingModel.Name,
                    bindingModel.Age,
                    bindingModel.Breed,
                    bindingModel.Picture
                );

                if (isCreated) return this.RedirectToAction("Success");
            }

            return View(bindingModel);
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            Dog? item = _context.Dogs.Find(id);
            if (item == null) return NotFound();

            DogEditViewModel dog = new DogEditViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogEditViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = _dogService.UpdateDog(
                    id,
                    bindingModel.Name,
                    bindingModel.Age,
                    bindingModel.Breed,
                    bindingModel.Picture
                );

                if (isUpdated) return RedirectToAction("Index");
            }

            return View(bindingModel);
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            Dog? item = _context.Dogs.Find(id);
            if (item == null) return NotFound();

            DogEditViewModel dog = new DogEditViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };

            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            bool isRemoved = _dogService.RemoveById(id);
            if (isRemoved) return RedirectToAction("Index");

            return NotFound();
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
