using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fogel
{
    interface ILeastCostMethodSolve
    {
        List<Grid> SolveTask(MatrixN matrix,MainWindow a);
        
    }
}
