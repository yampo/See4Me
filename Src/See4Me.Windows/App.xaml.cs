﻿using See4Me.Views;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using GalaSoft.MvvmLight.Threading;
using Windows.ApplicationModel;
using Microsoft.Practices.ServiceLocation;
using See4Me.Services;
using See4Me.ViewModels;
using Windows.Globalization;
using Template10.Common;

namespace See4Me
{
    sealed partial class App : BootStrapper
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            ApplicationLanguages.PrimaryLanguageOverride = ViewModelLocator.GetLanguage();

            var keys = PageKeys<Pages>();
            keys.Add(Pages.MainPage, typeof(MainPage));
            keys.Add(Pages.SettingsPage, typeof(SettingsPage));
            keys.Add(Pages.AboutPage, typeof(AboutPage));
            keys.Add(Pages.RecognizeTextPage, typeof(RecognizeTextPage));
            keys.Add(Pages.PrivacyPolicyPage, typeof(PrivacyPolicyPage));

            var navigationService = new NavigationService();
            var locator = Resources["Locator"] as ViewModelLocator;
            locator.Initialize(navigationService);

            // MVVM Light's DispatcherHelper for cross-thread handling.
            DispatcherHelper.Initialize();

            return base.OnInitializeAsync(args);
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(Pages.MainPage);
            return Task.CompletedTask;
        }

        public override async void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            // Resumes the camera streaming.
            await ViewModelLocator.ResumeAsync();

            base.OnResuming(s, e, previousExecutionState);
        }
    }
}
