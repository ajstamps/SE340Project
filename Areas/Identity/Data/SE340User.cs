using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SE340.Models;

namespace SE340.Areas.Identity.Data
{
    public class SE340User : IdentityUser
    {
        [PersonalData]
        public List<Vehicle> FavoritedVehicles { get; set; }

        public SE340User ()
        {
            FavoritedVehicles = new List<Vehicle>();
        }
    }
}
