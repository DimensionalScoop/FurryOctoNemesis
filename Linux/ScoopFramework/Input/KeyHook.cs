﻿using System;
using System.Runtime.InteropServices;


namespace ScoopFramework.Input
{
    class CharacterEventArgs : EventArgs
    {
        private readonly char character;
        private readonly int lParam;
        public CharacterEventArgs(char character, int lParam)
        {
            this.character = character;
            this.lParam = lParam;
        }
        public char Character
        {
            get
            {
                return character;
            }
        }
        public int Param
        {
            get
            {
                return lParam;
            }
        }
        public int RepeatCount
        {
            get
            {
                return lParam & 0xffff;
            }
        }
        public bool ExtendedKey
        {
            get
            {
                return (lParam & (1 << 24)) > 0;
            }
        }
        public bool AltPressed
        {
            get
            {
                return (lParam & (1 << 29)) > 0;
            }
        }
        public bool PreviousState
        {
            get
            {
                return (lParam & (1 << 30)) > 0;
            }
        }
        public bool TransitionState
        {
            get
            {
                return (lParam & (1 << 31)) > 0;
            }
        }
    }
    class KeyEventArgs : EventArgs
    {
        private ConsoleKey keyCode;
        public KeyEventArgs(ConsoleKey keyCode)
        {
            this.keyCode = keyCode;
        }
        public ConsoleKey KeyCode
        {
            get
            {
                return keyCode;
            }
        }
    }
    delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
    delegate void KeyEventHandler(object sender, KeyEventArgs e);
    static class EventInput
    {		/// <summary>		
        /// Event raised when a character has been entered.		
        public static event CharEnteredHandler CharEntered;
        /// /// <summary>		/// Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.	
        public static event KeyEventHandler KeyDown;
        /// /// <summary>		/// Event raised when a key has been released.	
        public static event KeyEventHandler KeyUp;
        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        public static bool initialized { get; private set; }
        static IntPtr prevWndProc;
        static WndProc hookProcDelegate;
        static IntPtr hIMC;
        //various Win32 constants that we need		
        const int GWL_WNDPROC = -4;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x102;
        const int WM_IME_SETCONTEXT = 0x0281;
        const int WM_INPUTLANGCHANGE = 0x51;
        const int WM_GETDLGCODE = 0x87;
        const int WM_IME_COMPOSITION = 0x10f;
        const int DLGC_WANTALLKEYS = 4;
        //Win32 functions that we're using		
        [DllImport("Imm32.dll")]
        static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("Imm32.dll")]
        static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);
        [DllImport("user32.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        /// /// <summary>
        /// /// Initialize the TextInput with the given GameWindow.		
        /// </summary>		
        /// <param name="window">The XNA window to which text variables should be linked.</param>	
        public static void Initialize(IntPtr WindowHandle)
        {
            if (initialized) throw new InvalidOperationException("TextInput.Initialize can only be called once!");
            hookProcDelegate = new WndProc(HookProc);
            prevWndProc = (IntPtr)SetWindowLong(WindowHandle, GWL_WNDPROC, (int)Marshal.GetFunctionPointerForDelegate(hookProcDelegate));
            hIMC = ImmGetContext(WindowHandle);
            initialized = true;
        }

        [DllImport("kernel32.dll")]
        public extern static IntPtr GetConsoleWindow();

        static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr returnCode = CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
            switch (msg)
            {
                case WM_GETDLGCODE: returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
                    break;
                case WM_KEYDOWN: if (KeyDown != null) KeyDown(null, new KeyEventArgs((ConsoleKey)wParam));
                    break;
                case WM_KEYUP: if (KeyUp != null) KeyUp(null, new KeyEventArgs((ConsoleKey)wParam));
                    break;
                case WM_CHAR: if (CharEntered != null) CharEntered(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
                    break;
                case WM_IME_SETCONTEXT: if (wParam.ToInt32() == 1) ImmAssociateContext(hWnd, hIMC);
                    break;
                case WM_INPUTLANGCHANGE: ImmAssociateContext(hWnd, hIMC);
                    returnCode = (IntPtr)1;
                    break;
            } return returnCode;
        }
    }
}