using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApis()
              => new List<ApiResource>{
                  new ApiResource
                  {
                      Name="ApiOne",
                      Scopes = {"ApiOne.fullAccess" }
                  },
                  new ApiResource{
                      Name= "ApiTwo",
                      Scopes = {"ApiTwo.readAccess"}
                  }
              };
        public static IEnumerable<ApiScope> GetApiScopes()
               => new List<ApiScope> {
                   new ApiScope
                   {
                       Name ="ApiOne.fullAccess"
                   },
                   new ApiScope{
                       Name = "ApiTwo.readAccess"
                   }
               };
        public static IEnumerable<IdentityResource> GetIdentityResources()
         => new List<IdentityResource>{
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
                 new IdentityResource
                 {
                     Name = "rc.scope",
                     UserClaims =
                     {
                         "myCustomeCliam"
                     }
                 }
         };
        public static IEnumerable<Client> GetClients()
              => new List<Client>{
                  new Client {
                      ClientId ="client_id",
                      ClientSecrets = {
                          new Secret("client_secret".ToSha256())
                      },
                      AllowedGrantTypes = GrantTypes.ClientCredentials,
                      AllowedScopes = {"ApiOne.fullAccess"}

                     }
                     ,
                     new Client {
                      ClientId ="client_id_mvc",
                      ClientSecrets = {
                          new Secret("client_secret_mvc".ToSha256())
                      },
                      AllowedGrantTypes = GrantTypes.Code,
                      RedirectUris = {"https://localhost:5006/signin-oidc"},
                      AllowedScopes = {"ApiOne.fullAccess","ApiTwo.readAccess",
                             "openid",
                             "profile",
                             "rc.scope"
                         },
                    //  AlwaysIncludeUserClaimsInIdToken = true

                    },
                     
              };
    }
}