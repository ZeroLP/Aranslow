using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Aranslow.GameObjects;
using Aranslow.Tools;

namespace Aranslow
{
    public class ObjectManager
    {
        public static ASLocalClient LocalPlayer;
        public static List<ASBaseClient> Objects;

        public static bool IsPopulated => Objects != null && Objects.Count >= 1 && LocalPlayer != null;

        public static void PopulateObjects()
        {
            if (!IsPopulated)
            {
                Logger.Log("Populating ObjectManager");

                LocalPlayer = new ASLocalClient(new Vector2(300, 300));

                Objects = new List<ASBaseClient>();
                Objects.Add(LocalPlayer);

                Logger.Log($"Successfully populated ObjectManager: {Objects.Count}");
            }
        }

        public delegate void ObjectManagementEvent(GameTime timeManaged, ASBaseClient objectManaged);

        public static event ObjectManagementEvent OnAddObject;
        public static event ObjectManagementEvent OnCreateObject;
        public static event ObjectManagementEvent OnDeleteObject;

        public static bool AddObject(ASBaseClient baseObject)
        {
            lock (Objects)
            {
                try
                {
                    if (baseObject != null)
                    {
                        Objects.Add(baseObject);
                        OnAddObject?.Invoke(Engine.SecondaryGameTimeHandle, baseObject);
                    }
                    else
                        return false;
                }
                catch (Exception ex) { Logger.Log($"Failed to add object: {ex.Message}"); return false; }
            }

            return true;
        }

        public static T CreateObject<T>(Vector2 wPosition)
        {
            var objCreated = Activator.CreateInstance(typeof(T), new object[] { wPosition });

            lock (Objects)
            {
                try
                {
                    Objects.Add((ASBaseClient)objCreated);
                    OnCreateObject?.Invoke(Engine.SecondaryGameTimeHandle, (ASBaseClient)objCreated);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to create object: {ex.Message}");
                    return default(T);
                }
            }

            return (T)objCreated;
        }

        public static bool DeleteObject(ASBaseClient objToDelete)
        {
            lock (Objects)
            {
                try
                {
                    if (objToDelete != null && Objects.Contains(objToDelete))
                    {
                        Objects.Remove(objToDelete);
                        OnDeleteObject?.Invoke(Engine.SecondaryGameTimeHandle, objToDelete);
                    }
                    else
                        return false;
                }
                catch (Exception) { return false; }
            }

            return true;
        }
    }
}
