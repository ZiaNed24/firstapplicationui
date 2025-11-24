using Newtonsoft.Json;

namespace FirstApplicationUI.Models
{
    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user")]
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("roleId")]
        public int RoleId { get; set; }
    }

}
