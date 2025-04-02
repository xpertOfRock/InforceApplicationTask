namespace InforceApplicationTask.Server.Exceptions
{
    public class NotFoundException<T> : Exception
    {
        private readonly string typeName;
        public NotFoundException() : base()
        {
            typeName = typeof(T).Name;
        }

        public override string Message => $"Object of {typeName} type was not found.";
    }
}
