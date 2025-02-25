namespace NetProg2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Async Server");

            AsyncServer asyncServer = new AsyncServer("127.0.0.1", 45600);
            asyncServer.Start();
        }
    }
}
