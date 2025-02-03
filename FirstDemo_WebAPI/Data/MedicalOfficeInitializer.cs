using FirstDemo_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FirstDemo_WebAPI.Data
{
    public static class MedicalOfficeInitializer
    {
        /// <summary>
        /// Prepares the Database and seeds data as required
        /// </summary>
        /// <param name="serviceProvider">DI Container</param>
        /// <param name="DeleteDatabase">Delete the database and start from scratch</param>
        /// <param name="UseMigrations">Use Migrations or EnsureCreated</param>
        /// <param name="SeedSampleData">Add optional sample data</param>
        public static void Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new MedicalOfficeContext(
                serviceProvider.GetRequiredService<DbContextOptions<MedicalOfficeContext>>()))
            {
                //Refresh the database as per the parameter options
                #region Prepare the Database
                try
                {
                    //Note: .CanConnect() will return false if the database is not there!
                    if (DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted(); //Delete the existing version 
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Create the Database and apply all migrations
                        }
                        else
                        {
                            context.Database.EnsureCreated(); //Create and update the database as per the Model
                        }
                        //Now create any additional database objects such as Triggers or Views
                        //--------------------------------------------------------------------
                        //Patient Table Triggers for Concurrency
                        string sqlCmd = @"
                            CREATE TRIGGER SetPatientTimestampOnUpdate
                            AFTER UPDATE ON Patients
                            BEGIN
                                UPDATE Patients
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetPatientTimestampOnInsert
                            AFTER INSERT ON Patients
                            BEGIN
                                UPDATE Patients
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        //Doctor Table Triggers for Concurrency
                        sqlCmd = @"
                            CREATE TRIGGER SetDoctorTimestampOnUpdate
                            AFTER UPDATE ON Doctors
                            BEGIN
                                UPDATE Doctors
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetDoctorTimestampOnInsert
                            AFTER INSERT ON Doctors
                            BEGIN
                                UPDATE Doctors
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                    }
                    else //The database is already created
                    {
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Apply all migrations
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                //Seed data needed for production and during development
                #region Seed Required Data
                try
                {

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                //Seed meaningless data as sample data during development
                #region Seed Sample Data 
                if (SeedSampleData)
                {
                    //To randomly generate data
                    Random random = new Random();

                    //Seed a few specific Doctors and Patients. We will add more with random values later,
                    //but it can be useful to know we will have some specific records in the sample data.
                    try
                    {
                        // Seed Doctors first since we can't have Patients without Doctors.
                        if (!context.Doctors.Any())
                        {
                            context.Doctors.AddRange(
                            new Doctor
                            {
                                FirstName = "Gregory",
                                MiddleName = "A",
                                LastName = "House"
                            },
                            new Doctor
                            {
                                FirstName = "Doogie",
                                MiddleName = "R",
                                LastName = "Houser"
                            },
                            new Doctor
                            {
                                FirstName = "Charles",
                                LastName = "Xavier"
                            });
                            context.SaveChanges();
                        }


#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        // Seed Patients if there aren't any.
                        if (!context.Patients.Any())
                        {
                            context.Patients.AddRange(
                            new Patient
                            {
                                FirstName = "Fred",
                                MiddleName = "Reginald",
                                LastName = "Flintstone",
                                OHIP = "1231231234",
                                DOB = DateTime.Parse("1955-09-01"),
                                ExpYrVisits = 6,
                                DoctorID = context.Doctors.FirstOrDefault(static d => d.FirstName == "Gregory" && d.LastName == "House").ID
                            },
                            new Patient
                            {
                                FirstName = "Wilma",
                                MiddleName = "Jane",
                                LastName = "Flintstone",
                                OHIP = "1321321324",
                                DOB = DateTime.Parse("1964-04-23"),
                                ExpYrVisits = 2,
                                DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Gregory" && d.LastName == "House").ID
                            },
                            new Patient
                            {
                                FirstName = "Barney",
                                LastName = "Rubble",
                                OHIP = "3213213214",
                                DOB = DateTime.Parse("1964-02-22"),
                                ExpYrVisits = 2,
                                DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Doogie" && d.LastName == "Houser").ID
                            },
                            new Patient
                            {
                                FirstName = "Jane",
                                MiddleName = "Samantha",
                                LastName = "Doe",
                                OHIP = "3213213215",
                                DOB = DateTime.Parse("1979-02-01"),
                                ExpYrVisits = 2,
                                DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Charles" && d.LastName == "Xavier").ID
                            });
                            context.SaveChanges();
                        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }

                    //Leave those in place but add more using random values
                    try
                    {
                        //Add more Doctors
                        if (context.Doctors.Count() < 4)//Don't add a second time
                        {
                            string[] firstNames = new string[] { "Woodstock", "Violet", "Charlie", "Lucy", "Linus", "Franklin", "Marcie", "Schroeder" };
                            string[] lastNames = new string[] { "Hightower", "Broomspun", "Jones" };

                            //Loop through names and add more
                            foreach (string lastName in lastNames)
                            {
                                foreach (string firstname in firstNames)
                                {
                                    //Construct some details
                                    Doctor a = new Doctor()
                                    {
                                        FirstName = firstname,
                                        LastName = lastName,
                                        //Take second character of the last name and make it the middle name
                                        MiddleName = lastName[1].ToString().ToUpper(),
                                    };
                                    context.Doctors.Add(a);
                                }
                            }
                            context.SaveChanges();
                        }

                        //So we can gererate random data, create collections of the primary keys
                        int[] doctorIDs = context.Doctors.Select(a => a.ID).ToArray();
                        int doctorIDCount = doctorIDs.Length;// Why does this help efficiency?

                        //Add more Patients.  Now it gets more interesting because we
                        //have Foreign Keys to worry about
                        //and more complicated property values to generate
                        if (context.Patients.Count() < 5)
                        {
                            string[] firstNames = new string[] { "Lyric", "Antoinette", "Kendal", "Vivian", "Ruth", "Jamison", "Emilia", "Natalee", "Yadiel", "Jakayla", "Lukas", "Moses", "Kyler", "Karla", "Chanel", "Tyler", "Camilla", "Quintin", "Braden", "Clarence" };
                            string[] lastNames = new string[] { "Watts", "Randall", "Arias", "Weber", "Stone", "Carlson", "Robles", "Frederick", "Parker", "Morris", "Soto", "Bruce", "Orozco", "Boyer", "Burns", "Cobb", "Blankenship", "Houston", "Estes", "Atkins", "Miranda", "Zuniga", "Ward", "Mayo", "Costa", "Reeves", "Anthony", "Cook", "Krueger", "Crane", "Watts", "Little", "Henderson", "Bishop" };
                            int firstNameCount = firstNames.Length;

                            // Birthdate for randomly produced Patients 
                            // We will subtract a random number of days from today
                            DateTime startDOB = DateTime.Today;// More efficiency?

                            foreach (string lastName in lastNames)
                            {
                                //Choose a random HashSet of 5 (Unique) first names
                                HashSet<string> selectedFirstNames = new HashSet<string>();
                                while (selectedFirstNames.Count() < 5)
                                {
                                    selectedFirstNames.Add(firstNames[random.Next(firstNameCount)]);
                                }

                                foreach (string firstName in selectedFirstNames)
                                {
                                    //Construct some Patient details
                                    Patient patient = new Patient()
                                    {
                                        FirstName = firstName,
                                        LastName = lastName,
                                        MiddleName = lastName[1].ToString().ToUpper(),
                                        OHIP = random.Next(2, 9).ToString() + random.Next(213214131, 989898989).ToString(),
                                        ExpYrVisits = (byte)random.Next(2, 12),
                                        DOB = startDOB.AddDays(-random.Next(60, 34675)),
                                        DoctorID = doctorIDs[random.Next(doctorIDCount)]
                                    };
                                    try
                                    {
                                        //Could be a duplicate OHIP or combination of DOD, First and Last Name
                                        context.Patients.Add(patient);
                                        context.SaveChanges();
                                    }
                                    catch (Exception)
                                    {
                                        //Failed so remove it and go on to the next.
                                        //If you don't remove it from the context it
                                        //will keep trying to save it each time you 
                                        //call .SaveChanges() the the save process will stop
                                        //and prevent any other records in the que from getting saved.
                                        context.Patients.Remove(patient);
                                    }
                                }
                            }
                            //Since we didn't guarantee that evey Doctor had
                            //at least one Patient assigned, let's remove Doctors
                            //without any Patients.  We could do this other ways, but it
                            //gives a chance to show how to execute 
                            //raw SQL through our DbContext.
                            string cmd = "DELETE FROM Doctors WHERE NOT EXISTS(SELECT 1 FROM Patients WHERE Doctors.Id = Patients.DoctorID)";
                            context.Database.ExecuteSqlRaw(cmd);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }
                }

                #endregion

            }

        }
    }
}
