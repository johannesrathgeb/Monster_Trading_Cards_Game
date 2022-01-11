using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Cards
{
    static class UploadCardsToDB
    {
        
        
        public static void upload()
        {
            DB database = DB.getInstance();
            database.Connect();
            
            //Monsters
            database.createCard("InsaneFireDragon", 20, ICard.Element_type.fire, ICard.Monster_type.Dragon, ICard.Monster_type.Elve);
            database.createCard("WaterDragon", 5, ICard.Element_type.water, ICard.Monster_type.Dragon, ICard.Monster_type.Elve);
            database.createCard("NormalDragon", 10, ICard.Element_type.normal, ICard.Monster_type.Dragon, ICard.Monster_type.Elve);
            database.createCard("FireGoblin", 10, ICard.Element_type.fire, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon);
            database.createCard("WaterGoblin", 15, ICard.Element_type.water, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon);
            database.createCard("NormalGoblin", 5, ICard.Element_type.normal, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon);
            database.createCard("FireElve", 10, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None);
            database.createCard("WaterElve", 5, ICard.Element_type.water, ICard.Monster_type.Elve, ICard.Monster_type.None);
            database.createCard("NormalElve", 15, ICard.Element_type.normal, ICard.Monster_type.Elve, ICard.Monster_type.None);
            database.createCard("FireKnight", 5, ICard.Element_type.fire, ICard.Monster_type.Knight, ICard.Monster_type.Spell);
            database.createCard("WaterKnight", 5, ICard.Element_type.water, ICard.Monster_type.Knight, ICard.Monster_type.Spell);
            database.createCard("NormalKnight", 15, ICard.Element_type.normal, ICard.Monster_type.Knight, ICard.Monster_type.Spell);
            database.createCard("FireWizzard", 15, ICard.Element_type.fire, ICard.Monster_type.Wizzard, ICard.Monster_type.None);
            database.createCard("WaterWizzard", 10, ICard.Element_type.water, ICard.Monster_type.Wizzard, ICard.Monster_type.None);
            database.createCard("NormalWizzard", 10, ICard.Element_type.normal, ICard.Monster_type.Wizzard, ICard.Monster_type.None);
            database.createCard("FireKraken", 5, ICard.Element_type.fire, ICard.Monster_type.Kraken, ICard.Monster_type.None);
            database.createCard("InsaneWaterKraken", 20, ICard.Element_type.water, ICard.Monster_type.Kraken, ICard.Monster_type.None);
            database.createCard("NormalKraken", 10, ICard.Element_type.normal, ICard.Monster_type.Kraken, ICard.Monster_type.None);
            database.createCard("FireOrk", 5, ICard.Element_type.fire, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);
            database.createCard("WaterOrk", 5, ICard.Element_type.water, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);
            database.createCard("NormalOrk", 15, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);

            //Spells
            database.createCard("WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Spell, ICard.Monster_type.Kraken);
            database.createCard("FireSpell", 10, ICard.Element_type.fire, ICard.Monster_type.Spell, ICard.Monster_type.Kraken);
            database.createCard("InsaneWaterSpell", 20, ICard.Element_type.water, ICard.Monster_type.Spell, ICard.Monster_type.Kraken);
            database.createCard("InsaneFireSpell", 20, ICard.Element_type.fire, ICard.Monster_type.Spell, ICard.Monster_type.Kraken);

            database.Disconnect();
        }
    }
}
