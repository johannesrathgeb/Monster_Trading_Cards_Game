using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    interface ICard
    {
        protected string _name { get; set; }
        
        protected double _damage { get; set; }
        protected enum Element_type
        {
            fire,
            water
        }
    }
}
