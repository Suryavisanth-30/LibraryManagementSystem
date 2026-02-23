namespace LibraryManagement.DTO
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Role {  get; set; }
        public string Message { get; set; }
    }
}
