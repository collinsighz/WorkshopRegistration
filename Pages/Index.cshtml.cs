using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopRegistration.Models;

namespace WorkshopRegistration.Pages;

public class IndexModel : PageModel
{
    // Data the page can read with @Model.xxx
    public string message { get; set; } = "";
    public Registration? confirmed { get; set; }
    public List<string> errors { get; set; } = new List<string>();
    public List<Registration> savedRegistrations { get; set; } = new List<Registration>();

    public List<string> workshops { get; set; } = new List<string>
    {
        "C# Basics",
        "Web Pages with Razor",
        "Intro to Databases"
    };

    public int minAge { get; set; } = 16;
    public int maxAge { get; set; } = 100;

    // What the user typed (kept so the form is not wiped when there is an error)
    public string firstNameInput { get; set; } = "";
    public string ageInput { get; set; } = "";
    public string workshopInput { get; set; } = "";
    public string laptopInput { get; set; } = "";

    // Runs when the page is first opened
    public void OnGet()
    {
        LoadSavedRegistrations();
    }

    // Runs when the form is submitted
    public void OnPost()
    {
        // 1. FORM PROCESSING: read the values using each input's "name"
        firstNameInput = Request.Form["firstName"].ToString().Trim();
        ageInput = Request.Form["age"].ToString().Trim();
        workshopInput = Request.Form["workshop"].ToString();
        laptopInput = Request.Form["laptop"].ToString();

        // 2. SERVER-SIDE VALIDATION
        if (firstNameInput == "")
        {
            errors.Add("First name is required.");
        }
        else
        {
            if (firstNameInput.Length < 2 || firstNameInput.Length > 30)
            {
                errors.Add("First name must be between 2 and 30 characters.");
            }

            foreach (char c in firstNameInput)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-' && c != '\'')
                {
                    errors.Add("First name can only contain letters, spaces, hyphens and apostrophes.");
                    break;
                }
            }
        }

        int age = 0;
        if (ageInput == "")
        {
            errors.Add("Age is required.");
        }
        else if (!int.TryParse(ageInput, out age))
        {
            errors.Add("Age must be a whole number.");
        }
        else if (age < minAge || age > maxAge)
        {
            errors.Add($"Age must be between {minAge} and {maxAge}.");
        }

        if (workshopInput == "")
        {
            errors.Add("Please choose a workshop.");
        }
        else if (!workshops.Contains(workshopInput))
        {
            errors.Add("That workshop does not exist. Please choose one from the list.");
        }

        if (laptopInput != "true" && laptopInput != "false")
        {
            errors.Add("Please say whether you need a laptop.");
        }

        // 3. If anything is wrong, stop. Nothing is saved.
        if (errors.Count > 0)
        {
            LoadSavedRegistrations();
            return;
        }

        // 4. CREATE THE OBJECT
        bool needsLaptop = Convert.ToBoolean(laptopInput);
        Registration r = new Registration(firstNameInput, age, workshopInput, needsLaptop);
        confirmed = r;

        // 5. FILE HANDLING (writing)
        try
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(folder);

            string file = Path.Combine(folder, "registrations.txt");

            // AppendAllText adds to the END, so earlier registrations are kept
            System.IO.File.AppendAllText(file, r.ToFileLine() + Environment.NewLine);

            message = "Thank you! Your registration was saved.";

            // Clear the form for the next person
            firstNameInput = "";
            ageInput = "";
            workshopInput = "";
            laptopInput = "";
        }
        catch (IOException)
        {
            confirmed = null;
            errors.Add("Sorry, your registration could not be saved. Please try again.");
        }

        LoadSavedRegistrations();
    }

    // FILE HANDLING (reading)
    private void LoadSavedRegistrations()
    {
        string file = Path.Combine(Directory.GetCurrentDirectory(), "Data", "registrations.txt");

        if (!System.IO.File.Exists(file))
        {
            return;
        }

        string[] lines = System.IO.File.ReadAllLines(file);

        foreach (string line in lines)
        {
            string[] parts = line.Split('|');

            if (parts.Length != 4)
            {
                continue;
            }

            int age;
            bool needsLaptop;
            if (int.TryParse(parts[1], out age) && bool.TryParse(parts[3], out needsLaptop))
            {
                savedRegistrations.Add(new Registration(parts[0], age, parts[2], needsLaptop));
            }
        }
    }
}