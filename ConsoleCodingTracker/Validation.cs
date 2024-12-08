using ConsoleCodingTracker;
using DataAccessLibrary;
using System;
using System.Globalization;

namespace CodingTrackerConsoleUI
{

    public static class Validation
    {
        public static bool CheckValidTime(string timeInput) 
        {
            bool output = false;
            while (!DateTime.TryParseExact(timeInput, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid time. (Format: HH:mm). Type 0 to return to main manu or try again:");
                timeInput = Console.ReadLine();
                if (DateTime.TryParseExact(timeInput, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
                {
                    output = true;
                }
            }
            output = true;
            return output;
            
        }

        public static bool CheckValidNumber(string numberInput) 
        {
            bool output = false;
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\nInvalid number. Try again.");
                
            }
            output = true;
            return output;
        }

        public static int CheckValidRecord(SqliteCrud sql, int recordId)
        {
            bool recordExists = sql.CheckRecordExists(recordId);
            while (!recordExists)
            {
                recordId = UserInterface.GetNumberInput("\nRecord does not exist. Please type the ID of the record you want to update. Type 0 to return to Main Menu");
                recordExists = sql.CheckRecordExists(recordId);
            }
            return  recordId;
        }
    }
}