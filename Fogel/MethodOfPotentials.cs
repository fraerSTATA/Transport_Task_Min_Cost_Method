using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogel
{
    public class MethodOfPotentials
    {
        MinCost min;
        private Dictionary<int, int> consumers = new Dictionary<int, int>();
        private Dictionary<int,int> providers = new Dictionary<int, int>();
        public MethodOfPotentials(MinCost a) { min = a; }
        
        public async void OptimizePlan()
        {
            MatrixN matrix = min.Matrix;
           
            await Task.WhenAll(new[] { Task.Run(() => TakeProviders()), Task.Run(() => TakeCostumers()) });
            int i = 0;
        }

        protected void TakeProviders()
        {
            for (int i = 1; i < min.Matrix.getMatrix().GetLength(0); i++)
            {
                int sum = 0;
                int fakeSum = 0;
                var rows = min.MinValues.Keys.Where(t => t.Key == i).ToList();
                for (int j = 1; j < min.Matrix.getMatrix().GetLength(1); j++)
                {
                    sum += min.Matrix.getMatrix()[i, j];
                }
                foreach (var item in rows)
                {
                    fakeSum += min.Matrix.getMatrix()[item.Key, item.Value];
                }
                providers.Add(i, sum - fakeSum);

            }
        }

        protected  void TakeCostumers()
        {
            for (int j = 1; j < min.Matrix.getMatrix().GetLength(1); j++)
            {
                int sum = 0;
                int fakeSum = 0;
                var columns = min.MinValues.Keys.Where(t => t.Value == j).ToList();
                for (int i = 1; i < min.Matrix.getMatrix().GetLength(0); i++)
                {                  
                    sum += min.Matrix.getMatrix()[i, j];               
                }
                foreach (var item in columns)
                {
                    fakeSum += min.Matrix.getMatrix()[item.Key, item.Value];
                }
                consumers.Add(j, sum - fakeSum);

            }
        }


    }
}
