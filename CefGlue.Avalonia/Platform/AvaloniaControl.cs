using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Platform;
using Avalonia.Threading;
using Xilium.CefGlue.Common.Helpers;

namespace Xilium.CefGlue.Avalonia.Platform
{
    /// <summary>
    /// The Avalonia control wrapper.
    /// </summary>
    internal class AvaloniaControl : Common.Platform.IControl
    {
        private static IntPtr? _dummyHostView;

        private readonly Control _contextMenuDummyTarget;
        private IntPtr? _browserHandle;
        private Func<WindowBase> _getHostingWindow;

        protected readonly ContentControl _control;

        public event Action<CefSize> SizeChanged;

        public AvaloniaControl(ContentControl control, Func<WindowBase> getHostingWindow)
        {
            _control = control;
            _getHostingWindow = getHostingWindow;

            _contextMenuDummyTarget = new Control();
            _contextMenuDummyTarget.Width = 1;
            _contextMenuDummyTarget.Height = 1;
            _contextMenuDummyTarget.HorizontalAlignment = HorizontalAlignment.Left;
            _contextMenuDummyTarget.VerticalAlignment = VerticalAlignment.Top;

            var panel = new Panel();
            panel.Children.Add(_contextMenuDummyTarget);
            _control.Content = panel;

            _control.LayoutUpdated += OnLayoutUpdated;

            if (NeedsRootWindowStylesFix)
            {
                _control.AttachedToLogicalTree += OnAttachedToLogicalTree;
            }
        }

        protected virtual bool NeedsRootWindowStylesFix => CefRuntime.Platform == CefRuntimePlatform.Windows;

        private void OnAttachedToLogicalTree(object sender, LogicalTreeAttachmentEventArgs e)
        {
            if (e.Root is PopupRoot root)
            {
                // FIX avalonia popups dont apply the CLIPCHILDREN style, so we must force it
                var rootHandle = root.PlatformImpl.Handle.Handle;
                NativeExtensions.Windows.SetWindowLong(rootHandle, NativeExtensions.Windows.GWL.STYLE, NativeExtensions.Windows.WS.CLIPCHILDREN);
            }
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            SizeChanged?.Invoke(new CefSize((int)_control.Bounds.Width, (int)_control.Bounds.Height));
        }

        protected IPlatformHandle GetPlatformHandle()
        {
            return _getHostingWindow()?.PlatformImpl.Handle;
        }

        public virtual IntPtr? GetHostViewHandle()
        {
            if (CefRuntime.Platform == CefRuntimePlatform.MacOSX)
            {
                if (_dummyHostView == null)
                {
                    // create a dummy nsview to host all browsers
                    var nsViewClass = NativeExtensions.OSX.objc_getClass("NSView");
                    var nsViewType = NativeExtensions.OSX.objc_msgSend(nsViewClass, NativeExtensions.OSX.sel_registerName("alloc"));
                    _dummyHostView = NativeExtensions.OSX.objc_msgSend(nsViewType, NativeExtensions.OSX.sel_registerName("init"));
                }
                return _dummyHostView.Value;
            }
            return GetPlatformHandle()?.Handle;
        }

        public void OpenContextMenu(IEnumerable<MenuEntry> menuEntries, int x, int y, CefRunContextMenuCallback callback)
        {
            Dispatcher.UIThread.Post(
                () =>
                {
                    var menu = new ExtendedAvaloniaContextMenu();
                    var menuItems = new List<TemplatedControl>();

                    foreach (var menuEntry in menuEntries)
                    {
                        if (menuEntry.IsSeparator)
                        {
                            menuItems.Add(new Separator());
                        }
                        else
                        {
                            var menuItem = new MenuItem()
                            {
                                Header = menuEntry.Label.Replace("&", "_"),
                                IsEnabled = menuEntry.IsEnabled,
                                // TODO
                                //IsChecked = menuEntry.IsChecked ?? false,
                                //IsCheckable = menuEntry.IsChecked != null,
                            };
                            var commandId = menuEntry.CommandId;
                            menuItem.Click += delegate { callback.Continue(commandId, CefEventFlags.None); };
                            menuItems.Add(menuItem);
                        }
                    }

                    menu.MenuClosed += delegate
                    {
                        callback.Cancel();
                        _control.ContextMenu = null;
                    };

                    menu.Items = menuItems;

                    _control.ContextMenu = menu;
                    menu.Open(_contextMenuDummyTarget, new Point(x, y));
                },
                DispatcherPriority.Input);
        }

        public void CloseContextMenu()
        {
            Dispatcher.UIThread.Post(
               () =>
               {
                   _control.ContextMenu?.Close();
                   _control.ContextMenu = null;
               },
               DispatcherPriority.Input);
        }

        public void InitializeRender(IntPtr browserHandle)
        {
            _browserHandle = browserHandle;

            if (CefRuntime.Platform == CefRuntimePlatform.MacOSX)
            {
                // must retain the browser handle, as long as the browser lives, otherwise seg faults might occur
                NativeExtensions.OSX.objc_retain(browserHandle);
            }

            Dispatcher.UIThread.Post(() => SetContent(new ExtendedAvaloniaNativeControlHost(browserHandle)));
        }

        public void DestroyRender()
        {
            if (_browserHandle == null)
            {
                return;
            }

            switch (CefRuntime.Platform) {
                case CefRuntimePlatform.Windows:
                    NativeExtensions.Windows.DestroyWindow(_browserHandle.Value);
                    break;

                case CefRuntimePlatform.MacOSX:
                    NativeExtensions.OSX.objc_release(_browserHandle.Value);
                    break;   
            }

            _browserHandle = null;
        }

        protected void SetContent(Control content)
        {
            ((Panel)_control.Content).Children.Add(content); 
        }

        public virtual void SetTooltip(string text) { }
    }
}
