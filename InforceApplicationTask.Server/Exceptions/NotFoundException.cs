namespace InforceApplicationTask.Server.Exceptions
{
    public class NotFoundException : Exception
    {
        private readonly string typeName;
        public NotFoundException(string entityName) : base()
        {
            typeName = entityName;
        }

        public override string Message => $"Object of {typeName} type was not found.";
    }
}
