using System;
namespace kforceApp.Services
{
	public class Result<T>
	{
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}

