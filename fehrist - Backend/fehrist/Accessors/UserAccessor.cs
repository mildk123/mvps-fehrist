using fehrist.Helper;
using fehrist.Models;
using fehrist.Models.API_Models.Response.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Script.Serialization;

namespace fehrist.Accessors
{
    public class UserAccessor
    {
        private DBEntities DB = new DBEntities();

        public virtual int GET_RoleID(string v)
        {
            // Retrieve the role with the given name from the database
            var roleName = DB.ROLES.Where(x => x.NAME == v).FirstOrDefault();
            if (roleName != null)
            {
                // Return the role ID if found
                return roleName.ROLEID;
            }
            else
            {
                // Return 0 if the role is not found
                return 0;
            }
        }

        public  virtual string GET_UserHash(string email)
        {
            try
            {
                // Retrieve the user account with the given email from the database
                var userAccount = DB.ACCOUNTS.Where(x => x.EMAIL == email).FirstOrDefault();
                if (userAccount != null)
                {
                    // Return the password hash of the user account
                    return userAccount.PASS;
                }
                else
                {
                    // Return null if the user account is not found
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                // Return null if there is an exception
                return null;
            }
        }

        public virtual  ACCOUNT Get_UserAccount(string email)
        {
            try
            {
                // Retrieve the user account with the given email from the database
                var userAccount = DB.ACCOUNTS.Where(x => x.EMAIL == email).FirstOrDefault();
                if (userAccount != null)
                {
                    // Return the user account
                    return userAccount;
                }
                else
                {
                    // Return null if the user account is not found
                    return null;
                }
            }
            catch (Exception)
            {
                // Return null if there is an exception
                return null;
            }
        }

        public ACCOUNT SET_UserAccount(string name, string email, string passwordHash, int roleID)
        {
            try
            {
                var alreadyExists = DB.ACCOUNTS.Where(x => x.EMAIL == email).FirstOrDefault();
                if (alreadyExists != null)
                {
                    return null;
                }
                // Create a new user account object
                ACCOUNT newAcc = new ACCOUNT()
                {
                    NAME = name,
                    PASS = passwordHash,
                    AC_STATUS = "ACTIVE",
                    EMAIL = email,
                    ROLEID = roleID,
                };
                // Add the new account to the database
                DB.ACCOUNTS.Add(newAcc);
                DB.SaveChanges();
                // Return the newly created account
                return newAcc;
            }
            catch (DbUpdateException)
            {
                // Return null if there is an exception during database update
                return null;
            }
        }

        public List<GET_AllTasksResponse> GET_Tasks(int accID, string status)
        {
            var statusUp = status.ToUpper();

            // Retrieve all tasks matching the specified account ID and status
            var allTasks = DB.TASKS.Where(x => x.ACCOUNTID == accID && x.T_STATUS == statusUp).ToList();

            if (allTasks.Count != 0)
            {
                List<GET_AllTasksResponse> taskList = new List<GET_AllTasksResponse>();

                foreach (var task in allTasks)
                {
                    GET_AllTasksResponse taskResponse = new GET_AllTasksResponse
                    {
                        taskID = task.TASKID,
                        title = task.T_TITLE,
                        desc = task.T_DESC,
                        color = task.T_COLOR
                    };

                    // Retrieve the associated images for the current task
                    var taskImages = DB.TASK_IMAGES.Where(i => i.TASKID == task.TASKID).ToList();

                    if (taskImages != null)
                    {
                        List<ImagesModel> imgs = new List<ImagesModel>();

                        foreach (var item in taskImages)
                        {
                            ImagesModel img = new ImagesModel()
                            {
                                imageID = item.IMAGEID,
                                imagePath = item.TI_PATH
                            };

                            imgs.Add(img);
                        }

                        taskResponse.imageList = imgs;
                    }

                    taskList.Add(taskResponse);
                }

                return taskList;
            }
            else
            {
                return null;
            }
        }

        public GET_AllTasksResponse GET_Task_Single(int accID, int taskID)
        {
            // Retrieve a single task for the given account ID and task ID from the database
            var singleTask = DB.TASKS.FirstOrDefault(x => x.ACCOUNTID == accID && x.TASKID == taskID);
            if (singleTask != null)
            {
                GET_AllTasksResponse taskResponse = new GET_AllTasksResponse
                {
                    taskID = singleTask.TASKID,
                    title = singleTask.T_TITLE,
                    desc = singleTask.T_DESC,
                    color = singleTask.T_COLOR,
                    dueDate = singleTask.T_DUE_DATE_TIME
                };

                // Retrieve the associated images for the task
                var taskImages = DB.TASK_IMAGES.Where(i => i.TASKID == singleTask.TASKID).ToList();
                if (taskImages != null)
                {
                    List<ImagesModel> imgs = new List<ImagesModel>();
                    foreach (var item in taskImages)
                    {
                        ImagesModel img = new ImagesModel()
                        {
                            imageID = item.IMAGEID,
                            imagePath = item.TI_PATH
                        };
                        // Add the image to the image list
                        imgs.Add(img);
                    }
                    // Add the image list to the task response
                    taskResponse.imageList = imgs;
                }

                // Retrieve the associated checklist items for the task
                var taskChecklist = DB.CHECKLISTs.Where(c => c.TASKID == singleTask.TASKID).ToList();
                if (taskChecklist != null)
                {
                    List<ChecklistResponse> checklist = new List<ChecklistResponse>();
                    foreach (var item in taskChecklist)
                    {
                        ChecklistResponse chk = new ChecklistResponse()
                        {
                            checkID = item.CHECKID,
                            desc = item.CL_DESCRIPTION
                        };
                        // Add the checklist item to the checklist
                        checklist.Add(chk);
                    }
                    // Add the checklist to the task response
                    taskResponse.checkList = checklist;
                }

                // Return the task response
                return taskResponse;
            }
            else
            {
                // Return null if the task is not found
                return null;
            }
        }

        public string SET_Task(int accID, int taskID, string title, string desc, string status, string color, string dueDate, string addedDate, List<HttpPostedFile> filesList, string checkList)
        {
            if (taskID == 0)
            {
                // NEW ZERO METER TASK
                TASK newTask = new TASK()
                {
                    ACCOUNTID = accID,
                    T_TITLE = title,
                    T_DESC = desc,
                    T_COLOR = color,
                    T_ADDED_DATE_TIME = addedDate,
                    T_DUE_DATE_TIME = dueDate,
                    T_STATUS = status.ToUpper()
                };
                DB.TASKS.Add(newTask);
                DB.SaveChanges();

                // Save the attached images for the new task
                for (int i = 0; i < filesList.Count; i++)
                {
                    var file = filesList[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath($"~/storage/images/{accID}/"));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/storage/images/{accID}/"));
                        var path = Path.Combine(HttpContext.Current.Server.MapPath($@"~/storage/images/{accID}/"), fileName);
                        file.SaveAs(path);

                        TASK_IMAGES newImage = new TASK_IMAGES()
                        {
                            TASKID = newTask.TASKID,
                            TI_PATH = file != null ? $"/storage/images/{accID}/" + file.FileName : null
                        };
                        DB.TASK_IMAGES.Add(newImage);
                        DB.SaveChanges();
                    }
                }

                var serializer = new JavaScriptSerializer();
                var checkListRec = serializer.Deserialize<List<string>>(checkList);

                // Save the checklist items for the new task
                if (checkListRec != null)
                {
                    foreach (var item in checkListRec)
                    {
                        CHECKLIST newCheck = new CHECKLIST()
                        {
                            DATE_CREATED = DateTime.Now,
                            CL_DESCRIPTION = item.ToString(),
                            TASKID = newTask.TASKID,
                            CREATED_BY = accID.ToString()
                        };
                        DB.CHECKLISTs.Add(newCheck);
                        DB.SaveChanges();
                    }
                    DB.SaveChanges();
                }

                return "Successfully saved task";
            }
            else
            {
                // UPDATE OLD TASK NO PICS BEFORE
                var updateTask = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (updateTask != null)
                {
                    updateTask.T_COLOR = color;
                    updateTask.T_TITLE = title;
                    updateTask.T_DESC = desc;
                    updateTask.T_DUE_DATE_TIME = dueDate;
                    updateTask.TASK_IMAGES.Clear();
                    DB.CHECKLISTs.RemoveRange(updateTask.CHECKLISTs.Select(ti => ti));
                    DB.SaveChanges();

                    // Save the attached images for the updated task
                    for (int i = 0; i < filesList.Count; i++)
                    {
                        var file = filesList[i];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath($"~/storage/images/{accID}/"));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/storage/images/{accID}/"));
                            var path = Path.Combine(HttpContext.Current.Server.MapPath($@"~/storage/images/{accID}/"), fileName);
                            file.SaveAs(path);

                            TASK_IMAGES newImage = new TASK_IMAGES()
                            {
                                TASKID = updateTask.TASKID,
                                TI_PATH = file != null ? $"/storage/images/{accID}/" + file.FileName : null
                            };
                            DB.TASK_IMAGES.Add(newImage);
                        }
                    }

                    var serializer = new JavaScriptSerializer();
                    var checkListRec = serializer.Deserialize<List<string>>(checkList);

                    // Save the checklist items for the updated task
                    if (checkListRec != null)
                    {
                        foreach (var item in checkListRec)
                        {
                            CHECKLIST newCheck = new CHECKLIST()
                            {
                                DATE_CREATED = DateTime.Now,
                                CL_DESCRIPTION = item.ToString(),
                                TASKID = updateTask.TASKID,
                                CREATED_BY = accID.ToString()
                            };
                            DB.CHECKLISTs.Add(newCheck);
                            DB.SaveChanges();
                        }
                        DB.SaveChanges();
                    }

                    return "Successfully updated task";
                }
                else
                {
                    return null;
                }
            }
        }

        public string UPDATE_TaskImage(int accID, int taskID, string title, string desc, string status, string color, string dueDate, string addedDate, List<HttpPostedFile> filesList, string previmages, string checkList)
        {
            // Update an existing task with changes in images
            var updateTask = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
            if (updateTask != null)
            {
                // Update the task properties
                updateTask.T_COLOR = color;
                updateTask.T_TITLE = title;
                updateTask.T_DESC = desc;
                updateTask.T_DUE_DATE_TIME = dueDate;

                // Clear the existing task images and checklist items
                updateTask.TASK_IMAGES.Clear();
                DB.CHECKLISTs.RemoveRange(updateTask.CHECKLISTs.Select(ti => ti));

                // Save the changes to the database
                DB.SaveChanges();

                // Process the new image files and add them to the task
                for (int i = 0; i < filesList.Count; i++)
                {
                    var file = filesList[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath($"~/storage/images/{accID}/{fileName}"));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/storage/images/{accID}/"));
                        var path = Path.Combine(HttpContext.Current.Server.MapPath($@"~/storage/images/{accID}/"), fileName);
                        file.SaveAs(path);

                        // Create a new task image object
                        TASK_IMAGES newImage = new TASK_IMAGES()
                        {
                            TASKID = updateTask.TASKID,
                            TI_PATH = file != null ? $"/storage/images/{accID}/" + file.FileName : null
                        };
                        // Add the image to the task
                        DB.TASK_IMAGES.Add(newImage);
                    }
                }

                var serializer = new JavaScriptSerializer();
                var fetched = serializer.Deserialize<List<ImagesModel>>(previmages);

                // Update the existing task images
                if (fetched != null)
                {
                    foreach (var item in fetched)
                    {
                        var instanceFound = DB.TASK_IMAGES.FirstOrDefault(x => x.IMAGEID == item.imageID);
                        if (instanceFound != null)
                        {
                            instanceFound.TASKID = taskID;
                        }
                    }
                    DB.SaveChanges();
                }

                // Process the checklist items and add them to the task
                var checkListRec = serializer.Deserialize<List<string>>(checkList);
                if (checkListRec != null)
                {
                    foreach (var item in checkListRec)
                    {
                        // Create a new checklist item
                        CHECKLIST newCheck = new CHECKLIST()
                        {
                            CL_DESCRIPTION = item.ToString(),
                            TASKID = updateTask.TASKID,
                            DATE_UPDATED = DateTime.Now,
                            UPDATE_BY = accID.ToString()
                        };
                        // Add the checklist item to the task
                        DB.CHECKLISTs.Add(newCheck);
                        DB.SaveChanges();
                    }
                    DB.SaveChanges();

                }

                return "Successfully updated task";
            }
            else
            {
                return null;
            }
        }

        public string DELETE_Check(int accID, int checkID)
        {
            try
            {
                // Retrieve and delete a checklist item
                var checkITEM = DB.CHECKLISTs.Where(x => x.CHECKID == checkID && x.TASK.ACCOUNTID == accID).FirstOrDefault();
                if (checkITEM != null)
                {
                    DB.CHECKLISTs.Remove(checkITEM);
                    DB.SaveChanges();
                    return "Successfully removed checklist";
                }
                else
                {
                    return "Checklist not found.";
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string DELETE_Task(int accID, int taskID)
        {
            try
            {
                // Retrieve and delete a task
                var task = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (task != null)
                {
                    // Retrieve and delete the task images associated with the task
                    var taskImages = DB.TASK_IMAGES.Where(x => x.TASKID == taskID).ToList();
                    if (taskImages.Count > 0)
                    {
                        DB.TASK_IMAGES.RemoveRange(taskImages);
                    }

                    // Retrieve and delete the checklist items associated with the task
                    var taskChecks = DB.CHECKLISTs.Where(x => x.TASKID == taskID).ToList();
                    if (taskChecks.Count > 0)
                    {
                        DB.CHECKLISTs.RemoveRange(taskChecks);
                    }

                    // Remove the task from the database
                    DB.TASKS.Remove(task);
                    DB.SaveChanges();
                    return "Successfully removed task";
                }
                else
                {
                    return "Task not found.";
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UPDATE_Task_Status(int taskID, int accID, string status)
        {
            try
            {
                // Retrieve and update the status of a task
                var taskSelected = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (taskSelected != null)
                {
                    // Update the task status
                    taskSelected.T_STATUS = status.ToUpper();
                    DB.SaveChanges();
                    return "Successfully changed status of task";
                }
                else
                {
                    return "The selected task does not exist anymore.";
                }
            }
            catch (Exception)
            {
                return "An error occurred while updating the current task. Please log in again and refresh the caches.";
            }
        }

        public string UPDATE_Task_Color(int taskID, int accID, string color)
        {
            try
            {
                // Retrieve and update the color of a task
                var taskSelected = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (taskSelected != null)
                {
                    // Update the task color
                    taskSelected.T_COLOR = color.ToUpper();
                    DB.SaveChanges();
                    return "Successfully changed color of task";
                }
                else
                {
                    return "The selected task does not exist anymore.";
                }
            }
            catch (Exception)
            {
                return "An error occurred while updating the current task. Please log in again and refresh the caches.";
            }
        }

        public List<GET_AllTasksResponse> SEARCH_Tasks(int accID, string searchTerm, string status)
        {
            // Convert the search term to lowercase and the status to uppercase
            var termLower = searchTerm == null ? "*" : searchTerm.ToLower();
            var statusUpper = status.ToUpper();

            // Retrieve tasks that match the search term and status for the specified account
            var allTasks = DB.TASKS.Where(x => x.ACCOUNTID == accID &&
                ((x.T_STATUS == statusUpper && x.T_TITLE.Contains(termLower)) ||
                (x.T_STATUS == statusUpper && x.T_DESC.Contains(termLower))) &&
                x.ACCOUNTID == accID
            ).ToList();

            if (allTasks.Count != 0)
            {
                List<GET_AllTasksResponse> taskList = new List<GET_AllTasksResponse>();

                foreach (var task in allTasks)
                {
                    GET_AllTasksResponse taskResponse = new GET_AllTasksResponse
                    {
                        taskID = task.TASKID,
                        title = task.T_TITLE,
                        desc = task.T_DESC,
                    };

                    // Retrieve the associated images for the current task
                    var taskImages = DB.TASK_IMAGES.Where(i => i.TASKID == task.TASKID).ToList();
                    if (taskImages != null)
                    {
                        List<ImagesModel> imgs = new List<ImagesModel>();
                        foreach (var item in taskImages)
                        {
                            ImagesModel img = new ImagesModel()
                            {
                                imageID = item.IMAGEID,
                                imagePath = item.TI_PATH
                            };
                            imgs.Add(img);
                        }
                        taskResponse.imageList = imgs;
                    }

                    taskList.Add(taskResponse);
                }

                return taskList;
            }
            else
            {
                return null;
            }
        }


    }
}