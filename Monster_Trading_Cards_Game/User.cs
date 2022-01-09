using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class User
    {
        public CardStack stack;
        public CardDeck deck = new CardDeck();
        public CardDeck battleDeck = new CardDeck();
        private string _username, _password;
        private int _elo, _playedGames, _wonGames, _coins, _id;
        public User(int id, string username, string password, int elo, int coins, CardStack stack, CardDeck deck)
        {
            _id = id;
            _username = username;
            _password = password;
            _elo = elo;
            _coins = coins;
            this.stack = stack;
            this.deck = deck;
        }

        public void setDeck()
        {
            string input;
            ICard foundCard;
            deck.clearDeck(id);
            Console.Clear();
            for (int i = 1; i<=4; i++){
                stack.printList();
                Console.WriteLine("Choose Card " + i + " for your Deck");
                input = Console.ReadLine();
                foundCard = stack.searchCard(input);
                if (foundCard == null)
                {
                    Console.Clear();
                    Console.WriteLine("Could'nt find card. Try again!");
                    Console.WriteLine();
                    i--;
                }
                else
                {
                    Console.Clear();
                    if (deck.addCard(foundCard, id) == false)
                    {
                        Console.WriteLine("Card is already in deck!");
                        Console.WriteLine();
                        i--;
                    }
                }
            }
            Console.WriteLine("You created this deck:");
            deck.printDeck();
        }

        public void updateBattleDeck()
        {
            battleDeck = new CardDeck(deck.cards);
        }

        public int id { get => _id; set => _id = value; }
        public int elo { get => _elo; set => _elo = value; }
        public int playedGames { get => _playedGames; set => _playedGames = value; }
        public int wonGames { get => _wonGames; set => _wonGames = value; }
        public string username { get => _username; set => _username = value; }
        public string password { get => _password; set => _password = value; }
    } 
}
