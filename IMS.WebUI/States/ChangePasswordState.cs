﻿using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Syncfusion.PdfExport;

namespace IMS.WebUI.States
{
    public class ChangePasswordState
    {
        public string PasswordType = "password";
        public bool PasswordState = true;
        public string DisplayText = "Show";
        public Action? Changed;

        public void ChangePasswordType()
        {
            PasswordState = !PasswordState;
            if(!PasswordState)
            {
                PasswordType = "text";
                DisplayText = "hide";

            }
            else
            {
                PasswordType = "password";
                DisplayText = "Show";
            }
            Changed?.Invoke();
        }
    }
}
