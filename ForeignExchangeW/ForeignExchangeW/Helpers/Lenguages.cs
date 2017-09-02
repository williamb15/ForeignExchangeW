﻿

namespace ForeignExchangeW.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Lenguages
    {
        static Lenguages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Title
        {
            get { return Resource.Title; }
        }
        public static string AmountValidation
        {
            get { return Resource.AmountValidation; }
        }
        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string Accept
        {
            get { return Resource.Accept; }
        }
        public static string AmountNumericValidation
        {
            get { return Resource.AmountNumericValidation; }
        }
        public static string SourceRateValidation
        {
            get { return Resource.SourceRateValidation; }
        }
        public static string TargetRateValidation
        {
            get { return Resource.TargetRateValidation; }
        }
        public static string AmountLabel
        {
            get { return Resource.AmountLabel; }
        }
        public static string AmountPlaceHolder
        {
            get { return Resource.AmountPlaceHolder; }
        }
        public static string Convert
        {
            get { return Resource.Convert; }
        }
        public static string SourceRateTitle
        {
            get { return Resource.SourceRateTitle; }
        }
        public static string SourceRateLabel
        {
            get { return Resource.SourceRateLabel; }
        }
        public static string TargetRateTitle
        {
            get { return Resource.TargetRateTitle; }
        }
        public static string TargetRateLabel
        {
            get { return Resource.TargetRateLabel; }
        }
        public static string Loading
        {
            get { return Resource.Loading; }
        }
        public static string Ready
        {
            get { return Resource.Ready; }
        }
    }

}
