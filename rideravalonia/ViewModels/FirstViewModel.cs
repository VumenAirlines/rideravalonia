// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using ReactiveUI;

namespace rideravalonia.ViewModels;


    public class FirstViewModel : ViewModelBase, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = "first";//Guid.NewGuid().ToString().Substring(0, 5);
      
        public FirstViewModel(IScreen screen) => HostScreen = screen;
    }

