﻿using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Cursor
    {


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int OpenDelegate(IntPtr txn, uint dbi, out IntPtr cursor);

        private static OpenDelegate _openDelegate = null;

        internal static IntPtr Open(IntPtr txn, uint dbi)
        {
            IntPtr ptr;
            int err = _openDelegate(txn, dbi, out ptr);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_open", err);
            return ptr;
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CloseDelegate(IntPtr cursor);

        private static CloseDelegate _closeDelegate = null;

        internal static void Close(IntPtr cursor)
        {
            int err = _closeDelegate(cursor);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_close", err);
        }

        internal static void Bind()
        {
            _closeDelegate = Library.GetProcAddress<CloseDelegate>("mdbx_cursor_close") as CloseDelegate;
        }
    }
}