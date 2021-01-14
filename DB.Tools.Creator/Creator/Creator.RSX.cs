using DB.Properties;
using Rsx.Dumb;
using System;
using System.Diagnostics;
using System.IO;

namespace DB.Tools
{

    public partial class Creator
    {
        /// <summary>
        /// CREATOR RESOURCES
        /// Files, stuff from developer to deploy
        /// </summary>
        public static class RSX
        {
            public static string MainLIMSResource { get; set; }
            public static byte[] MainMatSSFResource { get; set; }
            public static byte[] MainSolCoiResource { get; set; }
            public static byte[] SQLResource { get; set; }

            /// <summary>
            /// Checks the directories. If restore = true, repopulation of resources is performed.
            /// Otherwise the FILE FLAG determines if restoration is performed
            /// </summary>
            /// <param name="restore"></param>
            public static void CheckDirectories(bool restore = false)
            {
                string checking = "Checking ";
                if (restore) checking = "Restoring ";
                string directories = "directories...";
                string resources = "resources...";

                Interface.IReport.Msg(checking, checking + directories);

                //check basic directory
                populateBaseDirectory(restore);

                //create developer file from mainLIMSResource


                bool overriderFound = false;
                if (!restore)
                {
                    //check for FILE FLAG = overriders
                    overriderFound = populateOverriders();
                    Help();
                }
                else overriderFound = true;

                //populate resources
                Interface.IReport.Msg(checking, checking + resources);


              

                populateDevFolders(overriderFound);

                populateXCOM(overriderFound);


                /*
                try
                {
                    path = Interface.IStore.FolderPath + Resources.WCalc;
                    developerPath = Application.StartupPath + Resources.DevFiles + Resources.WCalc;
                    populateReplaceFile(path, developerPath);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }
                */


                populateSolCoiResource(overriderFound);


                populateSQLResource(overriderFound);


                populateMatSSFResource(overriderFound);


            }





            
           
            private static void populateDevFolders(bool restore)
            {

                try
                {
                    string path = string.Empty;
                    path = Interface.IStore.FolderPath + Resources.Backups;
                    IO.MakeADirectory(path, restore);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }
                try
                {
                    string path = string.Empty;
                    path = Interface.IStore.FolderPath + Resources.Exceptions;
                    IO.MakeADirectory(path, restore);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }


                try
                {
                    string path = string.Empty;
                    path = Interface.IStore.DevPath;
                    IO.MakeADirectory(path, restore);
                    //create developer file from mainLIMSResource
            
                        populateADeveloperFile(MainLIMSResource, path+Resources.Linaa);
       

                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }
            }

            /// <summary>
            /// Writes the content of a given developer resource object into a file
            /// </summary>
            /// <param name="fileResourceContent"></param>
            /// <param name="fileName"></param>
            private static void populateADeveloperFile(object fileResourceContent, string path)
            {
               // string path = Interface.IStore.DevPath + fileName;
                if (File.Exists(path)) File.Delete(path);
                if (fileResourceContent == null) return;
                if (fileResourceContent.GetType().Equals(typeof(string)))
                {
                    File.WriteAllText(path, fileResourceContent as string);
                }
                else
                {
                    File.WriteAllBytes(path, fileResourceContent as byte[]);
                }
            }

            private static void populateBaseDirectory(bool restore = false)
            {
                //assign folderpath (like a App\Local folder)
                Interface.IStore.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                Interface.IStore.FolderPath += Resources.k0XFolder; //cambiar esto

                //populate main directory at folderPath
                try
                {
                    IO.MakeADirectory(Interface.IStore.FolderPath, restore);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }


            }

            /// <summary>
            /// populates MatSSF resources only
            /// </summary>
            private static void populateMatSSFResource(bool overrider)
            {
                try
                {
                    //NO USO MAKE PARA NO DESTRUIR LA DATA SINO PODER HACER OVERRIDE

                    string matssf = Interface.IStore.FolderPath + Resources.SSFFolder;
                    bool nossf = !Directory.Exists(matssf);
                    if (nossf || overrider)
                    {
                        string resourcePath = Interface.IStore.DevPath
                          + Resources.SSFResource + ".bak";


                        populateADeveloperFile(MainMatSSFResource, resourcePath);

                        //  IO.MakeADirectory(matssf, restore);
                        //no destruir diretorio sino reemplazar la distro
                        Directory.CreateDirectory(matssf);
                      


                        string startexecutePath = matssf;
                        string destFile = startexecutePath + Resources.SSFResource + ".CAB";
                        IO.CopyAndUnpackCABFile(resourcePath, destFile, startexecutePath, true);


                    }

                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }


            }

