using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using novasoft_technical_test.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace novasoft_technical_test.Pages.Accounts
{

    public class AccountDashboardModel : PageModel
    {
        [BindProperty]
        public Account Account { get; set; } = new Account();

        public List<Account> Accounts { get; set; } = new();

        [BindProperty]
        public string? CodCli { get; set; }

        private readonly IConfiguration _config;

        public string AccountsUrl { get; set; }


        public AccountDashboardModel(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
        }


        public async Task<IActionResult> OnGetAsync([FromQuery] string? codCli)
        {
            CodCli = codCli;
            var AccountsUrl = _config["ApiSettings:AccountsUrl"];
            using var httpClient = new HttpClient();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var token = Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Authentication token is missing.");
                return RedirectToPage("/Accounts/LoginAccounts");
            }

            var urlSuffix = string.IsNullOrEmpty(CodCli) ? "" : $"{CodCli}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await httpClient.GetAsync($"{AccountsUrl}/{CodCli}");
            if (response.IsSuccessStatusCode)
            {   
                var contentString = await response.Content.ReadAsStringAsync();
                Accounts = JsonSerializer.Deserialize<List<Account>>(contentString, options) ?? new List<Account>();
                return Page();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error retrieving accounts.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var httpClient = new HttpClient();
            var AccountsUrl = _config["ApiSettings:AccountsUrl"];
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var token = Request.Cookies["token"];

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Authentication token is missing.");
                return RedirectToPage("/Accounts/LoginAccounts");
            }

            var json = JsonSerializer.Serialize(Account, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await httpClient.PostAsync(
                AccountsUrl,
                content
            );

            var contentString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Account created successfully!";
                return Page();
            }
            else
            {
                var contentBody = JsonSerializer.Deserialize<APIErrorResponse>(contentString, new JsonSerializerOptions { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                ModelState.AddModelError(string.Empty, $"Error creating account: {contentBody}");
                return Page();
            }
        }

    }
}
