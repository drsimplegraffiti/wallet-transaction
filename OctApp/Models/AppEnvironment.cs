using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class AppEnvironment: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; } 
    }
}