using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    interface ICard
    {
        int id { get; }
        string name { get; set; }
        double damage { get;}
        enum Element_type
        {
            fire,
            water,
            normal
        }
        Element_type elementType { get; }
        enum Monster_type
        {
            Goblin,
            Dragon,
            Wizzard,
            Knight,
            Kraken,
            Elve,
            Ork,
            Spell,
            None
        }
        Monster_type monsterType { get; }
        Monster_type weakness { get; }
    }
}
