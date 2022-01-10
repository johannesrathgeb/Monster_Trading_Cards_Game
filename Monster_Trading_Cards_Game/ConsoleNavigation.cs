using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class ConsoleNavigation
    {
        int lineIndex = 0;
        int lineIndexTop = 1;
        int tempIndex;
        char specialChar = (char)215;
        ConsoleKeyInfo pressedKey;

        public int moveCursor(int choices, int currentTop)
        {
            int topRows = currentTop - choices;
            Console.SetCursorPosition(1, lineIndex + topRows);
            Console.Write(specialChar);
            pressedKey = Console.ReadKey();
            if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (lineIndex + 1 < choices)
                {
                    lineIndex++;
                }
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (lineIndex - 1 >= 0)
                {
                    lineIndex--;
                }  
            }
            else if(pressedKey.Key == ConsoleKey.Enter)
            {
                tempIndex = lineIndex;
                lineIndex = 0;
                return tempIndex + 1;
            }
            return -1;
        }
    }
}
