using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.IO;

namespace modem_console
    
{
   
    class Program
    {
        static SerialPort com = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        static string number = "3964";
        static void Main(string[] args)
        {
            com.Handshake = Handshake.None;
            com.WriteTimeout = 500;
            com.RtsEnable = true;
            com.DtrEnable = true;
            com.DataReceived += new SerialDataReceivedEventHandler(dataReceiver);
            com.Open();
            string input;
            Console.Write("Podaj co chcesz zrobic (1 - zadzwonic, 2 - odbebrac): ");
            input = Console.ReadLine();
            
            if (input.Equals("1"))
            {
                Console.Write("Wybierz numer: ");
                number = Console.ReadLine();
                call(number);
            }

            else if (input.Equals("2"))
            {
                Console.WriteLine("poczekaj na dzwonek, a następnie wcisnij przycisk");
                Console.ReadKey();
                odbierz(number);
            }

            while (true)
            {
                string msg = Console.ReadLine();
                
                if (msg.Equals("end_call")) break;
                Console.WriteLine("--->" + msg);
                com.Write(msg + "\r\n");
            }

            rozlacz();
        }

        static void call(string number)
        {
            if (com.IsOpen)
            {
                com.Write("ATD" + number + "\r\n");
                Console.WriteLine("Nawiązywanie połączenia z numerem: " + number);
            }
            else Console.WriteLine("port zajęty");
        }

        static void odbierz(string number)
        {
            if (com.IsOpen)
            {
                com.Write("ATA\r\n");
                Console.WriteLine("Odbieranie połączenia od numeru: " + number);
            }
            else Console.WriteLine("port zajęty");
        }

        static void dataReceiver(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(1000);
            string data = com.ReadExisting();
            string s2 = "CONNECT";
            bool b = data.Contains(s2);
            if (b)
            {
                int index = data.IndexOf(s2);
                if (index >= 0)
                {
                    Thread.Sleep(2000);
                    data = "polaczenie udane";
                }
            }

            Console.WriteLine("<---" + data);
        }

        static void rozlacz()
        {
            if (com.IsOpen)
            {
                com.Write("+++");
                Thread.Sleep(2000);
                com.Write("ATH\r\n");
                Thread.Sleep(500);
                com.Write("ATH\r\n");
                if (com.IsOpen)
                {
                   com.Close();
                }
              
                Console.WriteLine("Połączenie zakończone");
            }
        }
    }
}
