namespace deliver
{
    partial class Program
    {
        public abstract class DbConnection
        {

            public String SqlStr;
            //public abstract void Connect(String connString);
            public abstract void DBdeleteOrder(int orderId);
            public abstract (int,int) InterfaceCreateOrder();
            public abstract int DBcreateOrder(int itemId, int quantity);
            public abstract int DBcreateOrderItem(int orderId, int itemId, int quantity);
            public abstract List<object> DBListItems();
            public abstract void DBAddItem(Item item);
            public abstract void DBDeleteItem(String id);
            public abstract void DBUpdateCustomer(String id);
            public abstract void DBDeleteCustomer(String id);
            public abstract void DBCreateMember(Member mem);
            public abstract bool DBCreateCustomer(Customer cust);
            public abstract int DBCheckLogin(Member mem);
            public abstract decimal GetItemPrice(int itemId);
        }

    }
}
