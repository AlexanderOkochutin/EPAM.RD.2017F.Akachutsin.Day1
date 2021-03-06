﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IAddRemoveNotifier
    {
        event EventHandler AddEvent;
        event EventHandler RemoveEvent;

        void AddNotification(object o, EventArgs args);
        void RemoveNotification(object o, EventArgs args);
    }
}
