using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenCompanion.Models
{
    public class MaterialDeposit
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
    }
}
