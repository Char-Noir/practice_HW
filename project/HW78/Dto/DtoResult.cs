namespace HW78.DTO
{
    public class DtoResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessed { get; set; }
        public List<string> Messages { get; }

        private DtoResult() {
            Messages = new List<string>();
            IsSuccessed = true;
        }

        public void AppendMessage(string message)
        {
            Messages.Add(message);
        }
        public static DtoResult<T> Success(T data)
        {
            var result = new DtoResult<T>();
            result.Data = data;
            result.IsSuccessed = true;
            return result;
        }

        public static DtoResult<T> Error(string message) {
            var result = new DtoResult<T>();
            result.AppendMessage(message);
            result.IsSuccessed = false;
            return result;
        }
        
        
    }
}
