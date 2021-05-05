namespace Optimization.Algorithms
{
    public interface ITargetFunctionCalculator<T>
    {
        /// <summary>
        /// Calculate target function value of dicision
        /// </summary>
        /// <param name="dicision">Target dicision</param>
        /// <returns>Target function value</returns>
        double Calculate(T dicision);
    }
}