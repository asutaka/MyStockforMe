using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyStock.BLL
{
    public class JobManager
    {
        /// <summary>
        /// Execute all Jobs.
        /// </summary>
        public void ExecuteAllJobs()
        {
            Console.WriteLine($"Begin Method");

            try
            {
                // get all job implementations of this assembly.
                IEnumerable<Type> jobs = GetAllTypesImplementingInterface(typeof(Job));
                // execute each job
                if (jobs != null && jobs.Count() > 0)
                {
                    Job instanceJob = null;
                    Thread thread = null;
                    foreach (Type job in jobs)
                    {
                        // only instantiate the job its implementation is "real"
                        if (IsRealClass(job))
                        {
                            try
                            {
                                // instantiate job by reflection
                                instanceJob = (Job)Activator.CreateInstance(job);
                                Console.WriteLine($"The Job \"{instanceJob.GetName()}\" has been instantiated successfully.");
                                // create thread for this job execution method
                                thread = new Thread(new ThreadStart(instanceJob.ExecuteJob));
                                // start thread executing the job
                                thread.Start();
                                Console.WriteLine($"The Job \"{instanceJob.GetName()}\" has its thread started successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"The Job \"{job.Name}\" could not be instantiated or executed.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"The Job \"{job.FullName}\" cannot be instantiated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured while instantiating or executing Jobs for the Scheduler Framework.");
            }

            Console.WriteLine($"End Method");
        }

        /// <summary>
        /// Returns all types in the current AppDomain implementing the interface or inheriting the type. 
        /// </summary>
        private IEnumerable<Type> GetAllTypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => desiredType.IsAssignableFrom(type));

        }

        /// <summary>
        /// Determine whether the object is real - non-abstract, non-generic-needed, non-interface class.
        /// </summary>
        /// <param name="testType">Type to be verified.</param>
        /// <returns>True in case the class is real, false otherwise.</returns>
        public static bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }
    }
}
