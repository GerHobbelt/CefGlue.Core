//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;
    
    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal unsafe struct cef_dev_tools_message_observer_t
    {
        internal cef_base_ref_counted_t _base;
        internal IntPtr _on_dev_tools_message;
        internal IntPtr _on_dev_tools_method_result;
        internal IntPtr _on_dev_tools_event;
        internal IntPtr _on_dev_tools_agent_attached;
        internal IntPtr _on_dev_tools_agent_detached;
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void add_ref_delegate(cef_dev_tools_message_observer_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int release_delegate(cef_dev_tools_message_observer_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int has_one_ref_delegate(cef_dev_tools_message_observer_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int has_at_least_one_ref_delegate(cef_dev_tools_message_observer_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int on_dev_tools_message_delegate(cef_dev_tools_message_observer_t* self, cef_browser_t* browser, void* message, UIntPtr message_size);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_dev_tools_method_result_delegate(cef_dev_tools_message_observer_t* self, cef_browser_t* browser, int message_id, int success, void* result, UIntPtr result_size);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_dev_tools_event_delegate(cef_dev_tools_message_observer_t* self, cef_browser_t* browser, cef_string_t* method, void* @params, UIntPtr params_size);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_dev_tools_agent_attached_delegate(cef_dev_tools_message_observer_t* self, cef_browser_t* browser);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_dev_tools_agent_detached_delegate(cef_dev_tools_message_observer_t* self, cef_browser_t* browser);
        
        private static int _sizeof;
        
        static cef_dev_tools_message_observer_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_dev_tools_message_observer_t));
        }
        
        internal static cef_dev_tools_message_observer_t* Alloc()
        {
            var ptr = (cef_dev_tools_message_observer_t*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_dev_tools_message_observer_t();
            ptr->_base._size = (UIntPtr)_sizeof;
            return ptr;
        }
        
        internal static void Free(cef_dev_tools_message_observer_t* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        
    }
}
