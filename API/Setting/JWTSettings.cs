using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Setting
{
    public class JWTSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; }= null!;
        public string Audience { get; set; }= null!;
        public double DurationInDays { get; set; }
    }
}