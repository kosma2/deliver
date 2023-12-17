namespace deliver
{
    class Member
    {
        public string login { get; set; } = "";
        public string pass { get; set; } = "";
        public Member(String log, String passWord)
        {
            login = log;
            pass = passWord;
        }
    }
}
