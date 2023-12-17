namespace deliver
{
    partial class Program
    {
        /*public static Member createMember(String login, String pass)
        {
            //Member member = new();
            member.login = login;
            member.pass = pass;
            return member;
        }*/
        public abstract class DbConnection
        {

            public String SqlStr;
            //public abstract void Connect(String connString);
            public abstract void DBdispItems();
            public abstract void DBAddItem(Item item);
            public abstract void DBDeleteItem(String id);
            public abstract void DBUpdateCustomer(String id);
            public abstract void DBDeleteCustomer(String id);
            public abstract void DBAddMember(Member mem);
            public abstract void DBCheckLogin(Member mem);
        }

    }
}
