using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace es_1
{
    internal class Program
    {
        struct SupportTickets
            {
            public int Index;
            public string TicketID;
            public string CustomerPlan;
            public string TicketSubject;
            public string TicketText;
            public string PriorityHint;
        }
        


        static void Inserisci()
        {

        }

        static void Visualizza()
        {

        }

        static void Modifica()
        {

        }   

        static void Cancella()
        {

        }


        static void Main(string[] args)
        {
          

            int opzione;
            bool a=true;
            opzione = Convert.ToInt32(Console.ReadLine());

            do
            {
                switch(opzione)
                {
                    case 1:
                        Inserisci();
                        break;
                    case 2:
                        Visualizza();
                        break;
                    case 3:
                        Modifica();
                        break;
                    case 4:
                        Cancella();
                        break;
                    case 0:
                        a = false;
                        break;
                    default:
                        Console.WriteLine("Opzione non valida");
                        break;
                }

            } while (a);
        }
    }
}
