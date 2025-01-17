﻿using System;
using System.IO;
using NLog.Web;

namespace StringInterpolation
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";

            // create instance of Logger
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();

            logger.Info("Program started");

            // ask for input
            Console.WriteLine("Enter 1 to create data file.");
            Console.WriteLine("Enter 2 to parse data.");
            Console.WriteLine("Enter anything else to quit.");
            // input response
            string resp = Console.ReadLine();

            if (resp == "1")
            {
                // TODO: create data file
                // create data file
                bool isValidNumberOfWeeks = false;
                int weeks = 0;
                while(!isValidNumberOfWeeks){
                 // ask a question
                Console.WriteLine("How many weeks of data is needed?");
                // input the response (convert to int)
                if (int.TryParse(Console.ReadLine(), out weeks)){
                    isValidNumberOfWeeks = true;
                }else{
                    logger.Error("Invalid input");
                }
                }
                // determine start and end date
                DateTime today = DateTime.Now;
                // we want full weeks sunday - saturday
                DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
                // subtract # of weeks from endDate to get startDate
                DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
                
                // random number generator
                Random rnd = new Random();

                                // create file
                StreamWriter sw = new StreamWriter("data.txt");
                // loop for the desired # of weeks
                while (dataDate < dataEndDate)
                {
                    // 7 days in a week
                    int[] hours = new int[7];
                    for (int i = 0; i < hours.Length; i++)
                    {
                        // generate random number of hours slept between 4-12 (inclusive)
                        hours[i] = rnd.Next(4, 13);
                    }
                    // M/d/yyyy,#|#|#|#|#|#|#
                    //Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
                    sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
                    // add 1 week to date
                    dataDate = dataDate.AddDays(7);
                }
                sw.Close();
            }
            else if (resp == "2")
            {
                // TODO: parse data file
                StreamReader sr = new StreamReader("data.txt");
                while (!sr.EndOfStream) {
                    //Input
                    string data = sr.ReadLine();

                    //Date formating
                    string[] split = data.Split(',');

                    //Delimitter
                    string[] dataSplit = split[1].Split('|');

                    //Parsing data
                    var parseDate = DateTime.Parse(split[0]);
                    int[] intData = new int[dataSplit.Length];
                    for (int i = 0; i < dataSplit.Length; i++) {
                        intData[i] = Int32.Parse(dataSplit[i]);
                    }
                    //Total
                    int total = intData[0] + intData[1] + intData[2] + intData[3] + intData[4] + intData[5] + intData[6];
                    double totalDouble = Convert.ToDouble(total);
                    //Average
                    double average = totalDouble / 7;

                    //Print weekly output
                    Console.WriteLine("Week of {0:MMM}, {0:dd}, {0:yyyy}", parseDate);
                    Console.WriteLine("Su Mo Tu We Th Fr Sa Tot Avg");
                    Console.WriteLine("-- -- -- -- -- -- -- --- ---");
                    //Shifted output to be Sunday - Saturday instead of Monday - Sunday
                    Console.WriteLine("{3,2} {0,2} {1,2} {2,2} {4,2} {5,2} {6,2} {7,3} {8,3:n1}", dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[4], dataSplit[5], dataSplit[6], total, average);

                }
                sr.Close();
            }
            logger.Info("Program ended");
        }
    }
}
