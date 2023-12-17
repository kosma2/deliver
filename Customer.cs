namespace deliver
{
    class Customer
    {
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string homeAddress { get; set; } = "";
        public string coords { get; set; } = "";
        public DateOnly dateCreated { get; set; }

        public Customer(string firName, string lasName, string homeAdd, DateOnly dateCrtd)
        {
            firstName = firName;
            lastName = lasName;
            homeAddress = homeAdd;
            dateCreated = dateCrtd;
        }
    }
}
