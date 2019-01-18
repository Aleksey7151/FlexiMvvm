﻿// <auto-generated />
// =========================================================================
// Copyright 2019 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================

using Android.Support.Design.Widget;
using Android.Views;
using System;
using FlexiMvvm.Weak.Delegates;
using FlexiMvvm.Weak.Subscriptions;

namespace FlexiMvvm.Views
{
    public static class BottomNavigationViewWeakEventsSubscriptionsExtensions
    {
        public static IDisposable NavigationItemSelectedWeakSubscribe(
            this BottomNavigationView eventSource,
            EventHandler<BottomNavigationView.NavigationItemSelectedEventArgs> eventHandler)
        {
            if (eventSource == null)
                throw new ArgumentNullException(nameof(eventSource));
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler));

            return new WeakEventSubscription<BottomNavigationView, BottomNavigationView.NavigationItemSelectedEventArgs>(
                eventSource,
                (source, handler) => source.NavigationItemSelected += handler,
                (source, handler) => source.NavigationItemSelected -= handler,
                eventHandler);
        }

    }
}

namespace FlexiMvvm.Views
{
    public static class ViewWeakEventsSubscriptionsExtensions
    {
        public static IDisposable ClickWeakSubscribe(
            this View eventSource,
            EventHandler eventHandler)
        {
            if (eventSource == null)
                throw new ArgumentNullException(nameof(eventSource));
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler));

            return new WeakEventSubscription<View>(
                eventSource,
                (source, handler) => source.Click += handler,
                (source, handler) => source.Click -= handler,
                eventHandler);
        }

    }
}
