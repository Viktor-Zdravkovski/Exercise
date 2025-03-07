namespace BasicWeb.Domain
{
    public class Company : BaseEntity
    {
        public int UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (name.All(char.IsDigit))
            {
                throw new ArgumentException("Company name cannot contain only numbers");
            }

            Name = name;

            return Id;
        }
    }
}
