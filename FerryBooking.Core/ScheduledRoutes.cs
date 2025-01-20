using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace FerryBooking.Core
{
    public class ScheduledRoutes
    {
        public ScheduledRoutes(Route route)
        {
            Route = route;
            Passengers = new List<Passenger>();
        }

        public Route Route { get; private set; }
        public Vessel Vessel { get; private set; }
        public List<Passenger> Passengers { get; private set; }

        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        public void SetVesselForRoute(Vessel aircraft)
        {
            Vessel = aircraft;
        }
        
        public string GetSummary()
        {
            double costOfJourney = 0;
            double profitFromJourney = 0;
            int totalLoyaltyPointsAccrued = 0;
            int totalLoyaltyPointsRedeemed = 0;
            int totalExpectedBaggage = 0;
            int seatsTaken = 0;

            //using StringBuilder instead for faster performance.
            string result;

            foreach (var passenger in Passengers)
            {
                switch (passenger.Type)
                {
                    case(PassengerType.General):
                        {
                            profitFromJourney += Route.BasePrice;
                            totalExpectedBaggage++;
                            break;
                        }
                    case(PassengerType.LoyaltyMember):
                        {
                            if (passenger.IsUsingLoyaltyPoints)
                            {
                                int loyaltyPointsRedeemed = Convert.ToInt32(Math.Ceiling(Route.BasePrice));
                                passenger.LoyaltyPoints -= loyaltyPointsRedeemed;
                                totalLoyaltyPointsRedeemed += loyaltyPointsRedeemed;
                            }
                            else
                            {
                                totalLoyaltyPointsAccrued += Route.LoyaltyPointsGained;
                                profitFromJourney += Route.BasePrice;                           
                            }
                            totalExpectedBaggage += 2;
                            break;
                        }
                    case(PassengerType.CarrierEmployee):
                        {
                            totalExpectedBaggage += 1;
                            break;
                        }
                }
                costOfJourney += Route.BaseCost;
                seatsTaken++;
            }


            //separating the summary building into a different function.
            //This improves readability, and if I want to add more to summary I can do so in the function.
            result = BuildSummary(costOfJourney, profitFromJourney, totalLoyaltyPointsAccrued, totalLoyaltyPointsRedeemed, totalExpectedBaggage, seatsTaken);
            return result;
        }


		
		public string BuildSummary(double costOfJourney, double profitFromJourney, int totalLoyaltyPointsAccrued, int totalLoyaltyPointsReedeemed, int totalExpectedBaggage, int seatsTaken)
        {
			//simple \n and \t should do for new lines and indentations. Reduces unnecessary lines of code.
			StringBuilder summaryBuilder = new StringBuilder();

            summaryBuilder.AppendLine("Journey summary for " + Route.Title);
			summaryBuilder.AppendLine("Total passengers: " + seatsTaken);
			summaryBuilder.AppendLine("\tGeneral sales: " + Passengers.Count(p => p.Type == PassengerType.General));
			summaryBuilder.AppendLine("\tLoyalty member sales: " + Passengers.Count(p => p.Type == PassengerType.LoyaltyMember));
			summaryBuilder.AppendLine("\tCarrier employee comps: " + Passengers.Count(p => p.Type == PassengerType.CarrierEmployee) + "\n");

			summaryBuilder.AppendLine("Total expected baggage: " + totalExpectedBaggage + "\n");

			summaryBuilder.AppendLine("Total revenue from route: " + profitFromJourney);
			summaryBuilder.AppendLine("Total costs from route: " + costOfJourney);

			double profitSurplus = profitFromJourney - costOfJourney;

			summaryBuilder.AppendLine((profitSurplus > 0 ? "Route generating profit of: " : "Route losing money of: ") + profitSurplus + "\n");

			summaryBuilder.AppendLine("Total loyalty points given away: " + totalLoyaltyPointsAccrued);
			summaryBuilder.AppendLine("Total loyalty points redeemed: " + totalLoyaltyPointsReedeemed + "\n");

			if (profitSurplus > 0 &&
				seatsTaken < Vessel.NumberOfSeats &&
				seatsTaken / (double)Vessel.NumberOfSeats > Route.MinimumTakeOffPercentage)
				summaryBuilder.AppendLine("THIS ROUTE MAY PROCEED");
			else
				summaryBuilder.AppendLine("ROUTE MAY NOT PROCEED");


			return summaryBuilder.ToString();

		}
    }
}
