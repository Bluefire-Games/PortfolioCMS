using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace PortfolioCMS.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string AuthKey = "isLoggedIn";
        private const string UserKey = "userCredentials";

        public event Action? OnAuthStateChanged; // << 👈 NEW!

        public AuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var result = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", AuthKey);
            return result == "true";
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var saved = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserKey);

            if (saved == $"{email}:{password}")
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthKey, "true");
                OnAuthStateChanged?.Invoke(); // << 👈 FIRE EVENT AFTER LOGIN
                return true;
            }
            return false;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password)
        {
            var existingCredentials = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserKey);

            if (!string.IsNullOrEmpty(existingCredentials))
            {
                var savedEmail = existingCredentials.Split(':')[0];
                if (savedEmail == email)
                {
                    return (false, "An account with this email already exists.");
                }
            }

            var newCredentials = $"{email}:{password}";
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserKey, newCredentials);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthKey, "true"); // Auto-login after register
            OnAuthStateChanged?.Invoke(); // << 👈 FIRE EVENT AFTER REGISTER
            return (true, "Account created successfully!");
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AuthKey);
            OnAuthStateChanged?.Invoke(); // << 👈 FIRE EVENT AFTER LOGOUT
        }
    }
}
