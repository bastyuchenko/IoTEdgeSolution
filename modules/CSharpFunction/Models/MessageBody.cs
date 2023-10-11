namespace CSharpFunction.Models
{
    class MessageBody
    {
        public Machine machine { get; set; }
        public Ambient ambient { get; set; }
        public string timeCreated { get; set; }
    }
}