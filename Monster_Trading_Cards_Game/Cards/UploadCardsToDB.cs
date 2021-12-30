using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Cards
{
    class UploadCardsToDB
    {
        DB database = new DB();
        
        public void upload()
        {
            database.Connect();
            
            //Monsters
            database.createCard("FireDragon", 15, ICard.Element_type.fire, ICard.Monster_type.Dragon, ICard.Monster_type.Elve);
            database.createCard("WaterGoblin", 10, ICard.Element_type.water, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon);
            database.createCard("FireElve", 5, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None);
            database.createCard("NormalKnight", 10, ICard.Element_type.normal, ICard.Monster_type.Knight, ICard.Monster_type.Spell);
            database.createCard("FireWizzard", 10, ICard.Element_type.fire, ICard.Monster_type.Wizzard, ICard.Monster_type.None);
            database.createCard("WaterKraken", 15, ICard.Element_type.water, ICard.Monster_type.Kraken, ICard.Monster_type.None);
            database.createCard("NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);

            //Spells
            database.createCard("FireSpell", 10, ICard.Element_type.fire, ICard.Monster_type.Spell, ICard.Monster_type.Kraken);
            database.createCard("WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Spell, ICard.Monster_type.Kraken);

            database.Disconnect();
        }
    }
}
