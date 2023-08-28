using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;


namespace HappyReset.Features.Shared.Services;

public static  class AccountNameService
{
    public static string DEFAULT_ACCOUNT_NAME = "default";
    public static async Task<string> UpdateAccountName()
    {
        var gw2ApiManager = Service.Gw2ApiManager;

        if (gw2ApiManager.HasPermissions(NecessaryApiTokenPermissions) == false)
        {

            return DEFAULT_ACCOUNT_NAME;
        }

        try
        {
            var accountInfo = await gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync();

            return accountInfo.Name;
        }
        catch (Exception e)
        {
            Debug.WriteLine("HappyReset: Warning, account name not recieved from the API "+e.Message);
            return DEFAULT_ACCOUNT_NAME;
        }
    }

    private static readonly List<TokenPermission> NecessaryApiTokenPermissions = new()
    {
        TokenPermission.Account
    };
}