using System;
using System.IO;

namespace GestioneOrganizzazioni
{
    struct Organizzazione
    {
        public int Numero;
        public string Codice;
    }

    internal class Program
    {
        // salvo i dati su file (numero,codice)
        static void SalvaSuFile(Organizzazione[] elenco, int quanti, string nomeFile, ref bool ok)
        {
            try
            {
                using (StreamWriter scrittore = new StreamWriter(nomeFile, false))
                {
                    for (int i = 0; i < quanti; i++)
                    {
                        scrittore.WriteLine(elenco[i].Numero + "," + elenco[i].Codice);
                    }
                }
                ok = true;
            }
            catch (Exception)
            {
                ok = false;
            }
        }

        // carico i dati da un file csv
        static void CaricaDaFile(Organizzazione[] elenco, ref int quanti, string nomeFile, ref bool ok)
        {
            if (!File.Exists(nomeFile))
            {
                ok = false;
                return;
            }

            using (StreamReader lettore = new StreamReader(nomeFile))
            {
                string riga;
                while ((riga = lettore.ReadLine()) != null)
                {
                    // salto le righe vuote
                    if (string.IsNullOrWhiteSpace(riga))
                        continue;

                    // se l'array e' pieno mi fermo
                    if (quanti >= elenco.Length)
                        break;

                    string[] pezzi = riga.Split(',');
                    if (pezzi.Length == 2)
                    {
                        int numero;
                        if (int.TryParse(pezzi[0], out numero))
                        {
                            AggiungiElemento(elenco, ref quanti, numero, pezzi[1]);
                        }
                    }
                }
            }
            ok = true;
        }

        // somma i numeri maggiori o uguali al limite
        static int SommaMaggiori(Organizzazione[] elenco, int quanti, int limite)
        {
            int totale = 0;
            for (int i = 0; i < quanti; i++)
            {
                if (elenco[i].Numero >= limite)
                {
                    totale = totale + elenco[i].Numero;
                }
            }
            return totale;
        }

        // elimina un elemento e sposta indietro quelli dopo
        static void Elimina(Organizzazione[] elenco, ref int quanti, int numeroDaTrovare)
        {
            int posizione = Trova(elenco, quanti, numeroDaTrovare);

            if (posizione != -1)
            {
                for (int i = posizione; i < quanti - 1; i++)
                {
                    elenco[i] = elenco[i + 1];
                }
                quanti--;
            }
            else
            {
                Console.WriteLine("Elemento non trovato.");
            }
        }

        // cambia i dati di un elemento gia' presente
        static void Cambia(Organizzazione[] elenco, int quanti, int numeroDaTrovare, int nuovoNumero, string nuovoCodice)
        {
            int posizione = Trova(elenco, quanti, numeroDaTrovare);

            if (posizione != -1)
            {
                elenco[posizione].Numero = nuovoNumero;
                elenco[posizione].Codice = nuovoCodice;
            }
            else
            {
                Console.WriteLine("Elemento non trovato.");
            }
        }

        // ritorna la posizione dell'elemento oppure -1
        static int Trova(Organizzazione[] elenco, int quanti, int numeroDaTrovare)
        {
            int posizione = -1;
            for (int i = 0; i < quanti; i++)
            {
                if (elenco[i].Numero == numeroDaTrovare)
                {
                    posizione = i;
                    break;
                }
            }
            return posizione;
        }

        // costruisce la stringa con tutti gli elementi
        static string Stampa(Organizzazione[] elenco, int quanti)
        {
            string testo = "Numero\tCodice\n";
            for (int i = 0; i < quanti; i++)
            {
                testo = testo + elenco[i].Numero + "\t" + elenco[i].Codice + "\n";
            }
            return testo;
        }

        // aggiunge un elemento in fondo all'array
        static void AggiungiElemento(Organizzazione[] elenco, ref int quanti, int numero, string codice)
        {
            if (quanti >= elenco.Length)
            {
                Console.WriteLine("Elenco pieno, impossibile aggiungere.");
                return;
            }

            elenco[quanti].Numero = numero;
            elenco[quanti].Codice = codice;
            quanti++;
        }

        static void Main(string[] args)
        {
            Organizzazione[] elenco = new Organizzazione[5];
            int quanti = 0;
            int scelta = 0;
            bool continua = true;

            while (continua)
            {
                Console.WriteLine("===== MENU =====");
                Console.WriteLine("1) Aggiungi un elemento");
                Console.WriteLine("2) Mostra tutti gli elementi");
                Console.WriteLine("3) Cambia un elemento");
                Console.WriteLine("4) Elimina un elemento");
                Console.WriteLine("5) Leggi da file");
                Console.WriteLine("6) Scrivi su file");
                Console.WriteLine("7) Calcola somma");
                Console.WriteLine("8) Esci");
                Console.Write("La tua scelta: ");
                scelta = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                switch (scelta)
                {
                    case 1:
                        Console.Write("Scrivi il numero: ");
                        int numero = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Scrivi il codice organizzazione: ");
                        string codice = Console.ReadLine();

                        AggiungiElemento(elenco, ref quanti, numero, codice);
                        Console.WriteLine("Elemento aggiunto (se c'era spazio).");
                        Console.WriteLine();
                        break;

                    case 2:
                        Console.WriteLine(Stampa(elenco, quanti));
                        break;

                    case 3:
                        Console.Write("Numero dell'elemento da cambiare: ");
                        int vecchioNumero = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Nuovo numero: ");
                        int nuovoNumero = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Nuovo codice organizzazione: ");
                        string nuovoCodice = Console.ReadLine();

                        Cambia(elenco, quanti, vecchioNumero, nuovoNumero, nuovoCodice);
                        Console.WriteLine("Operazione terminata.");
                        Console.WriteLine();
                        break;

                    case 4:
                        Console.Write("Numero dell'elemento da eliminare: ");
                        int numeroElimina = Convert.ToInt32(Console.ReadLine());
                        Elimina(elenco, ref quanti, numeroElimina);
                        Console.WriteLine();
                        break;

                    case 5:
                        Console.Write("Percorso del file da leggere: ");
                        string fileLettura = Console.ReadLine();
                        bool lettoOk = false;
                        CaricaDaFile(elenco, ref quanti, fileLettura, ref lettoOk);
                        if (lettoOk)
                            Console.WriteLine("File letto correttamente.");
                        else
                            Console.WriteLine("Impossibile leggere il file.");
                        Console.WriteLine();
                        break;

                    case 6:
                        Console.Write("Percorso del file dove salvare: ");
                        string fileScrittura = Console.ReadLine();
                        bool salvatoOk = false;
                        SalvaSuFile(elenco, quanti, fileScrittura, ref salvatoOk);
                        if (salvatoOk)
                            Console.WriteLine("File salvato correttamente.");
                        else
                            Console.WriteLine("Impossibile salvare il file.");
                        Console.WriteLine();
                        break;

                    case 7:
                        Console.Write("Somma i numeri maggiori o uguali a: ");
                        int limite = Convert.ToInt32(Console.ReadLine());
                        int totale = SommaMaggiori(elenco, quanti, limite);
                        Console.WriteLine("Il totale e': " + totale);
                        Console.WriteLine();
                        break;

                    case 8:
                        Console.WriteLine("Programma terminato.");
                        continua = false;
                        break;

                    default:
                        Console.WriteLine("Scelta sbagliata, riprova.");
                        break;
                }
            }
        }
    }
}
