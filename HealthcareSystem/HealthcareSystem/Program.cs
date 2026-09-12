using System;
using System.Collections.Generic;
using System.Linq;

// a. Generic Repository
public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(items);
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        T? item = items.FirstOrDefault(predicate);

        if (item != null)
        {
            items.Remove(item);
            return true;
        }

        return false;
    }
}

// b. Patient class
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

// c. Prescription class
public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(
        int id,
        int patientId,
        string medicationName,
        DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}

// g. HealthSystemApp
public class HealthSystemApp
{
    private Repository<Patient> _patientRepo =
        new Repository<Patient>();

    private Repository<Prescription> _prescriptionRepo =
        new Repository<Prescription>();

    private Dictionary<int, List<Prescription>> _prescriptionMap =
        new Dictionary<int, List<Prescription>>();

    // Seed data
    public void SeedData()
    {
        _patientRepo.Add(
            new Patient(1, "Alice Smith", 25, "Female"));

        _patientRepo.Add(
            new Patient(2, "John Mensah", 32, "Male"));

        _patientRepo.Add(
            new Patient(3, "Mary Adams", 40, "Female"));

        _prescriptionRepo.Add(
            new Prescription(
                101, 1, "Paracetamol", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(
                102, 1, "Vitamin C", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(
                103, 2, "Amoxicillin", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(
                104, 2, "Ibuprofen", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(
                105, 3, "Cough Syrup", DateTime.Now));
    }

    // Build dictionary
    public void BuildPrescriptionMap()
    {
        _prescriptionMap.Clear();

        foreach (Prescription prescription
                 in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] =
                    new List<Prescription>();
            }

            _prescriptionMap[prescription.PatientId]
                .Add(prescription);
        }
    }

    // d/f. Get prescriptions by patient ID
    public List<Prescription> GetPrescriptionsByPatientId(
        int patientId)
    {
        if (_prescriptionMap.ContainsKey(patientId))
        {
            return _prescriptionMap[patientId];
        }

        return new List<Prescription>();
    }

    // Print all patients
    public void PrintAllPatients()
    {
        Console.WriteLine("\n=== PATIENTS ===");

        foreach (Patient patient in _patientRepo.GetAll())
        {
            Console.WriteLine(
                $"ID: {patient.Id}, " +
                $"Name: {patient.Name}, " +
                $"Age: {patient.Age}, " +
                $"Gender: {patient.Gender}"
            );
        }
    }

    // Print prescriptions
    public void PrintPrescriptionsForPatient(int id)
    {
        Console.WriteLine(
            $"\n=== PRESCRIPTIONS FOR PATIENT {id} ===");

        List<Prescription> prescriptions =
            GetPrescriptionsByPatientId(id);

        if (prescriptions.Count == 0)
        {
            Console.WriteLine("No prescriptions found.");
            return;
        }

        foreach (Prescription prescription in prescriptions)
        {
            Console.WriteLine(
                $"Prescription ID: {prescription.Id}, " +
                $"Medication: {prescription.MedicationName}, " +
                $"Date: {prescription.DateIssued:d}"
            );
        }
    }
}

// Main
public class Program
{
    public static void Main()
    {
        HealthSystemApp app = new HealthSystemApp();

        app.SeedData();
        app.BuildPrescriptionMap();
        app.PrintAllPatients();

        // Display prescriptions for Patient ID 1
        app.PrintPrescriptionsForPatient(1);
    }
}
