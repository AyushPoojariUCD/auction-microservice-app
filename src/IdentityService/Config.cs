using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp", "Microservices based Auction App Access"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
        {
            ClientId = "postman",
            ClientName = "postman",
            AllowedScopes = { "openid", "profile", "auctionApp" },
            RedirectUris = { "https://oauth.pstmn.io/v1/callback" },
            ClientSecrets = { new Secret("NotASecret".Sha256()) },
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
        },
            // new Client
            //     {
            //         ClientId = "nextApp",
            //         ClientName = "nextApp",
            //         ClientSecrets = {new Secret("secret".Sha256())},
            //         AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
            //         RequirePkce = false,
            //         RedirectUris = {"http://localhost:3000/api/auth/callback/id-server"},
            //         AllowOfflineAccess = true,
            //         AllowedScopes = {"openid", "profile", "auctionApp"},
            //         AccessTokenLifetime = 3600*24*30,
            // }
    };
}
