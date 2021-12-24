using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class User
    {
        CardStack stack;
        public CardDeck deck = new CardDeck();
        private string _username, _password;
        private int _elo, _playedGames, _wonGames, _coins;
        public User(string username, string password, CardStack stack, CardDeck deck)
        {
            _username = username;
            _password = password;
            this.stack = stack;
            this.deck = deck;
            _elo = 100;
            _coins = 20;
        }

        public void setDeck()
        {
            string input;
            ICard foundCard;
            deck.clearDeck();
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
                    if (deck.addCard(foundCard) == false)
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

        public int elo { get => _elo; set => _elo = value; }
        public int playedGames { get => _playedGames; set => _playedGames = value; }
        public int wonGames { get => _wonGames; set => _wonGames = value; }
        public string username { get => _username; set => _username = value; }
        public string password { get => _password; set => _password = value; }
    }

    
}
