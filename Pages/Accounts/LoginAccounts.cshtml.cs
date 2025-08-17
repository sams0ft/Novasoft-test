using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using novasoft_technical_test.DTOs;
using System.Text;
using System.Text.Json;

namespace novasoft_technical_test.Pages.Accounts
{
    public class LoginAccountsModel : PageModel
    {

        [BindProperty]
        public string UserLogin { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConnectionName { get; set; }

        public string Message { get; set; }

        private readonly IConfiguration _config;

        public string? LoginUrl { get; set; }

        private readonly HttpClient _httpClient;


        public LoginAccountsModel(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;

        }

        public IActionResult OnGet()
        {
            var token = Request.Cookies["token"];

            if (string.IsNullOrEmpty(token))
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/Accounts/AccountDashboard");
            }
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var LoginUrl = _config["ApiSettings:LoginUrl"];
            var json = JsonSerializer.Serialize(new { UserLogin, Password, ConnectionName});
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(LoginUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseBody);
                Console.WriteLine("Login successful. Token: " + loginResponse?.token);
                if (loginResponse == null) return Page();
                var opt = new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10))};
                Response.Cookies.Append("token", loginResponse.token, opt);
                return RedirectToPage("/Accounts/AccountDashboard");
            }

            return Page();
        }

        
    }
}
