namespace Erx.Queue
{
    internal class RabbitMqOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
    }
}
