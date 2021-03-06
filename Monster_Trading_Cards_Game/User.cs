using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    public class User
    {
        ConsoleNavigation navigation = new ConsoleNavigation();
        public CardStack stack;
        public CardDeck deck = new CardDeck();
        public CardDeck battleDeck = new CardDeck();
        private string _username, _password, _token;
        private int _elo, _playedGames, _wonGames, _coins, _id;
        public User(int id, string username, string password, int coins, int elo, int playedGames, int wonGames, CardStack stack, CardDeck deck)
        {
            _id = id;
            _username = username;
            _password = password;
            _elo = elo;
            _coins = coins;
            _playedGames = playedGames;
            _wonGames = wonGames;
            this.stack = stack;
            this.deck = deck;
        }

        public void setDeck()
        {
            int input = -1;
            ICard foundCard;
            deck.clearDeck(id);
            Console.Clear();
            for (int i = 1; i<=4; i++){
                input = -1;
                while(input == -1)
                {
                    Console.Clear();
                    Console.WriteLine("Choose Card " + i + " for your Deck:");
                    stack.printList();
                    input = navigation.moveCursor(stack.cards.Count(), Console.CursorTop);
                }

                foundCard = stack.searchCard(stack.cards[input-1]);
                if (foundCard == null)
                {
                    Console.Clear();
                    Console.WriteLine("Could'nt find card. Try again!");
                    Console.Write("Press any Key to continue...");
                    Console.ReadKey();
                    i--;
                }
                else
                {
                    Console.Clear();
                    if (deck.addCard(foundCard, id) == false)
                    {
                        Console.WriteLine("Card is already in deck!");
                        Console.Write("Press any Key to continue...");
                        Console.ReadKey();
                        i--;
                    }
                }
            }
            Console.WriteLine("You created this deck:");
            deck.printDeck();
        }

        public void updateBattleDeck()
        {
            battleDeck.clearBattleDeck();
            foreach(ICard card in deck.cards)
            {
                battleDeck.cards.Add(card);
            }
        }

        public void printProfile()
        {
            Console.WriteLine("Name: " + username);
            Console.WriteLine("Password: " + password);
            Console.WriteLine("Coins: " + coins);
            Console.WriteLine("Elo: " + elo);
            Console.WriteLine("Games Played: " + playedGames);
            Console.WriteLine("Games Won: " + wonGames);
        }

        public int id { get => _id; set => _id = value; }
        public int elo { get => _elo; set => _elo = value; }
        public int coins { get => _coins; set => _coins = value; }
        public int playedGames { get => _playedGames; set => _playedGames = value; }
        public int wonGames { get => _wonGames; set => _wonGames = value; }
        public string username { get => _username; set => _username = value; }
        public string password { get => _password; set => _password = value; }
        public string token { get => _token; set => _token = value; }
    } 
}
