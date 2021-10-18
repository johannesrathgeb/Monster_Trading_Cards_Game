using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class SpellCard : ICard
    {
        private string _name;
        private readonly double _damage;
        private ICard.Element_type _elementType;
        private ICard.Monster_type _monsterType;

        public SpellCard(string name, double damage, ICard.Element_type elementType)
        {
            _name = name;
            _damage = damage;
            _elementType = elementType;
            _monsterType = ICard.Monster_type.Spell;
        }

        string ICard.name { get => _name; set => _name = value; }
        double ICard.damage { get => _damage; }
        ICard.Element_type ICard.elementType { get => _elementType; }
        ICard.Monster_type ICard.monsterType { get => _monsterType; }
    }
}
