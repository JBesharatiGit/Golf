using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf
{
    public class Ball
    {
        const double Gravity = 9.8;
        private double angleInRadians;
        public double Angle{get;set;}
        public double Velocity { get; set; }
        public double Distance { get; set; }
       // Distance: Math.Pow(velocity, 2) / GRAVITY* Math.Sin(2 * angleInRadians)
        public double CalcuateDistanc() 
        {
            angleInRadians = (Math.PI / 180) * Angle;


            return Math.Pow(Velocity, 2) / Gravity * Math.Sin(2 * angleInRadians);
        }

    }
}
