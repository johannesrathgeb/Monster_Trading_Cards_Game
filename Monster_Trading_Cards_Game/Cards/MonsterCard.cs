using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{

    public class MonsterCard : ICard
    {
        private int _id;
        private string _name;
        private readonly double _damage;
        private ICard.Element_type _elementType;
        private ICard.Monster_type _monsterType;
        private ICard.Monster_type _weakness;
        

        
        public MonsterCard(int id, string name, double damage, ICard.Element_type elementType, ICard.Monster_type monsterType, ICard.Monster_type weakness)
        {
            _id = id;
            _name = name;
            _damage = damage;
            _elementType = elementType;
            _monsterType = monsterType;
            _weakness = weakness;
        }

        string ICard.name { get => _name; set => _name = value; }
        double ICard.damage { get => _damage; }
        int ICard.id { get => _id; }
        ICard.Element_type ICard.elementType { get => _elementType; }
        ICard.Monster_type ICard.monsterType { get => _monsterType; }
        ICard.Monster_type ICard.weakness { get => _weakness; }
    }
}
