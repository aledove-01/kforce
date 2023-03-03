using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using kforceApp.Contexts;
using kforceApp.Models;
using kforceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kforceApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarRepository _carRepo;

        public CarController(ILogger<CarController> logger, ICarRepository carRepo)
        {
            _logger = logger;
            _carRepo = carRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateCar([FromBody] Car car)
        {
            var result = await _carRepo.UpdateAsync(car);

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete()]
        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var result = await _carRepo.DeleteAsync(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> AddCar([FromBody] Car car)
        {
            var result = await _carRepo.AddAsync(car);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet()]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var result = await _carRepo.GetByIdAsync(id);
            if (result.Success)
            {
                return result.Data;
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsWhitOutId()
        {
            var result = await _carRepo.GetCarsWhitOutIdAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet()]
        public async Task<ActionResult> CheckPrice(int id, int price, int priceEstimated)
        {
            try
            {
                var response = await GetCar(id);

               
                if (response.Value == null)
                {
                    return BadRequest("Not found");               
                }

                var car = response.Value;
                if (Math.Abs(car.Price - price) <= priceEstimated)
                {
                    return Ok(car);
                }
                else
                {
                    return Ok(null);
                    
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while retrieving the car. {ex.Message}");
            }
        }
    }
}

