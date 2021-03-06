﻿using System;
using Xamarin.Forms;

namespace GeoLocatorSample
{
    public class GeoCoordinatesPage : BaseContentPage<GeoCoordinatesViewModel>
    {
        #region Constructors
        public GeoCoordinatesPage()
        {
            var currentLocationTitleLabel = new TitleLabel { Text = "Lat/Long" };

            var currentLocationValueLabel = new CenteredTextLabel();
            currentLocationValueLabel.SetBinding(Label.TextProperty, nameof(ViewModel.LatLongText));

            var latLongAccuracyTitleLabel = new TitleLabel { Text = "Lat/Long Accuracy" };

            var latLongAccruacyValueLabel = new CenteredTextLabel();
            latLongAccruacyValueLabel.SetBinding(Label.TextProperty, nameof(ViewModel.LatLongAccuracyText));

            var altitudeTitleLabel = new TitleLabel { Text = "Altitude" };

            var altitudeValueLabel = new CenteredTextLabel();
            altitudeValueLabel.SetBinding(Label.TextProperty, nameof(ViewModel.AltitudeText));

            Content = new StackLayout
            {
                Spacing = 2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    currentLocationTitleLabel,
                    currentLocationValueLabel,
                    latLongAccuracyTitleLabel,
                    latLongAccruacyValueLabel,
                    altitudeTitleLabel,
                    altitudeValueLabel
                }
            };

            GeolocationService.GeolocationFailed += HandleGeolocationFailed;
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.StartUpdatingLocationCommand.Execute(null);
        }

        void HandleGeolocationFailed(object sender, Exception exception)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                switch (exception)
                {
                    default:
                    case Xamarin.Essentials.PermissionException permissionException when permissionException.Message.ToLower().Contains("main thread"):
                        await DisplayAlert("Geolocation Failed", exception.Message, "OK");
                        break;

                    case Exception javaLangException when javaLangException.Message.Contains("requestPermissions"):
                    case Xamarin.Essentials.PermissionException permissionException:
                        var shouldOpenSettings = await DisplayAlert("Geoloation Failed", "Geolocation Permission Disabled", "Open Settings", "Ignore");

                        if (shouldOpenSettings)
                            Xamarin.Essentials.AppInfo.ShowSettingsUI();
                        break;

                }
            });
        }
        #endregion

        #region Classes
        class TitleLabel : CenteredTextLabel
        {
            public TitleLabel()
            {
                TextColor = ColorConstants.TitleTextColor;
                FontAttributes = FontAttributes.Bold;
                Margin = new Thickness(0, 15, 0, 0);
            }
        }

        class CenteredTextLabel : Label
        {
            public CenteredTextLabel()
            {
                TextColor = ColorConstants.TextColor;
                HorizontalTextAlignment = TextAlignment.Center;
            }
        }
        #endregion
    }
}
