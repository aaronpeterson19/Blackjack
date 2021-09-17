using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Casino
    {
        

        

        /// <param name="hand">The hand to check</param>
        /// <returns>Returns true if the hand is blackjack</returns>
        public static bool IsHandBlackjack(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                if (hand[0].Face == Face.Ace && hand[1].Value == 10) return true;
                else if (hand[1].Face == Face.Ace && hand[0].Value == 10) return true;
            }
            return false;
        }

        /// <summary>
        /// Reset Console Colors to DarkGray on Black
        /// </summary>
        public static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    public class Player
    {
        

        public List<Card> Hand { get; set; }

        

        /// <returns>
        /// Value of all cards in Hand
        /// </returns>
        public int GetHandValue()
        {
            int value = 0;
            foreach (Card card in Hand)
            {
                value += card.Value;
            }
            return value;
        }

        /// <summary>
        /// Write player's hand to console.
        /// </summary>
        public void WriteHand()
        {
           

            Console.WriteLine();
            Console.WriteLine("Your Hand (" + GetHandValue() + "):");
            foreach (Card card in Hand)
            {
                card.WriteDescription();
            }
            Console.WriteLine();
        }
    }

    public class Dealer
    {
        public static List<Card> HiddenCards { get; set; } = new List<Card>();
        public static List<Card> RevealedCards { get; set; } = new List<Card>();

        /// <summary>
        /// Take the top card from HiddenCards, remove it, and add it to RevealedCards.
        /// </summary> 
        public static void RevealCard()
        {
            RevealedCards.Add(HiddenCards[0]);
            HiddenCards.RemoveAt(0);
        }

        /// <returns>
        /// Value of all cards in RevealedCards
        /// </returns>
        public static int GetHandValue()
        {
            int value = 0;
            foreach (Card card in RevealedCards)
            {
                value += card.Value;
            }
            return value;
        }

        /// <summary>
        /// Write Dealer's RevealedCards to Console.
        /// </summary>
        public static void WriteHand()
        {
            Console.WriteLine("Dealer's Hand (" + GetHandValue() + "):");
            foreach (Card card in RevealedCards)
            {
                card.WriteDescription();
            }
            for (int i = 0; i < HiddenCards.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("♠♥ Peterson Casino ♣♦");
                Casino.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
