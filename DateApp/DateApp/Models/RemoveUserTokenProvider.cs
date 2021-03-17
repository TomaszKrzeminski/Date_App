using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class RemoveUserTokenProvider
    {

        public string Name { get; set; } = "RemoveUserTokenProvider";
        public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromDays(1);

    }

    public class RemoveUserTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser>
    where TUser : class
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var email = await manager.GetEmailAsync(user);
            return "RemoveUser:" + purpose + ":" + email;
        }
    }

    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddRemoveUserTotpTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var totpProvider = typeof(RemoveUserTotpTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("RemoveUserTotpTokenProvider", totpProvider);
        }
    }









}
