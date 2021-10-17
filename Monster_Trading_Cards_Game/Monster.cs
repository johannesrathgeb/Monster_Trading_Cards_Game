using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    
    class Monster : ICard
    {
        string name;
        public Monster()
        {
            
        }

        string ICard._name { get => return name; set => throw new NotImplementedException(); }
        double ICard._damage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
