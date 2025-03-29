using System;
using System.IO.Ports;
using System.Linq;

namespace gpsCC
{
    class Program
    {

        static SerialPort port;
        static bool rmc = false;
        static bool gga = false;
        static double szerokosc;
        static string szerokosc_kierunek;
        static double dlugosc;
        static string dlugosc_kierunek;
        static string data;
        static string godzina;

        private static double convWys(string value)
        {
            double degrees = Convert.ToDouble(value.Substring(0, 2));
            double minutes = Convert.ToDouble(value.Substring(2))/60;
            double decinalDegrees = degrees + minutes;
            return decinalDegrees;
        }
        private static double convSzer(string value)
        {
            double degrees = Convert.ToDouble(value.Substring(0, 3));
            double minutes = Convert.ToDouble(value.Substring(3)) /60;
            double decinalDegrees = degrees + minutes;
            return decinalDegrees;
        }

        static void parsujDane(string dane)
        {
            string[] parametry = dane.Split(',');

            if (parametry[0] == "$GPRMC" && rmc == false)
            {
                rmc = true;
                szerokosc = convWys(parametry[3]);
                szerokosc_kierunek = parametry[4];
                dlugosc = convSzer(parametry[5]);
                dlugosc_kierunek = parametry[6];
                godzina = parametry[1];
                data = parametry[9];


                Console.WriteLine("szerokosc = " + szerokosc + szerokosc_kierunek);
                Console.WriteLine("dlugosc = " + dlugosc + dlugosc_kierunek);
                Console.WriteLine("godzina = " + godzina.Substring(0, 2) + ":" + godzina.Substring(2, 2) + ":" + godzina.Substring(4, 2) + " UTC");
                Console.WriteLine("data = " + data.Substring(0, 2) + "/" + data.Substring(2, 2) + "/20" + data.Substring(4, 2));

            }
            else if (parametry[0] == "$GPGGA" && gga == false)
            {
                gga = true;
                string liczba_satelitow = parametry[7];
                Console.WriteLine("Liczba satelitów = " + liczba_satelitow);
            }

        }

        

        static void Main(string[] args)
        {

            string[] ports = SerialPort.GetPortNames();
            int index = 0;

            foreach (string port in ports)
            {
                Console.WriteLine("[" + index + "] " + port);
                index++;
            }

            int choice = Convert.ToInt32(Console.ReadLine());

            port = new SerialPort(ports[choice]);
            port.BaudRate = 4800;
            port.Parity = Parity.None;
            port.DataBits = 8;
            port.StopBits = StopBits.One;

            try
            {
                port.Open();
                while (rmc == false || gga == false)
                {
                    string dane = port.ReadLine();
                    parsujDane(dane);
                }

                string link = "https://www.google.com/maps/search/?api=1&query=";
                if (szerokosc_kierunek == "S")
                {
                    link.Append('-');
                }
                link = link + szerokosc + "%2C";
                if (dlugosc_kierunek == "W")
                {
                    link.Append('-');
                }
                link = link + dlugosc;

                Console.WriteLine("Adres do google maps: " + link);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
