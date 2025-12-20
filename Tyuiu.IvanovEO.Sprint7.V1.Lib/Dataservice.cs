namespace Tyuiu.IvanovEO.Sprint7.V1.Lib
{
    public class DataService
    {
        public class Owner
        {
            public string LicenseNumber { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
        }

        public class Mechanic
        {
            public string EmployeeId { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Qualification { get; set; }
            public double HourlyRate { get; set; }
        }

        public class Car
        {
            public string CarId { get; set; }
            public string Brand { get; set; }
            public int Power { get; set; }
            public string Color { get; set; }
            public string OwnerLicenseNumber { get; set; }
        }

        public class Workshop
        {
            public string WorkshopId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public List<RepairOrder> Orders { get; set; } = new List<RepairOrder>();
        }

        public class RepairOrder
        {
            public string OrderId { get; set; }
            public string CarId { get; set; }
            public string WorkshopId { get; set; }
            public string MechanicId { get; set; }
            public DateTime RepairDate { get; set; }
            public string Description { get; set; }
            public double HoursWorked { get; set; }
            public double PartsCost { get; set; }
            public double TotalCost { get; set; }
        }

        public class DataServices
        {
            private List<Owner> owners;
            private List<Mechanic> mechanics;
            private List<Car> cars;
            private List<Workshop> workshops;
            private List<RepairOrder> orders;

            public DataServices()
            {
                owners = new List<Owner>();
                mechanics = new List<Mechanic>();
                cars = new List<Car>();
                workshops = new List<Workshop>();
                orders = new List<RepairOrder>();
            }

            public void LoadAllData(string basePath)
            {
                LoadOwners($"{basePath}\\owners.csv");
                LoadMechanics($"{basePath}\\mechanics.csv");
                LoadCars($"{basePath}\\cars.csv");
                LoadWorkshops($"{basePath}\\workshops.csv");
                LoadOrders($"{basePath}\\orders.csv");
            }

            public void SaveAllData(string basePath)
            {
                SaveOwners($"{basePath}\\owners.csv");
                SaveMechanics($"{basePath}\\mechanics.csv");
                SaveCars($"{basePath}\\cars.csv");
                SaveWorkshops($"{basePath}\\workshops.csv");
                SaveOrders($"{basePath}\\orders.csv");
            }

            
            public void AddOwner(Owner owner) => owners.Add(owner);
            public void UpdateOwner(Owner owner)
            {
                var existing = owners.FirstOrDefault(o => o.LicenseNumber == owner.LicenseNumber);
                if (existing != null)
                {
                    existing.FullName = owner.FullName;
                    existing.Address = owner.Address;
                    existing.Phone = owner.Phone;
                }
            }
            public void DeleteOwner(string licenseNumber) => owners.RemoveAll(o => o.LicenseNumber == licenseNumber);
            public Owner FindOwner(string licenseNumber) => owners.FirstOrDefault(o => o.LicenseNumber == licenseNumber);
            public List<Owner> SearchOwners(string searchTerm) => owners.Where(o =>
                o.FullName.Contains(searchTerm) ||
                o.Address.Contains(searchTerm) ||
                o.Phone.Contains(searchTerm)).ToList();

            
            public void AddMechanic(Mechanic mechanic) => mechanics.Add(mechanic);
            public void UpdateMechanic(Mechanic mechanic)
            {
                var existing = mechanics.FirstOrDefault(m => m.EmployeeId == mechanic.EmployeeId);
                if (existing != null)
                {
                    existing.FullName = mechanic.FullName;
                    existing.Address = mechanic.Address;
                    existing.Phone = mechanic.Phone;
                    existing.Qualification = mechanic.Qualification;
                    existing.HourlyRate = mechanic.HourlyRate;
                }
            }
            public void DeleteMechanic(string employeeId) => mechanics.RemoveAll(m => m.EmployeeId == employeeId);
            public Mechanic FindMechanic(string employeeId) => mechanics.FirstOrDefault(m => m.EmployeeId == employeeId);
            public List<Mechanic> SearchMechanics(string searchTerm) => mechanics.Where(m =>
                m.FullName.Contains(searchTerm) ||
                m.Qualification.Contains(searchTerm)).ToList();

            
            public Statistics GetStatistics()
            {
                var stats = new Statistics();
                if (orders.Any())
                {
                    stats.TotalOrders = orders.Count;
                    stats.TotalRevenue = orders.Sum(o => o.TotalCost);
                    stats.AverageOrderCost = orders.Average(o => o.TotalCost);
                    stats.MaxOrderCost = orders.Max(o => o.TotalCost);
                    stats.MinOrderCost = orders.Min(o => o.TotalCost);
                    stats.TotalHoursWorked = orders.Sum(o => o.HoursWorked);
                }
                return stats;
            }

            public Dictionary<string, double> GetRevenueByWorkshop()
            {
                return orders
                    .GroupBy(o => o.WorkshopId)
                    .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalCost));
            }

            public Dictionary<string, int> GetOrdersByMechanic()
            {
                return orders
                    .GroupBy(o => o.MechanicId)
                    .ToDictionary(g => g.Key, g => g.Count());
            }

            
            private void LoadOwners(string filePath)
            {
                if (!System.IO.File.Exists(filePath)) return;
                var lines = System.IO.File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 4)
                    {
                        owners.Add(new Owner
                        {
                            LicenseNumber = parts[0],
                            FullName = parts[1],
                            Address = parts[2],
                            Phone = parts[3]
                        });
                    }
                }
            }

            private void SaveOwners(string filePath)
            {
                var lines = new List<string> { "LicenseNumber;FullName;Address;Phone" };
                lines.AddRange(owners.Select(o => $"{o.LicenseNumber};{o.FullName};{o.Address};{o.Phone}"));
                System.IO.File.WriteAllLines(filePath, lines);
            }

            
            private void LoadMechanics(string filePath) { }
            private void SaveMechanics(string filePath) { }
            private void LoadCars(string filePath) { }
            private void SaveCars(string filePath) { }
            private void LoadWorkshops(string filePath) { }
            private void SaveWorkshops(string filePath) {}
            private void LoadOrders(string filePath) { }
            private void SaveOrders(string filePath) { }
        }

        public class Statistics
        {
            public int TotalOrders { get; set; }
            public double TotalRevenue { get; set; }
            public double AverageOrderCost { get; set; }
            public double MaxOrderCost { get; set; }
            public double MinOrderCost { get; set; }
            public double TotalHoursWorked { get; set; }
        }
    }
}
