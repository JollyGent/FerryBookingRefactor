using System;
using FerryBooking.Core;

namespace FerryBookingProblem
{
    class Program
    {
        private static ScheduledRoutes _scheduledRoutes ;

        static void Main(string[] args)
        {
            SetupVesselData();
            
            string command = "";
            do
            {
                command = Console.ReadLine() ?? "";
                var enteredText = command.ToLower();
                if (enteredText.Contains("print summary"))
                {
                    Console.WriteLine();
                    Console.WriteLine(_scheduledRoutes.GetSummary());
                }

                //There is no failsafe for when user misses out information or inputs too much.
                //Will have to add error handling to make this failsafe.
                //The user may not know the format, so when there's a error the program tells them the correct inputs.
                else if (enteredText.Contains("add general"))
                {
					string[] passengerSegments = enteredText.Split(' ');

					try
                    {
						//throws an exception if there is either too little or too many inputs.
						if (passengerSegments.Length > 4)
						{
							throw new Exception();
						}

						_scheduledRoutes.AddPassenger(new Passenger
                        {
                            Type = PassengerType.General,
                            Name = passengerSegments[2],
                            Age = Convert.ToInt32(passengerSegments[3])
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something has went wrong. Please try again.\n");
                        Console.WriteLine("required format is 'add general' command followed by name and age.");
                    }
                }
                else if (enteredText.Contains("add loyalty"))
                {
                    string[] passengerSegments = enteredText.Split(' ');

					try
                    {
						if (passengerSegments.Length > 6)
						{
							throw new Exception();
						}
						_scheduledRoutes.AddPassenger(new Passenger
                        {
                            Type = PassengerType.LoyaltyMember,
                            Name = passengerSegments[2],
                            Age = Convert.ToInt32(passengerSegments[3]),
                            LoyaltyPoints = Convert.ToInt32(passengerSegments[4]),
                            IsUsingLoyaltyPoints = Convert.ToBoolean(passengerSegments[5]),
                        });
                    } 
                    catch (Exception e)
                    {
						Console.WriteLine("Something has went wrong. Please try again.");
						Console.WriteLine("required format is 'add loyalty' command followed by the name, age, loyalty points and whether it's being used.");
					}
                }
                else if (enteredText.Contains("add vessel"))
                {
                    string[] passengerSegments = enteredText.Split(' ');

                    try
                    {

						if (passengerSegments.Length > 4)
						{
							throw new Exception();
						}

						_scheduledRoutes.AddPassenger(new Passenger
                        {
                            Type = PassengerType.CarrierEmployee,
                            Name = passengerSegments[2],
                            Age = Convert.ToInt32(passengerSegments[3]),
                        });
                    }
                    catch (Exception e)
                    {
						Console.WriteLine("Something has went wrong. Please try again.");
						Console.WriteLine("required format is 'add vessel' command followed by name and age.");
					}
                }
                else if (enteredText.Contains("exit"))
                {
                    Environment.Exit(1);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("UNKNOWN INPUT");
                    Console.ResetColor();
                }
            } while (command != "exit");
        }

        private static void SetupVesselData()
        {
            Route doverToDunkirk = new Route("Dover", "Dunkirk")
            {
                BaseCost = 50, 
                BasePrice = 100, 
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };

            _scheduledRoutes = new ScheduledRoutes(doverToDunkirk);

            _scheduledRoutes.SetVesselForRoute(
                new Vessel { Id = 123, Name = "MV Ulysses", NumberOfSeats = 2000 });
        }
    }
}
