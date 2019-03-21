﻿// =========================================================================
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

using System;
using Android.Content;
using Android.OS;
using FlexiMvvm.Formatters;
using FlexiMvvm.ViewModels;
using FlexiMvvm.Views;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentActivity = Android.Support.V4.App.FragmentActivity;

namespace FlexiMvvm.Navigation
{
    public abstract class NavigationService
    {
        /// <summary>
        /// Performs forward navigation from the <paramref name="sourceView"/> to the target one.
        /// </summary>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="targetViewIntent">The description of the target view.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="ForwardNavigationStrategy.StartActivity(Bundle?)"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> or <paramref name="targetViewIntent"/> is <c>null</c>.</exception>
        public void Navigate(
            INavigationView<ILifecycleViewModel> sourceView,
            Intent targetViewIntent,
            ForwardNavigationDelegate? navigationStrategy = null)
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));
            if (targetViewIntent == null)
                throw new ArgumentNullException(nameof(targetViewIntent));

            (navigationStrategy ?? NavigationStrategy.Forward.StartActivity()).Invoke(sourceView, targetViewIntent, RequestCode.InvalidRequestCode);
        }

        /// <summary>
        /// Performs forward navigation from the <paramref name="sourceView"/> to the target one.
        /// </summary>
        /// <typeparam name="TTargetView">The type of the target view.</typeparam>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="ForwardNavigationStrategy.StartActivity(Bundle?)"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sourceView" /> is derived from a class other than the <see cref="FragmentActivity"/> or <see cref="Fragment"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NavigationViewExtensions.GetActivity(INavigationView{ILifecycleViewModel})"/> returned <c>null</c> value for the <paramref name="sourceView"/>.
        /// </exception>
        public void Navigate<TTargetView>(
            INavigationView<ILifecycleViewModel> sourceView,
            ForwardNavigationDelegate? navigationStrategy = null)
            where TTargetView : FragmentActivity, IView<ILifecycleViewModel>
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));

            var context = sourceView.GetActivity();

            if (context == null)
            {
                throw new InvalidOperationException(
                    $"'{TypeFormatter.FormatName(sourceView.GetType())}.{nameof(NavigationViewExtensions.GetActivity)}' returned 'null' value.");
            }

            var intent = new Intent(context, typeof(TTargetView));
            (navigationStrategy ?? NavigationStrategy.Forward.StartActivity()).Invoke(sourceView, intent, RequestCode.InvalidRequestCode);
        }

        /// <summary>
        /// Performs forward navigation from the <paramref name="sourceView"/> to the target one with provided <paramref name="parameters"/>.
        /// </summary>
        /// <typeparam name="TTargetView">The type of the target view.</typeparam>
        /// <typeparam name="TParameters">The type of the target view model parameters.</typeparam>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="parameters">The target view model parameters. Can be <c>null</c>.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="ForwardNavigationStrategy.StartActivity(Bundle?)"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sourceView" /> is derived from a class other than the <see cref="FragmentActivity"/> or <see cref="Fragment"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NavigationViewExtensions.GetActivity(INavigationView{ILifecycleViewModel})"/> returned <c>null</c> value for the <paramref name="sourceView"/>.
        /// </exception>
        public void Navigate<TTargetView, TParameters>(
            INavigationView<ILifecycleViewModel> sourceView,
            TParameters? parameters,
            ForwardNavigationDelegate? navigationStrategy = null)
            where TTargetView : FragmentActivity, IView<ILifecycleViewModelWithParameters<TParameters>>
            where TParameters : Parameters
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));

            var context = sourceView.GetActivity();

            if (context == null)
            {
                throw new InvalidOperationException(
                    $"'{TypeFormatter.FormatName(sourceView.GetType())}.{nameof(NavigationViewExtensions.GetActivity)}' returned 'null' value.");
            }

            var intent = new Intent(context, typeof(TTargetView));
            intent.PutParameters(parameters);
            (navigationStrategy ?? NavigationStrategy.Forward.StartActivity()).Invoke(sourceView, intent, RequestCode.InvalidRequestCode);
        }

        /// <summary>
        /// Performs forward navigation from the <paramref name="sourceView"/> to the target one with handling a result when it finished.
        /// </summary>
        /// <typeparam name="TResultMapper">The type of the target view model result mapper.</typeparam>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="targetViewIntent">The description of the target view.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="ForwardNavigationStrategy.StartActivityForResult(Bundle?)"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> or <paramref name="targetViewIntent"/> is <c>null</c>.</exception>
        public void NavigateForResult<TResultMapper>(
            INavigationView<ILifecycleViewModelWithResultHandler> sourceView,
            Intent targetViewIntent,
            ForwardNavigationDelegate? navigationStrategy = null)
            where TResultMapper : IResultMapper<Result>, new()
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));
            if (targetViewIntent == null)
                throw new ArgumentNullException(nameof(targetViewIntent));

            var requestCode = sourceView.RequestCode.GetFor<TResultMapper>();
            (navigationStrategy ?? NavigationStrategy.Forward.StartActivityForResult()).Invoke(sourceView, targetViewIntent, requestCode);
        }

        /// <summary>
        /// Performs forward navigation from the <paramref name="sourceView"/> to the target one with handling a result when it finished.
        /// </summary>
        /// <typeparam name="TTargetView">The type of the target view.</typeparam>
        /// <typeparam name="TResult">The type of the target view model result.</typeparam>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="ForwardNavigationStrategy.StartActivityForResult(Bundle?)"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sourceView" /> is derived from a class other than the <see cref="FragmentActivity"/> or <see cref="Fragment"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NavigationViewExtensions.GetActivity(INavigationView{ILifecycleViewModel})"/> returned <c>null</c> value for the <paramref name="sourceView"/>.
        /// </exception>
        public void NavigateForResult<TTargetView, TResult>(
            INavigationView<ILifecycleViewModelWithResultHandler> sourceView,
            ForwardNavigationDelegate? navigationStrategy = null)
            where TTargetView : FragmentActivity, INavigationView<ILifecycleViewModelWithResult<TResult>>
            where TResult : Result
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));

            var context = sourceView.GetActivity();

            if (context == null)
            {
                throw new InvalidOperationException(
                    $"'{TypeFormatter.FormatName(sourceView.GetType())}.{nameof(NavigationViewExtensions.GetActivity)}' returned 'null' value.");
            }

            var intent = new Intent(context, typeof(TTargetView));
            var requestCode = sourceView.RequestCode.GetFor<DefaultResultMapper<TResult>>();
            (navigationStrategy ?? NavigationStrategy.Forward.StartActivityForResult()).Invoke(sourceView, intent, requestCode);
        }

        /// <summary>
        /// Performs forward navigation from the <paramref name="sourceView"/> to the target one with provided <paramref name="parameters"/> and handling a result when it finished.
        /// </summary>
        /// <typeparam name="TTargetView">The type of the target view.</typeparam>
        /// <typeparam name="TParameters">The type of the target view model parameters.</typeparam>
        /// <typeparam name="TResult">The type of the target view model result.</typeparam>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="parameters">The target view model parameters. Can be <c>null</c>.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="ForwardNavigationStrategy.StartActivityForResult(Bundle?)"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sourceView" /> is derived from a class other than the <see cref="FragmentActivity"/> or <see cref="Fragment"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NavigationViewExtensions.GetActivity(INavigationView{ILifecycleViewModel})"/> returned <c>null</c> value for the <paramref name="sourceView"/>.
        /// </exception>
        public void NavigateForResult<TTargetView, TParameters, TResult>(
            INavigationView<ILifecycleViewModelWithResultHandler> sourceView,
            TParameters? parameters,
            ForwardNavigationDelegate? navigationStrategy = null)
            where TTargetView : FragmentActivity, IView<ILifecycleViewModelWithParameters<TParameters>>, INavigationView<ILifecycleViewModelWithResult<TResult>>
            where TParameters : Parameters
            where TResult : Result
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));

            var context = sourceView.GetActivity();

            if (context == null)
            {
                throw new InvalidOperationException(
                    $"'{TypeFormatter.FormatName(sourceView.GetType())}.{nameof(NavigationViewExtensions.GetActivity)}' returned 'null' value.");
            }

            var intent = new Intent(context, typeof(TTargetView));
            intent.PutParameters(parameters);
            var requestCode = sourceView.RequestCode.GetFor<DefaultResultMapper<TResult>>();
            (navigationStrategy ?? NavigationStrategy.Forward.StartActivityForResult()).Invoke(sourceView, intent, requestCode);
        }

        /// <summary>
        /// Performs backward navigation from the <paramref name="sourceView"/>.
        /// </summary>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="BackwardNavigationStrategy.Finish()"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> is <c>null</c>.</exception>
        public void NavigateBack(
            INavigationView<ILifecycleViewModel> sourceView,
            BackwardNavigationDelegate? navigationStrategy = null)
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));

            (navigationStrategy ?? NavigationStrategy.Backward.Finish()).Invoke(sourceView);
        }

        /// <summary>
        /// Performs backward navigation from the <paramref name="sourceView"/> with a result.
        /// </summary>
        /// <typeparam name="TResult">The type of the source view model result.</typeparam>
        /// <param name="sourceView">The source navigation view from which navigation is performed from.</param>
        /// <param name="resultCode">Determines whether the result should be set as successful or canceled.</param>
        /// <param name="result">The source view model result. Can be <c>null</c>.</param>
        /// <param name="navigationStrategy">
        /// The strategy used for performing navigation. Can be <c>null</c>.
        /// <para>The default is <see cref="BackwardNavigationStrategy.Finish()"/>.</para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceView"/> is <c>null</c>.</exception>
        public void NavigateBack<TResult>(
            INavigationView<ILifecycleViewModelWithResult<TResult>> sourceView,
            ResultCode resultCode,
            TResult? result,
            BackwardNavigationDelegate? navigationStrategy = null)
            where TResult : Result
        {
            if (sourceView == null)
                throw new ArgumentNullException(nameof(sourceView));

            var resultIntent = new Intent();
            resultIntent.PutResult(result);
            sourceView.SetResult(resultCode, resultIntent);
            (navigationStrategy ?? NavigationStrategy.Backward.Finish()).Invoke(sourceView);
        }
    }
}
