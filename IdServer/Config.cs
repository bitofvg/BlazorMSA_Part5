﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdServer {
  public  class Config {

    private IConfiguration cfg;
    public Config(IConfiguration configuration) {
      cfg = configuration;
    }
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[] {
          new IdentityResources.OpenId(),
          new IdentityResources.Profile(),
          new IdentityResources.Email(),
          new IdentityResources.Phone(),
          new IdentityResource {
              Name="my_nice_scope",
              UserClaims = new List<string> { "my_nice_claim_1", "my_nice_claim_2" }
            }
        };

    public static IEnumerable<ApiResource> ApiResources =>
       new List<ApiResource> {
          new ApiResource("WApi1") {
              Scopes = {
                "WApi1.Weather.GetById",
                "WApi1.Weather.List",
                "WApi1.Weather.Insert",
                "WApi1.Weather.Update",
                "WApi1.Weather.Delete",
              },
              UserClaims =  { "name", "email" }
          },
          new ApiResource("WApiFAKE") {
              Scopes = {
                "WApi1.Weather.List",
              },
              UserClaims =  { "name", "website" }
          },
          new ApiResource("IdentityServer.Users") {
              Scopes = {
                "IdentityServer.Users.Add",
                "IdentityServer.Users.List",
              }
          },
      };

    public static IEnumerable<ApiScope> ApiScopes =>
      new ApiScope[] {
        new ApiScope("WApi1.Weather.GetById"),
        new ApiScope(){ 
          Name ="WApi1.Weather.List",
          UserClaims = { "my_nice_claim_3", "my_nice_claim_2" }
        },
        new ApiScope("WApi1.Weather.Insert"),
        new ApiScope("WApi1.Weather.Update"),
        new ApiScope("WApi1.Weather.Delete"),
        new ApiScope("IdentityServer.Users.Add"),
        new ApiScope("IdentityServer.Users.List"),
      };

    public IEnumerable<Client> Clients =>
        new Client[]
        {

          // Clent Blazor #1
          new Client
          {
            ClientId = "BlazorCli1",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            AllowedCorsOrigins = { cfg["ServicesUrls:BlazorClient1"] },
            AllowedScopes = {
              "openid", "profile",
              "email", "phone",
              "my_nice_scope",
              "WApi1.Weather.List",
              "IdentityServer.Users.Add",
              "IdentityServer.Users.List"
            },
            RedirectUris = { cfg["ServicesUrls:BlazorClient1"] + "/authentication/login-callback" },
            PostLogoutRedirectUris = { cfg["ServicesUrls:BlazorClient1"] + "/" },
            Enabled = true
          },

        };
  }
}