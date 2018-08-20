using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Models
{
    public class ValueModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
