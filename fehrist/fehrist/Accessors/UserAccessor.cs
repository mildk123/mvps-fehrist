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
        DBEntities DB = new DBEntities();

        public int GET_RoleID(string v)
        {
            var roleName = DB.ROLES.Where(x => x.NAME == v).FirstOrDefault();
            if (roleName != null)
            {
                return roleName.ROLEID;
            }
            else
            {
                return 0;
            }
        }

        public ACCOUNT SET_UserAccount(string name, string email, string passwordHash, int roleID)
        {
            try
            {
                ACCOUNT newAcc = new ACCOUNT()
                {
                    NAME = name,
                    PASS = passwordHash,
                    AC_STATUS = "ACTIVE",
                    EMAIL = email,
                    ROLEID = roleID,
                };
                DB.ACCOUNTS.Add(newAcc);
                DB.SaveChanges();
                return newAcc;
            }
            catch (DbUpdateException)
            {

                return null;
            }


        }

        public string GET_UserHash(string email)
        {
            try
            {
                var userAccount = DB.ACCOUNTS.Where(x => x.EMAIL == email).FirstOrDefault();
                if (userAccount != null)
                {
                    return userAccount.PASS;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) 
            {
                Console.Write(e);
                return null;
            }
        }

        public ACCOUNT Get_UserAccount(string email)
        {
            try
            {
                var userAccount = DB.ACCOUNTS.Where(x => x.EMAIL == email).FirstOrDefault();
                if (userAccount != null)
                {
                    return userAccount;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<GET_AllTasksResponse> GET_Tasks(int accID, string status)
        {
            var statusUp = status.ToUpper();
            var allTasks = DB.TASKS.Where(x => x.ACCOUNTID == accID && x.T_STATUS == statusUp).ToList();
            if (allTasks != null)
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
            var selectedTask = DB.TASKS.Where(x => x.ACCOUNTID == accID && x.TASKID == taskID).FirstOrDefault();
            if (selectedTask != null)
            {
                GET_AllTasksResponse taskResponse = new GET_AllTasksResponse
                {
                    taskID = selectedTask.TASKID,
                    title = selectedTask.T_TITLE,
                    desc = selectedTask.T_DESC,
                    color = selectedTask.T_COLOR,
                    dueDate = selectedTask.T_DUE_DATE_TIME
                };

                // Retrieve the associated images for the current task
                var taskImages = DB.TASK_IMAGES.Where(i => i.TASKID == selectedTask.TASKID).ToList();
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

                var checks = DB.CHECKLISTs.Where(x => x.TASKID == selectedTask.TASKID).ToList();
                if (checks != null)
                {
                    List<ChecklistResponse> checkListUser = new List<ChecklistResponse>();
                    foreach (var item in checks)
                    {
                        ChecklistResponse checkITEM = new ChecklistResponse()
                        {
                            checkID = item.CHECKID,
                            desc = item.CL_DESCRIPTION
                        };
                        checkListUser.Add(checkITEM);
                    }
                    taskResponse.checkList = checkListUser;
                }
                return taskResponse;
            }
            else
            {
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
                    DB.SaveChanges();

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
            // UPDATE OLD TASK WTIH CHANGE IN IMAGES
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

                        TASK_IMAGES newImage = new TASK_IMAGES()
                        {
                            TASKID = updateTask.TASKID,
                            TI_PATH = file != null ? $"/storage/images/{accID}/" + file.FileName : null
                        };
                        DB.TASK_IMAGES.Add(newImage);

                    }
                }
                var serializer = new JavaScriptSerializer();
                var fetched = serializer.Deserialize<List<ImagesModel>>(previmages);

                if (fetched != null)
                {
                    foreach (var item in fetched)
                    {
                        var instanceFound = DB.TASK_IMAGES.FirstOrDefault(x => x.IMAGEID == item.imageID);
                        if (instanceFound != null)
                        {
                            instanceFound.TASKID = taskID;
                        }
                        //DB.SaveChanges();
                    }
                    DB.SaveChanges();
                }

                var checkListRec = serializer.Deserialize<List<string>>(checkList);
                if (checkListRec != null)
                {
                    foreach (var item in checkListRec)
                    {
                        CHECKLIST newCheck = new CHECKLIST()
                        {
                            CL_DESCRIPTION = item.ToString(),
                            TASKID = updateTask.TASKID,
                            DATE_UPDATED = DateTime.Now,
                            UPDATE_BY = accID.ToString()
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

        public string DELETE_Check(int accID, int checkID)
        {
            try
            {
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
                var task = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (task != null)
                {

                    var taskImages = DB.TASK_IMAGES.Where(x => x.TASKID == taskID).ToList();
                    if (taskImages.Count > 0)
                    {
                        DB.TASK_IMAGES.RemoveRange(taskImages);
                    }

                    var taskChecks = DB.CHECKLISTs.Where(x => x.TASKID == taskID).ToList();
                    if (taskChecks.Count > 0)
                    {
                        DB.CHECKLISTs.RemoveRange(taskChecks);
                    }

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
                var taskSelected = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (taskSelected != null)
                {
                    taskSelected.T_STATUS = status.ToUpper();
                    DB.SaveChanges();
                    return "Sucessfully chaned status of task";
                }
                else
                {
                    return "The selected task does not exists anymore.";
                }
            }
            catch (Exception)
            {
                return "An error occured while updating current task. Please login again refresh the caches.";
            }

        }

        public string UPDATE_Task_Color(int taskID, int accID, string color)
        {
            try
            {
                var taskSelected = DB.TASKS.Where(x => x.TASKID == taskID && x.ACCOUNTID == accID).FirstOrDefault();
                if (taskSelected != null)
                {
                    taskSelected.T_COLOR = color.ToUpper();
                    DB.SaveChanges();
                    return "Sucessfully changed color of task";
                }
                else
                {
                    return "The selected task does not exists anymore.";
                }
            }
            catch (Exception)
            {
                return "An error occured while updating current task. Please login again refresh the caches.";
            }

        }

        public List<GET_AllTasksResponse> SEARCH_Tasks(int accID, string searchTerm, string status)
        {
            var termLower = searchTerm == null ? "*" : searchTerm.ToLower();
            var statusUpper = status.ToUpper();

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