﻿
using Xamarin.Forms;
[assembly: Dependency(typeof(ForeignExchangeW.Droid.Implementations.Config))]

namespace ForeignExchangeW.Droid.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using Interfaces;
    using SQLite.Net.Interop;

    public class Config : IConfig
        {
            private string directoryDB;
            private ISQLitePlatform platform;

            public string DirectoryDB
            {
                get
                {
                    if (string.IsNullOrEmpty(directoryDB))
                    {
                        directoryDB = System.Environment.GetFolderPath(
                            System.Environment.SpecialFolder.Personal);
                    }

                    return directoryDB;
                }
            }

            public ISQLitePlatform Platform
            {
                get
                {
                    if (platform == null)
                    {
                        platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                    }

                    return platform;

                }
            }
        }
}