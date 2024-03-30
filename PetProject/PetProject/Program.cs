namespace DataService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ctx = new PostgresContext();

            var x1 = ctx.USERS.ToList();
            var x2 = ctx.Expenses.ToList();
            var x3 = ctx.CachAccounts.ToList();
            var x4 = ctx.CashExpenses.ToList();

        }
    }
}
