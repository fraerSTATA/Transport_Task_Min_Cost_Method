﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Controls;

namespace Fogel
{
    interface IMatrixXML
    {
        void PutMatrixToXML();
        void TakeMatrixFromXML(string path);
    }
}
