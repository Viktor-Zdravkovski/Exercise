namespace BasicWeb.Domain
{
    public class Country : BaseEntity
    {
        public virtual ICollection<Contact> Contacts { get; set; }

        public Dictionary<string, int> GetCompanyStatistics()
        {
            return Contacts.GroupBy(x => x.Company.Name).ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
