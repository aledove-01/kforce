using System;
using kforceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace kforceApp.Services
{
	public interface ICarRepository
	{
        public Task<Result<IEnumerable<Car>>> GetCarsWhitOutIdAsync();
        public Task<Result<Car>> GetByIdAsync(int id);
        public Task<Result<Car>> AddAsync(Car car);
        public Task<Result<Car>> UpdateAsync(Car car);
        public Task<Result<Car>> DeleteAsync(int id);
    }
}

