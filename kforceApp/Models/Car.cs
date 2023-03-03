using System;
using System.ComponentModel.DataAnnotations;

namespace kforceApp.Models
{
	public class Car
	{
    
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Doors { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }

        public Car() { }
		
		
	
	}
}

