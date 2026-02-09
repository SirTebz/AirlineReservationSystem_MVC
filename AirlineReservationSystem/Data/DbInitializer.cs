using AirlineReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed Admin User
            if (await userManager.FindByEmailAsync("admin@airline.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@airline.com",
                    Email = "admin@airline.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Regular User
            if (await userManager.FindByEmailAsync("user@airline.com") == null)
            {
                var regularUser = new ApplicationUser
                {
                    UserName = "user@airline.com",
                    Email = "user@airline.com",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(regularUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }

            // Seed Flights
            if (!context.Flights.Any())
            {
                var flights = new List<Flight>
                {
                    new Flight
                    {
                        FlightNumber = "SA101",
                        Origin = "Johannesburg",
                        Destination = "Cape Town",
                        DepartureTime = DateTime.Now.AddDays(5).Date.AddHours(8),
                        ArrivalTime = DateTime.Now.AddDays(5).Date.AddHours(10).AddMinutes(30),
                        Price = 2499.00m,
                        TotalSeats = 180,
                        AircraftType = "SAA 737",
                        Description = "Direct flight from JNB to CPT",
                        IsActive = true
                    },
                    new Flight
                    {
                        FlightNumber = "SA202",
                        Origin = "Durban",
                        Destination = "Johannesburg",
                        DepartureTime = DateTime.Now.AddDays(7).Date.AddHours(10),
                        ArrivalTime = DateTime.Now.AddDays(7).Date.AddHours(11).AddMinutes(30),
                        Price = 1899.00m,
                        TotalSeats = 150,
                        AircraftType = "FlySafair A320",
                        Description = "Non-stop from DUR to JNB",
                        IsActive = true
                    },
                    new Flight
                    {
                        FlightNumber = "SA303",
                        Origin = "Cape Town",
                        Destination = "Durban",
                        DepartureTime = DateTime.Now.AddDays(10).Date.AddHours(6),
                        ArrivalTime = DateTime.Now.AddDays(10).Date.AddHours(8).AddMinutes(30),
                        Price = 2799.00m,
                        TotalSeats = 200,
                        AircraftType = "SAA 757",
                        Description = "Morning flight from CPT to DUR",
                        IsActive = true
                    },
                    new Flight
                    {
                        FlightNumber = "SA404",
                        Origin = "Port Elizabeth",
                        Destination = "Johannesburg",
                        DepartureTime = DateTime.Now.AddDays(3).Date.AddHours(15),
                        ArrivalTime = DateTime.Now.AddDays(3).Date.AddHours(16).AddMinutes(45),
                        Price = 1599.00m,
                        TotalSeats = 140,
                        AircraftType = "SAA 737",
                        Description = "Afternoon service from PLZ to JNB",
                        IsActive = true
                    },
                    new Flight
                    {
                        FlightNumber = "SA505",
                        Origin = "Johannesburg",
                        Destination = "George",
                        DepartureTime = DateTime.Now.AddDays(14).Date.AddHours(9),
                        ArrivalTime = DateTime.Now.AddDays(14).Date.AddHours(11).AddMinutes(15),
                        Price = 2199.00m,
                        TotalSeats = 160,
                        AircraftType = "FlySafair A321",
                        Description = "Scenic flight from JNB to GRJ",
                        IsActive = true
                    }
                };

                context.Flights.AddRange(flights);
                await context.SaveChangesAsync();

                // Seed Seats for each flight
                foreach (var flight in flights)
                {
                    var seats = new List<Seat>();

                    // First Class (rows 1-3, seats A-F)
                    for (int row = 1; row <= 3; row++)
                    {
                        foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                        {
                            seats.Add(new Seat
                            {
                                FlightId = flight.FlightId,
                                SeatNumber = $"{row}{col}",
                                SeatClass = "First",
                                IsAvailable = true,
                                IsWindowSeat = col == 'A' || col == 'F',
                                IsAisleSeat = col == 'C' || col == 'D'
                            });
                        }
                    }

                    // Business Class (rows 4-10, seats A-F)
                    for (int row = 4; row <= 10; row++)
                    {
                        foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                        {
                            seats.Add(new Seat
                            {
                                FlightId = flight.FlightId,
                                SeatNumber = $"{row}{col}",
                                SeatClass = "Business",
                                IsAvailable = true,
                                IsWindowSeat = col == 'A' || col == 'F',
                                IsAisleSeat = col == 'C' || col == 'D'
                            });
                        }
                    }

                    // Economy Class (remaining rows)
                    int economyRows = (flight.TotalSeats - seats.Count) / 6;
                    for (int row = 11; row <= 10 + economyRows; row++)
                    {
                        foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                        {
                            seats.Add(new Seat
                            {
                                FlightId = flight.FlightId,
                                SeatNumber = $"{row}{col}",
                                SeatClass = "Economy",
                                IsAvailable = true,
                                IsWindowSeat = col == 'A' || col == 'F',
                                IsAisleSeat = col == 'C' || col == 'D'
                            });
                        }
                    }

                    context.Seats.AddRange(seats.Take(flight.TotalSeats));
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
