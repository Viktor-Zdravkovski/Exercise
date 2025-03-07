namespace BasicWeb.Domain
{
    public class Company : BaseEntity
    {
        // added
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
