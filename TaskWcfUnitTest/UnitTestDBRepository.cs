using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskWcf.Infrastructure;

namespace TaskWcfUnitTest
{
    [TestClass]
    public class UnitTestDBRepository
    {
        Repository repository = new Repository();

        [TestMethod]
        public void TestMethodCheckParentTaskChange()
        {
            var check = repository.CheckParentTaskChange(new Guid("01B3356C-4E37-4FAE-9C36-56B8A9B9BC51"), new Guid("429137DA-3C15-4A14-9DB3-6FDA97179008"));
            Assert.AreEqual(check, false);
        }

        [TestMethod]
        public void TestMethodCreateTask()
        {
            var task1 = new TaskWcf.Model.DTO.Task()
            {
                ClientUid = new Guid("958890F2-F5E6-412E-98E2-7B547E7B8786"),
                CreateDate = DateTime.Now,
                Description = "Тестовое задание 1. Создано с помощью UnitTest",
                ModifiedDate = DateTime.Now,
                Name = "Тестовое задание 1.",
                PlanTime = 12,
                StateId = 1
            };


            
            var executors = new List<Guid>();

            executors.Add(new Guid("EFB639F6-874C-410F-AFCB-0B7BECF0D52F"));

            var newTask1Uid = repository.CreateTask(task1,executors);

            if (newTask1Uid == Guid.Empty)
            {
                Assert.AreNotEqual(newTask1Uid, Guid.Empty);
                return;
            }
            
            var task2 = new TaskWcf.Model.DTO.Task()
            {
                ClientUid = new Guid("958890F2-F5E6-412E-98E2-7B547E7B8786"),
                CreateDate = DateTime.Now,
                Description = "Тестовое задание 2. Создано с помощью UnitTest",
                ModifiedDate = DateTime.Now,
                Name = "Тестовое задание 2.",
                PlanTime = 12,
                StateId = 1,
                ParentUid = newTask1Uid
            };

            var newTask2Uid = repository.CreateTask(task2, executors);
            

            Assert.AreNotEqual(newTask2Uid, Guid.Empty);
        }

        [TestMethod]
        public void TestMethodUpdateTask()
        {
            var task = repository.GetTasks(new Guid("429137da-3c15-4a14-9db3-6fda97179008")).First();
            task.PlanTime = 4;
            task.FactTime = 4;
            task.StateId = 2;

            var result = repository.UpdateTask(task);

            Assert.AreEqual(result,true);
        }

        [TestMethod]
        public void TestMethodGetStates()
        {
            var states = repository.GetStates(new Guid("429137DA-3C15-4A14-9DB3-6FDA97179008")) ;
            Assert.AreEqual(states.Count, 2);
        }

        [TestMethod]
        public void TestMethodGetTasks()
        {
            var tasks = repository.GetTasks();
            Assert.AreEqual(tasks.Count,2);

            var tasksGuid = repository.GetTasks(new Guid("01B3356C-4E37-4FAE-9C36-56B8A9B9BC51"));
            Assert.AreEqual(tasksGuid.Count, 1);

            var tasksName = repository.GetTasks(taskName: "Тестовое");
            Assert.AreEqual(tasksName.Count, 2);
        }

        [TestMethod]
        public void TestMethodDeleteTask()
        {
            Assert.AreEqual(repository.DeleteTask(new Guid("429137DA-3C15-4A14-9DB3-6FDA97179008")), true);
        }

        [TestMethod]
        public void TestMethodCheckChildren()
        {
            var hasChildren = repository.CheckChildren(new Guid("01B3356C-4E37-4FAE-9C36-56B8A9B9BC51"));
            Assert.AreEqual(hasChildren, true);
        }

        [TestMethod]
        public void TestMethodGetUsers()
        {
            var users = repository.GetUsers();
            Assert.AreNotEqual(users.Count, 0);
        }
    }
}
