namespace WorkshopRegistration.Models
{
    public class Registration
    {
        // Private variables: only this class can see them
        private string firstName = "";
        private int age;
        private string workshop = "";
        private bool needsLaptop;

        // Default constructor
        public Registration()
        {
            firstName = "";
            age = 0;
            workshop = "";
            needsLaptop = false;
        }

        // Parameterized constructor ("this" separates the variable from the parameter)
        public Registration(string firstName, int age, string workshop, bool needsLaptop)
        {
            this.firstName = firstName;
            this.age = age;
            this.workshop = workshop;
            this.needsLaptop = needsLaptop;
        }

        // Properties (getter + setter combined)
        public string FirstName { get { return firstName; } set { firstName = value; } }
        public int Age { get { return age; } set { age = value; } }
        public string Workshop { get { return workshop; } set { workshop = value; } }
        public bool NeedsLaptop { get { return needsLaptop; } set { needsLaptop = value; } }

        // General method: true/false becomes Yes/No
        public string LaptopText()
        {
            if (needsLaptop)
            {
                return "Yes";
            }
            return "No";
        }

        // One line for the file: FirstName|Age|Workshop|NeedsLaptop
        public string ToFileLine()
        {
            return firstName + "|" + age + "|" + workshop + "|" + needsLaptop;
        }

        // All the information as text
        public override string ToString()
        {
            return $"Name: {firstName}, Age: {age}, Workshop: {workshop}, Laptop required: {LaptopText()}";
        }
    }
}