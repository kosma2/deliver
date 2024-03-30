namespace deliver
{
    class Customer : Member
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeAddress { get; set; } = "";
        public string GeoPoint { get; set; } = "";
        public DateTime DateCreated { get; set; }

        public Customer(int memberId,string firName, string lasName, string homeAdd, string geoPt)
        : base(null, null)
        {
            MemberId = memberId;
            FirstName = firName;
            LastName = lasName;
            HomeAddress = homeAdd;
            GeoPoint = geoPt;
            DateCreated = DateTime.Now;
        }
    }
}
