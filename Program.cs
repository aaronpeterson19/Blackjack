using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Blackjack;

namespace Blackjack
{
    public class Program
    {
        private static Deck deck = new Deck();
        private static Player player = new Player();


        private enum RoundResult
        {
            PUSH,
            PLAYER_WIN,
            PLAYER_BUST,
            PLAYER_BLACKJACK,
            DEALER_WIN,
            Quit,
            INVALID_BET
        }

        /// <summary>
        /// Initialize Deck, deal the player and dealer hands, and display them.
        /// </summary>
        static void InitializeHands()
        {
            deck.Initialize();

            player.Hand = deck.DealHand();
            Dealer.HiddenCards = deck.DealHand();
            Dealer.RevealedCards = new List<Card>();

            // If hand contains two aces, make one Hard.
            if (player.Hand[0].Face == Face.Ace && player.Hand[1].Face == Face.Ace)
            {
                player.Hand[1].Value = 1;
            }

            if (Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
            {
                Dealer.HiddenCards[1].Value = 1;
            }

            Dealer.RevealCard();

            player.WriteHand();
            Dealer.WriteHand();
        }

        /// <summary>
        /// Handles everything for the round.
        /// </summary>
        static void StartRound()
        {
            Console.Clear();

            

            InitializeHands();
            TakeActions();

            Dealer.RevealCard();

            Console.Clear();
            player.WriteHand();
            Dealer.WriteHand();

            

            if (player.Hand.Count == 0)
            {
                EndRound(RoundResult.Quit);
                return;
            }
            else if (player.GetHandValue() > 21)
            {
                EndRound(RoundResult.PLAYER_BUST);
                return;
            }

            while (Dealer.GetHandValue() <= 16)
            {
                Thread.Sleep(1000);
                Dealer.RevealedCards.Add(deck.DrawCard());

                Console.Clear();
                player.WriteHand();
                Dealer.WriteHand();
            }


            if (player.GetHandValue() > Dealer.GetHandValue())
            {
                
                if (Casino.IsHandBlackjack(player.Hand))
                {
                    EndRound(RoundResult.PLAYER_BLACKJACK);
                }
                else
                {
                    EndRound(RoundResult.PLAYER_WIN);
                }
            }
            else if (Dealer.GetHandValue() > 21)
            {
                
                EndRound(RoundResult.PLAYER_WIN);
            }
            else if (Dealer.GetHandValue() > player.GetHandValue())
            {
                EndRound(RoundResult.DEALER_WIN);
            }
            else
            {
                EndRound(RoundResult.PUSH);
            }

        }

        /// <summary>
        /// Ask user for action and perform that action until they stand, double, or bust.
        /// </summary>
        static void TakeActions()
        {
            string action;
            do
            {
                Console.Clear();
                player.WriteHand();
                Dealer.WriteHand();

                Console.Write("Hit or Stand? ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                action = Console.ReadLine();
                Casino.ResetColor();

                    switch (action.ToUpper())
                {
                    case "HIT":
                        player.Hand.Add(deck.DrawCard());
                        break;
                    case "STAND":
                        break;
                    case "QUIT":
                        Console.Clear();
                        Console.WriteLine("Thank you for playing!");
                        break;
                    
                    default:
                        Console.WriteLine("Valid Moves:");
                        Console.WriteLine("Hit, Stand, Quit");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        break;
                }

                if (player.GetHandValue() > 21)
                {
                    foreach (Card card in player.Hand)
                    {
                        if (card.Value == 11) // Only a soft ace can have a value of 11
                        {
                            card.Value = 1;
                            break;
                        }
                    }
                }
            } while (!action.ToUpper().Equals("STAND") && !action.ToUpper().Equals("DOUBLE")
                && !action.ToUpper().Equals("SURRENDER") && player.GetHandValue() <= 21);
        }

       

        /// <summary>
        /// Perform action based on result of round and start next round.
        /// </summary>
        /// <param name="result">The result of the round</param>
        static void EndRound(RoundResult result)
        {
            switch (result)
            {
                case RoundResult.PUSH:
                   
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Player and Dealer Push.");
                    break;
                case RoundResult.PLAYER_WIN:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Player Wins");
                    break;
                case RoundResult.PLAYER_BUST:
                   
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Player Busts");
                    break;
                case RoundResult.PLAYER_BLACKJACK:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Player Wins");
                    break;
                case RoundResult.DEALER_WIN:
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dealer Wins.");
                    break;
                case RoundResult.Quit:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You quit the game.");
                    
                    break;
                case RoundResult.INVALID_BET:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Bet.");
                    break;
            }

           

            Casino.ResetColor();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            StartRound();
        }

        static void Main(string[] args)
        {
            // Console cannot render unicode characters without this line
            Console.OutputEncoding = Encoding.UTF8;
            Menu();
        }
        public static void Menu()
        {
            string[] mainMenu = new string[] { "1: Play Blackjack", "2: Shuffle & Show Deck", "3: Exit" };
            int menuChoice = 0;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                ReadChoice("Choice: ", mainMenu, out menuChoice);

                switch (menuChoice)
                {
                    case 1:

                        StartRound();
                        break;

                    case 2:
                        deck.Option2();
                        break;

                    case 3:
                        exit = true;
                        Console.Clear();
                        Console.WriteLine("Thank you for playing!");
                        break;

                }
                Console.ReadKey();
            }
            
        }
        public static void ReadChoice(string prompt, string[] options, out int selection)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{options[i]} ");
            }

            selection = ReadInteger(prompt, 1, options.Length);

        }

        public static int ReadInteger(string prompt, int min, int max)
        {
            int readInt;
            while (true)
            {
                Console.Write(prompt);
                string select = Console.ReadLine();

                if (int.TryParse(select, out readInt) && min <= readInt && readInt <= max)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid option: ");
                }
            }

            return readInt;

        }
    }
}
