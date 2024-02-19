namespace deliver
{
    class Customer : Member
    {
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string homeAddress { get; set; } = "";
        public string coords { get; set; } = "2,3333 W, 34.9999 S";
        public DateTime dateCreated { get; set; }

        public Customer(int memberId,string firName, string lasName, string homeAdd)
        : base(null, null)
        {
            //login = log;
            //pass = passWord;
            MemberId = memberId;
            firstName = firName;
            lastName = lasName;
            homeAddress = homeAdd;
            dateCreated = DateTime.Now;
        }
    }
}
