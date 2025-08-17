namespace novasoft_technical_test.DTOs
{
    public class APIErrorResponse {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }

        public override string ToString()
        {
            return $"[type={Type},title={Title},status={Status},traceId={TraceId}]";
        }
    }
}
