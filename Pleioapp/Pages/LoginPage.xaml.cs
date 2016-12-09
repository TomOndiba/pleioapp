﻿using System;
using Xamarin.Forms;

namespace Pleioapp
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
			var browserService = DependencyService.Get<IBrowserService> ();

			ForgotPassword.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					browserService.OpenUrl("https://www.pleio.nl/forgotpassword");
				}),
				NumberOfTapsRequired = 1
			});

			RegisterAccount.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					browserService.OpenUrl("https://www.pleio.nl/register");
				}),
				NumberOfTapsRequired = 1
			});

		}

		async void OnLogin(object sender, EventArgs e)
		{
			var service = new LoginService ();
			AuthToken token = await service.Login (username.Text, password.Text);

			if (token != null) {
				var store = DependencyService.Get<ITokenStore> ();
				var app = (App) App.Current;

				store.saveToken (token);

				app.AuthToken = token;
				app.CurrentSite = app.MainSite;
				app.WebService = new WebService ();

				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "login_succesful");
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_menu");

				await app.MainPage.Navigation.PopModalAsync ();
			} else {
				await DisplayAlert ("Login", "Kon niet inloggen, controleer je gebruikersnaam en wachtwoord.", "Ok");
			}
		}
	}
}

