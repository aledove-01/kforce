using System;
using System.Runtime.ConstrainedExecution;
using kforceApp.Contexts;
using kforceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kforceApp.Services
{
	public class CarRepository : ICarRepository
    {
        private readonly appContext _context;

        public CarRepository(appContext context)
        {
            _context = context;
        }

        public async Task<Result<Car>> AddAsync(Car car)
        {
            try
            {
                var existingCar = await _context.Cars.FirstOrDefaultAsync(c => c.Make == car.Make &&
                                                                     c.Model == car.Model &&
                                                                     c.Year == car.Year &&
                                                                     c.Doors == car.Doors &&
                                                                     c.Color == car.Color);
                if (existingCar != null)
                {
                    return new Result<Car>
                    {
                        Success = false,
                        ErrorMessage = "In a car with the same characteristics already exists."
                    };
                }

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();

                return new Result<Car>
                {
                    Success = true,
                    Data = car
                };
                 
            }
            catch (Exception ex)
            {
                return new Result<Car>
                {
                    Success = false,
                    ErrorMessage = $"Error adding the car: {ex.Message}"
                };
            }
        }


        public  async Task<Result<Car>> DeleteAsync(int id)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                {
                    return new Result<Car>
                    {
                        Success = false,
                        ErrorMessage = "Not found"
                    };
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return new Result<Car>
                {
                    Success = true,
                    Data = car
                };
            }
            catch (Exception ex)
            {
                return new Result<Car>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred while retrieving the car. {ex.Message}"
                };
            }
        }

        public async Task<Result<Car>> GetByIdAsync(int id)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);

                if (car == null)
                {
                    return new Result<Car>
                    {
                        Success = false,
                        ErrorMessage = "Not found"
                    };
                }

                return new Result<Car>
                {
                    Success = true,
                    Data = car
                };
            }
            catch (Exception ex)
            {
                return new Result<Car>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred while retrieving the car. {ex.Message}"
                };
            }
        }

        public async Task<Result<IEnumerable<Car>>> GetCarsWhitOutIdAsync()
        {
            try
            {
                var cars = await _context.Cars
                    .Select(c => new Car { Make = c.Make, Model = c.Model, Year = c.Year, Doors = c.Doors, Color = c.Color, Id = c.Id })
                    .ToListAsync();
                return new Result<IEnumerable<Car>>
                {
                    Success = true,
                    Data = (IEnumerable<Car>)cars
                };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<Car>>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred while retrieving data. {ex.Message}"
                };
            }
        }

        public async Task<Result<Car>> UpdateAsync(Car car)
        {
            var existingCar = await _context.Cars.FindAsync(car.Id);

            if (existingCar == null)
            {
                return new Result<Car>
                {
                    Success = false,
                    ErrorMessage = "Not found"
                };
            }

            try
            {
                existingCar.Make = car.Make;
                existingCar.Model = car.Model;
                existingCar.Year = car.Year;
                existingCar.Doors = car.Doors;
                existingCar.Color = car.Color;
                existingCar.Price = car.Price;

                _context.Cars.Update(existingCar);
                await _context.SaveChangesAsync();

                return new Result<Car>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result<Car>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred while retrieving the car. {ex.Message}"
                };
            }
        }
    }
}

