namespace SistemaVenta.API.Utilidad
{
    public class Response<T>
    {
        public bool Status {  get; set; }
        public T Value { get; set; }
        public string Msg { get; set; }
    }
}
