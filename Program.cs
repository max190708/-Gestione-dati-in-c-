using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvManager
{
    // Nuova struttura: solo 3 campi
    public struct RecordData
    {
        public int Index;
        public string TicketID;
        public string CostumerPlan;
    }

    class Program
    {
        const string FilePath = "data.csv";

        static void Main(string[] args)
        {
            List<RecordData> records = ReadCsv(FilePath);
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. Inserimento di un nuovo record");
                Console.WriteLine("2. Visualizzazione dei record");
                Console.WriteLine("3. Modifica di un record");
                Console.WriteLine("4. Cancellazione di un record");
                Console.WriteLine("5. Carica dati da CSV");
                Console.WriteLine("6. Salva dati su CSV");
                Console.WriteLine("0. Esci");
                Console.Write("Scelta: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    {
                        // Raccolta dati in Main (tutta la logica della console qui)
                        RecordData rec = new RecordData();
                        Console.Write("Index: ");
                        rec.Index = int.TryParse(Console.ReadLine(), out int idx) ? idx : 0;
                        Console.Write("TicketID: ");
                        rec.TicketID = Console.ReadLine();
                        Console.Write("CostumerPlan: ");
                        rec.CostumerPlan = Console.ReadLine();

                        InserisciRecord(records, rec);
                        WriteCsv(FilePath, records);
                        break;
                    }
                    case "2":
                    {
                        var lines = VisualizzaRecord(records);
                        if (lines.Count == 0)
                        {
                            Console.WriteLine("Nessun record presente.");
                        }
                        else
                        {
                            foreach (var l in lines) Console.WriteLine(l);
                        }
                        break;
                    }
                    case "3":
                    {
                        Console.Write("Inserisci l'Index del record da modificare: ");
                        if (int.TryParse(Console.ReadLine(), out int targetIndex))
                        {
                            Console.Write("Nuovo TicketID (lascia vuoto per non modificare): ");
                            string newTicket = Console.ReadLine();
                            Console.Write("Nuovo CostumerPlan (lascia vuoto per non modificare): ");
                            string newPlan = Console.ReadLine();

                            bool ok = ModificaRecord(records, targetIndex, newTicket, newPlan);
                            Console.WriteLine(ok ? "Modifica completata." : "Record non trovato.");
                            if (ok) WriteCsv(FilePath, records);
                        }
                        else Console.WriteLine("Index non valido.");
                        break;
                    }
                    case "4":
                    {
                        Console.Write("Inserisci l'Index del record da cancellare: ");
                        if (int.TryParse(Console.ReadLine(), out int delIndex))
                        {
                            int removed = CancellaRecord(records, delIndex);
                            Console.WriteLine(removed > 0 ? "Record cancellato." : "Nessun record trovato con questo Index.");
                            if (removed > 0) WriteCsv(FilePath, records);
                        }
                        else Console.WriteLine("Index non valido.");
                        break;
                    }
                    case "5":
                    {
                        // Carica dati da file CSV (sostituisce la lista corrente)
                        records = ReadCsv(FilePath);
                        Console.WriteLine($"Caricati {records.Count} record da {FilePath}.");
                        break;
                    }
                    case "6":
                    {
                        // Salva dati su CSV
                        WriteCsv(FilePath, records);
                        Console.WriteLine($"Dati salvati su {FilePath}.");
                        break;
                    }
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Scelta non valida.");
                        break;
                }
            }
        }

        // ---------------------------------------------------------
        // FUNZIONI DI LETTURA E SCRITTURA 
        // ---------------------------------------------------------

        // 
        static List<RecordData> ReadCsv(string path)
        {
            List<RecordData> list = new List<RecordData>();
            if (!File.Exists(path)) return list;
            // 
            var lines = File.ReadAllLines(path);
            int start = 0;
            if (lines.Length > 0 && lines[0].IndexOf("Index", StringComparison.OrdinalIgnoreCase) >= 0) start = 1;
            for (int i = start; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 3)
                {
                    RecordData rec = new RecordData();
                    rec.Index = int.TryParse(parts[0], out int idx) ? idx : 0;
                    rec.TicketID = parts[1];
                    rec.CostumerPlan = parts[2];
                    list.Add(rec);
                }
            }
            return list;
        }

        // 
        static void WriteCsv(string path, List<RecordData> list)
        {
            var lines = new List<string>(list.Count + 1);
            lines.Add("Index,TicketID,CostumerPlan");
            foreach (var rec in list)
            {
                lines.Add($"{rec.Index},{rec.TicketID},{rec.CostumerPlan}");
            }
            File.WriteAllLines(path, lines);
        }

        // ---------------------------------------------------------
        // FUNZIONI DI INTERAZIONE 
        // ---------------------------------------------------------

        // Aggiunge una registrazione alla lista. Non usa la console.
        static void InserisciRecord(List<RecordData> records, RecordData rec)
        {
            records.Add(rec);
        }

        // Restituisce elenco di stringhe per la visualizzazione 
        static List<string> VisualizzaRecord(List<RecordData> records)
        {
            List<string> outLines = new List<string>();
            foreach (var r in records)
            {
                outLines.Add($"Idx: {r.Index} | Ticket: {r.TicketID} | Plan: {r.CostumerPlan}");
            }
            return outLines;
        }

        // Modifica la registrazione per Index. Restituisce true se trovata e modificata.
        static bool ModificaRecord(List<RecordData> records, int targetIndex, string newTicketID, string newCostumerPlan)
        {
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Index == targetIndex)
                {
                    RecordData rec = records[i];
                    if (!string.IsNullOrEmpty(newTicketID)) rec.TicketID = newTicketID;
                    if (!string.IsNullOrEmpty(newCostumerPlan)) rec.CostumerPlan = newCostumerPlan;
                    records[i] = rec;
                    return true;
                }
            }
            return false;
        }

        // Elimina le registrazioni con l'Index specificato. Restituisce il numero di elementi rimossi.
        static int CancellaRecord(List<RecordData> records, int targetIndex)
        {
            return records.RemoveAll(r => r.Index == targetIndex);
        }

    }
}