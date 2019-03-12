using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posttopinteresst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What would you like to write to our pinterest board?");
            string temp = Console.ReadLine();

            if (temp.Length < 100 && temp.Length > 0)
            {
                PostPinterest.Message(temp);
            }

            Console.WriteLine("You posted a message to our test board.");
        }
    }
}
