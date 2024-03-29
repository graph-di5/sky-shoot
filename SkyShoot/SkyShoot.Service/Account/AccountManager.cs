﻿using Microsoft.WindowsAzure.ServiceRuntime;

namespace SkyShoot.Service.Account
{
	public class AccountManager
	{
		/**
				(!) нужно будет делать регистрацию приводя все знаки username / password к строчным
				ещё проверять нужно будет пароли и имена, чтобы они содержали только a-z,A-Z,1-9 ,-,_ и никаких пробелов
		  
				Пароль в эти функции уже должен приходить как md5 хеш, это делается при вызове функций на клиенте...только где это писать
				**/

		public bool Register(string user_name, string password)
		{
			if (TestPattern.TestAccountName(user_name))
			{
				if (TestPattern.TestPassword(password))
				{
					Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
					{
						configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
					});

					string salt = HashHelper.GetRandomString();
					AccountManagerEntry entry = new AccountManagerEntry()
					{
						Account = user_name.ToLower(),
						HashPassword = HashHelper.GetMd5Hash(password + salt),
						Salt = salt,
						Email = "sky@shoot",
						Info = "--"
					};
					AccountManagerDataSource ds = new AccountManagerDataSource();
					return ds.AddAccountManagerEntry(entry);
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public bool Login(string user_name, string password)
		{
			Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
			{
				configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
			});

			AccountManagerDataSource ds = new AccountManagerDataSource();

			return ds.CheckAccountManagerEntry(user_name.ToLower(), password);
		}

		public bool CreatePassword(string user_name, string old_password, string new_password)
		{
			if (TestPattern.TestPassword(new_password))
			{
				Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
				{
					configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
				});

				AccountManagerDataSource ds = new AccountManagerDataSource();

				return ds.CreateAccountPassword(user_name.ToLower(), old_password, new_password);
			}
			else
			{
				return false;
			}
		}

		public bool DeleteAccount(string user_name)
		{
			Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
			{
				configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
			});

			AccountManagerDataSource ds = new AccountManagerDataSource();

			return ds.DeleteAccountManagerEntry(user_name.ToLower());
		}

	}
}