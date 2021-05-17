using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{

    // Made by Andreas
    public class Branch
    {

        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int BranchPostalCode { get; set; }

        public Branch () { }

        public Branch (string name, int postalcode)
        {
            BranchName = name;
            BranchPostalCode = postalcode;
        }
    }
}