            private static void populateXCOM(bool overrider)
            {
                try
                {
                    string path = Interface.IStore.FolderPath + Resources.XCOMFolder;
                    //NO USO MAKE PARA NO DESTRUIR LA DATA SINO PODER HACER OVERRIDE

                    bool noXCOM = !Directory.Exists(path);
                    if (noXCOM || overrider)
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }

            }


            /// <summary>
            /// Seek Help
            /// </summary>
            public static void Help()
            {
                string path = Interface.IStore.DevPath + Resources.Features;
                string currentpath = Interface.IStore.FolderPath + Resources.Features;
                bool features = populateReplaceFile(currentpath, path);
                if (!features) return;
                // if (!System.IO.File.Exists(path)) return;
                Process proceso = new Process();
                IO.Process(proceso, Interface.IStore.FolderPath, NOTEPAD_APP, path, false, false, 0);
            }


            private static void populateSQLResource(bool overrider)
            {
                try
                {
                    string devPath = Interface.IStore.DevPath;

                    string resourcePath = devPath
                                  + "sql.bak";


                    bool nosolcoi = !File.Exists(resourcePath);

                    if (nosolcoi || overrider)
                    {

                        populateADeveloperFile(SQLResource, resourcePath);


                        IO.CopyAndUnpackCABFile(resourcePath, resourcePath, devPath,true, false);


                    }
                }

                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }

            }



            private static bool populateOverriders()
            {
                string path;
                //override preferences
                path = Interface.IStore.FolderPath + Resources.Preferences + ".xml";
                string developerPath = Interface.IStore.DevPath + Resources.Preferences + ".xml";
                populateReplaceFile(path, developerPath);

                path = Interface.IStore.FolderPath + Resources.SSFPreferences + ".xml";
                developerPath = Interface.IStore.DevPath + Resources.SSFPreferences + ".xml";
                populateReplaceFile(path, developerPath);

                // path = folderPath + Resources.SolCoiFolder;

                bool overriderFound = false;
                try
                {
                    //does nothing
                    path = Interface.IStore.DevPath + Resources.ResourcesOverrider;
                    overriderFound = File.Exists(path);
                    //TODO:
                    if (overriderFound) File.Delete(path);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }

                return overriderFound;
            }

            private static bool populateReplaceFile(string path, string developerPath)
            {
                try
                {
                    //this overwrites the user preferences for the developers ones. in case I need to deploy them new preferences
                    if (File.Exists(developerPath))
                    {
                        File.Copy(developerPath, path, true);
                        File.Delete(developerPath);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    //                throw;
                }

                return File.Exists(path);
            }

            /// <summary>
            /// populates Solcoi resources only
            /// </summary>
            private static void populateSolCoiResource(bool overrider)
            {

                try
                {

                    string solcoi = Interface.IStore.FolderPath + Resources.SolCoiFolder;
                    bool nosolcoi = !Directory.Exists(solcoi);

                    if (nosolcoi || overrider)
                    {
                        string devPath = Interface.IStore.DevPath;
                        string resourcePath = devPath + Resources.SolCoiResource + ".bak";

                        populateADeveloperFile(MainSolCoiResource, resourcePath);

                        Directory.CreateDirectory(solcoi);
                        string startexecutePath = solcoi;

                        resourcePath = devPath + Resources.CurvesResource + ".bak";

                        string destFile = startexecutePath + Resources.CurvesResource + ".bak";
                        IO.CopyAndUnpackCABFile(resourcePath, destFile, startexecutePath, false);

                      
                        destFile = startexecutePath + Resources.SolCoiResource + ".bak";
                        IO.CopyAndUnpackCABFile(resourcePath, destFile, startexecutePath, false);
                    }

                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);//                throw;
                }


            }
        }

    }
}