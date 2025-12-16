using BuildingBlocks.Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; private set; } = default!;
        public string? AvatarUrl { get; private set; }

        public ApplicationUser(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new DomainException("Full Name is required");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required");

            FullName = fullName;

            UserName = email; 
            Email = email;
        }

        public void UpdateProfile(string fullName, string avatarUrl)
        {
            FullName = fullName;
            AvatarUrl = avatarUrl;
        }
    }
}
