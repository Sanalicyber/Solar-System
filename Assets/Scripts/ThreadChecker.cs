using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Smug
{
    public static class ThreadChecker
    {
        public static List<NewtonThread> threads = new();

        public static bool IsRunning = false;

        private static Thread thread;

        public static void StartThread()
        {
            thread = new Thread(DoWork);
            thread.Start();
        }

        private static void DoWork()
        {
            while (IsRunning)
            {
                AddTask(Newton.Cycle());
                Thread.Sleep(10);
            }
        }

        public static void AddTask(NewtonTask.TaskDelegate task)
        {
            NewtonTask newTask = new() { Task = task, IsDone = false };
            if (threads.Count == 0)
            {
                threads.Add(new NewtonThread());
            }
            else if (threads.All(x => x.Tasks.Count == 10))
            {
                threads.Add(new NewtonThread());
            }
            else if (threads.FirstOrDefault(x => x.Tasks.Count < 10) 
                     is NewtonThread thread)
            {
                thread.Tasks.Enqueue(newTask);
            }
        }

        public static void Reset()
        {
            IsRunning = false;
            threads.ForEach(x => x.Reset());
            threads.Clear();
        }
    }

    public class NewtonThread
    {
        public Thread Thread;
        public Queue<NewtonTask> Tasks = new(10);

        public NewtonThread()
        {
            Thread = new Thread(DoWork);
            Thread.Start();
        }

        private void DoWork()
        {
            while (ThreadChecker.IsRunning)
            {
                if (Tasks.Count > 0)
                {
                    NewtonTask task = Tasks.Dequeue();

                    if (task.IsDone) continue;

                    task.Task();
                    task.IsDone = true;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void Reset()
        {
            Thread.Abort();
            Tasks.Clear();
        }
    }

    public struct NewtonTask
    {
        public delegate void TaskDelegate();

        public TaskDelegate Task;
        public bool IsDone;
    }
}